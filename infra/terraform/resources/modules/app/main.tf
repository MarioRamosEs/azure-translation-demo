resource "azurerm_linux_web_app" "app" {
  name                = var.name
  location            = var.location
  resource_group_name = var.resource_group_name
  service_plan_id     = var.app_service_plan_id
  https_only          = var.https_only
  app_settings        = var.app_settings
  tags                = var.tags

  connection_string {
    name  = "AppConfig"
    type  = "Custom"
    value = var.app_configuration_connection_string
  }

  site_config {
    always_on  = var.always_on
    ftps_state = var.ftps_state
    application_stack {
      dotnet_version = "9.0"
    }
    app_command_line    = var.app_command_line
    minimum_tls_version = "1.2"
  }

  identity {
    type         = var.identity_type
    identity_ids = var.identity_ids
  }

  logs {
    detailed_error_messages = true
    failed_request_tracing  = false

    application_logs {
      file_system_level = "Information"
    }

    http_logs {
      file_system {
        retention_in_days = 7
        retention_in_mb   = 35
      }
    }
  }

  lifecycle {
    ignore_changes = [
      tags,
    ]
  }
}
