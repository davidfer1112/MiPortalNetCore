# Documentación del Proyecto MiPortalNetCore

## Descripción

Este proyecto es una aplicación web desarrollada con .NET Core, diseñada para gestionar y visualizar información relevante para los usuarios. Utiliza diversas tecnologías y paquetes para ofrecer una experiencia completa, incluyendo acceso a bases de datos, envío de correos electrónicos y generación de documentación API.

## Requisitos

Para ejecutar este proyecto, necesitas tener instalado:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- Un sistema gestor de bases de datos compatible, como PostgreSQL o MySQL, dependiendo de la configuración del proyecto.

## Instalación

1. Clona el repositorio en tu máquina local usando Git:

```bash
git clone https://github.com/davidfer1112/miportalnetcore.git
```

2. Navega al directorio del proyecto:

```bash
cd miportalnetcore
```

3. Restaura los paquetes NuGet necesarios para el proyecto:
   
```bash
dotnet restore
```

## Configuración
Antes de ejecutar la aplicación, asegúrate de configurar las cadenas de conexión a la base de datos y cualquier otra configuración específica del entorno en el archivo appsettings.json o mediante variables de entorno.

## Ejecución
Para ejecutar el proyecto, utiliza el siguiente comando en la terminal:

```bash
dotnet run --project ProyectoArqui.csproj
```

Esto iniciará la aplicación en el puerto configurado, por defecto en http://localhost:5064.

## Uso
Una vez que la aplicación esté ejecutándose, puedes acceder a ella a través de un navegador web usando la dirección URL proporcionada por la consola.

Para interactuar con la API, puedes utilizar herramientas como Postman o acceder a la documentación Swagger generada automáticamente en:

http://localhost:5064/users
