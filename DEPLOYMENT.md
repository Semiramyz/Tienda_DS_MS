# 🚀 Guía de Despliegue de Tienda_DS_MS con Docker

## 📋 Arquitectura de Microservicios

```
┌─────────────────────────────────────────────────────────┐
│                    TIENDA_DS_MS                         │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────────────────────────────────────────────┐  │
│  │   Frontend (Angular 21)                          │  │
│  │   Container: tienda-ds-web                       │  │
│  │   Ports: 80, 443                                 │  │
│  │   Tech: Nginx, Node.js                          │  │
│  └──────────────────────────────────────────────────┘  │
│                        ↓                                │
│  ┌──────────────────────────────────────────────────┐  │
│  │   API Gateway (Nginx Proxy)                      │  │
│  │   Routes /api → Backend                          │  │
│  └──────────────────────────────────────────────────┘  │
│                        ↓                                │
│  ┌──────────────────────────────────────────────────┐  │
│  │   Backend (.NET 8 API)                           │  │
│  │   Container: tienda-ds-api                       │  │
│  │   Ports: 8080, 8081                              │  │
│  │   Services: Auth, Clientes, Productos, etc.     │  │
│  └──────────────────────────────────────────────────┘  │
│                        ↓                                │
│  ┌──────────────────────────────────────────────────┐  │
│  │   MySQL Database Cluster                         │  │
│  │   Container: tienda-ds-mysql                     │  │
│  │   Port: 3306                                     │  │
│  │   Databases: 7 independientes (por dominio)      │  │
│  └──────────────────────────────────────────────────┘  │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

## 🛠️ Pre-requisitos

- **Docker 20.10+** - [Descargar](https://www.docker.com/products/docker-desktop)
- **Docker Compose 1.29+** - Incluido en Docker Desktop
- **Git** - Para clonar el repositorio
- **6GB RAM** mínimo para desarrollo
- **20GB almacenamiento** para imágenes y volúmenes

## 📦 Instalación

### 1. Clonar el repositorio

```bash
git clone https://github.com/Semiramyz/Tienda_DS_MS.git
cd Tienda_DS_MS
```

### 2. Configurar variables de entorno

```bash
# Copiar archivo de ejemplo
cp .env.docker .env.docker.local

# Editar con tus valores
nano .env.docker.local  # o usa tu editor favorito
```

**Variables importantes:**
```
MYSQL_ROOT_PASSWORD=TuContraseñaSegura123!
JWT_KEY=TuClaveJWTMayorA256Bits!
ASPNETCORE_ENVIRONMENT=Docker
```

### 3. Construir e iniciar

```bash
# Opción 1: Con Docker Compose directamente
docker-compose up -d

# Opción 2: Con el script de gestión (recomendado)
chmod +x docker-manage.sh  # En Linux/Mac
./docker-manage.sh up
```

## ✅ Verificación

### Verificar que todos los servicios están corriendo

```bash
docker-compose ps
```

Salida esperada:
```
NAME                COMMAND                  SERVICE      STATUS      PORTS
tienda-ds-api       "dotnet Tienda_DS_MS…"  tienda-api   Up 2 mins   0.0.0.0:8080->8080/tcp
tienda-ds-mysql     "docker-entrypoint.s…"  mysql-db     Up 3 mins   0.0.0.0:3306->3306/tcp
tienda-ds-web       "nginx -g daemon off;…" tienda-web   Up 2 mins   0.0.0.0:80->80/tcp
```

### Verificar health checks

```bash
# API health
curl http://localhost:8080/health

# Web health
curl http://localhost/health

# Swagger API
open http://localhost:8080/swagger
```

## 🌐 Acceso a la Aplicación

| Componente | URL | Usuario/Pass |
|-----------|-----|--------------|
| Frontend | http://localhost | - |
| API Swagger | http://localhost:8080/swagger | - |
| MySQL | localhost:3306 | root / Mojang_24 |
| API Direct | http://localhost:8080 | - |

## 🔄 Operaciones Diarias

### Iniciar servicios

```bash
./docker-manage.sh up
# o
docker-compose up -d
```

### Detener servicios

```bash
./docker-manage.sh down
# o
docker-compose down
```

### Ver logs en tiempo real

```bash
# Todos los servicios
./docker-manage.sh logs

# Solo API
./docker-manage.sh logs-api

# Solo Web
./docker-manage.sh logs-web

# Solo BD
./docker-manage.sh logs-db
```

### Reiniciar un servicio

```bash
./docker-manage.sh restart tienda-api
# o
docker-compose restart tienda-api
```

### Acceder a la terminal de un contenedor

```bash
# Bash en API
./docker-manage.sh shell tienda-api

# MySQL CLI
./docker-manage.sh db-shell
```

## 🗂️ Estructura de Directorios

```
Tienda_DS_MS/
├── Tienda_DS_MS.Server/
│   ├── Dockerfile              # Imagen del API
│   ├── appsettings.Docker.json # Config para Docker
│   ├── .dockerignore
│   └── [resto del código]
├── tienda_ds_ms.client/
│   ├── Dockerfile              # Imagen del Frontend
│   ├── nginx.conf              # Configuración Nginx
│   ├── .dockerignore
│   └── [resto del código]
├── docker-compose.yml          # Orquestación principal
├── docker-compose.dev.yml      # Overrides para desarrollo
├── docker-compose.prod.yml     # Overrides para producción
├── docker-manage.sh            # Script de gestión (Linux/Mac)
├── docker-manage.bat           # Script de gestión (Windows)
├── init-databases.sql          # Inicialización BD
├── .env.docker                 # Variables de entorno
└── README.DOCKER.md            # Documentación
```

## 📊 Escalamiento

### Escalar el API a múltiples instancias

```bash
# Escalar a 3 instancias
docker-compose up -d --scale tienda-api=3

# Ver todas las instancias
docker-compose ps | grep tienda-api
```

**Nota:** Con Nginx como reverse proxy, las peticiones se distribuirán automáticamente.

## 🔐 Seguridad en Producción

### 1. Cambiar contraseñas por defecto

```bash
# Editar .env.docker
MYSQL_ROOT_PASSWORD=ContraseñaMuySegura123!@#
JWT_KEY=GenerarConSeguridad256Bits!@#$%
```

### 2. Configurar CORS apropiadamente

En `appsettings.Docker.json`:
```json
"Cors": {
  "AllowedOrigins": [
    "https://midominio.com",
    "https://www.midominio.com"
  ]
}
```

### 3. Usar certificados SSL

```bash
# Copiar certificado y clave a tienda_ds_ms.client/
cp /ruta/certificado.crt tienda_ds_ms.client/
cp /ruta/clave.key tienda_ds_ms.client/

# Actualizar nginx.conf para usar HTTPS
```

### 4. Implementar un Reverse Proxy externo

Usar Traefik o Nginx Proxy con Let's Encrypt para SSL.

## 🐛 Troubleshooting

### El API no conecta a MySQL

```bash
# Verificar logs
docker-compose logs mysql-db

# Verificar conexión
docker-compose exec mysql-db mysql -uroot -p

# Esperar a que MySQL esté listo (health check)
docker-compose exec tienda-api dotnet /app/Tienda_DS_MS.Server.dll
```

### El Frontend no carga

```bash
# Verificar Nginx
docker-compose logs tienda-web

# Verificar archivos
docker-compose exec tienda-web ls -la /usr/share/nginx/html/

# Acceder al contenedor
docker-compose exec tienda-web bash
```

### Puerto ya está en uso

```bash
# Cambiar puerto en docker-compose.yml
# De: "80:80"
# A: "8000:80"

# O liberar el puerto
# Linux/Mac: lsof -i :80
# Windows: netstat -ano | findstr :80
```

### Limpiar todo y empezar de nuevo

```bash
# Detener y eliminar todo
./docker-manage.sh clean

# Reconstruir desde cero
./docker-manage.sh rebuild
```

## 📈 Monitoreo

### Ver uso de recursos

```bash
docker stats
```

### Ver estado de la BD

```bash
docker-compose exec mysql-db mysql -uroot -p -e "SHOW DATABASES;"
```

### Backup de la BD

```bash
# Crear backup
docker-compose exec mysql-db mysqldump -uroot -p --all-databases > backup.sql

# Restaurar backup
docker-compose exec -T mysql-db mysql -uroot -p < backup.sql
```

## 🚀 Despliegue en Producción

### Opción 1: En el mismo servidor

```bash
# Usar configuración de producción
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d

# Con variables de entorno seguras
MYSQL_ROOT_PASSWORD=*** JWT_KEY=*** docker-compose up -d
```

### Opción 2: En la nube

#### AWS ECS
```bash
# Crear task definition
aws ecs register-task-definition --cli-input-json file://task-definition.json

# Crear servicio
aws ecs create-service --cluster tienda-cluster --service-name tienda-svc ...
```

#### Azure Container Instances
```bash
az container create --resource-group tienda-rg \
  --name tienda-app \
  --image myregistry.azurecr.io/tienda-api:latest
```

#### DigitalOcean App Platform
```bash
doctl apps create --spec app.yaml
```

## 📚 Variables de Entorno Completas

```bash
# MySQL
MYSQL_ROOT_PASSWORD=Mojang_24
MYSQL_DATABASE=auth_db

# JWT
JWT_KEY=ClaveSuperSecretaParaProduccionDe256BitsAWS!!2025

# Ambiente
ASPNETCORE_ENVIRONMENT=Docker
ASPNETCORE_URLS=http://+:8080;https://+:8081

# Logging
Logging__LogLevel__Default=Information

# CORS
CORS__AllowedOrigins__0=http://localhost
CORS__AllowedOrigins__1=http://tienda-web
```

## 🔗 Links Útiles

- [Docker Documentation](https://docs.docker.com)
- [Docker Compose Documentation](https://docs.docker.com/compose)
- [.NET in Docker](https://docs.microsoft.com/dotnet/docker)
- [MySQL in Docker](https://hub.docker.com/_/mysql)
- [Nginx Documentation](https://nginx.org/en/docs/)

## ✨ Tips y Mejores Prácticas

1. **Siempre usar volúmenes para datos persistentes**
   ```bash
   volumes:
     - mysql-data:/var/lib/mysql
   ```

2. **Health checks para mayor confiabilidad**
   ```yaml
   healthcheck:
     test: ["CMD", "curl", "-f", "http://localhost/health"]
     interval: 30s
     timeout: 10s
     retries: 3
   ```

3. **No hardcodear credenciales**
   - Usar .env
   - Usar Docker Secrets en Swarm
   - Usar AWS Secrets Manager en ECS

4. **Logs estructurados para debugging**
   - Enviar a ELK Stack
   - Usar CloudWatch en AWS
   - Integrar con Datadog

5. **Automatizar backups de la BD**
   ```bash
   # Cron job
   0 2 * * * docker-compose exec mysql-db mysqldump -uroot -p > backup-$(date +%Y%m%d).sql
   ```

---

**¿Necesitas ayuda?** Revisa los logs:
```bash
./docker-manage.sh logs
```

**Última actualización:** 2025
