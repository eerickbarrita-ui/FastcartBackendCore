using FastCartBackendCore.Models;

namespace FastCartBackendCore.Services;

public static class OrdenamientoService
{
    public static void ShellSort(Producto[] catalogo)
    {
        int n = catalogo.Length;

        // Secuencia de Knuth: 1, 4, 13, 40...
        int gap = 1;

        while (gap < n / 3)
        {
            gap = gap * 3 + 1;
        }

        while (gap >= 1)
        {
            for (int i = gap; i < n; i++)
            {
                Producto temporal = catalogo[i];
                int j = i;

                while (
                    j >= gap &&
                    DebeIrDespues(catalogo[j - gap], temporal)
                )
                {
                    catalogo[j] = catalogo[j - gap];
                    j -= gap;
                }

                catalogo[j] = temporal;
            }

            gap /= 3;
        }
    }

    private static bool DebeIrDespues(Producto productoA, Producto productoB)
    {
        if (productoA.Precio != productoB.Precio)
        {
            return productoA.Precio < productoB.Precio;
        }

        return productoA.SKU > productoB.SKU;
    }
}