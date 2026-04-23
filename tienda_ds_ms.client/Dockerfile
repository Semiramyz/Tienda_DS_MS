# Dockerfile para tienda_ds_ms.client (Angular)
# Etapa 1: Build
FROM node:20-alpine AS build
WORKDIR /app

# Copiar package.json y package-lock.json
COPY tienda_ds_ms.client/package*.json ./

# Instalar dependencias
RUN npm install

# Copiar código fuente
COPY tienda_ds_ms.client/ .

# Build de la aplicación Angular
RUN npm run build

# Etapa 2: Nginx (Serve)
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# Copiar configuración custom de nginx
COPY tienda_ds_ms.client/nginx.conf /etc/nginx/nginx.conf

# Copiar los archivos build de Angular
COPY --from=build /app/dist/tienda_ds_ms.client/browser .

# Exponer puerto
EXPOSE 80
EXPOSE 443

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=10s --retries=3 \
    CMD wget --quiet --tries=1 --spider http://localhost/index.html || exit 1

# Iniciar nginx
CMD ["nginx", "-g", "daemon off;"]
