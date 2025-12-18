FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Person/Person.API/Person.API.csproj", "Services/Person/Person.API/"]
COPY ["BuildingBlocks/BuildingBlocks.API/BuildingBlocks.API.csproj", "BuildingBlocks/BuildingBlocks.API/"]
COPY ["Services/Person/Person.Application/Person.Application.csproj", "Services/Person/Person.Application/"]
COPY ["BuildingBlocks/BuildingBlocks.Application/BuildingBlocks.Application.csproj", "BuildingBlocks/BuildingBlocks.Application/"]
COPY ["BuildingBlocks/BuildingBlocks.Domain/BuildingBlocks.Domain.csproj", "BuildingBlocks/BuildingBlocks.Domain/"]
COPY ["Services/Person/Person.Domain/Person.Domain.csproj", "Services/Person/Person.Domain/"]
COPY ["Services/Person/Person.Infrastructure/Person.Infrastructure.csproj", "Services/Person/Person.Infrastructure/"]
COPY ["BuildingBlocks/BuildingBlocks.Infrastructure/BuildingBlocks.Infrastructure.csproj", "BuildingBlocks/BuildingBlocks.Infrastructure/"]
RUN dotnet restore "Services/Person/Person.API/Person.API.csproj"
COPY . .
WORKDIR "/src/Services/Person/Person.API"
RUN dotnet build "./Person.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Person.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Person.API.dll"]
