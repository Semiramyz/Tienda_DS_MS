.PHONY: help up down logs ps rebuild clean restart shell db-shell test health build

# Default target
.DEFAULT_GOAL := help

# Colors
BLUE := \033[0;34m
GREEN := \033[0;32m
RED := \033[0;31m
YELLOW := \033[1;33m
NC := \033[0m # No Color

help: ## Show this help message
	@echo '$(BLUE)=== Tienda_DS_MS Docker Commands ===$(NC)'
	@echo ''
	@echo '$(GREEN)Usage: make [target]$(NC)'
	@echo ''
	@echo 'Targets:'
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "  $(GREEN)%-20s$(NC) %s\n", $$1, $$2}'
	@echo ''

up: ## Start all services
	@echo '$(BLUE)Starting services...$(NC)'
	docker-compose up -d
	@sleep 3
	@make ps

down: ## Stop all services
	@echo '$(BLUE)Stopping services...$(NC)'
	docker-compose down

logs: ## Show all logs (use: make logs SERVICE=tienda-api)
	@if [ -z "$(SERVICE)" ]; then \
		docker-compose logs -f; \
	else \
		docker-compose logs -f $(SERVICE); \
	fi

logs-api: ## Show API logs
	docker-compose logs -f tienda-api

logs-web: ## Show Web logs
	docker-compose logs -f tienda-web

logs-db: ## Show Database logs
	docker-compose logs -f mysql-db

ps: ## Show running containers
	docker-compose ps

build: ## Build images
	@echo '$(BLUE)Building images...$(NC)'
	docker-compose build

rebuild: ## Rebuild and start services
	@echo '$(BLUE)Rebuilding all services...$(NC)'
	docker-compose up -d --build
	@make ps

restart: ## Restart services (use: make restart SERVICE=tienda-api)
	@if [ -z "$(SERVICE)" ]; then \
		echo '$(BLUE)Restarting all services...$(NC)'; \
		docker-compose restart; \
	else \
		echo '$(BLUE)Restarting $(SERVICE)...$(NC)'; \
		docker-compose restart $(SERVICE); \
	fi

clean: ## Remove all containers and volumes
	@echo '$(YELLOW)Removing all containers and volumes...$(NC)'
	docker-compose down -v
	@echo '$(GREEN)Cleanup completed!$(NC)'

shell: ## Open shell in service (use: make shell SERVICE=tienda-api)
	@if [ -z "$(SERVICE)" ]; then \
		SERVICE=tienda-api; \
	fi; \
	docker-compose exec $$SERVICE bash

shell-api: ## Open shell in API container
	docker-compose exec tienda-api bash

shell-web: ## Open shell in Web container
	docker-compose exec tienda-web bash

db-shell: ## Connect to MySQL
	docker-compose exec mysql-db mysql -uroot -p

test: ## Run all health checks
	@make test-api
	@make test-web

test-api: ## Test API health
	@echo '$(BLUE)Testing API health...$(NC)'
	@curl -s http://localhost:8080/health && echo '$(GREEN) ✓ API is healthy$(NC)' || echo '$(RED) ✗ API is down$(NC)'

test-web: ## Test Web health
	@echo '$(BLUE)Testing Web health...$(NC)'
	@curl -s http://localhost/health && echo '$(GREEN) ✓ Web is healthy$(NC)' || echo '$(RED) ✗ Web is down$(NC)'

health: test ## Alias for test

status: ps ## Alias for ps

prune: ## Remove unused Docker resources
	@echo '$(YELLOW)Pruning Docker resources...$(NC)'
	docker system prune -f
	@echo '$(GREEN)Pruning completed!$(NC)'

backup-db: ## Backup MySQL database
	@echo '$(BLUE)Backing up database...$(NC)'
	docker-compose exec -T mysql-db mysqldump -uroot -p --all-databases > backup-$(shell date +%Y%m%d_%H%M%S).sql
	@echo '$(GREEN)Backup completed!$(NC)'

scale-api: ## Scale API instances (use: make scale-api INSTANCES=3)
	@if [ -z "$(INSTANCES)" ]; then \
		echo '$(RED)Error: INSTANCES not set$(NC)'; \
		echo 'Usage: make scale-api INSTANCES=3'; \
	else \
		echo '$(BLUE)Scaling tienda-api to $(INSTANCES) instances...$(NC)'; \
		docker-compose up -d --scale tienda-api=$(INSTANCES); \
		@make ps; \
	fi

compose-validate: ## Validate docker-compose.yml
	@echo '$(BLUE)Validating docker-compose.yml...$(NC)'
	docker-compose config --quiet
	@echo '$(GREEN)✓ docker-compose.yml is valid$(NC)'

pull: ## Pull latest images
	@echo '$(BLUE)Pulling latest images...$(NC)'
	docker-compose pull

push: ## Push images to registry (requires Docker login)
	@echo '$(BLUE)Pushing images...$(NC)'
	docker-compose push

dev: ## Start with development config
	docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d

prod: ## Start with production config
	docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d

version: ## Show Docker versions
	@echo '$(BLUE)Docker Version:$(NC)'
	docker --version
	@echo '$(BLUE)Docker Compose Version:$(NC)'
	docker-compose --version

.PHONY: all help
