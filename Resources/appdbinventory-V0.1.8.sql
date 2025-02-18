--create database `dbinventory`;
--USE `dbinventory`;
--CHARACTER SET utf8mb4;
--COLLATE utf8mb4_spanish_ci;

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
  `status` tinyint(1) NOT NULL,
  `profilePictureUrl` TEXT NULL,
  `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `createdBy` CHAR(36),
  `updatedBy` CHAR(36)
) ENGINE=InnoDB;

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

CREATE TABLE `types` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `name` VARCHAR(255) NOT NULL,
    `description` TEXT,
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

CREATE TABLE `brands` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `ruc` VARCHAR(255) NOT NULL,
    `name` VARCHAR(255) NOT NULL,
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

CREATE TABLE `motorcycles` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `typeId` CHAR(36) NOT NULL,
    `brandId` CHAR(36) NOT NULL, 
    `name` VARCHAR(255) NOT NULL,
    `displacement` VARCHAR(255) NOT NULL, 
    `price` DECIMAL(10, 2) NOT NULL,
    `quantity` INT NOT NULL,
    `status` ENUM('available', 'not_available') NOT NULL,
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,   
    FOREIGN KEY (`typeId`) REFERENCES `types`(`id`),
    FOREIGN KEY (`brandId`) REFERENCES `brands`(`id`)
) ENGINE=InnoDB;

/*CREATE TABLE `motorcycle_instances` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `motorcycleId` CHAR(36) NOT NULL, -- Referencia al modelo
    `vin` VARCHAR(255) UNIQUE NOT NULL, -- Número de chasis único
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,   
    FOREIGN KEY (`motorcycleId`) REFERENCES `motorcycles`(`id`)
) ENGINE=InnoDB;
*/

CREATE TABLE `services` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `name` VARCHAR(255) NOT NULL,
    `description` TEXT NOT NULL,
    `price` DECIMAL(10,2) NOT NULL, 
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

CREATE TABLE `sales` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `userId` CHAR(36) NOT NULL,
    `ownerId` CHAR(36) NOT NULL,
    `quantity` INT NOT NULL,
    `totalPrice` DECIMAL(10, 2) NOT NULL,
    `date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (`userId`) REFERENCES `users`(`id`),
    FOREIGN KEY (`ownerId`) REFERENCES `owners`(`id`)
) ENGINE=InnoDB;

CREATE TABLE `motorcycle_services` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `saleId` CHAR(36) NULL,  -- Referencia a la venta de la moto (puede ser NULL si no está relacionado con una venta)
    `motorcycleInstanceId` CHAR(36)  NULL,  -- Identificador de la moto
    `motorcycleName` VARCHAR(255)  NULL, -- En caso solo se solicite un servicio a una moto no registrada, esto sin compra de la moto
    `serviceId` CHAR(36) NOT NULL,  -- Referencia al servicio realizado
    `date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,  -- Fecha del servicio
    `price` DECIMAL(10, 2) NOT NULL,  -- Precio del servicio
    `createdAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (`saleId`) REFERENCES `sales`(`id`),  -- Relación con la venta
    FOREIGN KEY (`motorcycleInstanceId`) REFERENCES `motorcycles`(`id`),  -- Relación con motocicleta
    --FOREIGN KEY (`motorcycleInstanceId`) REFERENCES `motorcycle_instances`(`id`)
    FOREIGN KEY (`serviceId`) REFERENCES `services`(`id`)  -- Relación con servicio
) ENGINE=InnoDB;


CREATE TABLE `sales_motorcycles` (
    `id` CHAR(36) PRIMARY KEY NOT NULL,
    `saleId` CHAR(36) NOT NULL,
    `motorcycleId` CHAR(36) NOT NULL,
    --`motorcycleInstanceId` CHAR(36) NOT NULL,
    `quantity` INT NOT NULL,
    `unitPrice` DECIMAL(10, 2) NOT NULL,
    `subTotal` DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (`saleId`) REFERENCES `sales`(`id`),
    FOREIGN KEY (`motorcycleId`) REFERENCES `motorcycles`(`id`)
    --FOREIGN KEY (`motorcycleInstanceId`) REFERENCES `motorcycle_instances`(`id`)
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
INSERT INTO `motorcycles` (`id`, `typeId`, `brandId`, `name`, `displacement`, `price`, `quantity`, `status`, `createdAt`, `updatedAt`)
VALUES
('1a2b3c4d-0003-0000-0000-000000000001', '1a2b3c4d-0001-0000-0000-000000000001', '1a2b3c4d-0002-0000-0000-000000000001','YZF R1', '998 cc', 20000.00, 5, 'available', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('1a2b3c4d-0003-0000-0000-000000000002', '1a2b3c4d-0001-0000-0000-000000000002', '1a2b3c4d-0002-0000-0000-000000000002','Ninja ZX-6R', '636 cc', 17000.00, 3, 'available', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('1a2b3c4d-0003-0000-0000-000000000003', '1a2b3c4d-0001-0000-0000-000000000003', '1a2b3c4d-0002-0000-0000-000000000003','CBR600RR', '599 cc', 15000.00, 4, 'available', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('1a2b3c4d-0003-0000-0000-000000000004', '1a2b3c4d-0001-0000-0000-000000000004', '1a2b3c4d-0002-0000-0000-000000000004', 'V-Strom 650', '645 cc', 12000.00, 6, 'available', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
select * from user