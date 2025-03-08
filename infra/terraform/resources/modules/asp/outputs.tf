output "id" {
  value       = azurerm_service_plan.asp.id
  description = "The ID of the App Service Plan"
}

output "name" {
  value       = azurerm_service_plan.asp.name
  description = "The name of the App Service Plan"
}
