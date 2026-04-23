#!/bin/bash

# Script para gestionar la aplicación con Docker Compose
# Uso: ./docker-manage.sh [command]

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo_info() {
    echo -e "${BLUE}ℹ${NC} $1"
}

echo_success() {
    echo -e "${GREEN}✓${NC} $1"
}

echo_error() {
    echo -e "${RED}✗${NC} $1"
}

echo_warn() {
    echo -e "${YELLOW}⚠${NC} $1"
}

# Load environment
if [ -f ".env.docker" ]; then
    export $(cat .env.docker | grep -v '#' | xargs)
    echo_info "Environment loaded from .env.docker"
fi

# Commands
case "${1:-help}" in
    up)
        echo_info "Starting all services..."
        docker-compose up -d
        echo_success "Services started!"
        sleep 3
        echo_info "Checking health..."
        docker-compose ps
        ;;
    
    down)
        echo_info "Stopping all services..."
        docker-compose down
        echo_success "Services stopped!"
        ;;
    
    logs)
        SERVICE=${2:-}
        if [ -z "$SERVICE" ]; then
            docker-compose logs -f
        else
            docker-compose logs -f "$SERVICE"
        fi
        ;;
    
    ps|status)
        docker-compose ps
        ;;
    
    rebuild)
        echo_info "Rebuilding images..."
        docker-compose up -d --build
        echo_success "Images rebuilt and services started!"
        ;;
    
    clean)
        echo_warn "Removing all containers and volumes..."
        docker-compose down -v
        echo_success "Cleanup completed!"
        ;;
    
    restart)
        SERVICE=${2:-}
        if [ -z "$SERVICE" ]; then
            echo_info "Restarting all services..."
            docker-compose restart
        else
            echo_info "Restarting $SERVICE..."
            docker-compose restart "$SERVICE"
        fi
        echo_success "Done!"
        ;;
    
    shell)
        SERVICE=${2:-tienda-api}
        echo_info "Opening shell in $SERVICE..."
        docker-compose exec "$SERVICE" /bin/bash
        ;;
    
    logs-api)
        docker-compose logs -f tienda-api
        ;;
    
    logs-web)
        docker-compose logs -f tienda-web
        ;;
    
    logs-db)
        docker-compose logs -f mysql-db
        ;;
    
    db-shell)
        echo_info "Connecting to MySQL..."
        docker-compose exec mysql-db mysql -uroot -p"${MYSQL_ROOT_PASSWORD}"
        ;;
    
    test-api)
        echo_info "Testing API health..."
        curl -s http://localhost:8080/health || echo_error "API health check failed"
        ;;
    
    test-web)
        echo_info "Testing Web health..."
        curl -s http://localhost/health || echo_error "Web health check failed"
        ;;
    
    help|*)
        cat << EOF
${BLUE}Tienda_DS_MS Docker Manager${NC}

Usage: ./docker-manage.sh [command] [options]

Commands:
    ${GREEN}up${NC}              Start all services
    ${GREEN}down${NC}            Stop all services
    ${GREEN}logs${NC}            Show logs (all or specific service)
    ${GREEN}logs-api${NC}        Show API logs
    ${GREEN}logs-web${NC}        Show Web logs
    ${GREEN}logs-db${NC}         Show Database logs
    ${GREEN}ps${NC}              Show running containers
    ${GREEN}status${NC}          Alias for ps
    ${GREEN}rebuild${NC}         Rebuild and start services
    ${GREEN}clean${NC}           Remove all containers and volumes
    ${GREEN}restart${NC}         Restart services
    ${GREEN}shell${NC}           Open bash in a service
    ${GREEN}db-shell${NC}        Connect to MySQL
    ${GREEN}test-api${NC}        Test API health
    ${GREEN}test-web${NC}        Test Web health
    ${GREEN}help${NC}            Show this help message

Examples:
    ./docker-manage.sh up
    ./docker-manage.sh logs tienda-api
    ./docker-manage.sh shell tienda-api
    ./docker-manage.sh restart tienda-web
    ./docker-manage.sh clean

EOF
        ;;
esac
