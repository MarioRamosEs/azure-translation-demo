data "azurerm_client_config" "current" {}

data "azurerm_subscription" "current" {}

locals {
  tags = merge(var.tags, {
    createdAt   = "${formatdate("YYYY-MM-DD hh:mm:ss", timestamp())} UTC"
    createdWith = "Terraform"
  })
}

resource "azurerm_resource_group" "rg" {
  name     = "rg-azure-translation-${var.suffix}"
  location = var.location
  tags     = local.tags

  lifecycle {
    ignore_changes = [
      tags,
    ]
  }
}

module "mi" {
  source              = "./modules/mi"
  name                = "id-azure-translation-${var.suffix}"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  tags                = local.tags
}

module "log" {
  source              = "./modules/log"
  name                = "log-azure-translation-${var.suffix}"
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  tags                = local.tags
}

module "appi" {
  source                     = "./modules/appi"
  name                       = "appi-azure-translation-${var.suffix}"
  location                   = var.location
  resource_group_name        = azurerm_resource_group.rg.name
  log_analytics_workspace_id = module.log.id
  tags                       = local.tags
}

module "st" {
  source                   = "./modules/st"
  name                     = "staztranslation${var.suffix}"
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = var.location
  account_tier             = var.storage_account_tier
  account_replication_type = var.storage_account_replication_type
  identity_id              = module.mi.id
  tags                     = local.tags
}

resource "azurerm_storage_table" "translations" {
  name                 = var.translations_table_name
  storage_account_name = module.st.name
}

// TODO Refactor in a module
resource "azurerm_servicebus_namespace" "servicebus" {
  name                = "sbns-azure-translation-${var.suffix}"
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "Basic"
  tags                = local.tags

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

resource "azurerm_servicebus_queue" "servicebus_queue" {
  name          = "sbq-translation-requests"
  namespace_id  = azurerm_servicebus_namespace.servicebus.id
  lock_duration = "PT5M"
}

// TODO Refactor in a module
resource "azurerm_cognitive_account" "translation" {
  name                = "trsl-azure-translation-${var.suffix}"
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  kind                = "TextTranslation"
  tags                = local.tags
  sku_name            = "F0"

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

resource "azurerm_cognitive_account" "language" {
  name                = "lang-azure-translation-${var.suffix}"
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  kind                = "TextAnalytics"
  tags                = local.tags
  sku_name            = "F0"

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

module "kv" {
  source                     = "./modules/kv"
  name                       = "kv-azure-translation-${var.suffix}"
  location                   = azurerm_resource_group.rg.location
  resource_group_name        = azurerm_resource_group.rg.name
  tenant_id                  = data.azurerm_client_config.current.tenant_id
  principal_id               = module.mi.principal_id
  soft_delete_retention_days = var.kv_soft_delete_retention_days
  sku                        = var.kv_sku
  tags                       = local.tags
  secrets = [
    {
      name  = "ConnectionStrings:ApplicationInsights"
      value = module.appi.connection_string
    },
    {
      name  = "ConnectionStrings:StorageAccount"
      value = module.st.connection_string
    },
    {
      name  = "ConnectionStrings:ServiceBus"
      value = azurerm_servicebus_namespace.servicebus.default_primary_connection_string
    },
    {
      name  = "TranslatorOptions:Key"
      value = azurerm_cognitive_account.translation.primary_access_key
    },
    {
      name  = "LanguageOptions:Key"
      value = azurerm_cognitive_account.language.primary_access_key
    },
  ]
}

module "appcs" {
  source                       = "./modules/appcs"
  name                         = "appcs-azure-translation-${var.suffix}"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = var.location
  sku                          = var.appcs_sku
  local_authentication_enabled = var.appcs_local_authentication_enabled
  public_network_access        = var.appcs_public_network_access
  soft_delete_retention_days   = var.appcs_soft_delete_retention_days
  identity_id                  = module.mi.id
  tags                         = local.tags
  secrets = [
    for secret in module.kv.secrets : {
      label     = var.appcs_label
      key       = secret.key
      reference = secret.reference
    }
  ]
  values = concat(
    [
      {
        label = var.appcs_label
        key   = "ServiceBusOptions:QueueName"
        value = azurerm_servicebus_queue.servicebus_queue.name
      },
      {
        label = var.appcs_label
        key   = "TableStorageOptions:TranslationsTableName"
        value = var.translations_table_name
      },
      {
        label = var.appcs_label
        key   = "TranslatorOptions:Region"
        value = azurerm_cognitive_account.translation.location
      },
      {
        label = var.appcs_label
        key   = "LanguageOptions:Endpoint"
        value = azurerm_cognitive_account.language.endpoint
      }
  ])
}

module "asp" {
  source              = "./modules/asp"
  name                = "asp-azure-translation-${var.suffix}"
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "B1" // Considerar P1v2 o superior para producción
  tags                = local.tags
}

module "app" {
  source                              = "./modules/app"
  name                                = "app-azure-translation-2-${var.suffix}"
  location                            = var.location
  resource_group_name                 = azurerm_resource_group.rg.name
  app_service_plan_id                 = module.asp.id
  app_command_line                    = "dotnet AzureTranslation.API.dll"
  https_only                          = true
  ftps_state                          = "Disabled"
  identity_type                       = "UserAssigned"
  identity_ids                        = [module.mi.id]
  app_configuration_connection_string = module.appcs.primary_read_key_connection_string
  tags                                = local.tags
  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY"        = module.appi.instrumentation_key
    "APPLICATIONINSIGHTS_CONNECTION_STRING" = module.appi.connection_string
    "WEBSITE_RUN_FROM_PACKAGE"              = 1
    "AZURE_CLIENT_ID"                       = module.mi.client_id

    //"ConnectionStrings__AppConfig"          = module.appcs.primary_read_key_connection_string
  }
}

module "func" {
  source               = "./modules/func"
  name                 = "func-azure-translation-2-${var.suffix}"
  location             = var.location
  resource_group_name  = azurerm_resource_group.rg.name
  app_service_plan_id  = module.asp.id
  storage_account_name = module.st.name
  storage_account_key  = module.st.primary_access_key
  identity_type        = "UserAssigned"
  identity_ids         = [module.mi.id]
  app_configuration_id = module.appcs.id
  tags                 = local.tags
  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY"        = module.appi.instrumentation_key
    "APPLICATIONINSIGHTS_CONNECTION_STRING" = module.appi.connection_string
    "WEBSITE_RUN_FROM_PACKAGE"              = 1
    "AZURE_CLIENT_ID"                       = module.mi.client_id
    "FUNCTIONS_EXTENSION_VERSION"           = "~4"
    "FUNCTIONS_WORKER_RUNTIME"              = "dotnet-isolated"

    "ConnectionStrings__AppConfig"  = module.appcs.primary_read_key_connection_string
    "AzureWebJobsStorage"           = module.st.connection_string
    "ServiceBusOptions__QueueName"  = azurerm_servicebus_queue.servicebus_queue.name
    "ConnectionStrings__ServiceBus" = azurerm_servicebus_namespace.servicebus.default_primary_connection_string
  }
}
