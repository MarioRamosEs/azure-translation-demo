output "id" {
  value       = azurerm_linux_function_app.func.id
  description = "The ID of the Function App"
}

output "name" {
  value       = azurerm_linux_function_app.func.name
  description = "The name of the Function App"
}

output "default_hostname" {
  value       = azurerm_linux_function_app.func.default_hostname
  description = "The default hostname of the Function App"
}
