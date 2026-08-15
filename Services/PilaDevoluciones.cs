using FastCartBackendCore.Models;

namespace FastCartBackendCore.Services;

public class PilaDevoluciones
{
    private NodoPila? _top;
    private int _totalDevoluciones;

    /// <summary>
    /// Indica si la pila de devoluciones está vacía.
    /// </summary>
    public bool EstaVacia()
    {
        return _top == null;
    }

    /// <summary>
    /// Obtiene la cantidad actual de devoluciones pendientes.
    /// </summary>
    public int TotalDevoluciones
    {
        get { return _totalDevoluciones; }
    }

    /// <summary>
    /// Inserta una devolución en la cima de la pila.
    /// Complejidad temporal O(1).
    /// </summary>
    public void PushDevolucion(Devolucion nuevaDevolucion)
    {
        if (nuevaDevolucion == null)
        {
            throw new ArgumentNullException(
                nameof(nuevaDevolucion),
                "La devolución no puede ser nula.");
        }

        NodoPila nuevoNodo =
            new NodoPila(nuevaDevolucion);

        nuevoNodo.Siguiente = _top;

        _top = nuevoNodo;

        _totalDevoluciones++;

        Console.WriteLine(
            $"[PILA] Devolución #{nuevaDevolucion.IdDevolucion} " +
            $"(SKU: {nuevaDevolucion.SKU}) registrada. " +
            $"Total en pila: {_totalDevoluciones}");
    }

    /// <summary>
    /// Procesa la devolución ubicada en la cima de la pila,
    /// reintegra el stock al inventario y registra la operación
    /// en la auditoría.
    /// </summary>
    public Devolucion? PopDevolucion(
        InventarioLista inventario,
        AuditoriaService auditoria)
    {
        ArgumentNullException.ThrowIfNull(inventario);
        ArgumentNullException.ThrowIfNull(auditoria);

        if (EstaVacia())
        {
            Console.WriteLine(
                "[PILA] ERROR: La pila está vacía. " +
                "No hay devoluciones para procesar.");

            return null;
        }

        Devolucion devolucionProcesada =
            _top!.Dato;

        Producto producto;

        try
        {
            producto =
                inventario.BuscarPorSKU(
                    devolucionProcesada.SKU);
        }
        catch (KeyNotFoundException)
        {
            auditoria.RegistrarEvento(
                "DEVOLUCION_FALLIDA",
                devolucionProcesada.SKU,
                $"Devolución #{devolucionProcesada.IdDevolucion}: " +
                $"SKU {devolucionProcesada.SKU} no encontrado.");

            Console.WriteLine(
                $"[PILA] ERROR: SKU " +
                $"{devolucionProcesada.SKU} no encontrado. " +
                "No se puede reintegrar el stock.");

            return null;
        }

        _top = _top.Siguiente;

        _totalDevoluciones--;

        int nuevoStock =
            producto.Stock + devolucionProcesada.Cantidad;

        inventario.ModificarStock(
            devolucionProcesada.SKU,
            nuevoStock,
            "DEVOLUCION");

        auditoria.RegistrarEvento(
            "DEVOLUCION_EXITOSA",
            devolucionProcesada.SKU,
            $"Devolución #{devolucionProcesada.IdDevolucion} procesada. " +
            $"Cantidad reintegrada: {devolucionProcesada.Cantidad}. " +
            $"Nuevo stock: {nuevoStock}. " +
            $"Cliente: {devolucionProcesada.Cliente}. " +
            $"Motivo: {devolucionProcesada.Motivo}.");

        Console.WriteLine(
            $"[PILA] Devolución " +
            $"#{devolucionProcesada.IdDevolucion} " +
            $"procesada correctamente. " +
            $"Stock reintegrado: +{devolucionProcesada.Cantidad}.");

        return devolucionProcesada;
    }
}