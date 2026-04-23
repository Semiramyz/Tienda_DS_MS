-- Script para crear todas las bases de datos necesarias

-- Crear base de datos de autenticación
CREATE DATABASE IF NOT EXISTS auth_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Crear base de datos de clientes
CREATE DATABASE IF NOT EXISTS clientes_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Crear base de datos de proveedores
CREATE DATABASE IF NOT EXISTS proveedores_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Crear base de datos de productos
CREATE DATABASE IF NOT EXISTS productos_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Crear base de datos de ventas
CREATE DATABASE IF NOT EXISTS ventas_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Crear base de datos de facturas
CREATE DATABASE IF NOT EXISTS facturas_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Crear base de datos de contabilidad
CREATE DATABASE IF NOT EXISTS contabilidad_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Seleccionar la base de datos auth_db para crear la tabla de usuarios
USE auth_db;

-- Crear tabla de usuarios con los campos correctos
CREATE TABLE IF NOT EXISTS usuarios (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    passwordHash VARCHAR(255) NOT NULL,
    rol VARCHAR(50) DEFAULT 'usuario',
    activo BOOLEAN DEFAULT 1,
    fechaCreacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Insertar usuarios de prueba
-- Contraseña: admin123 (encriptada con BCrypt)
INSERT IGNORE INTO usuarios (nombre, email, passwordHash, rol, activo) VALUES
('Admin', 'admin@tienda.com', '$2a$11$3CpRfbhqSKTnPXJOTNQPUucKdS3CjJQ7R.sQvJ3w.sP1qS7Sh2e/e', 'administrador', 1),
('Usuario', 'usuario@tienda.com', '$2a$11$3CpRfbhqSKTnPXJOTNQPUucKdS3CjJQ7R.sQvJ3w.sP1qS7Sh2e/e', 'usuario', 1);

-- Otorgar permisos al usuario root
GRANT ALL PRIVILEGES ON *.* TO 'root'@'%';
FLUSH PRIVILEGES;
