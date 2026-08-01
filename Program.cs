using System.Diagnostics;
using FastCartBackendCore.Models;
using FastCartBackendCore.Services;
using FastCartBackendCore.Utilities;

Producto[] catalogo = CatalogoGenerator.GenerarCatalogo(50);

Console.WriteLine("==============================================================");
Console.WriteLine("        FASTCART BACKEND CORE - FASE 1");
Console.WriteLine("==============================================================");
Console.WriteLine($"Total de productos: {catalogo.Length}");

Console.WriteLine("\n========== PRIMEROS 10 PRODUCTOS (ANTES DE ORDENAR) ==========\n");
MostrarCatalogo(catalogo, 10);

Stopwatch cronometro = Stopwatch.StartNew();

OrdenamientoService.ShellSort(catalogo);

cronometro.Stop();

Console.WriteLine("\n========== PRIMEROS 10 PRODUCTOS (DESPUÉS DE ORDENAR) ==========\n");
MostrarCatalogo(catalogo, 10);

Console.WriteLine("\n========== PRODUCTOS CON PRECIO $2,500.00 ==========\n");

foreach (Producto producto in catalogo)
{
    if (producto.Precio == 2500.00)
    {
        Console.WriteLine($"SKU: {producto.SKU}  Precio: {producto.Precio:C}");
    }
}

Console.WriteLine("\n========== TIEMPO DE EJECUCIÓN ==========");

Console.WriteLine($"Milisegundos : {cronometro.ElapsedMilliseconds}");

double microsegundos =
    cronometro.ElapsedTicks * 1_000_000.0 / Stopwatch.Frequency;

Console.WriteLine($"Microsegundos: {microsegundos:F2}");

Console.WriteLine($"Ticks        : {cronometro.ElapsedTicks}");

static void MostrarCatalogo(Producto[] catalogo, int cantidad)
{
    Console.WriteLine("SKU\tPrecio\t\tStock\tProveedor");

    Console.WriteLine("--------------------------------------------------------------");

    for (int i = 0; i < cantidad && i < catalogo.Length; i++)
    {
        Producto producto = catalogo[i];

        Console.WriteLine(
            $"{producto.SKU}\t" +
            $"{producto.Precio,10:C}\t" +
            $"{producto.Stock,3}\t" +
            $"{producto.DatosProveedor.NombreCorporativo}"
        );
    }
}