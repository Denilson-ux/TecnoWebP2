CREATE DATABASE IF NOT EXISTS `bdventa` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `bdventa`;

-- MySQL dump 10.13  Distrib 8.0.20, for Win64 (x86_64)
--
-- Host: localhost    Database: bdventa
-- ------------------------------------------------------
-- Server version	8.0.20

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structures and data
--

-- Table structure for table `tipo`
--
DROP TABLE IF EXISTS `tipo`;
/*!40101 SET @saved_cs_client      = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipo` (
  `id_tipo` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  PRIMARY KEY (`id_tipo`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipo`
--
LOCK TABLES `tipo` WRITE;
/*!40000 ALTER TABLE `tipo` DISABLE KEYS */;
INSERT INTO `tipo` VALUES (1,'Pequeño'),(2,'Mediano'),(3,'Grande'),(4,'extra grande');
/*!40000 ALTER TABLE `tipo` ENABLE KEYS */;
UNLOCK TABLES;

-- Table structure for table `productos`
--
DROP TABLE IF EXISTS `productos`;
/*!40101 SET @saved_cs_client      = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `productos` (
  `id_producto` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  `descripcion` varchar(255) DEFAULT NULL,
  `id_tipo` int NOT NULL,
  `precio` decimal(10,2) NOT NULL,
  `stock` int DEFAULT '0',
  PRIMARY KEY (`id_producto`),
  KEY `id_tipo` (`id_tipo`),
  CONSTRAINT `productos_ibfk_1` FOREIGN KEY (`id_tipo`) REFERENCES `tipo` (`id_tipo`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productos`
--
LOCK TABLES `productos` WRITE;
/*!40000 ALTER TABLE `productos` DISABLE KEYS */;
INSERT INTO `productos` VALUES (2,'Pizza Margarita','Salsa de tomate, mozzarella y albahaca',2,35.00,50),(3,'Pizza Margarita','Salsa de tomate, mozzarella y albahaca',3,45.00,50),(4,'Pizza Pepperoni','Salsa de tomate, mozzarella y pepperoni',1,30.00,40),(5,'Pizza Pepperoni','Salsa de tomate, mozzarella y pepperoni',2,40.00,40),(6,'Pizza Pepperoni','Salsa de tomate, mozzarella y pepperoni',3,50.00,40),(7,'Pizza Hawaiana','Salsa de tomate, mozzarella, jamón y piña',1,32.00,35),(8,'Pizza Hawaiana','Salsa de tomate, mozzarella, jamón y piña',2,42.00,35);
/*!40000 ALTER TABLE `productos` ENABLE KEYS */;
UNLOCK TABLES;

-- Table structure for table `clientes`
--
DROP TABLE IF EXISTS `clientes`;
/*!40101 SET @saved_cs_client      = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clientes` (
  `id_cliente` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `telefono` varchar(20) NOT NULL,
  `email` varchar(100) DEFAULT NULL,
  `direccion` varchar(255) DEFAULT NULL,
  `referencia` varchar(255) DEFAULT NULL,
  `zona` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_cliente`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clientes`
--
LOCK TABLES `clientes` WRITE;
/*!40000 ALTER TABLE `clientes` DISABLE KEYS */;
INSERT INTO `clientes` VALUES (1,'Juan','Pérez','70123456','juan@email.com','Av. Principal 123',NULL,'Centro'),(2,'María','García','71234567','maria@email.com','Calle Secundaria 456',NULL,'Norte'),(3,'Denilson','Quichu Llanos','67751732','denilsonquichu@gmail.com','Bolivia','ninguna',''),(4,'Denilson','Quichu Llanos','67751732','denilsonquichu@gmail.com','Bolivia','ninguna',''),(5,'Daniel','Ramos','67751732','denilsonquichu@gmail.com','Bolivia','','Sur');
/*!40000 ALTER TABLE `clientes` ENABLE KEYS */;
UNLOCK TABLES;

-- Table structure for table `ventas`
--
DROP TABLE IF EXISTS `ventas`;
/*!40101 SET @saved_cs_client      = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ventas` (
  `id_venta` int NOT NULL AUTO_INCREMENT,
  `numero_venta` varchar(50) NOT NULL,
  `fecha_venta` datetime DEFAULT CURRENT_TIMESTAMP,
  `id_cliente` int NOT NULL,
  `tipo_entrega` varchar(20) DEFAULT 'Local',
  `direccion_entrega` varchar(500) DEFAULT NULL,
  `subtotal` decimal(10,2) DEFAULT 0.00,
  `descuento` decimal(10,2) DEFAULT 0.00,
  `costo_delivery` decimal(10,2) DEFAULT 0.00,
  `total` decimal(10,2) NOT NULL,
  `metodo_pago` varchar(50) DEFAULT 'Efectivo',
  `estado_venta` varchar(20) DEFAULT 'Pendiente',
  `observaciones` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`id_venta`),
  UNIQUE KEY `numero_venta` (`numero_venta`),
  KEY `id_cliente` (`id_cliente`),
  CONSTRAINT `ventas_ibfk_1` FOREIGN KEY (`id_cliente`) REFERENCES `clientes` (`id_cliente`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

-- Table structure for table `detalle_ventas`
--
DROP TABLE IF EXISTS `detalle_ventas`;
/*!40101 SET @saved_cs_client      = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `detalle_ventas` (
  `id_detalle` int NOT NULL AUTO_INCREMENT,
  `id_venta` int NOT NULL,
  `id_producto` int NOT NULL,
  `id_tamanio` int DEFAULT 0,
  `cantidad` int NOT NULL,
  `precio_unitario` decimal(10,2) NOT NULL,
  `subtotal` decimal(10,2) NOT NULL,
  `notas_especiales` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`id_detalle`),
  KEY `id_venta` (`id_venta`),
  KEY `id_producto` (`id_producto`),
  CONSTRAINT `detalle_ventas_ibfk_1` FOREIGN KEY (`id_venta`) REFERENCES `ventas` (`id_venta`),
  CONSTRAINT `detalle_ventas_ibfk_2` FOREIGN KEY (`id_producto`) REFERENCES `productos` (`id_producto`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping routines for database 'bdventa'
--

-- =====================================================
-- PROCEDIMIENTOS ALMACENADOS PARA CLIENTES
-- =====================================================

DELIMITER ;;
DROP PROCEDURE IF EXISTS `actualizarCliente`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `actualizarCliente`(
  IN p_id_cliente INT,
  IN p_nombre VARCHAR(100),
  IN p_apellido VARCHAR(100),
  IN p_telefono VARCHAR(20),
  IN p_email VARCHAR(100),
  IN p_direccion VARCHAR(255),
  IN p_referencia VARCHAR(255),
  IN p_zona VARCHAR(50)
)
BEGIN
  UPDATE clientes
  SET nombre = p_nombre,
      apellido = p_apellido,
      telefono = p_telefono,
      email = p_email,
      direccion = p_direccion,
      referencia = p_referencia,
      zona = p_zona
  WHERE id_cliente = p_id_cliente;
END;;
DELIMITER ;

DELIMITER ;;
DROP PROCEDURE IF EXISTS `buscarCliente`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `buscarCliente`(
  IN buscar VARCHAR(50)
)
BEGIN
  SELECT 
    id_cliente,
    nombre,
    apellido,
    telefono,
    email,
    direccion,
    referencia,
    zona
  FROM clientes
  WHERE CONCAT(nombre, ' ', apellido) LIKE CONCAT('%', buscar, '%')
     OR telefono LIKE CONCAT('%', buscar, '%')
     OR buscar = '';
END;;
DELIMITER ;

DELIMITER ;;
DROP PROCEDURE IF EXISTS `eliminarCliente`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `eliminarCliente`(
  IN p_id_cliente INT
)
BEGIN
  DELETE FROM clientes
  WHERE id_cliente = p_id_cliente;
END;;
DELIMITER ;

DELIMITER ;;
DROP PROCEDURE IF EXISTS `insertarCliente`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `insertarCliente`(
  IN p_nombre VARCHAR(100),
  IN p_apellido VARCHAR(100),
  IN p_telefono VARCHAR(20),
  IN p_email VARCHAR(100),
  IN p_direccion VARCHAR(255),
  IN p_referencia VARCHAR(255),
  IN p_zona VARCHAR(50)
)
BEGIN
  INSERT INTO clientes (nombre, apellido, telefono, email, direccion, referencia, zona)
  VALUES (p_nombre, p_apellido, p_telefono, p_email, p_direccion, p_referencia, p_zona);
END;;
DELIMITER ;

-- =====================================================
-- PROCEDIMIENTOS ALMACENADOS PARA PRODUCTOS
-- =====================================================

DELIMITER ;;
DROP PROCEDURE IF EXISTS `actualizarProducto`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `actualizarProducto`(
  IN p_id_producto INT,
  IN p_nombre VARCHAR(100),
  IN p_descripcion VARCHAR(255),
  IN p_id_tipo INT,
  IN p_precio DECIMAL(10,2),
  IN p_stock INT
)
BEGIN
  UPDATE productos
  SET nombre = p_nombre,
      descripcion = p_descripcion,
      id_tipo = p_id_tipo,
      precio = p_precio,
      stock = p_stock
  WHERE id_producto = p_id_producto;
END;;
DELIMITER ;

DELIMITER ;;
DROP PROCEDURE IF EXISTS `buscarProducto`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `buscarProducto`(
  IN buscar VARCHAR(100)
)
BEGIN
  SELECT 
    p.id_producto,
    CONCAT('PIZ', LPAD(p.id_producto, 3, '0')) AS codigo_producto,
    p.nombre AS nombre_producto,
    t.nombre AS Nombre,
    p.precio AS precio_base,
    p.stock
  FROM productos p
  INNER JOIN tipo t ON p.id_tipo = t.id_tipo
  WHERE (buscar = '' OR p.nombre LIKE CONCAT('%', buscar, '%'));
END;;
DELIMITER ;

DELIMITER ;;
DROP PROCEDURE IF EXISTS `eliminarProducto`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `eliminarProducto`(
  IN p_id_producto INT
)
BEGIN
  DELETE FROM productos
  WHERE id_producto = p_id_producto;
END;;
DELIMITER ;

DELIMITER ;;
DROP PROCEDURE IF EXISTS `insertarProducto`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `insertarProducto`(
  IN p_nombre VARCHAR(100),
  IN p_descripcion VARCHAR(255),
  IN p_id_tipo INT,
  IN p_precio DECIMAL(10,2),
  IN p_stock INT
)
BEGIN
  INSERT INTO productos (nombre, descripcion, id_tipo, precio, stock)
  VALUES (p_nombre, p_descripcion, p_id_tipo, p_precio, p_stock);
END;;
DELIMITER ;

-- =====================================================
-- PROCEDIMIENTO PARA DESCONTAR STOCK
-- =====================================================

DELIMITER ;;
DROP PROCEDURE IF EXISTS `descontarStockProducto`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `descontarStockProducto`(
  IN p_id_producto INT,
  IN p_cantidad INT
)
BEGIN
  DECLARE stock_actual INT;
  
  -- Obtener el stock actual del producto
  SELECT stock INTO stock_actual
  FROM productos
  WHERE id_producto = p_id_producto;
  
  -- Verificar que hay suficiente stock
  IF stock_actual >= p_cantidad THEN
    -- Descontar la cantidad del stock
    UPDATE productos
    SET stock = stock - p_cantidad
    WHERE id_producto = p_id_producto;
  ELSE
    -- Si no hay suficiente stock, lanzar un error
    SIGNAL SQLSTATE '45000'
    SET MESSAGE_TEXT = 'Stock insuficiente para realizar la venta';
  END IF;
END;;
DELIMITER ;

-- =====================================================
-- PROCEDIMIENTOS ALMACENADOS PARA TIPOS
-- =====================================================

DELIMITER ;;
DROP PROCEDURE IF EXISTS `actualizarTipo`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `actualizarTipo`(
  IN p_id_tipo INT,
  IN p_nombre VARCHAR(50)
)
BEGIN
  UPDATE tipo
  SET nombre = p_nombre
  WHERE id_tipo = p_id_tipo;
END;;
DELIMITER ;

DELIMITER ;;
DROP PROCEDURE IF EXISTS `buscarTipo`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `buscarTipo`(
  IN p_nombre VARCHAR(50)
)
BEGIN
  IF p_nombre = '' OR p_nombre IS NULL THEN
    SELECT id_tipo, nombre
    FROM tipo
    ORDER BY nombre;
  ELSE
    SELECT id_tipo, nombre
    FROM tipo
    WHERE nombre LIKE CONCAT('%', p_nombre, '%')
    ORDER BY nombre;
  END IF;
END;;
DELIMITER ;

DELIMITER ;;
DROP PROCEDURE IF EXISTS `eliminarTipo`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `eliminarTipo`(
  IN p_id_tipo INT
)
BEGIN
  DELETE FROM tipo
  WHERE id_tipo = p_id_tipo;
END;;
DELIMITER ;

DELIMITER ;;
DROP PROCEDURE IF EXISTS `insertarTipo`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `insertarTipo`(
  IN p_nombre VARCHAR(50)
)
BEGIN
  INSERT INTO tipo (nombre)
  VALUES (p_nombre);
END;;
DELIMITER ;

-- =====================================================
-- PROCEDIMIENTOS ALMACENADOS PARA VENTAS
-- =====================================================

DELIMITER ;;
DROP PROCEDURE IF EXISTS `guardarVenta`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `guardarVenta`(
  IN num_ven VARCHAR(50),
  IN fec_ven DATETIME,
  IN id_cli INT,
  IN tip_ent VARCHAR(20),
  IN dir_ent VARCHAR(500),
  IN sub DECIMAL(10,2),
  IN descuento DECIMAL(10,2),
  IN del DECIMAL(10,2),
  IN tot DECIMAL(10,2),
  IN met_pag VARCHAR(50),
  IN est_ven VARCHAR(20),
  IN obs VARCHAR(500)
)
BEGIN
  INSERT INTO ventas (
    numero_venta,
    fecha_venta,
    id_cliente,
    tipo_entrega,
    direccion_entrega,
    subtotal,
    descuento,
    costo_delivery,
    total,
    metodo_pago,
    estado_venta,
    observaciones
  ) VALUES (
    num_ven,
    fec_ven,
    id_cli,
    tip_ent,
    dir_ent,
    sub,
    descuento,
    del,
    tot,
    met_pag,
    est_ven,
    obs
  );
END;;
DELIMITER ;

DELIMITER ;;
DROP PROCEDURE IF EXISTS `modificarVenta`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `modificarVenta`(
  IN id_ven INT,
  IN est_ven VARCHAR(20),
  IN obs VARCHAR(500)
)
BEGIN
  UPDATE ventas
  SET estado_venta = est_ven,
      observaciones = obs
  WHERE id_venta = id_ven;
END;;
DELIMITER ;

DELIMITER ;;
DROP PROCEDURE IF EXISTS `buscarVenta`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `buscarVenta`(
  IN buscar VARCHAR(50)
)
BEGIN
  SELECT 
    v.id_venta,
    v.numero_venta,
    v.fecha_venta,
    v.id_cliente,
    CONCAT(c.nombre, ' ', c.apellido) AS nombre_cliente,
    c.telefono,
    v.tipo_entrega,
    v.direccion_entrega,
    v.subtotal,
    v.descuento,
    v.costo_delivery,
    v.total,
    v.metodo_pago,
    v.estado_venta,
    v.observaciones
  FROM ventas v
  INNER JOIN clientes c ON v.id_cliente = c.id_cliente
  WHERE v.numero_venta LIKE CONCAT('%', buscar, '%')
     OR buscar = ''
  ORDER BY v.fecha_venta DESC;
END;;
DELIMITER ;

DELIMITER ;;
DROP PROCEDURE IF EXISTS `listarVentas`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `listarVentas`()
BEGIN
  SELECT 
    v.id_venta,
    v.numero_venta,
    v.fecha_venta,
    v.id_cliente,
    CONCAT(c.nombre, ' ', c.apellido) AS nombre_cliente,
    c.telefono,
    v.tipo_entrega,
    v.direccion_entrega,
    v.subtotal,
    v.descuento,
    v.costo_delivery,
    v.total,
    v.metodo_pago,
    v.estado_venta,
    v.observaciones
  FROM ventas v
  INNER JOIN clientes c ON v.id_cliente = c.id_cliente
  ORDER BY v.fecha_venta DESC;
END;;
DELIMITER ;

DELIMITER ;;
DROP PROCEDURE IF EXISTS `obtenerUltimaVenta`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `obtenerUltimaVenta`()
BEGIN
  SELECT id_venta
  FROM ventas
  ORDER BY id_venta DESC
  LIMIT 1;
END;;
DELIMITER ;

-- =====================================================
-- PROCEDIMIENTOS ALMACENADOS PARA DETALLE VENTAS
-- =====================================================

DELIMITER ;;
DROP PROCEDURE IF EXISTS `guardarDetalleVenta`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `guardarDetalleVenta`(
  IN id_ven INT,
  IN id_prod INT,
  IN id_tam INT,
  IN cant INT,
  IN precio DECIMAL(10,2),
  IN sub DECIMAL(10,2),
  IN notas VARCHAR(500)
)
BEGIN
  DECLARE stock_actual INT;
  
  -- Verificar el stock actual del producto
  SELECT stock INTO stock_actual
  FROM productos
  WHERE id_producto = id_prod;
  
  -- Verificar que hay suficiente stock
  IF stock_actual >= cant THEN
    -- Insertar el detalle de la venta
    INSERT INTO detalle_ventas (
      id_venta,
      id_producto,
      id_tamanio,
      cantidad,
      precio_unitario,
      subtotal,
      notas_especiales
    ) VALUES (
      id_ven,
      id_prod,
      id_tam,
      cant,
      precio,
      sub,
      notas
    );
    
    -- Descontar el stock del producto automáticamente
    UPDATE productos
    SET stock = stock - cant
    WHERE id_producto = id_prod;
  ELSE
    -- Si no hay suficiente stock, lanzar un error
    SIGNAL SQLSTATE '45000'
    SET MESSAGE_TEXT = 'Stock insuficiente para realizar la venta';
  END IF;
END;;
DELIMITER ;

DELIMITER ;;
DROP PROCEDURE IF EXISTS `listarDetallesPorVenta`;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `listarDetallesPorVenta`(
  IN id_ven INT
)
BEGIN
  SELECT 
    dv.id_detalle,
    dv.id_venta,
    dv.id_producto,
    p.nombre AS nombre_producto,
    dv.id_tamanio,
    dv.cantidad,
    dv.precio_unitario,
    dv.subtotal,
    dv.notas_especiales
  FROM detalle_ventas dv
  INNER JOIN productos p ON dv.id_producto = p.id_producto
  WHERE dv.id_venta = id_ven;
END;;
DELIMITER ;

/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed