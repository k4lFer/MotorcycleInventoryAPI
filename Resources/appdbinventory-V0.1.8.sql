--create database `dbinventory`;
--USE `dbinventory`;
--CHARACTER SET utf8mb4;
--COLLATE utf8mb4_spanish_ci;

-- Tabla de propietarios (admins/gerentes)
CREATE TABLE `owners` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `username` VARCHAR(50) UNIQUE NOT NULL,
    `password` VARCHAR(255) NOT NULL,
    `firstName` VARCHAR(255) NOT NULL,
    `lastName` VARCHAR(255) NOT NULL,
    `email` VARCHAR(255) UNIQUE NOT NULL,
    `dni` VARCHAR(255) UNIQUE NOT NULL,
    `ruc` VARCHAR(255) UNIQUE,
    `address` VARCHAR(100) NOT NULL,
    `phoneNumber` VARCHAR(30) UNIQUE NOT NULL,
    `role` ENUM('Admin', 'Manager') NOT NULL,
    `status` TINYINT(1) NOT NULL,
    `profilePictureUrl` TEXT NULL,
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `createdBy` CHAR(36),
    `updatedBy` CHAR(36)
) ENGINE=InnoDB;

-- Tabla de usuarios (clientes)
CREATE TABLE `users` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `firstName` VARCHAR(100) NOT NULL,
    `lastName` VARCHAR(100) NOT NULL,
    `documentType` VARCHAR(30),
    `documentNumber` VARCHAR(30) UNIQUE NOT NULL,
    `email` VARCHAR(100) UNIQUE NOT NULL,
    `phoneNumber` VARCHAR(50) NULL,
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Tabla de tipos de motocicletas
CREATE TABLE `types` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `name` VARCHAR(255) NOT NULL,
    `description` TEXT,
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Tabla de marcas de motocicletas
CREATE TABLE `brands` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `name` VARCHAR(255) NOT NULL,
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Tabla de motocicletas (modelos)
CREATE TABLE `motorcycles` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `typeId` CHAR(36) NOT NULL,
    `brandId` CHAR(36) NOT NULL,
    `name` VARCHAR(255) NOT NULL,
    `displacement` VARCHAR(255) NOT NULL,
    `price` DECIMAL(10, 2) NOT NULL,
    `status` ENUM('available', 'not_available') NOT NULL,
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (`typeId`) REFERENCES `types`(`id`),
    FOREIGN KEY (`brandId`) REFERENCES `brands`(`id`)
) ENGINE=InnoDB;

-- Tabla de unidades de motocicletas (cada una con su VIN)
CREATE TABLE `motorcycle_units` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `motorcycleId` CHAR(36) NOT NULL,
    `vin` VARCHAR(17) UNIQUE NOT NULL, -- Código VIN
    `status` ENUM('available', 'sold', 'reserved', 'maintenance') NOT NULL,
    `purchaseDate` DATE NOT NULL, -- Fecha de compra al proveedor
    `purchasePrice` DECIMAL(10, 2) NOT NULL, -- Precio de compra
    `salePrice` DECIMAL(10, 2), -- Precio de venta (si se vende)
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (`motorcycleId`) REFERENCES `motorcycles`(`id`)
) ENGINE=InnoDB;

-- Tabla de categorías de servicios
CREATE TABLE `service_categories` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `name` VARCHAR(255) NOT NULL,
    `description` TEXT,
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Tabla de servicios
CREATE TABLE `services` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `categoryId` CHAR(36) NOT NULL,
    `name` VARCHAR(255) NOT NULL,
    `description` TEXT,
    `price` DECIMAL(10, 2) NOT NULL CHECK (`price` >= 0),
    `status` ENUM('available', 'not_available') NOT NULL,
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (`categoryId`) REFERENCES `service_categories`(`id`)
) ENGINE=InnoDB;

-- Tabla de repuestos/partes para servicios
CREATE TABLE `service_parts` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `name` VARCHAR(255) NOT NULL,
    `description` TEXT,
    `quantity` INT NOT NULL CHECK (`quantity` >= 0),
    `price` DECIMAL(10, 2) NOT NULL CHECK (`price` >= 0),
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Tabla de relación entre servicios y repuestos
CREATE TABLE `service_part_mappings` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `serviceId` CHAR(36) NOT NULL,
    `partId` CHAR(36) NOT NULL,
    `quantityRequired` INT NOT NULL CHECK (`quantityRequired` >= 0),
    FOREIGN KEY (`serviceId`) REFERENCES `services`(`id`),
    FOREIGN KEY (`partId`) REFERENCES `service_parts`(`id`)
) ENGINE=InnoDB;

-- Tabla de ventas
CREATE TABLE `sales` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `userId` CHAR(36) NOT NULL, -- Cliente que realiza la compra/solicita el servicio
    `ownerId` CHAR(36) NOT NULL, -- Propietario/gerente que registra la venta
    `saleType` ENUM('motorcycle', 'service', 'both') NOT NULL, -- Tipo de venta
    `totalPrice` DECIMAL(10, 2) NOT NULL CHECK (`totalPrice` >= 0),
    `date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (`userId`) REFERENCES `users`(`id`),
    FOREIGN KEY (`ownerId`) REFERENCES `owners`(`id`)
) ENGINE=InnoDB;

-- Tabla de detalles de ventas de motocicletas
CREATE TABLE `sales_motorcycles` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `saleId` CHAR(36) NOT NULL,
    `motorcycleUnitId` CHAR(36) NOT NULL, -- Unidad específica vendida
    `quantity` INT NOT NULL CHECK (`quantity` = 1), -- Siempre será 1 por unidad
    `unitPrice` DECIMAL(10, 2) NOT NULL CHECK (`unitPrice` >= 0),
    `subTotal` DECIMAL(10, 2) NOT NULL CHECK (`subTotal` >= 0),
    FOREIGN KEY (`saleId`) REFERENCES `sales`(`id`),
    FOREIGN KEY (`motorcycleUnitId`) REFERENCES `motorcycle_units`(`id`)
) ENGINE=InnoDB;

-- Tabla de detalles de ventas de servicios
CREATE TABLE `sales_services` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `saleId` CHAR(36) NOT NULL,
    `serviceId` CHAR(36) NOT NULL,
    `quantity` INT NOT NULL CHECK (`quantity` >= 1),
    `unitPrice` DECIMAL(10, 2) NOT NULL CHECK (`unitPrice` >= 0),
    `subTotal` DECIMAL(10, 2) NOT NULL CHECK (`subTotal` >= 0),
    FOREIGN KEY (`saleId`) REFERENCES `sales`(`id`),
    FOREIGN KEY (`serviceId`) REFERENCES `services`(`id`)
) ENGINE=InnoDB;

-- Tabla de historial de servicios realizados
CREATE TABLE `service_history` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `userId` CHAR(36) NOT NULL, -- Cliente que solicitó el servicio
    `motorcycleUnitId` CHAR(36), -- Motocicleta asociada (opcional)
    `serviceId` CHAR(36) NOT NULL,
    `date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `notes` TEXT,
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (`userId`) REFERENCES `users`(`id`),
    FOREIGN KEY (`motorcycleUnitId`) REFERENCES `motorcycle_units`(`id`),
    FOREIGN KEY (`serviceId`) REFERENCES `services`(`id`)
) ENGINE=InnoDB;

-- Tabla de historial de servicios por motocicleta
CREATE TABLE `motorcycle_service_history` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `motorcycleUnitId` CHAR(36) NOT NULL,
    `serviceId` CHAR(36) NOT NULL,
    `date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `notes` TEXT,
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (`motorcycleUnitId`) REFERENCES `motorcycle_units`(`id`),
    FOREIGN KEY (`serviceId`) REFERENCES `services`(`id`)
) ENGINE=InnoDB;

INSERT INTO `owners` (`id`,`username`,`password`,`firstName`,`lastName`,`email`,`dni`,`ruc`,`address`,`phoneNumber`,`role`,`status`,`createdAt`,`updatedAt`,`createdBy`,`updatedBy`) VALUES (
	'7b775e32-8b78-4352-ba0e-b13983ec69a0',
	'k4lfer',
	'$2a$12$jOvClFASO8j5krWw.bBThufnWf9s8FSL2OlatZS8ZGhCDTqAG/l2C',
	'Kaled', 
	'Dropi Inventory', 
	'LYpTtANcdrLVtWVtknc0ALjFNFP9UGlWGcO8J4BxOQk=', 
	'm4hb4F+X8tNHgrJc0nEe2A==', 
	'zRhL4jFlWiAXF1dbsdEk6w==',
	'Abancay-Apurímac',
	'cwwyWVKxtTegG1PZAWcPYg==', 
	'Admin', 1, '2024-10-17 18:17:28.000', '2024-10-17 18:17:28.000', NULL, NULL);

-- CREDENCIALS ADMIN
  -- username: k4lfer
  -- 1001civilium: 1001inventory


INSERT INTO `users` (`id`, `firstName`, `lastName`, `documentType`, `documentNumber`, `email`, `phoneNumber`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0004-0000-0000-000000000001', 'Juan', 'Pérez', 'DNI', '87654321', 'juan@example.com', '999888777', NOW(), NOW()),
('1a2b3c4d-0004-0000-0000-000000000002', 'María', 'Gómez', 'DNI', '12345678', 'maria@example.com', '999888666', NOW(), NOW()),
('1a2b3c4d-0004-0000-0000-000000000003', 'Carlos', 'López', 'DNI', '56781234', 'carlos@example.com', '999888555', NOW(), NOW());

-- Insertando datos en la tabla `brands`
INSERT INTO `brands` (`id`, `ruc`, `name`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0002-0000-0000-000000000001', '10101010101', 'Yamaha', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('1a2b3c4d-0002-0000-0000-000000000002', '20202020202', 'Kawasaki', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('1a2b3c4d-0002-0000-0000-000000000003', '30303030303', 'Honda', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('1a2b3c4d-0002-0000-0000-000000000004', '40404040404', 'Suzuki', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('1a2b3c4d-0002-0000-0000-000000000005', '50505050505', 'Ducati', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('1a2b3c4d-0002-0000-0000-000000000006', '60606060606', 'BMW', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('1a2b3c4d-0002-0000-0000-000000000007', '70707070707', 'KTM', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

-- Insertando datos en la tabla `types`
INSERT INTO `types` (`id`, `name`, `description`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0001-0000-0000-000000000001', 'Sport', 'Motocicletas deportivas diseñadas para velocidad y rendimiento.', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('1a2b3c4d-0001-0000-0000-000000000002', 'Cruiser', 'Motocicletas diseñadas para recorridos largos y comodidad.', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('1a2b3c4d-0001-0000-0000-000000000003', 'Touring', 'Motocicletas de turismo, ideales para largos viajes.', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('1a2b3c4d-0001-0000-0000-000000000004', 'Adventure', 'Motocicletas diseñadas para viajes en carretera y fuera de ella.', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('1a2b3c4d-0001-0000-0000-000000000005', 'Naked', 'Motocicletas sin carenado, con un diseño minimalista.', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('1a2b3c4d-0001-0000-0000-000000000006', 'Scooter', 'Motocicletas ligeras y fáciles de manejar para la ciudad.', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('1a2b3c4d-0001-0000-0000-000000000007', 'Off-road', 'Motocicletas diseñadas para terrenos accidentados.', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

-- Insertando datos en la tabla `motorcycles`
INSERT INTO `motorcycles` (`id`, `typeId`, `brandId`, `name`, `displacement`, `price`, `status`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0003-0000-0000-000000000001', '1a2b3c4d-0001-0000-0000-000000000001', '1a2b3c4d-0002-0000-0000-000000000001', 'YZF R1', '998 cc', 20000.00, 'available', NOW(), NOW()),
('1a2b3c4d-0003-0000-0000-000000000002', '1a2b3c4d-0001-0000-0000-000000000002', '1a2b3c4d-0002-0000-0000-000000000002', 'Ninja ZX-6R', '636 cc', 17000.00, 'available', NOW(), NOW()),
('1a2b3c4d-0003-0000-0000-000000000003', '1a2b3c4d-0001-0000-0000-000000000003', '1a2b3c4d-0002-0000-0000-000000000003', 'CBR600RR', '599 cc', 15000.00, 'available', NOW(), NOW()),
('1a2b3c4d-0003-0000-0000-000000000004', '1a2b3c4d-0001-0000-0000-000000000004', '1a2b3c4d-0002-0000-0000-000000000004', 'V-Strom 650', '645 cc', 12000.00, 'available', NOW(), NOW());

-- Insertando datos en la tabla `motorcycle_units`
-- Unidades para YZF R1 (Modelo 1)
INSERT INTO `motorcycle_units` (`id`, `motorcycleId`, `vin`, `status`, `purchaseDate`, `purchasePrice`, `salePrice`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0005-0000-0000-000000000001', '1a2b3c4d-0003-0000-0000-000000000001', 'VIN123456789012345', 'available', '2024-01-01', 18000.00, NULL, NOW(), NOW()),
('1a2b3c4d-0005-0000-0000-000000000002', '1a2b3c4d-0003-0000-0000-000000000001', 'VIN123456789012346', 'available', '2024-01-01', 18000.00, NULL, NOW(), NOW()),
('1a2b3c4d-0005-0000-0000-000000000003', '1a2b3c4d-0003-0000-0000-000000000001', 'VIN123456789012347', 'available', '2024-01-01', 18000.00, NULL, NOW(), NOW()),
('1a2b3c4d-0005-0000-0000-000000000004', '1a2b3c4d-0003-0000-0000-000000000001', 'VIN123456789012348', 'available', '2024-01-01', 18000.00, NULL, NOW(), NOW());

-- Unidades para Ninja ZX-6R (Modelo 2)
INSERT INTO `motorcycle_units` (`id`, `motorcycleId`, `vin`, `status`, `purchaseDate`, `purchasePrice`, `salePrice`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0005-0000-0000-000000000005', '1a2b3c4d-0003-0000-0000-000000000002', 'VIN223456789012345', 'available', '2024-01-01', 16000.00, NULL, NOW(), NOW()),
('1a2b3c4d-0005-0000-0000-000000000006', '1a2b3c4d-0003-0000-0000-000000000002', 'VIN223456789012346', 'available', '2024-01-01', 16000.00, NULL, NOW(), NOW()),
('1a2b3c4d-0005-0000-0000-000000000007', '1a2b3c4d-0003-0000-0000-000000000002', 'VIN223456789012347', 'available', '2024-01-01', 16000.00, NULL, NOW(), NOW()),
('1a2b3c4d-0005-0000-0000-000000000008', '1a2b3c4d-0003-0000-0000-000000000002', 'VIN223456789012348', 'available', '2024-01-01', 16000.00, NULL, NOW(), NOW());

-- Unidades para CBR600RR (Modelo 3)
INSERT INTO `motorcycle_units` (`id`, `motorcycleId`, `vin`, `status`, `purchaseDate`, `purchasePrice`, `salePrice`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0005-0000-0000-000000000009', '1a2b3c4d-0003-0000-0000-000000000003', 'VIN323456789012345', 'available', '2024-01-01', 14000.00, NULL, NOW(), NOW()),
('1a2b3c4d-0005-0000-0000-000000000010', '1a2b3c4d-0003-0000-0000-000000000003', 'VIN323456789012346', 'available', '2024-01-01', 14000.00, NULL, NOW(), NOW()),
('1a2b3c4d-0005-0000-0000-000000000011', '1a2b3c4d-0003-0000-0000-000000000003', 'VIN323456789012347', 'available', '2024-01-01', 14000.00, NULL, NOW(), NOW()),
('1a2b3c4d-0005-0000-0000-000000000012', '1a2b3c4d-0003-0000-0000-000000000003', 'VIN323456789012348', 'available', '2024-01-01', 14000.00, NULL, NOW(), NOW());

-- Unidades para V-Strom 650 (Modelo 4)
INSERT INTO `motorcycle_units` (`id`, `motorcycleId`, `vin`, `status`, `purchaseDate`, `purchasePrice`, `salePrice`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0005-0000-0000-000000000013', '1a2b3c4d-0003-0000-0000-000000000004', 'VIN423456789012345', 'available', '2024-01-01', 11000.00, NULL, NOW(), NOW()),
('1a2b3c4d-0005-0000-0000-000000000014', '1a2b3c4d-0003-0000-0000-000000000004', 'VIN423456789012346', 'available', '2024-01-01', 11000.00, NULL, NOW(), NOW()),
('1a2b3c4d-0005-0000-0000-000000000015', '1a2b3c4d-0003-0000-0000-000000000004', 'VIN423456789012347', 'available', '2024-01-01', 11000.00, NULL, NOW(), NOW()),
('1a2b3c4d-0005-0000-0000-000000000016', '1a2b3c4d-0003-0000-0000-000000000004', 'VIN423456789012348', 'available', '2024-01-01', 11000.00, NULL, NOW(), NOW());

-- Insertando datos en la tabla `service_categories`
INSERT INTO `service_categories` (`id`, `name`, `description`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0006-0000-0000-000000000001', 'Mantenimiento', 'Servicios de mantenimiento preventivo y correctivo.', NOW(), NOW()),
('1a2b3c4d-0006-0000-0000-000000000002', 'Reparación', 'Reparación de motocicletas.', NOW(), NOW()),
('1a2b3c4d-0006-0000-0000-000000000003', 'Accesorios', 'Instalación de accesorios para motocicletas.', NOW(), NOW());

-- Insertando datos en la tabla `services`
INSERT INTO `services` (`id`, `categoryId`, `name`, `description`, `price`, `status`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0007-0000-0000-000000000001', '1a2b3c4d-0006-0000-0000-000000000001', 'Cambio de aceite', 'Cambio de aceite y filtro.', 50.00, 'available', NOW(), NOW()),
('1a2b3c4d-0007-0000-0000-000000000002', '1a2b3c4d-0006-0000-0000-000000000002', 'Reparación de frenos', 'Reparación y ajuste de frenos.', 80.00, 'available', NOW(), NOW()),
('1a2b3c4d-0007-0000-0000-000000000003', '1a2b3c4d-0006-0000-0000-000000000003', 'Instalación de alarma', 'Instalación de sistema de alarma.', 120.00, 'available', NOW(), NOW());

-- Insertando datos en la tabla `service_parts`
INSERT INTO `service_parts` (`id`, `name`, `description`, `quantity`, `price`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0008-0000-0000-000000000001', 'Aceite sintético', 'Aceite sintético para motocicletas.', 100, 20.00, NOW(), NOW()),
('1a2b3c4d-0008-0000-0000-000000000002', 'Pastillas de freno', 'Pastillas de freno para motocicletas.', 50, 30.00, NOW(), NOW()),
('1a2b3c4d-0008-0000-0000-000000000003', 'Alarma', 'Sistema de alarma para motocicletas.', 20, 80.00, NOW(), NOW());

-- Insertando datos en la tabla `service_part_mappings`
INSERT INTO `service_part_mappings` (`id`, `serviceId`, `partId`, `quantityRequired`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0009-0000-0000-000000000001', '1a2b3c4d-0007-0000-0000-000000000001', '1a2b3c4d-0008-0000-0000-000000000001', 1, NOW(), NOW()),
('1a2b3c4d-0009-0000-0000-000000000002', '1a2b3c4d-0007-0000-0000-000000000002', '1a2b3c4d-0008-0000-0000-000000000002', 2, NOW(), NOW()),
('1a2b3c4d-0009-0000-0000-000000000003', '1a2b3c4d-0007-0000-0000-000000000003', '1a2b3c4d-0008-0000-0000-000000000003', 1, NOW(), NOW());

-- Insertando datos en la tabla `sales`
INSERT INTO `sales` (`id`, `userId`, `ownerId`, `saleType`, `totalPrice`, `date`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0010-0000-0000-000000000001', '1a2b3c4d-0004-0000-0000-000000000001', '7b775e32-8b78-4352-ba0e-b13983ec69a0', 'motorcycle', 20000.00, NOW(), NOW(), NOW()),
('1a2b3c4d-0010-0000-0000-000000000002', '1a2b3c4d-0004-0000-0000-000000000002', '7b775e32-8b78-4352-ba0e-b13983ec69a0', 'service', 50.00, NOW(), NOW(), NOW()),
('1a2b3c4d-0010-0000-0000-000000000003', '1a2b3c4d-0004-0000-0000-000000000003', '7b775e32-8b78-4352-ba0e-b13983ec69a0', 'both', 20050.00, NOW(), NOW(), NOW());

-- Insertando datos en la tabla `sales_motorcycles`
INSERT INTO `sales_motorcycles` (`id`, `saleId`, `motorcycleUnitId`, `quantity`, `unitPrice`, `subTotal`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0011-0000-0000-000000000001', '1a2b3c4d-0010-0000-0000-000000000001', '1a2b3c4d-0005-0000-0000-000000000001', 1, 20000.00, 20000.00, NOW(), NOW());

-- Insertando datos en la tabla `sales_services`
INSERT INTO `sales_services` (`id`, `saleId`, `serviceId`, `quantity`, `unitPrice`, `subTotal`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0012-0000-0000-000000000001', '1a2b3c4d-0010-0000-0000-000000000002', '1a2b3c4d-0007-0000-0000-000000000001', 1, 50.00, 50.00, NOW(), NOW());

-- Insertando datos en la tabla `service_history`
INSERT INTO `service_history` (`id`, `userId`, `motorcycleUnitId`, `serviceId`, `date`, `notes`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0013-0000-0000-000000000001', '1a2b3c4d-0004-0000-0000-000000000001', '1a2b3c4d-0005-0000-0000-000000000001', '1a2b3c4d-0007-0000-0000-000000000001', NOW(), 'Cambio de aceite realizado.', NOW(), NOW());

-- Insertando datos en la tabla `motorcycle_service_history`
INSERT INTO `motorcycle_service_history` (`id`, `motorcycleUnitId`, `serviceId`, `date`, `notes`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0014-0000-0000-000000000001', '1a2b3c4d-0005-0000-0000-000000000001', '1a2b3c4d-0007-0000-0000-000000000001', NOW(), 'Cambio de aceite realizado.', NOW(), NOW());