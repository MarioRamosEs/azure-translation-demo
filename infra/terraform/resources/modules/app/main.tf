resource "azurerm_linux_web_app" "app" {
  name                      = var.name
  location                  = var.location
  resource_group_name       = var.resource_group_name
  service_plan_id           = var.app_service_plan_id
  https_only                = var.https_only
  app_settings              = var.app_settings
  
  site_config {
    always_on               = var.always_on
    ftps_state              = var.ftps_state
    application_stack {
      dotnet_version        = element(split("|", var.linux_fx_version), 1)
    }
    app_command_line        = var.app_command_line
    minimum_tls_version     = "1.2"
  }

  identity {
    type         = var.identity_type
    identity_ids = var.identity_ids
  }
  
  tags = var.tags

  lifecycle {
    ignore_changes = [
      tags,
      app_settings["WEBSITE_RUN_FROM_PACKAGE"]
    ]
  }
}
