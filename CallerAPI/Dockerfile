# syntax=docker/dockerfile:1.4

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build + publish stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1. Copy only csproj files first (restore cache-friendly)
COPY ["CallerAPI/CallerAPI.csproj", "CallerAPI/"]
COPY ["CallerApi.Infra/CallerApi.Infra.csproj", "CallerApi.Infra/"]
COPY ["CalllerApi.Core/CalllerApi.Core.csproj", "CalllerApi.Core/"]

# If you have private feeds, you can mount a nuget.config secret here instead
RUN dotnet restore "CallerAPI/CallerAPI.csproj"

# 2. Copy the rest of the source
COPY . .

# 3. Publish directly (no extra build; no second restore)
WORKDIR "/src/CallerAPI"
RUN dotnet publish "CallerAPI.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false \
    --no-restore

# Final runtime stage (used in production / normal runs)
FROM base AS final
WORKDIR /app

# Optional: harden security by using non-root (recommended)
RUN groupadd --system appgroup \
 && useradd  --system --gid appgroup --home-dir /nonexistent --create-home appuser \
 && mkdir -p /app/logs

COPY --from=build /app/publish ./

# Fix ownership & permissions so non-root can run and log
RUN chown -R appuser:appgroup /app \
 && chmod -R 755 /app \
 && chmod 770 /app/logs

USER appuser

ENTRYPOINT ["dotnet", "CallerAPI.dll"]
