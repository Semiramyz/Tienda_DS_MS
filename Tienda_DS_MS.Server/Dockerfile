# Dockerfile para Tienda_DS_MS.Server (.NET 8)
# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos del proyecto
COPY ["Tienda_DS_MS.Server/Tienda_DS_MS.Server.csproj", "Tienda_DS_MS.Server/"]

# Restaurar dependencias
RUN cd Tienda_DS_MS.Server && dotnet restore "Tienda_DS_MS.Server.csproj"

# Copiar código fuente
COPY ["Tienda_DS_MS.Server/", "Tienda_DS_MS.Server/"]

# Build de la aplicación
WORKDIR "/src/Tienda_DS_MS.Server"
RUN dotnet build "Tienda_DS_MS.Server.csproj" -c Release -o /app/build

# Etapa 2: Publish
FROM build AS publish
WORKDIR "/src/Tienda_DS_MS.Server"
RUN dotnet publish "Tienda_DS_MS.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados
COPY --from=publish /app/publish .

# Exponer puerto
EXPOSE 8080
EXPOSE 8081

# Variables de entorno
ENV ASPNETCORE_URLS=http://+:8080;https://+:8081
ENV ASPNETCORE_ENVIRONMENT=Docker

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD dotnet /app/Tienda_DS_MS.Server.dll --version || exit 1

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "Tienda_DS_MS.Server.dll"]
