# Base de Datos para Gestión de Motocicletas y Servicios V0.1.8

Este proyecto cuenta con dos versiones de la base de datos:

1. Versión 0.1.0: Versión inicial utilizada para el desarrollo de la API.

2. Versión 0.1.8: Versión actualizada con nuevas entidades y modificaciones en las existentes. Se realizará una migración progresiva a esta nueva estructura, incorporando funcionalidades adicionales.

Este esquema de base de datos está diseñado para gestionar tanto la venta de motocicletas como la prestación de servicios, permitiendo administrar clientes que pueden ser compradores, solicitantes de servicios o ambos.

## Tablas Principales

1. **`owners`**: Administradores y gerentes.
2. **`users`**: Clientes que pueden comprar motocicletas o solicitar servicios.
3. **`motorcycles`**: Modelos de motocicletas.
4. **`motorcycle_units`**: Unidades individuales de motocicletas con su VIN.
5. **`service_categories`**: Categorías de servicios (mantenimiento, reparación, etc.).
6. **`services`**: Servicios ofrecidos.
7. **`service_parts`**: Repuestos o partes utilizadas en los servicios.
8. **`sales`**: Ventas (pueden ser de motocicletas, servicios, o ambos).
9. **`sales_motorcycles`**: Detalles de ventas de motocicletas.
10. **`sales_services`**: Detalles de ventas de servicios.
11. **`service_history`**: Historial de servicios realizados a clientes.
12. **`motorcycle_service_history`**: Historial de servicios realizados a motocicletas específicas.

## Flujo de Trabajo

### 1. **Registro de Clientes**
   - Los clientes se registran en la tabla `users`.
   - Un cliente puede ser comprador, solicitante de servicios, o ambos.

### 2. **Registro de Motocicletas**
   - Los modelos de motocicletas se registran en `motorcycles`.
   - Cada unidad de motocicleta se registra en `motorcycle_units` con su VIN único.

### 3. **Registro de Servicios**
   - Los servicios se registran en `services`, asociados a una categoría en `service_categories`.
   - Los repuestos o partes utilizadas en los servicios se registran en `service_parts`.

### 4. **Ventas**
   - Las ventas se registran en `sales`, indicando el tipo de venta (`motorcycle`, `service`, o `both`).
   - Los detalles de ventas de motocicletas se registran en `sales_motorcycles`.
   - Los detalles de ventas de servicios se registran en `sales_services`.

### 5. **Historial de Servicios**
   - Los servicios realizados a clientes se registran en `service_history`.
   - Los servicios realizados a motocicletas específicas se registran en `motorcycle_service_history`.

## Casos de Uso

### **Caso 1: Cliente no registrado solicita un servicio**
1. Se registra al cliente en `users`.
2. Se registra la venta del servicio en `sales` con `saleType = 'service'`.
3. Se registran los detalles del servicio en `sales_services`.
4. Se registra el servicio en `service_history`.

### **Caso 2: Cliente compra una motocicleta después de solicitar un servicio**
1. Si el cliente ya está registrado, se usa su ID existente en `users`.
2. Se registra la venta de la motocicleta en `sales` con `saleType = 'motorcycle'`.
3. Se registran los detalles de la venta en `sales_motorcycles`.
4. Si el cliente también solicita un servicio, se registra en `sales_services` y `service_history`.

### **Caso 3: Servicios con repuestos**
1. Se registran los repuestos en `service_parts`.
2. Se asocian los repuestos a los servicios en `service_part_mappings`.
3. Al realizar un servicio, se verifica el inventario de repuestos y se actualiza la cantidad en `service_parts`.

## Ejemplo de Consultas

1. **Obtener todas las unidades disponibles de un modelo**:
   SELECT u.id, u.vin, u.purchaseDate
   FROM `motorcycle_units` u
   WHERE u.motorcycleId = 'ID_DEL_MODELO' AND u.status = 'available';

2. **Obtener el historial de servicios de un cliente:**:
    SELECT s.name, sh.date, sh.notes
    FROM `service_history` sh
    JOIN `services` s ON sh.serviceId = s.id
    WHERE sh.userId = 'ID_DEL_CLIENTE';

3. **Obtener todas las ventas de servicios**:
    SELECT ss.id, s.date, u.firstName, u.lastName, sv.name, ss.quantity, ss.subTotal
    FROM `sales_services` ss
    JOIN `sales` s ON ss.saleId = s.id
    JOIN `users` u ON s.userId = u.id
    JOIN `services` sv ON ss.serviceId = sv.id;