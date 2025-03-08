variable "name" {
  type        = string
  description = "The name of the App Service Plan"
}

variable "location" {
  type        = string
  description = "The Azure region where the App Service Plan will be created"
}

variable "resource_group_name" {
  type        = string
  description = "The name of the resource group in which to create the App Service Plan"
}

variable "os_type" {
  type        = string
  description = "The OS type for the App Service Plan (Linux or Windows)"
  default     = "Linux"
}

variable "sku_name" {
  type        = string
  description = "The SKU name for the App Service Plan"
}

variable "tags" {
  type        = map(string)
  description = "A mapping of tags to assign to the resource"
  default     = {}
}
