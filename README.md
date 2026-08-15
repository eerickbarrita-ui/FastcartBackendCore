# FastCart Backend Core

Proyecto académico desarrollado en C# y .NET para implementar estructuras de datos aplicadas a la gestión de inventario y auditoría de operaciones de FastCart.

## Fase 3 - Motor de Navegación Bidireccional para el Historial de Auditoría

En esta fase se implementó un sistema de auditoría integrado al inventario desarrollado durante la Fase 2.

El sistema registra automáticamente las operaciones realizadas sobre los productos y almacena los movimientos mediante una lista doblemente enlazada.

## Funcionalidades

### Gestión de inventario

El sistema permite:

- Insertar productos.
- Insertar productos ordenados por precio.
- Buscar productos mediante SKU.
- Modificar el precio de un producto.
- Eliminar productos mediante SKU.
- Mostrar el catálogo completo.

### Sistema de auditoría

Cada modificación del inventario genera automáticamente un registro de auditoría.

Las operaciones registradas son:

- INSERT
- UPDATE
- DELETE

Cada registro contiene:

- Tipo de operación.
- Identificador del producto.
- Referencia o descripción del movimiento.
- Fecha y hora UTC.

## Lista doblemente enlazada

El historial de auditoría utiliza una lista doblemente enlazada.

Cada `NodoAuditoria` contiene:

- Un objeto `LogMovimiento`.
- Una referencia `Siguiente`.
- Una referencia `Anterior`.

Esto permite recorrer el historial en dos direcciones:

- Orden cronológico: del registro más antiguo al más reciente.
- Orden inverso: del registro más reciente al más antiguo.

## Estructura del proyecto

```text
FastCartBackendCore/
│
├── Models/
│   ├── Producto.cs
│   ├── Proveedor.cs
│   ├── NodoProducto.cs
│   ├── LogMovimiento.cs
│   └── NodoAuditoria.cs
│
├── Services/
│   ├── InventarioLista.cs
│   └── AuditoriaService.cs
│
├── FastCartBackendCore.Tests/
│   └── AuditoriaServiceTests.cs
│
├── Program.cs
├── FastCartBackendCore.csproj
└── README.md