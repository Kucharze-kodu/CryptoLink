# CryptoLink - Dokumentacja Bezpieczeństwa i Infrastruktury

**Projekt:** Wdrożenie bezpiecznej aplikacji CryptoLink na Azure AKS z implementacją best practices w zakresie bezpieczeństwa.

**Data:** Styczeń 2026

---

## 📋 Spis treści

1. [Architektura systemu](#architektura-systemu)
2. [Wymagania bezpieczeństwa](#wymagania-bezpieczeństwa)
3. [Zarządzanie dostępem (Least Privilege)](#zarządzanie-dostępem)
4. [Bezpieczeństwo sieci](#bezpieczeństwo-sieci)
5. [Szyfrowanie danych](#szyfrowanie-danych)
6. [Monitoring i alerty](#monitoring-i-alerty)
7. [Backup i disaster recovery](#backup-i-disaster-recovery)
8. [Pipeline CI/CD](#pipeline-cicd)
9. [Instrukcje wdrożenia](#instrukcje-wdrożenia)
10. [Weryfikacja bezpieczeństwa](#weryfikacja-bezpieczeństwa)

---

## Architektura systemu

### Diagram architektury

```
┌─────────────────────────────────────────────────────────────────┐
│                     Azure Subscription                           │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │                      Sieć Wirtualna (VNet)                 │ │
│  │                   10.0.0.0/16                              │ │
│  ├────────────────────────────────────────────────────────────┤ │
│  │                                                             │ │
│  │  ┌──────────────────────────────────────────────────────┐  │ │
│  │  │  Podsieć Publiczna (10.0.0.0/24)                     │  │ │
│  │  │  ┌──────────────────────────────────────────────┐   │  │ │
│  │  │  │  Bastion Host (Jump Host)                   │   │  │ │
│  │  │  │  - SSH access tylko z authorized IP         │   │  │ │
│  │  │  │  - Encrypted disk (Premium SSD)             │   │  │ │
│  │  │  │  - Azure Bastion service                    │   │  │ │
│  │  │  └──────────────────────────────────────────────┘   │  │ │
│  │  └──────────────────────────────────────────────────────┘  │ │
│  │                                                             │ │
│  │  ┌──────────────────────────────────────────────────────┐  │ │
│  │  │  Podsieć Kubernetes (10.0.1.0/24) PRYWATNA          │  │ │
│  │  │  ┌──────────────────────────────────────────────┐   │  │ │
│  │  │  │  AKS Cluster (aks-cryptolink)               │   │  │ │
│  │  │  │  ┌──────────────────────────────────────┐   │   │  │ │
│  │  │  │  │  cryptolink-app namespace            │   │   │  │ │
│  │  │  │  │  ┌────────────────────────────────┐ │   │   │  │ │
│  │  │  │  │  │ Pod: cryptolink-webui          │ │   │   │  │ │
│  │  │  │  │  │ - ServiceAccount: restricted   │ │   │   │  │ │
│  │  │  │  │  │ - SecurityContext: non-root    │ │   │   │  │ │
│  │  │  │  │  │ - Network Policy: limited      │ │   │   │  │ │
│  │  │  │  │  └────────────────────────────────┘ │   │   │  │ │
│  │  │  │  │  ┌────────────────────────────────┐ │   │   │  │ │
│  │  │  │  │  │ Pod: postgresql-statefulset    │ │   │   │  │ │
│  │  │  │  │  │ - User: cryptolink_app (no admin) │   │   │  │ │
│  │  │  │  │  │ - Encrypted storage at rest    │ │   │   │  │ │
│  │  │  │  │  │ - Private DNS                  │ │   │   │  │ │
│  │  │  │  │  └────────────────────────────────┘ │   │   │  │ │
│  │  │  │  │  Network Policies: ENABLED            │   │   │  │ │
│  │  │  │  │  RBAC: ServiceAccounts configured    │   │   │  │ │
│  │  │  │  └──────────────────────────────────────┘   │   │  │ │
│  │  │  └──────────────────────────────────────────────┘   │  │ │
│  │  └──────────────────────────────────────────────────────┘  │ │
│  │                                                             │ │
│  │  ┌──────────────────────────────────────────────────────┐  │ │
│  │  │  Podsieć Bazy Danych (10.0.2.0/24) PRYWATNA         │  │ │
│  │  │  ┌──────────────────────────────────────────────┐   │  │ │
│  │  │  │  Azure Database for PostgreSQL Flexible     │   │  │ │
│  │  │  │  - Private endpoint only                     │   │  │ │
│  │  │  │  - System-managed encryption                │   │  │ │
│  │  │  │  - Geo-redundant backups (30 days)          │   │  │ │
│  │  │  │  - Private DNS zone                         │   │  │ │
│  │  │  └──────────────────────────────────────────────┘   │  │ │
│  │  └──────────────────────────────────────────────────────┘  │ │
│  │                                                             │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                   │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │  Azure Container Registry (ACR)                            │ │
│  │  - Admin user DISABLED                                     │ │
│  │  - RBAC-based access control                               │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                   │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │  Monitoring & Logging                                       │ │
│  │  - Azure Monitor / Log Analytics                           │ │
│  │  - Metric alerts (CPU, Memory)                             │ │
│  │  - Container Insights                                      │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                   │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │  Backup & Recovery                                          │ │
│  │  - Recovery Services Vault                                 │ │
│  │  - Daily automated snapshots                               │ │
│  │  - 30-day retention policy                                 │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                   │
└─────────────────────────────────────────────────────────────────┘
```

### Komponenty

| Komponent | Typ | Region | Bezpieczeństwo |
|-----------|-----|--------|-----------------|
| AKS Cluster | Container Orchestration | North Europe | Network Policies, RBAC, NSG |
| PostgreSQL Flexible | Database | North Europe | Private Endpoint, Encryption, Backups |
| ACR | Container Registry | North Europe | RBAC (no admin user) |
| Azure Bastion | Jump Host | Public Subnet | SSH key-based auth only |
| Log Analytics | Monitoring | North Europe | Encrypted, 30-day retention |
| Recovery Vault | Backup Service | North Europe | Geo-redundant, LRS |

---

## Wymagania bezpieczeństwa

### Spełnione wymagania (ocena 3.0+)

✅ **Zarządzanie dostępem (TK07)**
- [x] Brak konta root/admin do codziennych prac
- [x] Dedykowany użytkownik PostgreSQL (`cryptolink_app` zamiast `postgres`)
- [x] RBAC w Kubernetes z ServiceAccount
- [x] Least privilege principle wszędzie

✅ **Bezpieczeństwo sieci (TK02, TK07)**
- [x] Security Group (NSG) z ograniczonymi portami
- [x] Aplikacja w prywatnej podsieci
- [x] Bastion Host w podsieci publicznej
- [x] Network Policies w Kubernetes
- [x] Private DNS dla bazy danych

✅ **Segmentacja sieci (TK03, TK06)**
- [x] Prywatna podsieć dla AKS
- [x] Prywatna podsieć dla bazy danych
- [x] Publiczna podsieć dla Bastiona
- [x] Load Balancer dla dostępu do aplikacji

✅ **Monitoring (TK05)**
- [x] Azure Monitor z Log Analytics
- [x] Metric alerts (CPU, Memory)
- [x] Container Insights
- [x] Diagnostics dla AKS

✅ **Backup & Disaster Recovery (TK04)**
- [x] Recovery Services Vault
- [x] Daily automated backups
- [x] Geo-redundant backups (PostgreSQL)
- [x] 30-day retention policy

✅ **Szyfrowanie danych (TK08)**
- [x] Encryption at rest dla VM disk
- [x] System-managed encryption dla PostgreSQL
- [x] Secure transport (TLS/HTTPS)

✅ **Infrastructure as Code (IaC) (TK03, TK05)**
- [x] Bicep dla całej infrastruktury
- [x] Modularny design
- [x] Parametryzowane szablony

✅ **DevSecOps (TK05)**
- [x] GitHub Actions CI/CD pipeline
- [x] Automated deployment
- [x] Security scanning możliwy

✅ **Bezpieczeństwo kontenerów (TK05, TK09)**
- [x] AKS managed cluster
- [x] Network Policies
- [x] RBAC + ServiceAccounts
- [x] Non-root containers
- [x] SecurityContext z least privilege

---

## Zarządzanie dostępem

### Strategia Least Privilege

#### PostgreSQL
```sql
-- PRZED (Admin Access ❌):
Username: postgres
Password: POSTGRES_ADMIN_PASSWORD

-- PO (Application User ✅):
Username: cryptolink_app
Password: POSTGRES_APP_PASSWORD
Uprawnienia: SELECT, INSERT, UPDATE, DELETE naDatabase 'cryptolink'
Brak dostępu do: pg_catalog, information_schema, postgres DB
```

**Utworzenie użytkownika (w SQL):**
```sql
CREATE USER cryptolink_app WITH PASSWORD 'your_strong_password';
GRANT CONNECT ON DATABASE cryptolink TO cryptolink_app;
GRANT USAGE ON SCHEMA public TO cryptolink_app;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO cryptolink_app;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO cryptolink_app;
```

#### Kubernetes RBAC
```yaml
ServiceAccount: cryptolink-app
Permissions:
  - Read: Secrets, ConfigMaps, Pods, Pods/log
  - No write access
  - No delete access
  - No cluster-level permissions
```

#### Azure Container Registry
```
Admin User: DISABLED (adminUserEnabled: false)
Access Method: Azure RBAC + Identity-based authentication
```

#### Azure Bastion / Jump Host
```
Authentication: SSH public key ONLY
No password login
User: azureuser (limited sudoers)
```

---

## Bezpieczeństwo sieci

### Network Policies w Kubernetes

**1. PostgreSQL Ingress Policy**
```yaml
- Zezwalaj: Traffic z pod aplikacji (cryptolink-webui)
- Port: 5432 (TCP)
- Blokuj: Ruch z innych podów
```

**2. CryptoLink App Ingress/Egress Policy**
```yaml
Ingress:
  - Port 80/443 z Load Balancera

Egress:
  - Port 5432 do PostgreSQL
  - Port 53 (UDP) do DNS
  - Port 443 do internetu (jeśli potrzeba)
```

**3. Default Deny All**
```yaml
- Blokuj cały ruch domyślnie
- Zezwalaj tylko explicite zdefiniowanemu
```

### Network Security Groups (NSG)

| Direction | Source | Destination | Protocol | Port | Action |
|-----------|--------|-------------|----------|------|--------|
| Inbound | Internet | Bastion | TCP | 22 | Allow |
| Inbound | AKS Subnet | PostgreSQL Subnet | TCP | 5432 | Allow |
| Inbound | Internet | AKS Subnet | TCP | 80,443 | Allow (Load Balancer) |
| Outbound | AKS Subnet | PostgreSQL Subnet | TCP | 5432 | Allow |
| Outbound | AKS Subnet | Internet | TCP | 443 | Allow (DNS, updates) |
| Outbound | All | All | - | - | Default: Allow |

---

## Szyfrowanie danych

### Encryption at Rest

#### VM Disk (Bastion Host)
```bicep
managedDisk: {
  storageAccountType: 'Premium_LRS'
  securityProfile: {
    securityEncryptionType: 'VMGuestStateOnly'
  }
}
```
- **Typ:** Azure Disk Encryption
- **Status:** ✅ Enabled
- **Klucz:** Azure-managed (no CMK needed)

#### PostgreSQL
```bicep
dataEncryption: {
  type: 'SystemManaged'
}
```
- **Typ:** TDE (Transparent Data Encryption)
- **Status:** ✅ Enabled
- **Klucz:** Azure-managed

#### Backup Storage
- **Typ:** Recovery Services Vault encryption
- **Status:** ✅ Enabled
- **Redundancja:** Geo-redundant

### Encryption in Transit

- **TLS 1.2+** dla wszystkich połączeń
- **SSH key-based** dla Jump Host
- **Private DNS** dla komunikacji wewnętrznej

---

## Monitoring i alerty

### Azure Monitor Setup

**Metryki monitorowane:**

| Metryka | Alert | Threshold | Action |
|---------|-------|-----------|--------|
| Node CPU Usage | High CPU | > 80% | Email notification |
| Node Memory Usage | High Memory | > 85% | Email notification |
| Pod Restart Count | Pod Restarts | > 3 in 10m | Investigation |
| Network In/Out | DDoS Detection | Anomaly | Block traffic |
| Disk I/O | Disk Bottleneck | > 90% | Scale up |

**Logi zbierane:**
- Cluster autoscaling
- Kube API server
- Guard (security)
- Container stdout/stderr
- All pod logs

**Retention:** 30 dni w Log Analytics

### Dashboard
```
Dostęp: Azure Portal → Monitor → Insights
Metryki: Real-time + Historical
Alerty: Email notifications (konfigurować adres)
```

---

## Backup i disaster recovery

### Strategia backupów

**VM Disk (Bastion):**
```
Frequency: Daily o 2:00 UTC
Retention: 30 dni
Type: Snapshot
Storage: Recovery Services Vault
```

**PostgreSQL:**
```
Automatic backup: Daily
Retention: 30 dni
Geo-redundancy: Enabled
PITR: 7 dni
```

### Procedura odtworzenia (Recovery)

#### 1. PostgreSQL z backupu

```bash
# Azure CLI
az postgres flexible-server restore \
  --resource-group <RG> \
  --name cryptolink-restored \
  --source-server cryptolink-pg-server-xxx \
  --restore-point-in-time "2024-01-20T10:00:00"

# Zmienić connection string w secrets
kubectl set env deployment/cryptolink-app \
  -n cryptolink-app \
  ConnectionStrings__Default="Host=cryptolink-restored..."
```

#### 2. VM Disk z snapshotu

```bash
# Azure Portal:
# 1. Recovery Services Vault → Backup items → Virtual Machines
# 2. Select Bastion VM → Restore

# Or CLI:
az backup recovery point list \
  --resource-group <RG> \
  --vault-name cryptolink-backup-vault \
  --container-name jumphost-vm
```

#### 3. AKS Cluster

```bash
# Kubernetes backup (etcd) już jest w Recovery Vault
# Aby przywrócić:
az aks command invoke \
  --resource-group <RG> \
  --name aks-cryptolink \
  --command "kubectl get all"
```

---

## Pipeline CI/CD

### GitHub Actions Workflow (`deploy.yml`)

**Etapy:**

```mermaid
graph LR
    A[Checkout] → B[Login Azure]
    B → C[Deploy Bicep Infra]
    C → D[Build Docker Image]
    D → E[Push to ACR]
    E → F[Deploy to AKS]
    F → G[Verify Deployment]
    
    style C fill:#f9f
    style D fill:#9f9
    style F fill:#99f
```

### Triggers
```yaml
on:
  push:
    branches: [master, cicd_kubernetes]
  workflow_dispatch
```

### Secrets wymagane (w GitHub)
```
AZURE_CREDENTIALS          # Azure SPN credentials
AZURE_SUBSCRIPTION_ID      # Subscription ID
RESOURCE_GROUP             # RG name
SSH_PUBLIC_KEY            # For Bastion access
POSTGRES_ADMIN_PASSWORD   # For initial setup
POSTGRES_APP_PASSWORD     # Application database user
JWT_SECRET_KEY            # For authentication
```

---

## Instrukcje wdrożenia

### Warunki wstępne

```bash
# Tools
- Azure CLI 2.50+
- kubectl 1.27+
- Docker CLI (dla image builds)
- git

# Uprawnienia Azure
- Contributor na Resource Group
- Role: Virtual Machine Contributor
- Role: Database Administrator
```

### 1. Setup Azure CLI

```bash
az login
az account set --subscription "<SUBSCRIPTION_ID>"
```

### 2. Przygotowanie secrets (GitHub)

```bash
# Wygeneruj SSH key
ssh-keygen -t rsa -b 4096 -N "" -f ~/.ssh/id_rsa

# Wygeneruj strong passwords
openssl rand -base64 32  # POSTGRES_APP_PASSWORD
openssl rand -base64 32  # JWT_SECRET_KEY

# Utwórz Azure SPN dla CI/CD
az ad sp create-for-rbac --name "cryptolink-ci" --role Contributor

# Skopiuj do GitHub Secrets
```

### 3. Wdrożenie

**Opcja A: Automatycznie (GitHub Actions)**
```bash
git push origin master
# GitHub Actions automatycznie:
# - Deploy infrastruktury (Bicep)
# - Build image Docker
# - Push do ACR
# - Deploy na AKS
```

**Opcja B: Ręcznie (CLI)**
```bash
# Deploy infra
az deployment group create \
  --resource-group <RG> \
  --template-file infra/main.bicep \
  --parameters sshPublicKey="$(cat ~/.ssh/id_rsa.pub)" \
               postgresAdminPassword="..." \
               aksClusterName="aks-cryptolink" \
               acrName="cryptolinkBRCh169606169600"

# Apply Kubernetes manifests
kubectl apply -f kubernetes/
```

### 4. Weryfikacja

```bash
# Check AKS cluster
az aks get-credentials -g <RG> -n aks-cryptolink
kubectl get nodes
kubectl get pods -n cryptolink-app

# Check Database
az postgres flexible-server show -g <RG> -n <DB_NAME>

# Check ACR
az acr show -g <RG> -n cryptolinkBRCh169606169600
```

---

## Weryfikacja bezpieczeństwa

### Checklist

- [ ] **Brak admin users**
  ```bash
  kubectl get pods -n cryptolink-app -o json | jq '.items[].spec.securityContext.runAsUser'
  # Powinien zwrócić: 1000 (non-root)
  ```

- [ ] **RBAC aktywny**
  ```bash
  kubectl auth can-i get pods --as=system:serviceaccount:cryptolink-app:cryptolink-app
  # Powinno: yes
  ```

- [ ] **Network Policies aktywne**
  ```bash
  kubectl get networkpolicies -n cryptolink-app
  # Powinno wylistować all policies
  ```

- [ ] **Encryption włączone**
  ```bash
  az vm show -g <RG> -n jumphost-vm --query 'storageProfile.osDisk.encryptionSettings'
  # Powinno pokazać encryption settings
  ```

- [ ] **Monitoring aktywny**
  ```bash
  az monitor metrics list-definitions -g <RG> --namespace Microsoft.ContainerService/managedClusters
  # Powinno zwrócić metryki
  ```

- [ ] **Backupy działają**
  ```bash
  az backup job list -g <RG> --vault-name cryptolink-backup-vault --output table
  # Powinny być recent successful backups
  ```

- [ ] **Application connectivity**
  ```bash
  kubectl exec -it <pod-name> -n cryptolink-app -- \
    psql -h postgres-db-service -U cryptolink_app -d cryptolink -c "SELECT 1"
  # Powinno zwrócić: 1
  ```

### Testing Admin Account Restrictions

```bash
# Try SSH with default admin (should fail after Bastion setup)
ssh -i ~/.ssh/id_rsa azureuser@jumphost-ip
# ✅ Should work (non-admin user)

# Try to connect as root (should fail)
su - root
# ❌ Should fail

# Try kubectl as regular user (should fail for cluster-admin)
kubectl get clusterrolebindings --as=system:serviceaccount:cryptolink-app:cryptolink-app
# ❌ Should be forbidden
```

### Network Policy Testing

```bash
# Test: Pod powinien móc połączyć się z DB
kubectl exec -it <app-pod> -n cryptolink-app -- \
  nc -zv postgres-db-service 5432
# ✅ Connection successful

# Test: Inny pod powinien być zablokowany
kubectl run test-pod --image=busybox -n cryptolink-app --rm -it -- \
  sh -c "nc -zv postgres-db-service 5432"
# ❌ Connection refused (Network Policy)
```

---

## Podsumowanie zmian bezpieczeństwa

| Aspekt | Przed | Po | Korzyści |
|--------|-------|-----|----------|
| **DB User** | `postgres` (admin) | `cryptolink_app` (limited) | Least privilege, damage control |
| **ACR Admin** | Enabled | Disabled | RBAC-based access, audit trail |
| **K8s Pods** | Default pod identity | ServiceAccount + RBAC | Fine-grained permissions |
| **Network** | Open traffic | Network Policies | Segmentation, DDoS mitigation |
| **Disk Encryption** | Not enforced | Enabled | Data protection at rest |
| **DB Encryption** | Not specified | System-managed TDE | Compliance-ready |
| **Backups** | Ad-hoc (7d) | Automated daily (30d) | DR capability |
| **Monitoring** | Manual | Azure Monitor + Alerts | Proactive incident response |

---

## Kontakt i support

- **GitHub:** [CryptoLink Repo]
- **Issues:** github.com/username/CryptoLink/issues
- **Documentation:** Folder `/docs`

---

**Dokument wersja:** 1.0  
**Ostatnia aktualizacja:** Styczeń 2026  
**Status:** ✅ Gotowy do wdrażania
