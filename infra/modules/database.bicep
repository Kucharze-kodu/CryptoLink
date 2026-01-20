// infra/modules/database.bicep
@description('The Azure region for all resources.')
param location string

@description('The resource ID of the subnet for the database.')
param subnetId string

@description('The admin username for the PostgreSQL server.')
param postgresAdminUser string

@description('The password for the PostgreSQL admin user.')
@secure()
param postgresAdminPassword string

var postgresServerName = 'cryptolink-pg-server-${uniqueString(resourceGroup().id)}'
var databaseName = 'cryptolink'

// Tworzymy prywatną strefę DNS najpierw
resource privateDnsZone 'Microsoft.Network/privateDnsZones@2020-06-01' = {
  name: 'privatelink.postgres.database.azure.com'
  location: 'global'
}

resource postgresServer 'Microsoft.DBforPostgreSQL/flexibleServers@2023-03-01-preview' = {
  name: postgresServerName
  location: location
  sku: {
    name: 'Standard_B1ms' // Mały, oszczędny SKU
    tier: 'Burstable'
  }
  properties: {
    administratorLogin: postgresAdminUser
    administratorLoginPassword: postgresAdminPassword
    version: '16'
    network: {
      delegatedSubnetResourceId: subnetId
      privateDnsZoneArmResourceId: privateDnsZone.id
    }
    storage: { 
      storageSizeGB: 32
    }
    backup: { 
      backupRetentionDays: 30
      geoRedundantBackup: 'Enabled'
    }
    dataEncryption: {
      type: 'SystemManaged'
    }
    highAvailability: {
      mode: 'Disabled'
    }
  }
}

resource vnetLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2020-06-01' = {
  parent: privateDnsZone
  name: '${postgresServerName}-dnslink'
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: resourceId(subscription().subscriptionId, resourceGroup().name, 'Microsoft.Network/virtualNetworks', split(subnetId, '/')[8])
    }
  }
}

resource db 'Microsoft.DBforPostgreSQL/flexibleServers/databases@2023-03-01-preview' = {
  parent: postgresServer
  name: databaseName
}

// --- OUTPUTS ---
@description('The fully qualified domain name of the PostgreSQL server.')
output serverFqdn string = postgresServer.properties.fullyQualifiedDomainName

@description('The name of the database.')
output dbName string = db.name
