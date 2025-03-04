output "resource_group_name" {
  value       = azurerm_resource_group.rg.name
  description = "The name of the resource group."
}

output "app_configuration_connection_string" {
  description = "The read connection string to the Azure App Configuration."
  value       = module.appcs.primary_read_key_connection_string
}
