// infra/modules/acr.bicep
@description('The Azure region for all resources.')
param location string

@description('A globally unique name for the Container Registry.')
param acrName string

resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  name: acrName
  location: location
  sku: {
    name: 'Basic'
  }
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    // UWAGA: To włączy admina. Jeśli polityka zabrania, zmień na false.
    adminUserEnabled: true 
  }
}

// ---- OUTPUTS ----
@description('The name of the ACR login server.')
output loginServer string = acr.properties.loginServer

@description('The name of the ACR resource.')
output name string = acr.name

@description('The principal ID of the ACR for role assignments.')
output principalId string = acr.identity.principalId
