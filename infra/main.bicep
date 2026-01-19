// infra/main.bicep

// === PARAMETERS ===
@description('The Azure region for all resources.')
param location string = resourceGroup().location

@description('The admin username for the jumphost VM and the database.')
param adminUsername string = 'azureuser'

@description('The SSH public key for the jumphost VM.')
@secure()
param sshPublicKey string

@description('The password for the PostgreSQL admin user.')
@secure()
param postgresAdminPassword string

@description('A globally unique name for the Azure Container Registry.')
// ZMIANA: Wpisujemy nazwę na sztywno, aby mieć pewność, że trafiamy w istniejący zasób
param acrName string = 'cryptolinkBRCh169606169600'

@description('The name of the AKS cluster provided by CI/CD pipeline.')
param aksClusterName string = 'aks-cryptolink'

// === MODULES ===

module networking './modules/networking.bicep' = {
  params: {
    location: location
    baseName: 'cryptolink'
  }
}

// Ten moduł musi być w pliku ./modules/acr.bicep (treść podałeś poprawnie niżej)
module acr './modules/acr.bicep' = {
  // Zmieniamy nazwę deploymentu dla pewności (omijanie historii)
  name: 'acr-deployment-fix-v3' 
  params: {
    // KLUCZOWE: Wymuszamy region North Europe, bo tam fizycznie jest Twój ACR
    location: 'northeurope'
    acrName: acrName
  }
}

module database './modules/database.bicep' = {
  params: {
    location: location
    subnetId: networking.outputs.databaseSubnetId
    postgresAdminUser: adminUsername
    postgresAdminPassword: postgresAdminPassword
  }
}

module aks './modules/aks.bicep' = {
  params: {
    location: location
    aksSubnetId: networking.outputs.aksSubnetId
    dnsPrefix: 'cryptolink-aks'
    aksClusterName: aksClusterName
  }
}

module management './modules/management.bicep' = {
  params: {
    location: location
    managementSubnetId: networking.outputs.managementSubnetId
    bastionSubnetId: networking.outputs.bastionSubnetId
    adminUsername: adminUsername
    sshPublicKey: sshPublicKey
  }
}

// === OUTPUTS ===

@description('The login server of the Azure Container Registry.')
output acrLoginServer string = acr.outputs.loginServer

@description('The name of the Azure Container Registry resource.')
// ZMIANA: Pobieramy nazwę dynamicznie z modułu (clean code)
output acrName string = acr.outputs.name

@description('The name of the AKS cluster.')
output aksClusterName string = aks.outputs.clusterName

@description('The FQDN of the PostgreSQL server.')
output databaseServerFqdn string = database.outputs.serverFqdn
