# Etap 1: base - środowisko uruchomieniowe (najlepiej użyć lżejszego obrazu)
# Jeśli to tylko host Blazora, lepiej użyć aspnet:8.0-alpine
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# Zmieniono na standardowe porty dla kontenerów (8080/8081)
EXPOSE 8080
EXPOSE 8081


# Etap 2: build - kompilacja aplikacji
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1. Kopiowanie pliku Solution (.sln) - KLUCZOWE
# Zakładam, że CryptoLink.sln jest w katalogu głównym projektu
COPY ["CryptoLink.sln", "./"]

# 2. Kopiowanie wszystkich plików .csproj
# Ścieżka docelowa MUSI odzwierciedlać pełną ścieżkę, jakiej oczekuje .sln
#
# BŁĄD: COPY ["CryptoLink.WebUI/CryptoLink.WebUI/CryptoLink.WebUI.csproj", "CryptoLink.WebUI/"]
#
# POPRAWA: Użyj pełnej ścieżki docelowej, aby stworzyć katalog CryptoLink.WebUI/CryptoLink.WebUI/
COPY ["CryptoLink.WebUI/CryptoLink.WebUI/CryptoLink.WebUI.csproj", "CryptoLink.WebUI/CryptoLink.WebUI/"]
COPY ["CryptoLink.WebUI/CryptoLink.WebUI.Client/CryptoLink.WebUI.Client.csproj", "CryptoLink.WebUI/CryptoLink.WebUI.Client/"]
COPY ["CryptoLink.Application/CryptoLink.Application.csproj", "CryptoLink.Application/"]
COPY ["CryptoLink.Domain/CryptoLink.Domain.csproj", "CryptoLink.Domain/"]
COPY ["CryptoLink.Architecture/CryptoLink.Architecture.csproj", "CryptoLink.Architecture/"]

# 3. Restore zależności - Wg pliku Solution
RUN dotnet restore "CryptoLink.sln"

# 4. Kopiowanie reszty kodu źródłowego
COPY . .

# 5. Publikacja aplikacji
# Teraz ścieżka do projektu musi być pełna, jak w logu błędu
RUN dotnet publish "CryptoLink.WebUI/CryptoLink.WebUI/CryptoLink.WebUI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# Etap 3: final - końcowy, lekki obraz
FROM base AS final
WORKDIR /app
# Kopiowanie tylko opublikowanych plików z /app/publish do końcowego obrazu
COPY --from=build /app/publish .

# Uruchomienie aplikacji hostującej
# W standardowym hostingu ASP.NET Core Blazor WAM jest serwowany z tego DLL.
ENTRYPOINT ["dotnet", "CryptoLink.WebUI.dll"]

# Jeśli korzystasz z kontenerów, pamiętaj aby w konfiguracji uruchomienia (np. docker run lub docker-compose)
# ustawić zmienne środowiskowe lub argumenty, które mapują porty kontenera (np. 8080) na porty hosta.