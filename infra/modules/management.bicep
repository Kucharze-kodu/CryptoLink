// infra/modules/management.bicep
@description('The Azure region for all resources.')
param location string

@description('The resource ID of the subnet for the Jumphost VM.')
param managementSubnetId string

@description('The resource ID of the subnet for Azure Bastion.')
param bastionSubnetId string

@description('The admin username for the Jumphost VM.')
param adminUsername string

@description('The SSH public key for the Jumphost VM.')
@secure()
param sshPublicKey string

var jumphostVmName = 'jumphost-vm'

// Publiczny IP dla Bastionu
resource bastionPip 'Microsoft.Network/publicIPAddresses@2023-05-01' = {
  name: 'jumphost-bastion-pip'
  location: location
  sku: { name: 'Standard' }
  properties: { publicIPAllocationMethod: 'Static' }
}

// Bastion
resource bastion 'Microsoft.Network/bastionHosts@2023-05-01' = {
  name: 'jumphost-bastion'
  location: location
  sku: { name: 'Basic' }
  properties: {
    ipConfigurations: [
      {
        name: 'bastion-ipconf'
        properties: {
          subnet: { id: bastionSubnetId }
          publicIPAddress: { id: bastionPip.id }
        }
      }
    ]
  }
}

// Karta sieciowa dla Jumphosta
resource jumphostNic 'Microsoft.Network/networkInterfaces@2023-05-01' = {
  name: '${jumphostVmName}-nic'
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig1'
        properties: {
          privateIPAllocationMethod: 'Dynamic'
          subnet: { id: managementSubnetId }
        }
      }
    ]
  }
}

// Maszyna wirtualna Jumphost
resource jumphostVm 'Microsoft.Compute/virtualMachines@2023-07-01' = {
  name: jumphostVmName
  location: location
  properties: {
    hardwareProfile: { vmSize: 'Standard_B1s' }
    osProfile: {
      computerName: jumphostVmName
      adminUsername: adminUsername
      linuxConfiguration: {
        disablePasswordAuthentication: true
        ssh: { publicKeys: [ { path: '/home/${adminUsername}/.ssh/authorized_keys', keyData: sshPublicKey } ] }
      }
    }
    storageProfile: {
      imageReference: { publisher: 'Canonical', offer: '0001-com-ubuntu-server-jammy', sku: '22_04-lts-gen2', version: 'latest' }
      osDisk: { 
        createOption: 'FromImage'
        managedDisk: {
          storageAccountType: 'Premium_LRS'
          securityProfile: {
            securityEncryptionType: 'VMGuestStateOnly'
          }
        }
      }
    }
    networkProfile: {
      networkInterfaces: [ { id: jumphostNic.id } ]
    }
  }
}

// --- OUTPUTS ---
// Nie potrzebujemy outputów z tego modułu w głównym pliku, ale są dobre dla celów informacyjnych
