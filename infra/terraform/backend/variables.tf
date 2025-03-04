variable "subscription_id" {
  description = "(Required) The subscription ID which should be used for deployments. This value is required when performing a `plan`. `apply` or `destroy` operation. Since version 4.0 of the Azure Provider (`azurerm`), it's now required to specify the Azure Subscription ID when configuring a provider instance in the configuration. More info: https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/guides/4.0-upgrade-guide#specifying-subscription-id-is-now-mandatory"
  type        = string
  nullable    = false
}

variable "location" {
  description = "(Required) Specifies the location for the resource group and most of its resources. Defaults to `francecentral`"
  type        = string
  nullable    = false
  default     = "francecentral"
}

variable "resource_group_name" {
  description = "(Required) Specifies the name of the resource group for the Terraform backend."
  type        = string
  nullable    = false
  default     = "rg-azure-translation-tfstate"
}

variable "storage_account_name" {
  description = "(Required) Specifies the name of the Azure Storage Account for the Terraform backend."
  type        = string
  nullable    = false
  default     = "staztranslationtfstate"
}

variable "container_name" {
  description = "(Required) Specifies the name of the Blob Storage container for the Terraform backend."
  type        = string
  nullable    = false
  default     = "azure-translation-tfstate"
}

variable "tags" {
  description = "(Optional) Specifies tags for all the resources."
  nullable    = false
  default = {
    createdUsing = "Terraform"
  }
}
