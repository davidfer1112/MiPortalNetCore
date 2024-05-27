# Utiliza la imagen base oficial de .NET para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Utiliza la imagen base oficial de .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ProyectoArqui.csproj", "."]
RUN dotnet restore "ProyectoArqui.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "ProyectoArqui.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "ProyectoArqui.csproj" -c Release -o /app/publish

# Configuración final para ejecutar la aplicación
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProyectoArqui.dll"]
