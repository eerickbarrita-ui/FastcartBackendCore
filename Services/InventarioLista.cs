using FastCartBackendCore.Models;

namespace FastCartBackendCore.Services;

public class InventarioLista
{
    private NodoProducto? _cabeza;
    private readonly AuditoriaService _auditoria;

    /// <summary>
    /// Inicializa el inventario con el servicio de auditoría.
    /// </summary>
    public InventarioLista(AuditoriaService auditoria)
    {
        _auditoria = auditoria
            ?? throw new ArgumentNullException(nameof(auditoria));
    }

    /// <summary>
    /// Inserta un producto al inicio de la lista.
    /// </summary>
    public void InsertarInicio(Producto producto)
    {
        NodoProducto nuevoNodo = new NodoProducto(producto);

        nuevoNodo.Siguiente = _cabeza;
        _cabeza = nuevoNodo;

        RegistrarAuditoriaSegura(
            "INSERT",
            producto.SKU,
            $"Producto '{producto.Nombre}' agregado al inicio del inventario. " +
            $"Precio: ${producto.Precio:F2}. Stock: {producto.Stock}."
        );
    }

    /// <summary>
    /// Inserta un producto manteniendo la lista ordenada
    /// por precio ascendente.
    /// </summary>
    public void InsertarOrdenado(Producto producto)
    {
        NodoProducto nuevoNodo = new NodoProducto(producto);

        if (_cabeza == null ||
            producto.Precio < _cabeza.Data.Precio)
        {
            nuevoNodo.Siguiente = _cabeza;
            _cabeza = nuevoNodo;

            RegistrarAuditoriaSegura(
                "INSERT",
                producto.SKU,
                $"Producto '{producto.Nombre}' agregado al inventario. " +
                $"Precio: ${producto.Precio:F2}. Stock: {producto.Stock}."
            );

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

        RegistrarAuditoriaSegura(
            "INSERT",
            producto.SKU,
            $"Producto '{producto.Nombre}' agregado al inventario. " +
            $"Precio: ${producto.Precio:F2}. Stock: {producto.Stock}."
        );
    }

    /// <summary>
    /// Busca un producto mediante su SKU.
    /// </summary>
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
    /// Modifica el precio de un producto mediante su SKU.
    /// </summary>
    public void ModificarPrecio(
        int sku,
        double nuevoPrecio)
    {
        if (nuevoPrecio < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(nuevoPrecio),
                "El precio no puede ser negativo.");
        }

        NodoProducto? actual = _cabeza;

        while (actual != null)
        {
            if (actual.Data.SKU == sku)
            {
                Producto productoActual = actual.Data;

                double precioAnterior =
                    productoActual.Precio;

                productoActual.Precio =
                    nuevoPrecio;

                actual.Data =
                    productoActual;

                RegistrarAuditoriaSegura(
                    "UPDATE",
                    sku,
                    $"Precio de '{productoActual.Nombre}' actualizado " +
                    $"de ${precioAnterior:F2} a ${nuevoPrecio:F2}."
                );

                return;
            }

            actual = actual.Siguiente;
        }

        throw new KeyNotFoundException(
            $"El producto con SKU {sku} no fue encontrado.");
    }

    /// <summary>
    /// Modifica el stock real de un producto mediante su SKU.
    /// </summary>
    public void ModificarStock(
        int sku,
        int nuevoStock,
        string motivo)
    {
        if (nuevoStock < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(nuevoStock),
                "El stock no puede ser negativo.");
        }

        NodoProducto? actual = _cabeza;

        while (actual != null)
        {
            if (actual.Data.SKU == sku)
            {
                Producto productoActual =
                    actual.Data;

                int stockAnterior =
                    productoActual.Stock;

                productoActual.Stock =
                    nuevoStock;

                actual.Data =
                    productoActual;

                RegistrarAuditoriaSegura(
                    "UPDATE_STOCK",
                    sku,
                    $"Stock de '{productoActual.Nombre}' actualizado " +
                    $"de {stockAnterior} a {nuevoStock}. " +
                    $"Motivo: {motivo}."
                );

                return;
            }

            actual = actual.Siguiente;
        }

        throw new KeyNotFoundException(
            $"El producto con SKU {sku} no fue encontrado.");
    }

    /// <summary>
    /// Incrementa el stock actual de un producto.
    /// Este método se utiliza principalmente al procesar devoluciones.
    /// </summary>
    public void IncrementarStock(
        int sku,
        int cantidad,
        string motivo)
    {
        if (cantidad <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(cantidad),
                "La cantidad debe ser mayor que cero.");
        }

        Producto producto =
            BuscarPorSKU(sku);

        int nuevoStock =
            producto.Stock + cantidad;

        ModificarStock(
            sku,
            nuevoStock,
            motivo);
    }

    /// <summary>
    /// Decrementa el stock actual de un producto.
    /// </summary>
    public void DecrementarStock(
        int sku,
        int cantidad,
        string motivo)
    {
        if (cantidad <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(cantidad),
                "La cantidad debe ser mayor que cero.");
        }

        Producto producto =
            BuscarPorSKU(sku);

        if (producto.Stock < cantidad)
        {
            throw new InvalidOperationException(
                $"Stock insuficiente para SKU {sku}. " +
                $"Disponible: {producto.Stock}. " +
                $"Requerido: {cantidad}.");
        }

        int nuevoStock =
            producto.Stock - cantidad;

        ModificarStock(
            sku,
            nuevoStock,
            motivo);
    }

    /// <summary>
    /// Elimina un producto utilizando su SKU.
    /// </summary>
    public bool EliminarPorSKU(int sku)
    {
        if (_cabeza == null)
        {
            return false;
        }

        if (_cabeza.Data.SKU == sku)
        {
            string nombreEliminado =
                _cabeza.Data.Nombre;

            _cabeza =
                _cabeza.Siguiente;

            RegistrarAuditoriaSegura(
                "DELETE",
                sku,
                $"Producto '{nombreEliminado}' eliminado del inventario."
            );

            return true;
        }

        NodoProducto actual =
            _cabeza;

        while (actual.Siguiente != null)
        {
            if (actual.Siguiente.Data.SKU == sku)
            {
                string nombreEliminado =
                    actual.Siguiente.Data.Nombre;

                actual.Siguiente =
                    actual.Siguiente.Siguiente;

                RegistrarAuditoriaSegura(
                    "DELETE",
                    sku,
                    $"Producto '{nombreEliminado}' eliminado del inventario."
                );

                return true;
            }

            actual =
                actual.Siguiente;
        }

        return false;
    }

    /// <summary>
    /// Muestra todos los productos almacenados
    /// en el inventario.
    /// </summary>
    public void MostrarProductos()
    {
        if (_cabeza == null)
        {
            Console.WriteLine(
                "El inventario está vacío.");

            return;
        }

        NodoProducto? actual =
            _cabeza;

        Console.WriteLine(
            "SKU\tNombre\t\tPrecio\t\tStock\tProveedor");

        Console.WriteLine(
            "---------------------------------------------------------------");

        while (actual != null)
        {
            Producto producto =
                actual.Data;

            Console.WriteLine(
                $"{producto.SKU}\t" +
                $"{producto.Nombre,-15}\t" +
                $"${producto.Precio,8:F2}\t" +
                $"{producto.Stock}\t" +
                $"{producto.DatosProveedor.NombreCorporativo}");

            actual =
                actual.Siguiente;
        }
    }

    /// <summary>
    /// Registra una operación en la bitácora sin permitir
    /// que un error de auditoría detenga la operación principal.
    /// </summary>
    private void RegistrarAuditoriaSegura(
        string tipoOperacion,
        int productoId,
        string referencia)
    {
        try
        {
            _auditoria.RegistrarEvento(
                tipoOperacion,
                productoId,
                referencia);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(
                $"[AUDIT-ERROR] {tipoOperacion} " +
                $"Producto={productoId}: {ex.Message}");
        }
    }
}