resource "azurerm_linux_function_app" "func" {
  name                       = var.name
  location                   = var.location
  resource_group_name        = var.resource_group_name
  service_plan_id            = var.app_service_plan_id
  storage_account_name       = var.storage_account_name
  storage_account_access_key = var.storage_account_key
  https_only                 = var.https_only
  app_settings               = var.app_settings
  tags                       = var.tags

  site_config {
    ftps_state                             = var.ftps_state
    always_on                              = true
    application_insights_key               = lookup(var.app_settings, "APPINSIGHTS_INSTRUMENTATIONKEY", null)
    application_insights_connection_string = lookup(var.app_settings, "APPLICATIONINSIGHTS_CONNECTION_STRING", null)
    minimum_tls_version                    = "1.2"

    application_stack {
      dotnet_version              = "9.0"
      use_dotnet_isolated_runtime = true
    }

    app_service_logs {
      disk_quota_mb         = 35
      retention_period_days = 7
    }
  }

  connection_string {
    name  = "AppConfig"
    type  = "Custom"
    value = var.app_configuration_connection_string
  }

  connection_string {
    name  = "ServiceBus"
    type  = "ServiceBus"
    value = var.service_bus_connection_string
  }

  identity {
    type         = var.identity_type
    identity_ids = var.identity_ids
  }

  lifecycle {
    ignore_changes = [
      tags,
      app_settings["WEBSITE_RUN_FROM_PACKAGE"]
    ]
  }
}
