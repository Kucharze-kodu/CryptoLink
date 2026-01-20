// infra/modules/monitoring.bicep
// Azure Monitor setup dla aplikacji CryptoLink
// Monitoruje: CPU, Memory, Disk, Network, Logs

@description('The Azure region for all resources.')
param location string

@description('The name of the AKS cluster.')
param aksClusterName string

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

// === METRICS ALERTS ===

// Alert: High CPU usage
resource cpuAlert 'Microsoft.Insights/metricAlerts@2018-03-01' = {
  name: '${aksClusterName}-high-cpu'
  location: 'global'
  properties: {
    description: 'Alert when node CPU usage exceeds 80%'
    severity: 2
    enabled: true
    scopes: [
      aksClusterResourceId
    ]
    evaluationFrequency: 'PT5M'
    windowSize: 'PT15M'
    criteria: {
      'odata.type': 'Microsoft.Azure.Monitor.MultipleResourceMultipleMetricCriteria'
      allOf: [
        {
          name: 'cpuUsage'
          criterionType: 'StaticThresholdCriterion'
          metricName: 'cpuUsagePercentage'
          operator: 'GreaterThan'
          threshold: 80
          timeAggregation: 'Average'
          dimensions: []
        }
      ]
    }
  }
}

// Alert: Low available memory
resource memoryAlert 'Microsoft.Insights/metricAlerts@2018-03-01' = {
  name: '${aksClusterName}-low-memory'
  location: 'global'
  properties: {
    description: 'Alert when node memory usage exceeds 85%'
    severity: 2
    enabled: true
    scopes: [
      aksClusterResourceId
    ]
    evaluationFrequency: 'PT5M'
    windowSize: 'PT15M'
    criteria: {
      'odata.type': 'Microsoft.Azure.Monitor.MultipleResourceMultipleMetricCriteria'
      allOf: [
        {
          name: 'memoryUsage'
          criterionType: 'StaticThresholdCriterion'
          metricName: 'memoryUsagePercentage'
          operator: 'GreaterThan'
          threshold: 85
          timeAggregation: 'Average'
          dimensions: []
        }
      ]
    }
  }
}

// === OUTPUTS ===
@description('The resource ID of the Log Analytics workspace.')
output logAnalyticsWorkspaceId string = logAnalyticsWorkspace.id

@description('The name of the Log Analytics workspace.')
output logAnalyticsWorkspaceName string = logAnalyticsWorkspace.name
