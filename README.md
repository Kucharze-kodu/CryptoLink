# CryptoLink

CryptoLink to aplikacja webowa w .NET, która zamienia krótki URL na długi „rozbudowany” link i pozwala zarządzać nim po zalogowaniu.

## Co robi projekt

- tworzy wydłużone linki na podstawie docelowego adresu URL,
- generuje losowe fragmenty linku ze słowników kategorii (BookWords),
- zapisuje i udostępnia listę linków użytkownika (podgląd, edycja, usuwanie),
- obsługuje publiczne wejście na wygenerowany link,
- udostępnia logowanie/rejestrację z wyzwaniem PGP oraz autoryzację JWT (cookie `CookiesAuth`).

## Architektura

Repozytorium jest podzielone na warstwy:

- `CryptoLink.Domain` – encje domenowe i reguły biznesowe,
- `CryptoLink.Application` – komendy/zapytania (MediatR), walidacja i kontrakty,
- `CryptoLink.Architecture` – EF Core, PostgreSQL, repozytoria, auth, cache,
- `CryptoLink.WebUI` – backend HTTP + host Blazor,
- `CryptoLink.WebUI.Client` – interfejs użytkownika (Blazor WebAssembly).

## Stos technologiczny

- .NET / ASP.NET Core / Blazor
- MediatR + FluentValidation
- Entity Framework Core
- PostgreSQL
- JWT + PGP challenge flow
- Docker / docker-compose

## Wdrożenie chmurowe (Azure & Kubernetes)

Projekt został w pełni zautomatyzowany i przystosowany do bezpiecznego wdrożenia w chmurze Microsoft Azure. Kompletna konfiguracja infrastruktury oraz szczegółowa dokumentacja z tym związana znajdują się na osobnym branchu.

Architektura chmurowa obejmuje m.in.:
* **Azure Kubernetes Service (AKS)** – konteneryzacja i orkiestracja aplikacji,
* **Infrastructure as Code (IaC)** – modularne wdrażanie środowiska za pomocą Azure Bicep,
* **CI/CD** – w pełni zautomatyzowany pipeline w GitHub Actions (budowanie obrazów, wdrażanie IaC i aplikacji),
* **Zaawansowane bezpieczeństwo (Zero Trust & Least Privilege)** – prywatne podsieci (VNet), dostęp administracyjny przez Azure Bastion, Network Policies wewnątrz klastra oraz odizolowana baza Azure Database for PostgreSQL.

👉 **[Zobacz szczegółowy opis architektury i instrukcję wdrożenia na branchu `cicd_kubernetes`](https://github.com/Kucharze-kodu/CryptoLink/tree/cicd_kubernetes)**


## Uruchomienie lokalne

### Wymagania

- .NET SDK (zgodny z projektem)
- PostgreSQL **albo** Docker

### Opcja 1: docker-compose

1. W katalogu repozytorium uruchom:
   `docker compose up --build`
2. Aplikacja będzie dostępna pod adresem `http://localhost`.

### Opcja 2: uruchomienie z poziomu .NET

1. Uruchom PostgreSQL i ustaw poprawny connection string `ConnectionStrings:Default`.
2. Uruchom aplikację:
   `dotnet run --project CryptoLink.WebUI/CryptoLink.WebUI`
3. Migracje i seed danych są wykonywane podczas startu aplikacji.

## API (skrót)

- `POST /api/auth/register/init`
- `POST /api/auth/register/complete`
- `POST /api/auth/login/init`
- `POST /api/auth/login/complete`
- `POST|PUT|DELETE /api/linkExtended`
- `GET /api/linkExtended`
- `GET /api/linkExtendedbyId`
- `GET /api/LoadlinkExtended` (publiczny)
- `POST /api/bookword`
- `DELETE /api/bookword`
- `POST /api/generateLink`
