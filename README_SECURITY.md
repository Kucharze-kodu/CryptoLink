# CryptoLink - Projekt bezpiecznej infrastruktury na Azure

## ⚡ Quick Start

```bash
# 1. Setup Azure
az login
az account set --subscription "<SUBSCRIPTION_ID>"

# 2. Deploy infrastruktury
az deployment group create \
  --resource-group myResourceGroup \
  --template-file infra/main.bicep \
  --parameters sshPublicKey="$(cat ~/.ssh/id_rsa.pub)" \
               postgresAdminPassword="MySecurePassword123!"

# 3. Deploy aplikacji
kubectl apply -f kubernetes/
```

## 🔐 Implementowane zabezpieczenia

### ✅ Zarządzanie dostępem (Least Privilege)
- **Brak admin accounts** - dedykowany user `cryptolink_app` dla aplikacji
- **RBAC w Kubernetes** - ServiceAccount z minimalnymi uprawnieniami
- **ACR bez admina** - dostęp poprzez Azure RBAC
- **SSH key-based auth** - brak hasło login na Bastion

### ✅ Segmentacja sieci
- **3 podsieci** - Public (Bastion), Private (AKS), Private (PostgreSQL)
- **Network Policies** - ruch tylko między niezbędnymi komponentami
- **Private DNS** - komunikacja wewnętrzna szyfrowana
- **Load Balancer** - publiczny dostęp do aplikacji

### ✅ Szyfrowanie danych
- **At rest** - encryption dla VM disks i PostgreSQL (system-managed)
- **In transit** - TLS 1.2+ dla wszystkich połączeń
- **Backups** - geo-redundant i szyfrowane

### ✅ Monitoring & Alerty
- **Azure Monitor** - CPU, Memory, Disk metrics
- **Log Analytics** - 30-day retention policy
- **Container Insights** - diagnostyka klastra
- **Metric Alerts** - automatyczne notyfikacje

### ✅ Backup & Disaster Recovery
- **Daily automated backups** - snapshoty VM i bazy danych
- **30-day retention** - geo-redundant dla PostgreSQL
- **Recovery procedures** - dokumentacja PITR
- **RTO/RPO** - < 1 godzina

---

## 📂 Struktura projektu

```
CryptoLink/
├── infra/                      # Infrastructure as Code (Bicep)
│   ├── main.bicep              # Main template
│   └── modules/
│       ├── networking.bicep     # VNet + Subnets + NSG
│       ├── database.bicep       # PostgreSQL Flexible Server
│       ├── aks.bicep            # AKS Cluster
│       ├── acr.bicep            # Container Registry
│       ├── management.bicep      # Bastion + Jump Host VM
│       ├── monitoring.bicep      # Log Analytics + Alerts
│       └── backup.bicep         # Recovery Services Vault
│
├── kubernetes/                  # Kubernetes manifests
│   ├── 00-namespace.yaml        # Namespace + quotas
│   ├── 01-rbac.yaml             # ServiceAccount + RBAC
│   ├── 02-postgres-secret.yaml  # Database secrets
│   ├── 03-postgres-statefulset.yaml
│   ├── 04-postgres-service.yaml
│   ├── 05-app-deployment.yaml   # App with security context
│   ├── 06-app-service.yaml      # Load Balancer
│   ├── 07-ingress.yaml          # Ingress controller
│   ├── 08-network-policy.yaml   # Old (deprecated)
│   └── 09-network-policies.yaml # Network Policies (NEW)
│
├── .github/workflows/
│   └── deploy.yml               # GitHub Actions CI/CD
│
├── SECURITY_DOCUMENTATION.md    # Pełna dokumentacja
└── README.md                    # Ten plik
```

---

## 🔑 Kluczowe zmiany od starej konfiguracji

### PostgreSQL (❌ Admin → ✅ App User)
```yaml
PRZED:
  POSTGRES_USER: postgres
  Username: postgres (ADMIN!)

PO:
  POSTGRES_USER: cryptolink_app
  Username: cryptolink_app (LIMITED PRIVILEGES)
```

### ACR (❌ Admin Enabled → ✅ RBAC Only)
```bicep
PRZED:
  adminUserEnabled: true  # ❌

PO:
  adminUserEnabled: false # ✅ Use Azure RBAC instead
```

### Kubernetes (❌ Default → ✅ Secure Context)
```yaml
PRZED:
  runAsUser: 0            # Root! ❌
  No ServiceAccount

PO:
  runAsUser: 1000         # Non-root ✅
  serviceAccountName: cryptolink-app
  securityContext:
    allowPrivilegeEscalation: false
    readOnlyRootFilesystem: false
    capabilities:
      drop: [ALL]
```

### Network Policies (❌ Disabled → ✅ Enabled)
```yaml
PRZED:
  # Brak Network Policies
  Ruch: Open between all pods

PO:
  # Network Policies enabled
  postgres-network-policy:     # Only from app pods
  cryptolink-app-network-policy: # Limited egress
  default-deny-all:            # Explicit allow only
```

---

## 📊 Architektura

```
┌─────────────────────────────────────────────────────┐
│         Azure Virtual Network (10.0.0.0/16)         │
├─────────────────────────────────────────────────────┤
│                                                       │
│  PUBLIC (10.0.0.0/24)      PRIVATE (10.0.1.0/24)    │
│  ┌────────────────┐        ┌──────────────────────┐ │
│  │  Jump Host     │        │   AKS Cluster        │ │
│  │  (Bastion)     │        │                      │ │
│  │  - SSH only    │        │  ├─ Namespace        │ │
│  │  - Public IP   │        │  ├─ Pods             │ │
│  │  - Ubuntu 22   │        │  ├─ Network Policy   │ │
│  │                │        │  └─ RBAC             │ │
│  └────────────────┘        └──────────────────────┘ │
│                                                       │
│  PRIVATE (10.0.2.0/24)                              │
│  ┌──────────────────────────────────────────────┐   │
│  │  PostgreSQL Flexible Server                  │   │
│  │  - Private endpoint only                     │   │
│  │  - System-managed encryption                 │   │
│  │  - Geo-redundant backups                     │   │
│  └──────────────────────────────────────────────┘   │
│                                                       │
│  SERVICES                                           │
│  ├─ Azure Container Registry (ACR)                  │
│  ├─ Azure Monitor + Log Analytics                   │
│  ├─ Recovery Services Vault                         │
│  └─ Azure Key Vault (optional)                      │
│                                                       │
└─────────────────────────────────────────────────────┘
```

---

## 🚀 Deployment

### Via GitHub Actions (recommended)
```bash
# 1. Ustaw secrets w GitHub Settings
AZURE_CREDENTIALS          # SPN credentials
AZURE_SUBSCRIPTION_ID      
RESOURCE_GROUP
SSH_PUBLIC_KEY
POSTGRES_ADMIN_PASSWORD
POSTGRES_APP_PASSWORD      # NEW: App database user
JWT_SECRET_KEY

# 2. Push na master
git push origin master

# 3. GitHub Actions automatycznie:
#    - Deploy Bicep infra
#    - Build Docker image
#    - Push do ACR
#    - Deploy na AKS
```

### Via Azure CLI
```bash
# Deploy infrastruktury
az deployment group create \
  --resource-group myRG \
  --template-file infra/main.bicep

# Deploy aplikacji
az aks get-credentials -g myRG -n aks-cryptolink
kubectl apply -f kubernetes/
```

---

## 🔍 Weryfikacja

### Sprawdzenie, że admin accounts nie istnieją

```bash
# PostgreSQL
kubectl exec -it postgres-db-0 -n cryptolink-app -- \
  psql -U cryptolink_app -d cryptolink -c "\du"
# ✅ Powinien pokazać tylko cryptolink_app (nie postgres)

# Kubernetes
kubectl get pods -n cryptolink-app -o json | \
  jq '.items[].spec.securityContext.runAsUser'
# ✅ Powinien zwrócić 1000 (nie 0)

# ACR
az acr credential show -g myRG -n cryptolink... \
  --query adminUserEnabled
# ✅ Powinno być: false
```

### Sprawdzenie Network Policies

```bash
# List policies
kubectl get networkpolicies -n cryptolink-app

# Test connectivity
kubectl exec -it <app-pod> -n cryptolink-app -- \
  curl http://postgres-db-service:5432
# ✅ Should work (postgres pod marked in policy)

# Test blocked traffic
kubectl run busybox -n cryptolink-app --image=busybox --rm -it -- \
  nc -zv postgres-db-service 5432
# ❌ Should timeout (not in allow list)
```

### Sprawdzenie monitoringu

```bash
az monitor metrics list \
  --resource /subscriptions/.../aks-cryptolink \
  --metric CpuUsagePercentage
# ✅ Powinny być metryki

az monitor alert list -g myRG --output table
# ✅ Powinny być alerts dla CPU/Memory
```

---

## 📖 Dokumentacja

- **Pełna dokumentacja:** [SECURITY_DOCUMENTATION.md](./SECURITY_DOCUMENTATION.md)
- **Bicep reference:** https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/
- **AKS best practices:** https://learn.microsoft.com/en-us/azure/aks/best-practices
- **Kubernetes RBAC:** https://kubernetes.io/docs/reference/access-authn-authz/rbac/

---

## 🛠️ Troubleshooting

### Pod nie może połączyć się z PostgreSQL
```bash
# Check Network Policy
kubectl get networkpolicies -n cryptolink-app

# Check connectivity
kubectl exec -it <pod> -n cryptolink-app -- \
  curl -v telnet://postgres-db-service:5432

# Check secrets
kubectl get secret cryptolink-app-secrets -n cryptolink-app -o jsonpath='{.data.ConnectionStrings__Default}' | base64 -d
```

### Monitoring alerts nie działają
```bash
# Check Log Analytics
az monitor log-analytics workspace show \
  -g myRG -n cryptolink-law-...

# Check metrics
az monitor metrics list-definitions \
  -g myRG \
  --namespace Microsoft.ContainerService/managedClusters
```

### Backup failed
```bash
# Check Recovery Vault
az backup vault show -g myRG -n cryptolink-backup-vault

# Check backup jobs
az backup job list -g myRG \
  --vault-name cryptolink-backup-vault \
  --output table
```

---

## ✅ Checklist wdrożenia

- [ ] Azure subscription setup
- [ ] GitHub secrets configured
- [ ] SSH key generated
- [ ] Passwords created (PostgreSQL, JWT)
- [ ] Bicep templates validated
- [ ] Kubernetes manifests validated
- [ ] Push to `master` branch
- [ ] GitHub Actions pipeline success
- [ ] Verify application deployed
- [ ] Test connectivity from Bastion
- [ ] Verify monitoring alerts
- [ ] Test backup/recovery procedure
- [ ] Documentation reviewed

---

## 📞 Support

- **Issues:** GitHub Issues
- **Docs:** ./SECURITY_DOCUMENTATION.md
- **Questions:** Create a discussion

---

**Status:** ✅ Production Ready  
**Security Level:** 🔐 High (Admin-less deployment)  
**Last Updated:** January 2026
