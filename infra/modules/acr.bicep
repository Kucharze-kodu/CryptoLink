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
  // NAPRAWA: Włączamy tożsamość zarządzaną, aby output 'principalId' zadziałał
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
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
