# 🏗️ Arquitectura de Tienda_DS_MS Dockerizada

## Diagrama General

```
┌─────────────────────────────────────────────────────────────────────────┐
│                          TIENDA_DS_MS - ARQUITECTURA                    │
└─────────────────────────────────────────────────────────────────────────┘

                              🌍 CLIENTE
                                  │
                    ┌─────────────┴─────────────┐
                    │                           │
            ┌──────▼──────┐            ┌──────▼──────┐
            │ Navegador   │            │  REST CLI   │
            │  (Browser)  │            │  (cURL)     │
            └──────┬──────┘            └──────┬──────┘
                   │                          │
                   └──────────────┬───────────┘
                                  │
                   ┌──────────────▼───────────────┐
                   │                              │
                   │   NGINX REVERSE PROXY        │
                   │   (Port 80/443)              │
                   │   - Static files             │
                   │   - API routing to /api/     │
                   │   - Caching                  │
                   │   - Gzip compression         │
                   │                              │
                   └────────┬──────────────┬──────┘
                            │              │
        ┌───────────────────┘              └───────────────────┐
        │                                                      │
        ▼                                                      ▼
    Angular                                    .NET 8 API
    (Frontend)                                 (Backend)
    Port: 80                                   Port: 8080
    Container: tienda-ds-web                  Container: tienda-ds-api
    
    • Login Page                               • Auth Controller
    • Dashboard                                • Clientes Controller
    • Gestión Clientes                         • Proveedores Controller
    • Gestión Productos                        • Productos Controller
    • Gestión Proveedores                      • Ventas Controller
    • Gestión Ventas                           • Facturas Controller
    • Gestión Facturas                         • Contabilidad Controller
    • Gestión Contabilidad
    
        │                                              │
        └──────────────────┬──────────────────────────┘
                           │
                           ▼
                   ┌─────────────────┐
                   │  MySQL Database │
                   │   (Port 3306)   │
                   │ Container:      │
                   │ tienda-ds-mysql │
                   └────────┬────────┘
                            │
         ┌──────────────────┼──────────────────┐
         │                  │                  │
    ┌────▼────┐  ┌─────────▼─────────┐  ┌────▼────┐
    │ auth_db │  │  clientes_db      │  │products │
    │          │  │  proveedores_db   │  │_db      │
    │ JWT      │  │  ventas_db        │  │         │
    │ Users    │  │  facturas_db      │  │Products │
    └──────────┘  │  contabilidad_db  │  └─────────┘
                  └───────────────────┘
```

## Estructura de Contenedores

```
Docker Network: tienda-network

┌─────────────────────────────────────────────────┐
│         🐳 CONTENEDOR: tienda-ds-mysql          │
├─────────────────────────────────────────────────┤
│ Imagen: mysql:8.0                               │
│ Puerto: 3306:3306                               │
│ Volumen: mysql-data:/var/lib/mysql              │
│ Health: mysqladmin ping                         │
├─────────────────────────────────────────────────┤
│ Bases de datos:                                 │
│ • auth_db ................ Autenticación        │
│ • clientes_db ............ Gestión de clientes  │
│ • proveedores_db ......... Gestión proveedores  │
│ • productos_db ........... Gestión productos    │
│ • ventas_db .............. Gestión ventas       │
│ • facturas_db ............ Generación facturas  │
│ • contabilidad_db ........ Registros contables  │
└─────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────┐
│        🐳 CONTENEDOR: tienda-ds-api             │
├─────────────────────────────────────────────────┤
│ Imagen: .NET 8 (compilada)                      │
│ Puertos: 8080:8080, 8081:8081                   │
│ Health: curl /health                            │
├─────────────────────────────────────────────────┤
│ Componentes:                                    │
│ • AuthService .......... JWT + Autenticación    │
│ • ClienteService ....... CRUD Clientes          │
│ • ProveedorService ..... CRUD Proveedores       │
│ • ProductoService ...... CRUD Productos         │
│ • VentaService ......... CRUD Ventas            │
│ • FacturaService ....... Generación facturas    │
│ • ContabilidadService .. Registros contables    │
│ • DbContexts (7) ....... Acceso a datos         │
│ • JwtBearer Auth ....... Seguridad API          │
│ • Swagger/OpenAPI ...... Documentación API      │
└─────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────┐
│        🐳 CONTENEDOR: tienda-ds-web             │
├─────────────────────────────────────────────────┤
│ Imagen: nginx:alpine                            │
│ Puertos: 80:80, 443:443                         │
│ Health: curl /health                            │
├─────────────────────────────────────────────────┤
│ Funcionalidades:                                │
│ • Serve archivos estáticos Angular              │
│ • Proxy inverso a /api → API Backend            │
│ • Gzip compression                              │
│ • Caché de assets estáticos                     │
│ • SPA routing (index.html fallback)             │
│ • Health check endpoint                         │
└─────────────────────────────────────────────────┘
```

## Flujo de Peticiones

### Caso 1: Usuario accede a la aplicación

```
1. Usuario abre http://localhost
   │
2. Nginx (tienda-ds-web) sirve index.html de Angular
   │
3. Angular carga (JS, CSS, imágenes)
   │
4. App se inicializa en el navegador
   │
5. Usuario hace Login
   └─> POST /api/auth/login
       └─> Nginx proxy a http://tienda-api:8080/api/auth/login
           └─> API .NET retorna JWT token
               └─> Angular guarda token en localStorage
                   └─> Interceptor Auth añade header: Authorization: Bearer <token>
```

### Caso 2: Petición a la API

```
Cliente (Angular)
  │
  ├─> GET /api/clientes
  │   ├─ Header: Authorization: Bearer <token>
  │   └─ Header: Content-Type: application/json
  │
  └─> Nginx Reverse Proxy (tienda-ds-web:80)
      └─> Redirige a http://tienda-api:8080/api/clientes
          │
          └─> API .NET (tienda-ds-api:8080)
              ├─ Valida JWT
              ├─ Ejecuta AuthController → ClienteController
              ├─ Llama a ClienteService
              ├─ Consulta a ClientesDbContext
              └─> MySQL (tienda-ds-mysql:3306)
                  ├─ Database: clientes_db
                  ├─ Query: SELECT * FROM clientes
                  └─> Retorna resultados
                      │
                      └─> API retorna JSON
                          │
                          └─> Nginx lo proxea al cliente
                              │
                              └─> Angular recibe respuesta
                                  │
                                  └─> Renderiza en la UI
```

## Arquitectura de Microservicios

Cada módulo funciona de manera **independiente** pero integrada:

```
┌──────────────────────────────────────────────────┐
│              TIENDA_DS_MS MICROSERVICIOS           │
├──────────────────────────────────────────────────┤
│                                                  │
│  ┌─────────────────────────────────────────┐   │
│  │  Módulo: AUTENTICACIÓN                  │   │
│  ├─────────────────────────────────────────┤   │
│  │ • AuthService                           │   │
│  │ • AuthController                        │   │
│  │ • AuthDbContext                         │   │
│  │ • Database: auth_db                     │   │
│  │ Responsabilidad: JWT, Usuarios          │   │
│  └─────────────────────────────────────────┘   │
│                                                  │
│  ┌─────────────────────────────────────────┐   │
│  │  Módulo: GESTIÓN DE CLIENTES            │   │
│  ├─────────────────────────────────────────┤   │
│  │ • ClienteService                        │   │
│  │ • ClienteController                     │   │
│  │ • ClientesDbContext                     │   │
│  │ • Database: clientes_db                 │   │
│  │ Responsabilidad: CRUD Clientes          │   │
│  └─────────────────────────────────────────┘   │
│                                                  │
│  ┌─────────────────────────────────────────┐   │
│  │  Módulo: GESTIÓN DE PRODUCTOS           │   │
│  ├─────────────────────────────────────────┤   │
│  │ • ProductoService                       │   │
│  │ • ProductoController                    │   │
│  │ • ProductosDbContext                    │   │
│  │ • Database: productos_db                │   │
│  │ Responsabilidad: CRUD Productos         │   │
│  └─────────────────────────────────────────┘   │
│                                                  │
│  ┌─────────────────────────────────────────┐   │
│  │  Módulo: GESTIÓN DE PROVEEDORES         │   │
│  ├─────────────────────────────────────────┤   │
│  │ • ProveedorService                      │   │
│  │ • ProveedorController                   │   │
│  │ • ProveedoresDbContext                  │   │
│  │ • Database: proveedores_db              │   │
│  │ Responsabilidad: CRUD Proveedores       │   │
│  └─────────────────────────────────────────┘   │
│                                                  │
│  ┌─────────────────────────────────────────┐   │
│  │  Módulo: GESTIÓN DE VENTAS              │   │
│  ├─────────────────────────────────────────┤   │
│  │ • VentaService                          │   │
│  │ • VentaController                       │   │
│  │ • VentasDbContext                       │   │
│  │ • Database: ventas_db                   │   │
│  │ Responsabilidad: Registro de Ventas     │   │
│  └─────────────────────────────────────────┘   │
│                                                  │
│  ┌─────────────────────────────────────────┐   │
│  │  Módulo: GENERACIÓN DE FACTURAS         │   │
│  ├─────────────────────────────────────────┤   │
│  │ • FacturaService                        │   │
│  │ • FacturaController                     │   │
│  │ • FacturasDbContext                     │   │
│  │ • Database: facturas_db                 │   │
│  │ Responsabilidad: Generación Facturas    │   │
│  └─────────────────────────────────────────┘   │
│                                                  │
│  ┌─────────────────────────────────────────┐   │
│  │  Módulo: CONTABILIDAD                   │   │
│  ├─────────────────────────────────────────┤   │
│  │ • ContabilidadService                   │   │
│  │ • ContabilidadController                │   │
│  │ • ContabilidadDbContext                 │   │
│  │ • Database: contabilidad_db             │   │
│  │ Responsabilidad: Registros Contables    │   │
│  └─────────────────────────────────────────┘   │
│                                                  │
└──────────────────────────────────────────────────┘
```

## Stack Tecnológico

```
┌────────────────────────────────────────────────────┐
│           STACK TECNOLÓGICO COMPLETO               │
├────────────────────────────────────────────────────┤
│                                                    │
│ 🖥️  FRONTEND                                      │
│   • Angular 21 (TypeScript)                        │
│   • RxJS 7.8 (Reactive)                           │
│   • Angular Router                                 │
│   • HTTP Client                                    │
│   • Forms (Template-driven)                        │
│   • CSS/SCSS                                       │
│                                                    │
│ 🔧 BACKEND                                        │
│   • .NET 8 (C#)                                    │
│   • ASP.NET Core 8                                 │
│   • Entity Framework Core 8                        │
│   • JWT Bearer Authentication                      │
│   • Pomelo MySQL EF Core                           │
│   • Dependency Injection                           │
│   • CORS                                           │
│   • Swagger/OpenAPI                                │
│                                                    │
│ 🗄️  DATABASE                                      │
│   • MySQL 8.0                                      │
│   • 7 Databases Independientes (por dominio)       │
│   • utf8mb4 encoding                               │
│   • InnoDB storage engine                          │
│                                                    │
│ 🐳 CONTAINERIZATION                               │
│   • Docker 20.10+                                  │
│   • Docker Compose 1.29+                           │
│   • Multi-stage builds                             │
│   • Health checks                                  │
│   • Volume management                              │
│   • Network isolation                              │
│                                                    │
│ 🌐 NETWORKING                                     │
│   • Nginx 1.25 (Alpine)                            │
│   • Reverse Proxy                                  │
│   • Load Balancing                                 │
│   • Gzip Compression                               │
│   • Static file serving                            │
│   • SPA routing                                    │
│                                                    │
│ 🛠️  TOOLS & UTILITIES                             │
│   • Make (Makefile)                                │
│   • PowerShell (docker-manage.bat)                 │
│   • Bash (docker-manage.sh)                        │
│   • Git / GitHub                                   │
│   • GitHub Actions (CI/CD)                         │
│                                                    │
└────────────────────────────────────────────────────┘
```

## Ventajas de esta Arquitectura

✅ **Modularidad**: Cada servicio es independiente pero integrado
✅ **Escalabilidad**: Fácil escalar servicios individuales
✅ **Portabilidad**: Funciona igual en desarrollo, staging y producción
✅ **Aislamiento**: Cada módulo tiene su propia BD
✅ **Seguridad**: JWT para autenticación, CORS configurado
✅ **Observabilidad**: Health checks, logs centralizados
✅ **Velocidad**: Desarrollo local es idéntico a producción
✅ **Fácil deployment**: Docker Compose simplifica orchestración

---

**Última actualización**: Enero 2025
