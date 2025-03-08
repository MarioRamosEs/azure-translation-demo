output "connection_string" {
  description = "The connection string for the storage account."
  value       = azurerm_storage_account.sa.primary_connection_string
  sensitive   = true
}

output "name" {
  description = "The name of the storage account."
  value       = azurerm_storage_account.sa.name
}