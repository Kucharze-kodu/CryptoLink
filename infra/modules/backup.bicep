// infra/modules/backup.bicep
// Azure Backup configuration dla dysków VM i bazy danych
// Implementuje automatic daily backups z 30-dniowym retencją

@description('The Azure region for all resources.')
param location string

@description('The resource IDs of disks to backup.')
param diskResourceIds array = []

var recoveryServicesVaultName = 'cryptolink-backup-${uniqueString(resourceGroup().id)}'
var backupPolicyName = 'cryptolink-backup-policy'

// === RECOVERY SERVICES VAULT ===
resource recoveryServicesVault 'Microsoft.RecoveryServices/vaults@2023-04-01' = {
  name: recoveryServicesVaultName
  location: location
  sku: {
    name: 'RS0'
    tier: 'Standard'
  }
  properties: {
    publicNetworkAccess: 'Enabled'
  }
}

// === BACKUP POLICY DLA DYSKÓW ===
resource backupPolicy 'Microsoft.RecoveryServices/vaults/backupPolicies@2023-04-01' = {
  parent: recoveryServicesVault
  name: backupPolicyName
  properties: {
    backupManagementType: 'AzureIaasVM'
    instantRpRetentionRangeInDays: 5
    schedulePolicy: {
      schedulePolicyType: 'SimpleSchedulePolicy'
      scheduleRunFrequency: 'Daily'
      scheduleRunTimes: [
        '02:00'  // 2 AM w UTC
      ]
      scheduleWeeklyFrequency: 0
    }
    retentionPolicy: {
      retentionPolicyType: 'LongTermRetentionPolicy'
      dailySchedule: {
        retentionTimes: [
          '02:00'
        ]
        retentionDuration: {
          count: 30
          durationType: 'Days'
        }
      }
    }
    timeZone: 'UTC'
    protectedItemsCount: 0
  }
}

// === BACKUP DLA POSTGRESQL ===
// Azure Database for PostgreSQL ma wbudowany backup (7 dni domyślnie, konfigurujemy w database.bicep)

// === OUTPUTS ===
@description('The name of the Recovery Services Vault.')
output vaultName string = recoveryServicesVault.name

@description('The resource ID of the Recovery Services Vault.')
output vaultId string = recoveryServicesVault.id

@description('The name of the backup policy.')
output backupPolicyName string = backupPolicy.name
