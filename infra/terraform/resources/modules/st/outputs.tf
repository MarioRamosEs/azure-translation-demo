output "connection_string" {
  description = "The connection string for the storage account."
  value       = azurerm_storage_account.sa.primary_connection_string
  sensitive   = true
}

output "name" {
  description = "The name of the storage account."
  value       = azurerm_storage_account.sa.name
}

output "primary_access_key" {
  description = "The primary access key for the storage account."
  value       = azurerm_storage_account.sa.primary_access_key
  sensitive   = true
}