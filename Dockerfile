FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN apt-get update \
    && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Vitacore.Test.Web/Vitacore.Test.Web.csproj", "src/Vitacore.Test.Web/"]
COPY ["src/Vitacore.Test.Contracts/Vitacore.Test.Contracts.csproj", "src/Vitacore.Test.Contracts/"]
COPY ["src/Vitacore.Test.Core/Vitacore.Test.Core.csproj", "src/Vitacore.Test.Core/"]
COPY ["src/Vitacore.Test.Data.Postgres/Vitacore.Test.Data.Postgres.csproj", "src/Vitacore.Test.Data.Postgres/"]
COPY ["src/Vitacore.Test.Infrastructure/Vitacore.Test.Infrastructure.csproj", "src/Vitacore.Test.Infrastructure/"]
COPY ["src/Vitacore.Test.Migrator/Vitacore.Test.Migrator.csproj", "src/Vitacore.Test.Migrator/"]
RUN dotnet restore "src/Vitacore.Test.Web/Vitacore.Test.Web.csproj"
COPY . .
WORKDIR "/src/src/Vitacore.Test.Web"
RUN dotnet build "./Vitacore.Test.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Vitacore.Test.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM build AS publish-migrator
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/src/Vitacore.Test.Migrator"
RUN dotnet publish "./Vitacore.Test.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/publish-migrator /p:UseAppHost=false

FROM base AS web
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vitacore.Test.Web.dll"]

FROM base AS migrator
WORKDIR /app
COPY --from=publish-migrator /app/publish-migrator .
ENTRYPOINT ["dotnet", "Vitacore.Test.Migrator.dll"]
