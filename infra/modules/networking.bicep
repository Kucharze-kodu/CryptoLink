// infra/modules/networking.bicep
@description('The Azure region for all resources.')
param location string

@description('The base name for network resources.')
param baseName string

resource vnet 'Microsoft.Network/virtualNetworks@2024-05-01' = {
  name: '${baseName}-vnet'
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [ '10.0.0.0/16' ]
    }
    subnets: [
      {
        name: 'aks-subnet'
        properties: { addressPrefix: '10.0.1.0/24' }
      }
      {
        name: 'database-subnet'
        properties: {
          addressPrefix: '10.0.2.0/24'
          delegations: [ { name: 'db-delegation', properties: { serviceName: 'Microsoft.DBforPostgreSQL/flexibleServers' } } ]
          privateEndpointNetworkPolicies: 'Disabled'
        }
      }
      {
        name: 'management-subnet'
        properties: { addressPrefix: '10.0.3.0/24' }
      }
      {
        name: 'AzureBastionSubnet'
        properties: { addressPrefix: '10.0.4.0/26' }
      }
    ]
  }
}

// --- OUTPUTS ---
@description('Resource ID of the main VNet.')
output vnetId string = vnet.id

@description('Name of the main VNet.')
output vnetName string = vnet.name

@description('Resource ID of the AKS subnet.')
output aksSubnetId string = '${vnet.id}/subnets/aks-subnet'

@description('Resource ID of the Database subnet.')
output databaseSubnetId string = '${vnet.id}/subnets/database-subnet'

@description('Resource ID of the Management (Jumphost) subnet.')
output managementSubnetId string = '${vnet.id}/subnets/management-subnet'

@description('Resource ID of the Bastion subnet.')
output bastionSubnetId string = '${vnet.id}/subnets/AzureBastionSubnet'
