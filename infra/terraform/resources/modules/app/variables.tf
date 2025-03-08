variable "name" {
  type        = string
  description = "The name of the App Service"
}

variable "location" {
  type        = string
  description = "The Azure region where the App Service will be created"
}

variable "resource_group_name" {
  type        = string
  description = "The name of the resource group in which to create the App Service"
}

variable "app_service_plan_id" {
  type        = string
  description = "The ID of the App Service Plan"
}

variable "app_settings" {
  type        = map(string)
  description = "A map of app settings for the App Service"
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

variable "always_on" {
  type        = bool
  description = "Whether the app is always on"
  default     = true
}

variable "identity_type" {
  type        = string
  description = "The type of identity to use (SystemAssigned or UserAssigned)"
  default     = "UserAssigned"
}

variable "identity_ids" {
  type        = list(string)
  description = "A list of User Assigned Identity IDs to be used by the App Service"
  default     = []
}

variable "app_command_line" {
  type        = string
  description = "The app command line to launch"
  nullable    = false
}

variable "app_configuration_connection_string" {
  type        = string
  description = "The connection string for the App Configuration"
  nullable    = false
}

variable "tags" {
  type        = map(string)
  description = "A mapping of tags to assign to the resource"
  default     = {}
}
