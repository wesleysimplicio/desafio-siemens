# ── Stage 1: Build ───────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files first for NuGet restore layer caching
COPY BookManager.slnx ./
COPY BookManager.API/BookManager.API.csproj BookManager.API/
COPY BookManager.Domain/BookManager.Domain.csproj BookManager.Domain/
COPY BookManager.Infrastructure/BookManager.Infrastructure.csproj BookManager.Infrastructure/
COPY BookManager.Tests/BookManager.Tests.csproj BookManager.Tests/

RUN dotnet restore BookManager.API/BookManager.API.csproj

# Copy remaining source and publish
COPY . .

# Remove Windows-style backslash directories (macOS may create "bin\Debug" as literal dir names)
RUN find . -depth -mindepth 1 | while IFS= read -r f; do \
      case "$f" in *\\*) rm -rf "$f";; esac; \
    done 2>/dev/null || true

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
