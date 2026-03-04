# ── Stage 1: Build ───────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files first (layer cache for restore)
COPY BookManager.slnx ./
COPY BookManager.Domain/BookManager.Domain.csproj           BookManager.Domain/
COPY BookManager.Infrastructure/BookManager.Infrastructure.csproj  BookManager.Infrastructure/
COPY BookManager.API/BookManager.API.csproj                 BookManager.API/
COPY BookManager.Tests/BookManager.Tests.csproj             BookManager.Tests/

RUN dotnet restore BookManager.API/BookManager.API.csproj

# Copy all source code
COPY . .

RUN dotnet publish BookManager.API/BookManager.API.csproj \
    -c Release \
    --no-restore \
    -o /app/publish

# ── Stage 2: Runtime ─────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "BookManager.API.dll"]
