data "azurerm_client_config" "current" {}

data "azurerm_subscription" "current" {}

locals {
  name_app_insights            = "${var.app_insights_name}-${var.suffix}"
  name_appcs                   = "${var.appcs_name}-${var.suffix}"
  name_kv                      = "${var.kv_name}-${var.suffix}"
  name_log_analytics_workspace = "${var.log_analytics_workspace_name}-${var.suffix}"
  name_resource_group          = "${var.resource_group_name}-${var.suffix}"
  name_manage_identity         = "${var.managed_identity_name}-${var.suffix}"
  name_storage_account         = "${var.storage_account_name}${var.suffix}"
  name_servicebus_namespace    = "sbns-azure-translation-${var.suffix}"   // TODO refactor
  name_servicebus_queue        = "sbq-translation-requests-${var.suffix}" // TODO refactor ,  we dont need the sufix here
  name_storage_table           = "Translations"

  tags = merge(var.tags, {
    createdAt   = "${formatdate("YYYY-MM-DD hh:mm:ss", timestamp())} UTC"
    createdWith = "Terraform"
    suffix      = var.suffix
  })
}

resource "azurerm_resource_group" "rg" {
  name     = local.name_resource_group
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
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  name                = local.name_manage_identity
  tags                = local.tags
}

module "log" {
  source              = "./modules/log"
  name                = local.name_log_analytics_workspace
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  tags                = local.tags
}

module "appi" {
  source                     = "./modules/appi"
  name                       = local.name_app_insights
  location                   = var.location
  resource_group_name        = azurerm_resource_group.rg.name
  log_analytics_workspace_id = module.log.id
  tags                       = local.tags
}

module "st" {
  source                   = "./modules/st"
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = var.location
  name                     = local.name_storage_account
  account_tier             = var.storage_account_tier
  account_replication_type = var.storage_account_replication_type
  identity_id              = module.mi.id
  tags                     = local.tags
}

resource "azurerm_storage_table" "translations" {
  name                 = local.name_storage_table
  storage_account_name = local.name_storage_account
}

// TODO Refactor in a module
resource "azurerm_servicebus_namespace" "servicebus" {
  name                = local.name_servicebus_namespace
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "Basic"

  tags = local.tags

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

resource "azurerm_servicebus_queue" "servicebus_queue" {
  name         = local.name_servicebus_queue
  namespace_id = azurerm_servicebus_namespace.servicebus.id
}

module "kv" {
  source                     = "./modules/kv"
  location                   = azurerm_resource_group.rg.location
  resource_group_name        = azurerm_resource_group.rg.name
  name                       = local.name_kv
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
  ]
}

module "appcs" {
  source                       = "./modules/appcs"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = var.location
  name                         = local.name_appcs
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
        key   = "ServiceBus:QueueName"
        value = azurerm_servicebus_queue.servicebus_queue.name
      },
  ])
}


