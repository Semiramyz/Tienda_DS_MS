# Quick API Test Commands for Tienda_DS_MS

## 🧪 Testing con cURL

### Health Checks
```bash
# API health
curl -i http://localhost:8080/health

# Web health
curl -i http://localhost/health
```

### Authentication
```bash
# Login
curl -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "usuario": "admin",
    "contrasena": "password123"
  }'

# Con JWT token
TOKEN="your-jwt-token-here"
curl -H "Authorization: Bearer $TOKEN" \
  http://localhost:8080/api/clientes
```

### Clientes
```bash
# Obtener todos los clientes
curl http://localhost:8080/api/clientes

# Obtener cliente por ID
curl http://localhost:8080/api/clientes/1

# Crear cliente
curl -X POST http://localhost:8080/api/clientes \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Cliente Test",
    "email": "test@example.com",
    "telefono": "+1234567890",
    "estado": true
  }'

# Actualizar cliente
curl -X PUT http://localhost:8080/api/clientes/1 \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Cliente Actualizado",
    "estado": true
  }'

# Eliminar cliente
curl -X DELETE http://localhost:8080/api/clientes/1
```

### Productos
```bash
# Obtener productos
curl http://localhost:8080/api/productos

# Crear producto
curl -X POST http://localhost:8080/api/productos \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Producto Test",
    "descripcion": "Descripción del producto",
    "precio": 99.99,
    "stock": 100,
    "activo": true
  }'
```

### Proveedores
```bash
# Obtener proveedores
curl http://localhost:8080/api/proveedores

# Crear proveedor
curl -X POST http://localhost:8080/api/proveedores \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Proveedor Test",
    "email": "proveedor@example.com",
    "telefono": "+1234567890",
    "estado": true
  }'
```

### Ventas
```bash
# Obtener ventas
curl http://localhost:8080/api/ventas

# Crear venta
curl -X POST http://localhost:8080/api/ventas \
  -H "Content-Type: application/json" \
  -d '{
    "clienteId": 1,
    "fecha": "2025-01-15",
    "total": 299.99,
    "estado": "pendiente"
  }'
```

### Facturas
```bash
# Obtener facturas
curl http://localhost:8080/api/facturas

# Generar factura
curl -X POST http://localhost:8080/api/facturas \
  -H "Content-Type: application/json" \
  -d '{
    "ventaId": 1,
    "serie": "FAC",
    "numero": "0001"
  }'
```

### Contabilidad
```bash
# Obtener movimientos
curl http://localhost:8080/api/contabilidad/movimientos

# Registrar movimiento
curl -X POST http://localhost:8080/api/contabilidad/movimientos \
  -H "Content-Type: application/json" \
  -d '{
    "tipo": "ingreso",
    "descripcion": "Venta registrada",
    "monto": 100.00,
    "fecha": "2025-01-15"
  }'

# Obtener resumen diario
curl http://localhost:8080/api/contabilidad/resumen-diario?fecha=2025-01-15
```

## 📡 Testing con Postman

1. **Importar colección:**
   - Abrir Postman
   - Ir a `File → Import`
   - Seleccionar `tienda-postman-collection.json`

2. **Configurar variables de entorno:**
   - `{{base_url}}` = `http://localhost:8080`
   - `{{token}}` = Token JWT obtenido del login
   - `{{client_id}}` = ID de cliente de prueba

## 🧹 Script de pruebas automatizadas

```bash
#!/bin/bash

API_URL="http://localhost:8080"

echo "🧪 Iniciando pruebas de API..."

# Health check
echo "✓ Health check..."
curl -s "$API_URL/health" > /dev/null && echo "  ✓ API respondiendo" || echo "  ✗ API no responde"

# Verificar Swagger
echo "✓ Verificando Swagger..."
curl -s "$API_URL/swagger" > /dev/null && echo "  ✓ Swagger disponible" || echo "  ✗ Swagger no disponible"

# Listar endpoints
echo "✓ Obteniendo endpoints..."
curl -s "$API_URL/swagger/v1/swagger.json" | jq '.paths | keys[]' 2>/dev/null | head -10

echo "✓ Pruebas completadas!"
```

## 🔍 Debugging

### Ver headers de respuesta
```bash
curl -i http://localhost:8080/api/clientes
```

### Ver body completo con formato
```bash
curl -s http://localhost:8080/api/clientes | jq .
```

### Seguir redirects
```bash
curl -L http://localhost:8080/api/endpoint
```

### Ver información de timing
```bash
curl -w "@curl-format.txt" -o /dev/null -s http://localhost:8080/api/clientes
```

### Enviar datos con archivo
```bash
curl -F "file=@/path/to/file.csv" http://localhost:8080/api/import
```

## 📊 Performance Testing

### con Apache Bench
```bash
# 100 peticiones, 10 concurrentes
ab -n 100 -c 10 http://localhost:8080/api/clientes
```

### con wrk
```bash
# 4 threads, 100 conexiones, 30 segundos
wrk -t4 -c100 -d30s http://localhost:8080/api/clientes
```

## 🔐 Testing de Seguridad

### Verificar CORS
```bash
curl -H "Origin: http://localhost" \
  -H "Access-Control-Request-Method: POST" \
  -H "Access-Control-Request-Headers: Content-Type" \
  -X OPTIONS http://localhost:8080/api/clientes -v
```

### Verificar SSL/TLS (en producción)
```bash
curl -v https://localhost:8081/api/clientes
```

### Testing de inyección SQL
```bash
curl "http://localhost:8080/api/clientes?search=1' OR '1'='1"
```

## 📝 Crear archivo .rest para VS Code

Instalar extensión "REST Client" y crear archivo `api-tests.rest`:

```
### Variables
@baseUrl = http://localhost:8080
@token = your-jwt-token

### Health Check
GET @baseUrl/health

### Login
POST @baseUrl/api/auth/login
Content-Type: application/json

{
  "usuario": "admin",
  "contrasena": "password"
}

### Get Clientes
GET @baseUrl/api/clientes
Authorization: Bearer @token

### Create Cliente
POST @baseUrl/api/clientes
Content-Type: application/json
Authorization: Bearer @token

{
  "nombre": "Nuevo Cliente",
  "email": "nuevo@example.com",
  "telefono": "+1234567890"
}
```

Luego presionar "Send Request" sobre cada endpoint.

---

Para más información, consulta la documentación en Swagger: http://localhost:8080/swagger
