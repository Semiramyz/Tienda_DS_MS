# Dockerización de Tienda_DS_MS

Este documento describe cómo ejecutar la aplicación Tienda_DS_MS usando Docker y Docker Compose.

## 📋 Requisitos

- Docker 20.10+
- Docker Compose 1.29+
- Git

## 🏗️ Estructura de la Aplicación

La aplicación está dividida en los siguientes servicios:

```
tienda-ds-network
├── mysql-db (Puerto 3306)
│   └── 7 bases de datos independientes
├── tienda-api (Puerto 8080/8081)
│   └── API REST .NET 8 con microservicios
└── tienda-web (Puerto 80/443)
    └── Frontend Angular 21
```

## 🚀 Inicio Rápido

### 1. Ejecutar todos los servicios juntos

```bash
# Usar el archivo .env.docker
docker-compose --env-file .env.docker up -d

# O simplemente
docker-compose up -d
```

### 2. Ejecutar servicios individuales

#### Iniciar solo la base de datos:
```bash
docker-compose up -d mysql-db
```

#### Iniciar solo el API:
```bash
docker-compose up -d tienda-api
```

#### Iniciar solo el Web:
```bash
docker-compose up -d tienda-web
```

## 📍 URLs de Acceso

- **Frontend (Web)**: http://localhost
- **API**: http://localhost:8080
- **MySQL**: localhost:3306
- **Health Check API**: http://localhost:8080/health

## 🔧 Configuración

### Variables de Entorno

Edita el archivo `.env.docker`:

```
MYSQL_ROOT_PASSWORD=Mojang_24
JWT_KEY=ClaveSuperSecretaParaProduccionDe256BitsAWS!!2025
ASPNETCORE_ENVIRONMENT=Docker
```

### Configurar para Desarrollo

Para desarrollo con logs más detallados:

```bash
docker-compose -f docker-compose.yml -f docker-compose.dev.yml up
```

## 📊 Monitoreo

### Ver logs de todos los servicios:
```bash
docker-compose logs -f
```

### Ver logs de un servicio específico:
```bash
docker-compose logs -f tienda-api
```

### Ver estado de los contenedores:
```bash
docker-compose ps
```

### Health check:
```bash
curl http://localhost:8080/health
curl http://localhost/health
```

## 🛠️ Comandos Útiles

### Detener todos los servicios:
```bash
docker-compose down
```

### Detener y eliminar volúmenes (limpiar datos):
```bash
docker-compose down -v
```

### Reconstruir imágenes:
```bash
docker-compose up -d --build
```

### Ejecutar bash en un contenedor:
```bash
docker exec -it tienda-ds-api bash
docker exec -it tienda-ds-mysql mysql -uroot -pMojang_24
```

## 🔄 Escalamiento

### Escalar el API a múltiples instancias (con load balancer):
```bash
docker-compose up -d --scale tienda-api=3
```

## 🐛 Troubleshooting

### El API no puede conectarse a MySQL
```bash
# Verificar que MySQL está listo
docker-compose ps
# Verificar logs de MySQL
docker-compose logs mysql-db
# Esperar unos segundos y reintentar
```

### El Frontend no carga
```bash
# Verificar que nginx está corriendo
docker exec tienda-ds-web ps aux | grep nginx
# Verificar logs
docker-compose logs tienda-web
```

### Limpiar todo y comenzar de nuevo
```bash
docker-compose down -v
docker system prune -a
docker-compose up -d --build
```

## 🏢 Microservicios Incluidos

El API .NET 8 expone los siguientes controladores:

- **AuthController** - Autenticación y JWT
- **ClientesController** - Gestión de clientes
- **ProveedoresController** - Gestión de proveedores
- **ProductosController** - Gestión de productos
- **VentasController** - Gestión de ventas
- **FacturasController** - Generación de facturas
- **ContabilidadController** - Registros contables

Cada microservicio tiene su propia base de datos MySQL independiente.

## 📦 Bases de Datos

```
MySQL (puerto 3306)
├── auth_db (Autenticación)
├── clientes_db (Clientes)
├── proveedores_db (Proveedores)
├── productos_db (Productos)
├── ventas_db (Ventas)
├── facturas_db (Facturas)
└── contabilidad_db (Contabilidad)
```

## 🔐 Seguridad en Producción

Para producción, cambiar:

1. **Contraseña de MySQL** - En `.env.docker`
2. **JWT Key** - Generar una nueva clave más segura
3. **CORS** - Configurar dominios permitidos en `appsettings.Docker.json`
4. **SSL/TLS** - Configurar certificados en nginx

## 📝 Logs de Build

Ver los procesos de compilación:

```bash
# Build con verbose
docker-compose build --verbose

# Ver el historial de construcción
docker image history tienda-ds-api
docker image history tienda-ds-web
```

## 🚀 Despliegue en Producción

### Usar un archivo docker-compose.prod.yml:
```bash
docker-compose -f docker-compose.prod.yml up -d
```

### Con Kubernetes:
```bash
kubectl apply -f k8s/deployment.yaml
```

## 📚 Referencias

- [Docker Documentation](https://docs.docker.com)
- [Docker Compose Documentation](https://docs.docker.com/compose)
- [.NET in Docker](https://docs.microsoft.com/dotnet/docker)
- [Angular in Docker](https://angular.io/guide/build)
