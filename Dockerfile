# Utiliza la imagen base oficial de .NET para runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Utiliza la imagen base oficial de .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MiPortalNetCore/MiPortalNetCore.csproj", "MiPortalNetCore/"]
RUN dotnet restore "MiPortalNetCore/MiPortalNetCore.csproj"
COPY . .
WORKDIR "/src/MiPortalNetCore"
RUN dotnet build "MiPortalNetCore.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "MiPortalNetCore.csproj" -c Release -o /app/publish

# Configuración final para ejecutar la aplicación
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiPortalNetCore.dll"]
