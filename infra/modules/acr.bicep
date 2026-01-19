// infra/modules/acr.bicep

@description('The Azure region for the ACR.')
param location string

@description('A globally unique name for the Container Registry.')
param acrName string

// Definiujemy zasób w pełni - to pozwala na jego stworzenie LUB aktualizację
resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  name: acrName
  location: location
  sku: {
    name: 'Standard' // Standard jest bezpieczniejszy i wspiera Identity
  }
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    adminUserEnabled: false // Zgodnie z Twoją polityką bezpieczeństwa
    publicNetworkAccess: 'Enabled'
    zoneRedundancy: 'Disabled'
  }
}

// ---- OUTPUTS ----
@description('The name of the ACR login server.')
output loginServer string = acr.properties.loginServer

@description('The name of the ACR resource.')
output name string = acr.name

@description('The principal ID of the ACR for role assignments.')
output principalId string = acr.identity.principalId
