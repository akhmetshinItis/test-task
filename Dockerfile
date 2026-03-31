FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

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

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vitacore.Test.Web.dll"]
