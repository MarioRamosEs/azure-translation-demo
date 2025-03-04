locals {
  terraform_init_command = "terraform init -backend-config=\"resource_group_name=${azurerm_resource_group.rg.name}\" -backend-config=\"storage_account_name=${azurerm_storage_account.st.name}\" -backend-config=\"container_name=${azurerm_storage_container.container.name}\" -backend-config=\"key=<blob key name>.tfstate\""
  terraform_message      = "Initialize a Terraform depoloyment using the following command as example: ${local.terraform_init_command}"
}

output "resource_group_name" {
  description = "The name of the resource group containing the Storage Accout for the remote Terraform state management."
  value       = azurerm_resource_group.rg.name
}

output "storage_account_name" {
  description = "The name of the Storage Account for the remote Terraform state management."
  value       = azurerm_storage_account.st.name
}

output "container_name" {
  description = "The name of the Storage Container for the remote Terraform state management."
  value       = azurerm_storage_container.container.name
}

output "terraform_init_backend" {
  description = "Shows an example of the Terraform command to initialize a deployment with this backend."
  value       = local.terraform_message
}
