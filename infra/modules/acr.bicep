@description('The Azure region for all resources.')
param location string

@description('A globally unique name for the Container Registry.')
param acrName string

resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  name: acrName
  location: location
  sku: {
    // ZMIANA: Standard wspiera Identity natywnie i stabilnie.
    // Jeśli musisz zostać przy Basic, usuń sekcję 'identity' i output 'principalId',
    // ale wtedy integracja z AKS będzie trudniejsza.
    name: 'Standard' 
  }
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    // ZMIANA: Wyłączamy admina zgodnie z Twoją polityką bezpieczeństwa
    adminUserEnabled: false 
  }
}

// ---- OUTPUTS ----
@description('The name of the ACR login server.')
output loginServer string = acr.properties.loginServer

@description('The name of the ACR resource.')
output name string = acr.name

@description('The principal ID of the ACR for role assignments.')
output principalId string = acr.identity.principalId
