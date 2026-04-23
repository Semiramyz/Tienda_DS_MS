@echo off
REM Script para gestionar la aplicación con Docker Compose en Windows
REM Uso: docker-manage.bat [command]

setlocal enabledelayedexpansion

if "%1"=="" goto help
if "%1"=="help" goto help
if "%1"=="up" goto up
if "%1"=="down" goto down
if "%1"=="logs" goto logs
if "%1"=="ps" goto ps
if "%1"=="status" goto ps
if "%1"=="rebuild" goto rebuild
if "%1"=="clean" goto clean
if "%1"=="restart" goto restart
if "%1"=="shell" goto shell
if "%1"=="logs-api" goto logs-api
if "%1"=="logs-web" goto logs-web
if "%1"=="logs-db" goto logs-db
if "%1"=="db-shell" goto db-shell
if "%1"=="test-api" goto test-api
if "%1"=="test-web" goto test-web

:help
echo.
echo Tienda_DS_MS Docker Manager
echo.
echo Usage: docker-manage.bat [command] [options]
echo.
echo Commands:
echo   up              Start all services
echo   down            Stop all services
echo   logs            Show logs (all or specific service)
echo   logs-api        Show API logs
echo   logs-web        Show Web logs
echo   logs-db         Show Database logs
echo   ps              Show running containers
echo   status          Alias for ps
echo   rebuild         Rebuild and start services
echo   clean           Remove all containers and volumes
echo   restart         Restart services
echo   shell           Open cmd in a service
echo   db-shell        Connect to MySQL
echo   test-api        Test API health
echo   test-web        Test Web health
echo   help            Show this help message
echo.
echo Examples:
echo   docker-manage.bat up
echo   docker-manage.bat logs tienda-api
echo   docker-manage.bat rebuild
echo.
goto end

:up
echo [*] Starting all services...
docker-compose up -d
echo [+] Services started!
echo [*] Checking health...
timeout /t 3
docker-compose ps
goto end

:down
echo [*] Stopping all services...
docker-compose down
echo [+] Services stopped!
goto end

:logs
if "%2"=="" (
    docker-compose logs -f
) else (
    docker-compose logs -f %2
)
goto end

:ps
docker-compose ps
goto end

:rebuild
echo [*] Rebuilding images...
docker-compose up -d --build
echo [+] Images rebuilt and services started!
goto end

:clean
echo [!] Removing all containers and volumes...
docker-compose down -v
echo [+] Cleanup completed!
goto end

:restart
if "%2"=="" (
    echo [*] Restarting all services...
    docker-compose restart
) else (
    echo [*] Restarting %2...
    docker-compose restart %2
)
echo [+] Done!
goto end

:shell
if "%2"=="" (
    set service=tienda-api
) else (
    set service=%2
)
echo [*] Opening cmd in !service!...
docker-compose exec !service! cmd
goto end

:logs-api
docker-compose logs -f tienda-api
goto end

:logs-web
docker-compose logs -f tienda-web
goto end

:logs-db
docker-compose logs -f mysql-db
goto end

:db-shell
echo [*] Connecting to MySQL...
docker-compose exec mysql-db mysql -uroot -p
goto end

:test-api
echo [*] Testing API health...
curl http://localhost:8080/health
goto end

:test-web
echo [*] Testing Web health...
curl http://localhost/health
goto end

:end
