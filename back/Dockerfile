# ── Stage 1: Build ───────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy all source code
COPY . .

# Remove Windows build artifacts.
# IMPORTANT: macOS/Windows may create "bin\Debug" (with literal backslash) which
# is a single directory name on Linux that breaks MSBuild glob expansion.
RUN find . -depth -mindepth 1 | while IFS= read -r f; do \
      case "$f" in *\\*) rm -rf "$f";; esac; \
    done 2>/dev/null || true && \
    find . -name "bin"  -type d -exec rm -rf {} + 2>/dev/null || true && \
    find . -name "obj"  -type d -exec rm -rf {} + 2>/dev/null || true

# Restore and publish
RUN dotnet publish BookManager.API/BookManager.API.csproj \
    -c Release \
    -o /app/publish

# ── Stage 2: Runtime ─────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "BookManager.API.dll"]
