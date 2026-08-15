using FastCartBackendCore.Models;
using FastCartBackendCore.Services;

Console.WriteLine("==============================================");
Console.WriteLine("     FASTCART BACKEND CORE - FASE 3");
Console.WriteLine("==============================================\n");

AuditoriaService auditoria = new AuditoriaService();
InventarioLista inventario = new InventarioLista(auditoria);

Proveedor proveedor1 = new Proveedor
{
    IdProveedor = 1,
    NombreCorporativo = "Proveedor Norte"
};

Proveedor proveedor2 = new Proveedor
{
    IdProveedor = 2,
    NombreCorporativo = "Proveedor Centro"
};

Proveedor proveedor3 = new Proveedor
{
    IdProveedor = 3,
    NombreCorporativo = "Proveedor Sur"
};

Producto[] productos =
{
    new Producto
    {
        SKU = 1001,
        Nombre = "Laptop",
        Precio = 15999.00,
        Stock = 8,
        DatosProveedor = proveedor1
    },
    new Producto
    {
        SKU = 1002,
        Nombre = "Monitor",
        Precio = 4299.00,
        Stock = 15,
        DatosProveedor = proveedor2
    },
    new Producto
    {
        SKU = 1003,
        Nombre = "Teclado",
        Precio = 899.00,
        Stock = 30,
        DatosProveedor = proveedor3
    },
    new Producto
    {
        SKU = 1004,
        Nombre = "Mouse",
        Precio = 499.00,
        Stock = 40,
        DatosProveedor = proveedor1
    },
    new Producto
    {
        SKU = 1005,
        Nombre = "Impresora",
        Precio = 3199.00,
        Stock = 12,
        DatosProveedor = proveedor2
    },
    new Producto
    {
        SKU = 1006,
        Nombre = "Webcam",
        Precio = 1299.00,
        Stock = 18,
        DatosProveedor = proveedor3
    },
    new Producto
    {
        SKU = 1007,
        Nombre = "Router",
        Precio = 1899.00,
        Stock = 22,
        DatosProveedor = proveedor1
    },
    new Producto
    {
        SKU = 1008,
        Nombre = "Tablet",
        Precio = 7499.00,
        Stock = 10,
        DatosProveedor = proveedor2
    },
    new Producto
    {
        SKU = 1009,
        Nombre = "Smartphone",
        Precio = 11999.00,
        Stock = 14,
        DatosProveedor = proveedor3
    },
    new Producto
    {
        SKU = 1010,
        Nombre = "Bocina",
        Precio = 999.00,
        Stock = 25,
        DatosProveedor = proveedor1
    },
    new Producto
    {
        SKU = 1011,
        Nombre = "Audifonos",
        Precio = 1499.00,
        Stock = 35,
        DatosProveedor = proveedor2
    },
    new Producto
    {
        SKU = 1012,
        Nombre = "Disco SSD",
        Precio = 2199.00,
        Stock = 20,
        DatosProveedor = proveedor3
    },
    new Producto
    {
        SKU = 1013,
        Nombre = "Memoria RAM",
        Precio = 1699.00,
        Stock = 28,
        DatosProveedor = proveedor1
    },
    new Producto
    {
        SKU = 1014,
        Nombre = "Proyector",
        Precio = 8999.00,
        Stock = 6,
        DatosProveedor = proveedor2
    },
    new Producto
    {
        SKU = 1015,
        Nombre = "Microfono",
        Precio = 2499.00,
        Stock = 16,
        DatosProveedor = proveedor3
    }
};

Console.WriteLine("REGISTRO INICIAL DE PRODUCTOS\n");

foreach (Producto producto in productos)
{
    inventario.InsertarOrdenado(producto);
}

Console.WriteLine("CATÁLOGO ORDENADO POR PRECIO ASCENDENTE");
Console.WriteLine();

inventario.MostrarProductos();

Console.WriteLine("\n==============================================");
Console.WriteLine("BÚSQUEDA DE PRODUCTO");
Console.WriteLine("==============================================");

try
{
    Producto encontrado = inventario.BuscarPorSKU(1008);

    Console.WriteLine("Producto encontrado:");
    Console.WriteLine($"SKU: {encontrado.SKU}");
    Console.WriteLine($"Nombre: {encontrado.Nombre}");
    Console.WriteLine($"Precio: ${encontrado.Precio:F2}");
    Console.WriteLine($"Stock: {encontrado.Stock}");
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine(ex.Message);
}

Console.WriteLine("\n==============================================");
Console.WriteLine("ACTUALIZACIÓN DE PRECIO");
Console.WriteLine("==============================================");

try
{
    inventario.ModificarPrecio(1008, 6999.00);

    Console.WriteLine(
        "El precio del producto con SKU 1008 fue actualizado correctamente.");
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine(ex.Message);
}

Console.WriteLine("\n==============================================");
Console.WriteLine("ELIMINACIÓN DE PRODUCTO");
Console.WriteLine("==============================================");

bool eliminado = inventario.EliminarPorSKU(1005);

if (eliminado)
{
    Console.WriteLine("El producto con SKU 1005 fue eliminado.");
}
else
{
    Console.WriteLine("El producto no fue encontrado.");
}

Console.WriteLine("\n==============================================");
Console.WriteLine("PRUEBA DE SKU INEXISTENTE");
Console.WriteLine("==============================================");

try
{
    inventario.BuscarPorSKU(9999);
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine($"Excepción controlada: {ex.Message}");
}

Console.WriteLine("\n==============================================");
Console.WriteLine("CATÁLOGO FINAL");
Console.WriteLine("==============================================\n");

inventario.MostrarProductos();

Console.WriteLine("\n==============================================");
Console.WriteLine("HISTORIAL CRONOLÓGICO DE AUDITORÍA");
Console.WriteLine("==============================================");

auditoria.ImprimirHistorial();

Console.WriteLine("\n==============================================");
Console.WriteLine("HISTORIAL INVERSO DE AUDITORÍA");
Console.WriteLine("==============================================");

auditoria.ImprimirHistorialInverso();

Console.WriteLine("\n==============================================");
Console.WriteLine($"TOTAL DE REGISTROS DE AUDITORÍA: {auditoria.TotalRegistros}");
Console.WriteLine("==============================================");