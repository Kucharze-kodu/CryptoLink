# Etap 1: base - środowisko uruchomieniowe
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Porty, na których aplikacja będzie nasłuchiwać w kontenerze
EXPOSE 5000
EXPOSE 5001

# Etap 2: build - kompilacja aplikacji
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
# Ustawiamy katalog roboczy na /src, gdzie zostaną skopiowane wszystkie projekty
WORKDIR /src

# Kopiowanie plików .csproj dla wykorzystania cache warstw Docker (szybsze restore)
# Uproszczone ścieżki docelowe (np. ./CryptoLink.WebUI/) są zgodne z referencjami między projektami.
COPY ["CryptoLink.WebUI/CryptoLink.WebUI.csproj", "./CryptoLink.WebUI/"]
COPY ["CryptoLink.WebUI.Client/CryptoLink.WebUI.Client.csproj", "./CryptoLink.WebUI.Client/"]
COPY ["CryptoLink.Application/CryptoLink.Application.csproj", "./CryptoLink.Application/"]
COPY ["CryptoLink.Domain/CryptoLink.Domain.csproj", "./CryptoLink.Domain/"]
COPY ["CryptoLink.Architecture/CryptoLink.Architecture.csproj", "./CryptoLink.Architecture/"]

# Restore zależności
# Ścieżka wewnątrz kontenera: /src/CryptoLink.WebUI/CryptoLink.WebUI.csproj
RUN dotnet restore "CryptoLink.WebUI.csproj"

# Kopiowanie reszty kodu źródłowego
# Kopiujemy całą zawartość folderu src/WebUI z hosta do naszego WORKDIR /src w kontenerze
COPY ./CryptoLink.WebUI .

# Budowanie projektu
# Przechodzimy do katalogu głównego projektu hostującego wewnątrz kontenera
WORKDIR "CryptoLink.WebUI"
# Użycie `dotnet build` dla głównego projektu hostującego.
RUN dotnet build "CryptoLink.WebUI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etap 3: publish - publikacja aplikacji
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# Publikacja głównego projektu hostującego, który automatycznie publikuje klienta WASM
RUN dotnet publish "CryptoLink.WebUI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etap 4: final - końcowy, lekki obraz
FROM base AS final
WORKDIR /app
# Kopiowanie tylko opublikowanych plików (API + skompilowany klient WASM)
COPY --from=publish /app/publish .
# Uruchomienie aplikacji hostującej
ENTRYPOINT ["dotnet", "CryptoLink.WebUI.dll"]