FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Configurar variables de entorno para NuGet
ENV NUGET_PACKAGES=/src/.nuget/packages
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1

# Copiar el archivo de solución
COPY ["EstiloLibre.sln", "./"]

# Copiar los archivos de proyecto
COPY ["EstiloLibre/EstiloLibre.csproj", "EstiloLibre/"]
COPY ["EstiloLibre_CapaNegocio/EstiloLibre_CapaNegocio.csproj", "EstiloLibre_CapaNegocio/"]

# Restaurar dependencias con la configuración limpia
RUN dotnet restore "EstiloLibre.sln" --configfile ./nuget.config --disable-parallel

# Copiar todo el código fuente (sin bin/obj gracias al .dockerignore)
COPY . .

# Compilar el proyecto principal
WORKDIR "/src/EstiloLibre"
RUN dotnet build "EstiloLibre.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "EstiloLibre.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Crear carpeta para adjuntos
RUN mkdir -p /app/Adjuntos

ENTRYPOINT ["dotnet", "EstiloLibre.dll"]