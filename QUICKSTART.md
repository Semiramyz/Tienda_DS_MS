# 🚀 Quick Start - Tienda_DS_MS con Docker

## ⏱️ 5 minutos para ejecutar la aplicación

### 1️⃣ Pre-requisitos (Si no los tienes)
```bash
# Instalar Docker
# Descargar: https://www.docker.com/products/docker-desktop

# Verificar instalación
docker --version
docker-compose --version
```

### 2️⃣ Clonar repositorio
```bash
git clone https://github.com/Semiramyz/Tienda_DS_MS.git
cd Tienda_DS_MS
```

### 3️⃣ Ejecutar (Elige una opción)

**Opción A: Comando simple (Recomendado)**
```bash
docker-compose up -d
```

**Opción B: Con script**
```bash
# Linux/Mac
chmod +x docker-manage.sh
./docker-manage.sh up

# Windows
docker-manage.bat up
```

**Opción C: Con Makefile**
```bash
make up
```

### 4️⃣ Esperar 10-15 segundos mientras se descargan imágenes y se inician servicios

### 5️⃣ ¡Listo! Accede a:

| Servicio | URL | 
|----------|-----|
| 🌐 **Frontend** | [http://localhost](http://localhost) |
| 📚 **Swagger API** | [http://localhost:8080/swagger](http://localhost:8080/swagger) |
| 🗄️ **MySQL** | `localhost:3306` |

---

## 🎯 Acciones Comunes

### Ver estado
```bash
docker-compose ps
# o
make ps
```

### Ver logs en tiempo real
```bash
docker-compose logs -f
# o
make logs
```

### Detener todo
```bash
docker-compose down
# o
make down
```

### Acceder a MySQL
```bash
docker-compose exec mysql-db mysql -uroot -p
# Contraseña: Mojang_24
```

### Reiniciar un servicio
```bash
docker-compose restart tienda-api
# o
make restart SERVICE=tienda-api
```

---

## 📊 Verificar que todo está funcionando

```bash
# Test del API
curl http://localhost:8080/health

# Test del Web
curl http://localhost/health

# Ver contenedores
docker-compose ps
```

Salida esperada:
```
NAME              STATUS      PORTS
tienda-ds-api     Up 2 min    0.0.0.0:8080->8080/tcp
tienda-ds-web     Up 2 min    0.0.0.0:80->80/tcp
tienda-ds-mysql   Up 3 min    0.0.0.0:3306->3306/tcp
```

---

## 🆘 Problemas comunes

### ❌ "Port 80 is already in use"
```bash
# Cambiar en docker-compose.yml
# De: "80:80"
# A: "8000:80"

docker-compose up -d
```

### ❌ "Cannot connect to MySQL"
```bash
# Esperar a que MySQL esté listo
docker-compose logs mysql-db | tail -20

# Reiniciar
docker-compose restart mysql-db
```

### ❌ "Frontend no carga"
```bash
# Ver logs del web
docker-compose logs tienda-web

# Reiniciar
docker-compose restart tienda-web
```

### ❌ Limpiar todo y empezar de nuevo
```bash
docker-compose down -v
docker-compose up -d --build
```

---

## 📖 Documentación completa

- **Despliegue detallado**: Ver `DEPLOYMENT.md`
- **Testing de API**: Ver `API_TESTING.md`
- **Guía Docker**: Ver `README.DOCKER.md`

---

## 🎓 Próximos pasos

1. **Login** en http://localhost (credenciales: revisar servidor)
2. **Explorar** la aplicación
3. **Probar** endpoints en Swagger
4. **Consultar** logs para debugging
5. **Leer** documentación para configuración avanzada

---

## 💡 Tips

- Cambiar puertos en `docker-compose.yml` si necesitas
- Editar credenciales en `.env.docker`
- Ver comandos disponibles con `make help`
- Hacer backups antes de cambios importantes

---

## ✅ ¡Todo listo!

La aplicación completa está corriendo con:
- ✓ API .NET 8 en microservicios
- ✓ Frontend Angular 21
- ✓ MySQL con 7 bases de datos independientes
- ✓ Proxy Nginx automático
- ✓ Health checks automáticos

¿Necesitas ayuda? 👉 Revisa los logs:
```bash
make logs
```

**¡Bienvenido a Tienda_DS_MS con Docker! 🎉**
