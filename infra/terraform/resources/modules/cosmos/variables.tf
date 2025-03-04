variable "development_mode" {
  description = "(Optional) Specifies whether this resource should be created with configurations suitable for develpment purposes. Default is `false`."
  type        = bool
  nullable    = false
  default     = false
}

variable "resource_group_name" {
  description = "(Required) Specifies the name of the resource group."
  type        = string
  nullable    = false
}

variable "location" {
  description = "(Required) Specifies the location of the Cosmos DB."
  type        = string
  nullable    = false
}

variable "name" {
  description = "(Required) Specifies the name of the Cosmos DB."
  type        = string
  nullable    = false
}

variable "automatic_failover_enabled" {
  description = "(Optional) Enable automatic failover for this Cosmos DB account. Defaults to `false`."
  type        = bool
  nullable    = false
  default     = false
}

variable "database_name" {
  description = "(Required) Specifies the name of the database in CosmosDB."
  type        = string
  nullable    = false
}

variable "container_name_translations" {
  description = "(Required) Specifies the name of the container in CosmosDB that will store the translations indexed by `id`."
  type        = string
  nullable    = false
}

variable "container_partition_key_paths" {
  description = "(Required) Specifies the partition key paths for the container."
  type        = list(string)
  nullable    = false
}

variable "local_authentication_disabled" {
  description = "(Optional) Disable local authentication and ensure only Manage Identities and Role-Based Access Control (RBAC) can be used exclusively for authentication. Can be set only when using the SQL API. Defaults to `false`."
  type        = bool
  nullable    = false
  default     = false
}

variable "throughput" {
  description = "(Required) Cosmos DB database throughput. This value should be equal to or greater than 400 and less than or equal to 1000000, in increments of 100. Default is 400."
  type        = number
  nullable    = false
  default     = 400

  validation {
    condition     = var.throughput >= 400 && var.throughput <= 1000000
    error_message = "Cosmos DB manual throughput should be equal to or greater than 400 and less than or equal to 1000000."
  }

  validation {
    condition     = var.throughput % 100 == 0
    error_message = "Cosmos DB throughput should be in increments of 100."
  }
}

variable "geo_locations" {
  description = "(Optional) A list of Cosmos DB locations. Each location has a `location` and `failover_priority`."
  nullable    = true
  type = list(object({
    location          = string
    failover_priority = number
  }))
  default = []
}

variable "identity_id" {
  description = "(Required) Specifies a the ID of a User Assigned Managed Identities to be associated with this resource."
  type        = string
  nullable    = false
}

variable "identity_service_principal_ids" {
  description = "(Optional) Specifies a list Service Principal IDs for proper role assigments associated with this resource."
  type        = list(string)
  nullable    = false
  default     = []
}

variable "tags" {
  description = "(Optional) Specifies the tags of this Cosmos DB resource."
  default     = {}
  nullable    = false
}
