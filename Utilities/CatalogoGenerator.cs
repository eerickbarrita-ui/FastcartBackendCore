using FastCartBackendCore.Models;

namespace FastCartBackendCore.Utilities;

public static class CatalogoGenerator
{
    private static readonly Random random = new(2026);

    public static Producto[] GenerarCatalogo(int cantidad)
    {
        if (cantidad < 50)
        {
            throw new ArgumentException(
                "El catálogo debe contener al menos 50 productos."
            );
        }

        Producto[] catalogo = new Producto[cantidad];

        for (int i = 0; i < cantidad; i++)
        {
            int numeroProveedor = random.Next(1, 11);

            catalogo[i] = new Producto
            {
                SKU = 1001 + i,
                Nombre = $"Producto {i + 1}",
                Precio = Math.Round(
                    random.NextDouble() * (9999.99 - 10.00) + 10.00,
                    2
                ),
                Stock = random.Next(0, 501),

                DatosProveedor = new Proveedor
                {
                    IdProveedor = numeroProveedor,
                    NombreCorporativo = $"Proveedor {numeroProveedor}"
                }
            };
        }

        catalogo[0].Precio = 2500.00;
        catalogo[1].Precio = 2500.00;
        catalogo[2].Precio = 2500.00;

        return catalogo;
    }
}