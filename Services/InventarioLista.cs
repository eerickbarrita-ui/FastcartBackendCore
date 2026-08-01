using FastCartBackendCore.Models;

namespace FastCartBackendCore.Services;

public class InventarioLista
{
    private NodoProducto? _cabeza;

    /// <summary>
    /// Inserta un producto al inicio de la lista.
    /// </summary>
    /// <param name="producto">Producto que será agregado.</param>
    public void InsertarInicio(Producto producto)
    {
        NodoProducto nuevoNodo = new NodoProducto(producto);

        nuevoNodo.Siguiente = _cabeza;
        _cabeza = nuevoNodo;
    }

    /// <summary>
    /// Inserta un producto manteniendo la lista ordenada por precio ascendente.
    /// </summary>
    /// <param name="producto">Producto que será agregado.</param>
    public void InsertarOrdenado(Producto producto)
    {
        NodoProducto nuevoNodo = new NodoProducto(producto);

        if (_cabeza == null ||
            producto.Precio < _cabeza.Data.Precio)
        {
            nuevoNodo.Siguiente = _cabeza;
            _cabeza = nuevoNodo;
            return;
        }

        NodoProducto actual = _cabeza;

        while (actual.Siguiente != null &&
               actual.Siguiente.Data.Precio <= producto.Precio)
        {
            actual = actual.Siguiente;
        }

        nuevoNodo.Siguiente = actual.Siguiente;
        actual.Siguiente = nuevoNodo;
    }

    /// <summary>
    /// Busca un producto por su SKU.
    /// </summary>
    /// <param name="sku">SKU del producto que se desea localizar.</param>
    /// <returns>Producto encontrado.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Se produce cuando el SKU no existe en la lista.
    /// </exception>
    public Producto BuscarPorSKU(int sku)
    {
        NodoProducto? actual = _cabeza;

        while (actual != null)
        {
            if (actual.Data.SKU == sku)
            {
                return actual.Data;
            }

            actual = actual.Siguiente;
        }

        throw new KeyNotFoundException(
            $"El producto con SKU {sku} no fue encontrado.");
    }

    /// <summary>
    /// Elimina un producto de la lista utilizando su SKU.
    /// </summary>
    /// <param name="sku">SKU del producto que se desea eliminar.</param>
    /// <returns>
    /// true si el producto fue eliminado; false si el SKU no existe.
    /// </returns>
    public bool EliminarPorSKU(int sku)
    {
        if (_cabeza == null)
        {
            return false;
        }

        if (_cabeza.Data.SKU == sku)
        {
            _cabeza = _cabeza.Siguiente;
            return true;
        }

        NodoProducto actual = _cabeza;

        while (actual.Siguiente != null)
        {
            if (actual.Siguiente.Data.SKU == sku)
            {
                actual.Siguiente = actual.Siguiente.Siguiente;
                return true;
            }

            actual = actual.Siguiente;
        }

        return false;
    }

    /// <summary>
    /// Muestra en consola todos los productos almacenados en la lista.
    /// </summary>
    public void MostrarProductos()
    {
        if (_cabeza == null)
        {
            Console.WriteLine("El inventario está vacío.");
            return;
        }

        NodoProducto? actual = _cabeza;

        Console.WriteLine(
            "SKU\tNombre\t\tPrecio\t\tStock\tProveedor");
        Console.WriteLine(
            "---------------------------------------------------------------");

        while (actual != null)
        {
            Producto producto = actual.Data;

            Console.WriteLine(
                $"{producto.SKU}\t" +
                $"{producto.Nombre,-15}\t" +
                $"${producto.Precio,8:F2}\t" +
                $"{producto.Stock}\t" +
                $"{producto.DatosProveedor.NombreCorporativo}");

            actual = actual.Siguiente;
        }
    }
}
