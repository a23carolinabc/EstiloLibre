FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo de solución
COPY ["EstiloLibre.sln", "./"]

# Copiar los archivos de proyecto
COPY ["EstiloLibre/EstiloLibre.csproj", "EstiloLibre/"]
COPY ["EstiloLibre_CapaNegocio/EstiloLibre_CapaNegocio.csproj", "EstiloLibre_CapaNegocio/"]

# Restaurar dependencias para toda la solución
RUN dotnet restore "EstiloLibre.sln"

# Copiar todo el código fuente
COPY . .

# Compilar el proyecto principal
WORKDIR "/src/EstiloLibre"
RUN dotnet build "EstiloLibre.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EstiloLibre.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EstiloLibre.dll"]