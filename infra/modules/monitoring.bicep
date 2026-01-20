// infra/modules/monitoring.bicep
// Azure Monitor setup dla aplikacji CryptoLink
// Monitoruje: CPU, Memory, Disk, Network, Logs

@description('The Azure region for all resources.')
param location string

@description('The name of the Log Analytics workspace.')
param logAnalyticsWorkspaceName string = 'cryptolink-law-${uniqueString(resourceGroup().id)}'

// === LOG ANALYTICS WORKSPACE ===
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-12-01-preview' = {
  name: logAnalyticsWorkspaceName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
  }
}

// NOTE: Diagnostic settings configured in aks.bicep where we have resource reference
// NOTE: Metric alerts configured in aks.bicep where we have resource reference

// === OUTPUTS ===
@description('The resource ID of the Log Analytics workspace.')
output logAnalyticsWorkspaceId string = logAnalyticsWorkspace.id

@description('The name of the Log Analytics workspace.')
output logAnalyticsWorkspaceName string = logAnalyticsWorkspace.name
