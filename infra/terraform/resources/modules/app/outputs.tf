output "id" {
  value       = azurerm_linux_web_app.app.id
  description = "The ID of the App Service"
}

output "name" {
  value       = azurerm_linux_web_app.app.name
  description = "The name of the App Service"
}

output "default_hostname" {
  value       = azurerm_linux_web_app.app.default_hostname
  description = "The default hostname of the App Service"
}
