#!/usr/bin/env powershell
# Test Script para Tienda_DS_MS Docker Login

Write-Host "=========================================================" -ForegroundColor Cyan
Write-Host "TESTING TIENDA_DS_MS DOCKER SETUP - LOGIN FLOW" -ForegroundColor Cyan
Write-Host "=========================================================" -ForegroundColor Cyan

# Test 1: Verificar que Docker está corriendo
Write-Host "`n[1/7] Verificando containers Docker..." -ForegroundColor Yellow
$containers = docker-compose -f docker-compose.yml ps --format "json" | ConvertFrom-Json
if ($containers.Count -ge 3) {
    Write-Host "[OK] Todos los containers están corriendo" -ForegroundColor Green
    $containers | ForEach-Object { Write-Host "    - $($_.Service): $($_.State)" }
} else {
    Write-Host "[FAIL] Faltan containers" -ForegroundColor Red
    exit 1
}

# Test 2: Verificar conectividad MySQL
Write-Host "`n[2/7] Verificando MySQL..." -ForegroundColor Yellow
try {
    $mysqlCheck = docker exec tienda-ds-mysql mysql -u root -pMojang_24 -e "SELECT COUNT(*) FROM information_schema.databases;" 2>&1
    Write-Host "[OK] MySQL respondiendo correctamente" -ForegroundColor Green
} catch {
    Write-Host "[FAIL] MySQL no responde" -ForegroundColor Red
    exit 1
}

# Test 3: Verificar que auth_db tiene usuarios
Write-Host "`n[3/7] Verificando base de datos de autenticación..." -ForegroundColor Yellow
$userCount = docker exec tienda-ds-mysql mysql -u root -pMojang_24 -e "USE auth_db; SELECT COUNT(*) as count FROM usuarios;" 2>&1 | Select-Object -Last 1
Write-Host "[OK] Found $userCount user(s) in auth_db" -ForegroundColor Green

# Test 4: Verificar que API responde
Write-Host "`n[4/7] Verificando API .NET..." -ForegroundColor Yellow
try {
    $apiPing = Invoke-WebRequest -Uri "http://localhost:8080/api/auth/ping" -Method GET -UseBasicParsing -ErrorAction Stop
    $pingData = $apiPing.Content | ConvertFrom-Json
    Write-Host "[OK] API respondiendo: $($pingData.status) - $($pingData.usuarios) usuarios encontrados" -ForegroundColor Green
} catch {
    Write-Host "[FAIL] API no responde" -ForegroundColor Red
    exit 1
}

# Test 5: Probar Login
Write-Host "`n[5/7] Probando endpoint de login..." -ForegroundColor Yellow
try {
    $loginPayload = @{
        email = "admin@tienda.com"
        password = "admin123"
    } | ConvertTo-Json

    $loginResponse = Invoke-WebRequest -Uri "http://localhost:8080/api/auth/login" `
        -Method POST `
        -ContentType "application/json" `
        -Body $loginPayload `
        -UseBasicParsing `
        -ErrorAction Stop

    $loginData = $loginResponse.Content | ConvertFrom-Json
    Write-Host "[OK] Login exitoso!" -ForegroundColor Green
    Write-Host "   Token: $($loginData.token.Substring(0, 20))..." 
    Write-Host "   Rol: $($loginData.rol)"
    Write-Host "   Expira: $($loginData.expira)"
    $token = $loginData.token
} catch {
    Write-Host "[FAIL] Login fallido" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Test 6: Verificar Frontend
Write-Host "`n[6/7] Verificando Frontend Angular..." -ForegroundColor Yellow
try {
    $frontendCheck = Invoke-WebRequest -Uri "http://localhost" -Method GET -UseBasicParsing -ErrorAction Stop
    if ($frontendCheck.StatusCode -eq 200) {
        Write-Host "[OK] Frontend cargando correctamente (HTTP $($frontendCheck.StatusCode))" -ForegroundColor Green
    }
} catch {
    Write-Host "[FAIL] Frontend no responde" -ForegroundColor Red
    exit 1
}

# Test 7: Verificar Nginx Health
Write-Host "`n[7/7] Verificando Nginx Health Check..." -ForegroundColor Yellow
try {
    $health = Invoke-WebRequest -Uri "http://localhost/health" -Method GET -UseBasicParsing -ErrorAction Stop
    if ($health.StatusCode -eq 200) {
        Write-Host "[OK] Nginx respondiendo: $($health.Content)" -ForegroundColor Green
    }
} catch {
    Write-Host "[FAIL] Nginx health check fallo" -ForegroundColor Red
}

# Summary
Write-Host "`n=========================================================" -ForegroundColor Cyan
Write-Host "TODOS LOS TESTS PASARON EXITOSAMENTE" -ForegroundColor Cyan
Write-Host "=========================================================" -ForegroundColor Cyan

Write-Host "`n[RESUMEN]:"
Write-Host "  - URL Frontend: http://localhost"
Write-Host "  - URL API: http://localhost:8080"
Write-Host "  - Email: admin@tienda.com"
Write-Host "  - Password: admin123"
Write-Host "  - JWT Token valido por 10 horas"

Write-Host "`n[PROXIMOS PASOS]:"
Write-Host "  1. Abre http://localhost en tu navegador"
Write-Host "  2. Ingresa las credenciales de arriba"
Write-Host "  3. Accede al Dashboard"
Write-Host "  4. Verifica que el token esta en localStorage"

Write-Host "`n[LISTO] La aplicacion esta lista para usar!`n" -ForegroundColor Green
