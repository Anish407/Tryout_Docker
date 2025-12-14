# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# docker build -t callerapiwithsample34 -f CallerAPI/Dockerfile .
# docker run -d -p 5012:5002 --name callerapiwithsample35 callerapiwithsample34
# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CallerAPI/CallerAPI.csproj", "CallerAPI/"]
COPY ["CallerApi.Infra/CallerApi.Infra.csproj", "CallerApi.Infra/"]
COPY ["CalllerApi.Core/CalllerApi.Core.csproj", "CalllerApi.Core/"]
RUN dotnet restore "./CallerAPI/CallerAPI.csproj"
COPY . .
WORKDIR "/src/CallerAPI"
#RUN dotnet build "./CallerAPI.csproj" -c $BUILD_CONFIGURATION 

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
RUN dotnet publish "./CallerAPI.csproj"  -o /app/publish /p:UseAppHost=false --no-restore 


# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5002
EXPOSE 8081


# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
ENV AspnetCoreEnvironment=development
ENV ASPNETCORE_HTTP_PORTS=5002
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CallerAPI.dll"]