-- ============================================================================
-- TIENDA_DS_MS - INIT ALL DATABASES
-- ============================================================================
-- This script initializes all 7 databases with proper structure
-- ============================================================================

SET NAMES utf8mb4;
SET SESSION sql_mode='STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- ============================================================================
-- 1. AUTH DATABASE
-- ============================================================================
CREATE DATABASE IF NOT EXISTS auth_db CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE auth_db;

DROP TABLE IF EXISTS usuarios;
CREATE TABLE usuarios (
  id int NOT NULL AUTO_INCREMENT,
  nombre varchar(100) NOT NULL,
  email varchar(150) NOT NULL,
  password_hash varchar(256) NOT NULL,
  rol varchar(20) NOT NULL DEFAULT 'administrador',
  activo tinyint(1) DEFAULT 1,
  fecha_creacion datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  UNIQUE KEY email (email),
  KEY rol (rol)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO usuarios (id, nombre, email, password_hash, rol, activo, fecha_creacion) VALUES 
(2, 'Administrador', 'admin@tienda.com', '$2a$11$drzH57uesfck.LT0qc.PGOc6epTx72yTa47QxW7iE79pwiKZf6AV.', 'administrador', 1, '2026-03-09 18:15:13');

-- ============================================================================
-- 2. CLIENTES DATABASE
-- ============================================================================
CREATE DATABASE IF NOT EXISTS clientes_db CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE clientes_db;

DROP TABLE IF EXISTS clientes;
CREATE TABLE clientes (
  id int NOT NULL AUTO_INCREMENT,
  nombre varchar(150) NOT NULL,
  nit varchar(20) NOT NULL UNIQUE,
  email varchar(150),
  telefono varchar(20),
  direccion varchar(300),
  fecha_registro datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  KEY idx_nombre (nombre)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ============================================================================
-- 3. PROVEEDORES DATABASE
-- ============================================================================
CREATE DATABASE IF NOT EXISTS proveedores_db CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE proveedores_db;

DROP TABLE IF EXISTS proveedores;
CREATE TABLE proveedores (
  id int NOT NULL AUTO_INCREMENT,
  nombre_empresa varchar(200) NOT NULL,
  contacto varchar(150),
  nit varchar(20) NOT NULL UNIQUE,
  email varchar(150),
  telefono varchar(20),
  direccion varchar(300),
  fecha_registro datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  KEY idx_nombre_empresa (nombre_empresa)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ============================================================================
-- 4. PRODUCTOS DATABASE
-- ============================================================================
CREATE DATABASE IF NOT EXISTS productos_db CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE productos_db;

DROP TABLE IF EXISTS productos;
CREATE TABLE productos (
  id int NOT NULL AUTO_INCREMENT,
  nombre varchar(200) NOT NULL,
  descripcion text,
  precio_compra decimal(10,2) DEFAULT 0,
  precio_venta decimal(10,2) DEFAULT 0,
  stock int DEFAULT 0,
  proveedor_id int,
  activo tinyint(1) DEFAULT 1,
  fecha_registro datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  KEY idx_nombre (nombre),
  KEY idx_activo (activo),
  KEY idx_proveedor_id (proveedor_id)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ============================================================================
-- 5. VENTAS DATABASE
-- ============================================================================
CREATE DATABASE IF NOT EXISTS ventas_db CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE ventas_db;

DROP TABLE IF EXISTS venta_detalle;
DROP TABLE IF EXISTS ventas;

CREATE TABLE ventas (
  id int NOT NULL AUTO_INCREMENT,
  cliente_id int NOT NULL,
  usuario_id int NOT NULL,
  fecha datetime DEFAULT CURRENT_TIMESTAMP,
  subtotal decimal(12,2) DEFAULT 0,
  impuesto decimal(12,2) DEFAULT 0,
  total decimal(12,2) DEFAULT 0,
  estado varchar(20) DEFAULT 'completada',
  PRIMARY KEY (id),
  KEY idx_cliente_id (cliente_id),
  KEY idx_usuario_id (usuario_id),
  KEY idx_fecha (fecha),
  KEY idx_estado (estado)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE venta_detalle (
  id int NOT NULL AUTO_INCREMENT,
  venta_id int NOT NULL,
  producto_id int NOT NULL,
  nombre_producto varchar(200),
  cantidad int NOT NULL,
  precio_unitario decimal(10,2) NOT NULL,
  subtotal decimal(12,2) NOT NULL,
  PRIMARY KEY (id),
  KEY idx_venta_id (venta_id),
  KEY idx_producto_id (producto_id),
  FOREIGN KEY (venta_id) REFERENCES ventas(id) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ============================================================================
-- 6. FACTURAS DATABASE
-- ============================================================================
CREATE DATABASE IF NOT EXISTS facturas_db CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE facturas_db;

DROP TABLE IF EXISTS facturas;
CREATE TABLE facturas (
  id int NOT NULL AUTO_INCREMENT,
  numero_factura varchar(50) NOT NULL UNIQUE,
  venta_id int NOT NULL,
  cliente_id int NOT NULL,
  nombre_cliente varchar(150),
  nit_cliente varchar(20),
  fecha_emision datetime DEFAULT CURRENT_TIMESTAMP,
  subtotal decimal(12,2) DEFAULT 0,
  impuesto decimal(12,2) DEFAULT 0,
  total decimal(12,2) DEFAULT 0,
  estado varchar(20) DEFAULT 'emitida',
  PRIMARY KEY (id),
  KEY idx_venta_id (venta_id),
  KEY idx_cliente_id (cliente_id),
  KEY idx_fecha_emision (fecha_emision),
  KEY idx_estado (estado)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ============================================================================
-- 7. CONTABILIDAD DATABASE
-- ============================================================================
CREATE DATABASE IF NOT EXISTS contabilidad_db CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE contabilidad_db;

DROP TABLE IF EXISTS resumen_diario;
DROP TABLE IF EXISTS movimientos;

CREATE TABLE movimientos (
  id int NOT NULL AUTO_INCREMENT,
  tipo varchar(10) NOT NULL,
  referencia_id int,
  descripcion varchar(300),
  monto decimal(12,2) NOT NULL,
  fecha datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  KEY idx_tipo (tipo),
  KEY idx_referencia_id (referencia_id),
  KEY idx_fecha (fecha)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE resumen_diario (
  id int NOT NULL AUTO_INCREMENT,
  fecha date NOT NULL,
  ingresos decimal(12,2) DEFAULT 0,
  egresos decimal(12,2) DEFAULT 0,
  saldo decimal(12,2) DEFAULT 0,
  PRIMARY KEY (id),
  UNIQUE KEY idx_fecha (fecha)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ============================================================================
-- VERIFY ALL DATABASES CREATED
-- ============================================================================
-- SELECT 'Databases created:' as status;
-- SHOW DATABASES LIKE '%_db';

SET FOREIGN_KEY_CHECKS=1;
