#!/bin/bash
# QUICK REFERENCE - Tienda_DS_MS Docker Commands

echo "════════════════════════════════════════════════════════════"
echo "  TIENDA_DS_MS DOCKER - QUICK REFERENCE"
echo "════════════════════════════════════════════════════════════"
echo ""

# START
echo "[START] Iniciar Docker Compose:"
echo "  docker-compose up -d"
echo "  # O con rebuild:"
echo "  docker-compose up -d --build"
echo ""

# STOP
echo "[STOP] Detener Docker Compose:"
echo "  docker-compose down"
echo "  # O eliminar volúmenes también:"
echo "  docker-compose down -v"
echo ""

# STATUS
echo "[STATUS] Ver estado de containers:"
echo "  docker-compose ps"
echo ""

# LOGS
echo "[LOGS] Ver logs de servicios:"
echo "  docker logs tienda-ds-mysql"
echo "  docker logs tienda-ds-api"
echo "  docker logs tienda-ds-web"
echo ""

# DATABASE
echo "[DATABASE] Acceder a MySQL:"
echo "  docker exec -it tienda-ds-mysql mysql -u root -pMojang_24"
echo "  # Dentro de MySQL:"
echo "    SHOW DATABASES;"
echo "    USE auth_db;"
echo "    SELECT * FROM usuarios;"
echo ""

# API TEST
echo "[API] Probar login via API:"
echo "  curl -X POST http://localhost:8080/api/auth/login \\"
echo "    -H 'Content-Type: application/json' \\"
echo "    -d '{\"email\":\"admin@tienda.com\",\"password\":\"admin123\"}'"
echo ""

# HEALTH
echo "[HEALTH] Verificar health checks:"
echo "  curl http://localhost/health"
echo "  curl http://localhost:8080/api/auth/ping"
echo ""

# REBUILD
echo "[REBUILD] Reconstruir todo:"
echo "  docker-compose down -v"
echo "  docker-compose up -d --build"
echo ""

# SHELL ACCESS
echo "[SHELL] Acceso a containers:"
echo "  # API Shell:"
echo "    docker exec -it tienda-ds-api /bin/bash"
echo "  # Nginx Shell:"
echo "    docker exec -it tienda-ds-web /bin/sh"
echo "  # MySQL Shell:"
echo "    docker exec -it tienda-ds-mysql /bin/bash"
echo ""

# CREDENTIALS
echo "════════════════════════════════════════════════════════════"
echo "  CREDENCIALES"
echo "════════════════════════════════════════════════════════════"
echo "  Frontend:    http://localhost"
echo "  API:         http://localhost:8080"
echo "  MySQL:       localhost:3307"
echo ""
echo "  Email:       admin@tienda.com"
echo "  Password:    admin123"
echo "════════════════════════════════════════════════════════════"
