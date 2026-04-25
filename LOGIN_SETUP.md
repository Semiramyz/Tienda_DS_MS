# 🚀 SETUP COMPLETADO - TIENDA DS MS CON DOCKER

## ✅ STATUS ACTUAL

La aplicación Tienda_DS_MS está **COMPLETAMENTE FUNCIONAL** con todas las bases de datos importadas desde el servidor local.

### Componentes Verificados:
- ✅ MySQL en Docker corriendo en puerto 3307
- ✅ 7 bases de datos creadas e importadas
- ✅ API .NET 8 corriendo en puerto 8080
- ✅ Frontend Angular corriendo en puerto 80 con Nginx
- ✅ Proxy Nginx funcionando correctamente

---

## 🔐 CREDENCIALES DE ACCESO

### Para Login en la Aplicación:

```
Email:    admin@tienda.com
Password: admin123
```

**Rol:** Administrador  
**Estado:** Activo ✓

---

## 📍 ACCESO A LA APLICACIÓN

### URLs Disponibles:

| Servicio | URL | Puerto |
|----------|-----|--------|
| **Frontend** | http://localhost | 80 |
| **API** | http://localhost:8080 | 8080 |
| **MySQL** (desde host) | localhost | 3307 |
| **Nginx Health** | http://localhost/health | 80 |

### Desde Docker Network:
- API: `http://tienda-api:8080`
- MySQL: `tienda-db:3306`

---

## 📊 BASES DE DATOS IMPORTADAS

```
✓ auth_db          (Usuarios y Autenticación)
✓ clientes_db      (Gestión de Clientes)
✓ proveedores_db   (Gestión de Proveedores)
✓ productos_db     (Inventario de Productos)
✓ ventas_db        (Registro de Ventas)
✓ facturas_db      (Emisión de Facturas)
✓ contabilidad_db  (Contabilidad y Movimientos)
```

Todas las tablas fueron migradas desde el servidor local MySQL (puerto 3306).

---

## 🧪 TESTING DEL LOGIN

### Opción 1: Frontend (Recomendado)
```
1. Abre: http://localhost
2. Ingresa:
   - Email: admin@tienda.com
   - Password: admin123
3. Presiona "Login"
4. Se redirigirá al Dashboard
```

### Opción 2: API Direct (cURL)
```bash
curl -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@tienda.com",
    "password": "admin123"
  }'
```

**Respuesta esperada:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "rol": "administrador",
  "expira": "2026-04-26T04:11:47Z"
}
```

### Opción 3: Postman
```
POST http://localhost:8080/api/auth/login
Content-Type: application/json

{
  "email": "admin@tienda.com",
  "password": "admin123"
}
```

---

## 🔑 JWT TOKEN INFORMATION

Los tokens JWT incluyen estos claims:
- `nameid`: ID del usuario (2)
- `name`: Nombre del usuario (Administrador)
- `email`: Email (admin@tienda.com)
- `role`: Rol (administrador)
- `exp`: Expiración (10 horas)
- `iss`: Issuer (TiendaMS)
- `aud`: Audience (TiendaFrontend)

---

## 🔧 ARQUITECTURA ACTUAL

```
┌─────────────────────┐
│   NGINX (80)        │  ◄── Frontend Angular
│  tienda-web:80      │      - Proxy /api/ → tienda-api:8080
└──────────┬──────────┘
           │
           │ /api/
           ▼
┌─────────────────────┐
│   .NET 8 API        │  ◄── Backend
│  tienda-api:8080    │      - Autenticación JWT
│                     │      - Microservicios
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│   MySQL 8.0         │  ◄── 7 Bases de Datos
│  mysql-db:3306      │      - auth_db, clientes_db, etc.
│  Port 3307 (host)   │
└─────────────────────┘
```

---

## 📝 PRÓXIMOS PASOS

### 1. Registrar nuevos usuarios
```
POST /api/auth/registro
Authorization: Bearer {token}
Content-Type: application/json

{
  "nombre": "Nuevo Usuario",
  "email": "usuario@tienda.com",
  "password": "password123",
  "rol": "vendedor"
}
```

### 2. Consumir otros endpoints
- `GET /api/clientes` - Obtener clientes
- `GET /api/productos` - Obtener productos
- `GET /api/ventas` - Obtener ventas
- `POST /api/clientes` - Crear cliente
- etc.

### 3. Agregar Interceptor de JWT en Angular
El `AuthService` ya almacena el token en `localStorage`. Se recomienda crear un interceptor HTTP que automáticamente añada el header `Authorization: Bearer {token}` a todas las peticiones.

---

## 🐛 TROUBLESHOOTING

### Error 500 en login
✓ **RESUELTO** - Las bases de datos ahora están correctamente importadas con los datos preexistentes.

### Connection refused
Si algún servicio no responde:
```bash
docker-compose ps
docker logs tienda-ds-api
docker logs tienda-ds-web
docker logs tienda-ds-mysql
```

### Limpiar y reiniciar
```bash
docker-compose down -v
docker-compose up -d --build
```

---

## 📦 ARCHIVOS GENERADOS

- `init-databases-complete.sql` - Script SQL con esquema + datos (estructura)
- `init-databases-full.sql` - Script con todos los datos importados (backup completo)
- `db_backups/` - Carpeta con backups individuales de cada base de datos
- `docker-compose.yml` - Actualizado para usar init-databases-full.sql

---

## ✨ ESTADO FINAL

```
✅ Docker Compose corriendo
✅ MySQL inicializado con datos
✅ API .NET respondiendo
✅ Frontend cargando
✅ Nginx proxy funcional
✅ Login verificado y funcionando
✅ JWT tokens generados correctamente
```

**¡La aplicación está lista para usar! 🎉**

---

*Actualizado: 2026-04-25*
*Proyecto: Tienda_DS_MS con Docker*
