resource "azurerm_cosmosdb_account" "db" {
  name                          = var.name
  location                      = var.location
  resource_group_name           = var.resource_group_name
  offer_type                    = "Standard" # Specifies the `Offer Type` to use for this CosmosDB Account; currently, this can only be set to `Standard`. More info: https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/cosmosdb_account#offer_type
  kind                          = "GlobalDocumentDB"
  automatic_failover_enabled    = var.automatic_failover_enabled
  local_authentication_disabled = var.local_authentication_disabled
  tags                          = var.tags

  consistency_policy {
    consistency_level       = "BoundedStaleness"
    max_interval_in_seconds = 300
    max_staleness_prefix    = 100000
  }

  geo_location {
    location          = var.location
    failover_priority = 0
  }

  dynamic "geo_location" {
    for_each = var.geo_locations
    content {
      location          = geo_location.value.location
      failover_priority = geo_location.value.failover_priority
    }
  }

  identity {
    type         = "UserAssigned"
    identity_ids = [var.identity_id]
  }

  lifecycle {
    ignore_changes = [
      tags,
    ]
  }
}

resource "azurerm_cosmosdb_sql_database" "database" {
  name                = var.database_name
  resource_group_name = var.resource_group_name
  account_name        = azurerm_cosmosdb_account.db.name
  throughput          = var.throughput
}

resource "azurerm_cosmosdb_sql_container" "container_translations" {
  name                  = var.container_name_translations
  resource_group_name   = var.resource_group_name
  account_name          = azurerm_cosmosdb_account.db.name
  database_name         = azurerm_cosmosdb_sql_database.database.name
  partition_key_paths   = var.container_partition_key_paths
  partition_key_version = 1
  throughput            = var.throughput

  indexing_policy {
    indexing_mode = "consistent"

    included_path {
      path = "/*"
    }

    included_path {
      path = "/included/?"
    }

    excluded_path {
      path = "/excluded/?"
    }
  }
}

# Set role assignment for Cosmos DB

data "azurerm_cosmosdb_sql_role_definition" "built_in_data_contributor" {
  resource_group_name = azurerm_cosmosdb_account.db.resource_group_name
  account_name        = azurerm_cosmosdb_account.db.name
  role_definition_id  = "00000000-0000-0000-0000-000000000002" # Reference: https://learn.microsoft.com/en-us/azure/cosmos-db/how-to-setup-rbac#built-in-role-definitions
}

resource "azurerm_cosmosdb_sql_role_assignment" "built_in_data_contributor_service_principals_role_assignment" {
  for_each = { for i, val in var.identity_service_principal_ids : i => val }

  resource_group_name = azurerm_cosmosdb_account.db.resource_group_name
  account_name        = azurerm_cosmosdb_account.db.name
  role_definition_id  = data.azurerm_cosmosdb_sql_role_definition.built_in_data_contributor.id
  principal_id        = each.value
  scope               = azurerm_cosmosdb_account.db.id

}

# Development mode configurations

data "azurerm_client_config" "current" {
  count = var.development_mode ? 1 : 0
}

resource "azurerm_cosmosdb_sql_role_assignment" "built_in_data_contributor_user_principals_role_assignment" {
  count = var.development_mode ? 1 : 0

  resource_group_name = azurerm_cosmosdb_account.db.resource_group_name
  account_name        = azurerm_cosmosdb_account.db.name
  role_definition_id  = data.azurerm_cosmosdb_sql_role_definition.built_in_data_contributor.id
  principal_id        = data.azurerm_client_config.current.0.object_id
  scope               = azurerm_cosmosdb_account.db.id
}
