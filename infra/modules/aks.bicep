// infra/modules/aks.bicep
@description('The Azure region for all resources.')
param location string

@description('The resource ID of the subnet for the AKS cluster nodes.')
param aksSubnetId string

@description('The name of the AKS cluster.')
param aksClusterName string

@description('DNS prefix for the AKS cluster FQDN.')
param dnsPrefix string = 'cryptolink-aks'

resource aksCluster 'Microsoft.ContainerService/managedClusters@2023-09-01' = {
  name: aksClusterName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    dnsPrefix: dnsPrefix
    // USUNIĘTO kubernetesVersion - Azure dobierze domyślną, darmową wersję
    agentPoolProfiles: [
      {
        name: 'nodepool1'
        count: 1
        vmSize: 'Standard_B2s'
        osType: 'Linux'
        mode: 'System'
        vnetSubnetID: aksSubnetId
      }
    ]
    networkProfile: {
      networkPlugin: 'azure'
      networkPolicy: 'azure'
      serviceCidr: '172.16.0.0/16'
      dnsServiceIP: '172.16.0.10'
      // USUNIĘTO dockerBridgeCidr - powodował błędy i nie jest już wymagany
    }
  }
}

// --- OUTPUTS ---
@description('The name of the AKS cluster.')
output clusterName string = aksCluster.name

@description('The resource ID of the AKS cluster.')
output clusterResourceId string = aksCluster.id

@description('The principal ID of the AKS cluster managed identity.')
output aksPrincipalId string = aksCluster.identity.principalId
