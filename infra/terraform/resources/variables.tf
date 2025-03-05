## ---- COMMON VARIABLES & RESOURCE GROUP ---- ##

variable "subscription_id" {
  description = "(Required) The subscription ID which should be used for deployments. This value is required when performing a `plan`. `apply` or `destroy` operation. Since version 4.0 of the Azure Provider (`azurerm`), it's now required to specify the Azure Subscription ID when configuring a provider instance in the configuration. More info: https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/guides/4.0-upgrade-guide#specifying-subscription-id-is-now-mandatory"
  type        = string
  nullable    = false
}

variable "suffix" {
  description = "(Optional) A suffix for the name of the resource group and its resources. It can only contain letters (any case) and numbers. Defaults to `null`."
  type        = string
  nullable    = true
  default     = null

  validation {
    condition     = can(regex("^[a-zA-Z0-9]*$", var.suffix))
    error_message = "The suffix can only contain letters (any case) and numbers."
  }
}

variable "location" {
  description = "(Required) Specifies the location for the resource group and most of its resources. Defaults to `francecentral`"
  type        = string
  nullable    = false
  default     = "francecentral"
}

variable "tags" {
  description = "(Optional) Specifies tags for all the resources."
  nullable    = false
  default = {
    createdUsing = "Terraform"
  }
}

/* RESOURCE GROUP */

variable "resource_group_name" {
  description = "(Required) The name of the resource group."
  type        = string
  nullable    = false
  default     = "rg-azure-translation"
}

## ---- SPECIFIC RESOURCES & SERVICES ---- ##

/* MANAGED IDENTITY */

variable "managed_identity_name" {
  description = "(Required) Specifies the name of the Managed Identity."
  type        = string
  nullable    = false
  default     = "id-azure-translation"
}

/* APP CONFIGURATION */

variable "appcs_name" {
  description = "(Required) Specifies the name of the Azure App Configuration."
  type        = string
  nullable    = false
  default     = "appcs-azure-translation"
}

variable "appcs_sku" {
  description = "(Required) Specifies the SKU of the Azure App Configuration. Possible values are `free` and `standard`. Defaults to `standard`."
  type        = string
  nullable    = false
  default     = "standard"

  validation {
    condition     = contains(["free", "standard"], var.appcs_sku)
    error_message = "The Azure App Configuration SKU is incorrect. Possible values are `free` and `standard`."
  }
}

variable "appcs_local_authentication_enabled" {
  description = "(Optional) Specifies whether or not local authentication should be enabled for this Azure App Configuration resource. Defaults to `true`."
  type        = bool
  nullable    = false
  default     = true
}

variable "appcs_soft_delete_retention_days" {
  description = "(Optional) The number of days that items should be retained for once soft-deleted. This field only works for standard sku. This value can be between 1 and 7 days. Defaults to 7. Changing this forces a new resource to be created."
  type        = number
  nullable    = false
  default     = 7

  validation {
    condition     = var.appcs_soft_delete_retention_days >= 1 && var.appcs_soft_delete_retention_days <= 7
    error_message = "The soft delete retention days must be between 1 and 7 days and only works for the standard SKU."
  }
}

variable "appcs_public_network_access" {
  description = "(Optional) The Public Network Access setting of the App Configuration. Possible values are `Enabled` and `Disabled`. Defaults to `Enabled`."
  type        = string
  nullable    = false
  default     = "Enabled"

  validation {
    condition     = contains(["Enabled", "Disabled"], var.appcs_public_network_access)
    error_message = "The Public Network Access setting of the App Configuration is incorrect. Possible values are `Enabled` and `Disabled`."
  }
}

variable "appcs_label" {
  description = "(Optional) Specifies the label to use for common values in the Azure App Configuration."
  type        = string
  nullable    = true
  default     = null
}

/* APPLICATION INSIHGTS */

variable "app_insights_name" {
  description = "(Required) Specifies the name of the Application Insights."
  type        = string
  nullable    = false
  default     = "appi-azure-translation"
}

/* STORAGE ACCOUNT */

variable "storage_account_name" {
  description = "(Required) Specifies the name of the Azure Virtual Network."
  type        = string
  nullable    = false
  default     = "staztranslation"
}

variable "storage_account_tier" {
  description = "(Required) Defines the Tier to use for this storage account. Valid options are `Standard` and `Premium`. Changing this forces a new resource to be created. Defaults to `Standard`."
  type        = string
  nullable    = false
  default     = "Standard"

  validation {
    condition     = can(regex("^(Standard|Premium)$", var.storage_account_tier))
    error_message = "Invalid account_tier. Valid options are `Standard` and `Premium`."
  }
}

variable "storage_account_replication_type" {
  description = "(Required) Defines the type of replication to use for this storage account. Valid options are `LRS`, `GRS`, `RAGRS`, `ZRS`, `GZRS` and `RAGZRS`. Changing this forces a new resource to be created when types `LRS`, `GRS` and `RAGRS` are changed to `ZRS`, `GZRS` or `RAGZRS` and vice versa. Defaults to `LRS`."
  type        = string
  nullable    = false
  default     = "LRS"

  validation {
    condition     = can(regex("^(LRS|GRS|RAGRS|ZRS|GZRS|RAGZRS)$", var.storage_account_replication_type))
    error_message = "Invalid account_replication_type. Valid options are `LRS`, `GRS`, `RAGRS`, `ZRS`, `GZRS` and `RAGZRS`."
  }
}

/* KEY VAULT */

variable "kv_name" {
  description = "(Required) Specifies the name of the Key Vault."
  type        = string
  nullable    = false
  default     = "kv-azure-translation"
}

variable "kv_soft_delete_retention_days" {
  description = "(Optional) The number of days that items should be retained for once soft-deleted. This value can be between 7 and 90 (the default) days. Default is 7 days."
  type        = number
  nullable    = false
  default     = 7

  validation {
    condition     = var.kv_soft_delete_retention_days >= 7 && var.kv_soft_delete_retention_days <= 90
    error_message = "The number of days that items should be retained for once soft-deleted should be between 7 and 90 days."
  }
}

variable "kv_sku" {
  description = "(Required) The SKU name of the Key Vault. Possible values are `Standard` and `Premium`. Default is `Standard`."
  type        = string
  nullable    = true
  default     = "Standard"

  validation {
    condition     = var.kv_sku == "Standard" || var.kv_sku == "Premium"
    error_message = "The SKU name of the Key Vault must be either `Standard` or `Premium`."
  }
}

/* LOG ANALYTICS WORKSPACE */

variable "log_analytics_workspace_name" {
  description = "(Required) Specifies the name of the Log Analytics Workspace."
  type        = string
  nullable    = false
  default     = "log-azure-translation"
}