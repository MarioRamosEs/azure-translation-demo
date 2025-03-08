variable "name" {
  type        = string
  description = "The name of the Function App"
}

variable "location" {
  type        = string
  description = "The Azure region where the Function App will be created"
}

variable "resource_group_name" {
  type        = string
  description = "The name of the resource group in which to create the Function App"
}

variable "app_service_plan_id" {
  type        = string
  description = "The ID of the App Service Plan"
}

variable "storage_account_name" {
  type        = string
  description = "The name of the storage account for the Function App"
}

variable "storage_account_key" {
  type        = string
  description = "The access key of the storage account for the Function App"
}

variable "app_settings" {
  type        = map(string)
  description = "A map of app settings for the Function App"
  default     = {}
}

variable "https_only" {
  type        = bool
  description = "Whether HTTPS Only is enabled"
  default     = true
}

variable "ftps_state" {
  type        = string
  description = "The FTPs state"
  default     = "Disabled"
}

variable "identity_type" {
  type        = string
  description = "The type of identity to use (SystemAssigned or UserAssigned)"
  default     = "UserAssigned"
}

variable "identity_ids" {
  type        = list(string)
  description = "A list of User Assigned Identity IDs to be used by the Function App"
  default     = []
}

variable "app_configuration_id" {
  type        = string
  description = "The ID of the App Configuration"
}

variable "os_type" {
  type        = string
  description = "The OS type for the Function App"
  default     = "linux"
}

variable "tags" {
  type        = map(string)
  description = "A mapping of tags to assign to the resource"
  default     = {}
}
