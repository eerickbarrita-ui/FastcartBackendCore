using FastCartBackendCore.Models;

namespace FastCartBackendCore.Services;

public class AuditoriaService
{
    private NodoAuditoria? _cabeza;
    private NodoAuditoria? _cola;

    public int TotalRegistros { get; private set; }

    /// <summary>
    /// Registra un nuevo evento al final de la lista doblemente enlazada.
    /// </summary>
    public void RegistrarEvento(
        string tipoOperacion,
        int productoId,
        string referencia)
    {
        LogMovimiento movimiento = new LogMovimiento
        {
            TipoOperacion = tipoOperacion,
            ProductoId = productoId,
            Referencia = referencia,
            FechaHora = DateTime.UtcNow
        };

        NodoAuditoria nuevoNodo = new NodoAuditoria(movimiento);

        if (_cabeza == null)
        {
            _cabeza = nuevoNodo;
            _cola = nuevoNodo;
        }
        else
        {
            // 1. El nuevo nodo apunta hacia la cola anterior.
            nuevoNodo.Anterior = _cola;

            // 2. La cola anterior apunta hacia el nuevo nodo.
            _cola!.Siguiente = nuevoNodo;

            // 3. El nuevo nodo se convierte en la nueva cola.
            _cola = nuevoNodo;
        }

        TotalRegistros++;
    }

    /// <summary>
    /// Imprime el historial desde el evento más antiguo
    /// hasta el evento más reciente.
    /// </summary>
    public void ImprimirHistorial()
    {
        if (_cabeza == null)
        {
            Console.WriteLine(
                "No existen registros de auditoría.");

            return;
        }

        Console.WriteLine(
            "\n=== HISTORIAL DE AUDITORÍA ===");

        NodoAuditoria? actual = _cabeza;

        while (actual != null)
        {
            Console.WriteLine(
                $"{actual.Dato.FechaHora:yyyy-MM-dd HH:mm:ss} UTC | " +
                $"{actual.Dato.TipoOperacion} | " +
                $"Producto: {actual.Dato.ProductoId} | " +
                $"{actual.Dato.Referencia}"
            );

            actual = actual.Siguiente;
        }

        Console.WriteLine(
            $"Total de registros: {TotalRegistros}");
    }

    /// <summary>
    /// Imprime el historial desde el evento más reciente
    /// hasta el evento más antiguo.
    /// </summary>
    public void ImprimirHistorialInverso()
    {
        if (_cola == null)
        {
            Console.WriteLine(
                "No existen registros de auditoría.");

            return;
        }

        Console.WriteLine(
            "\n=== HISTORIAL INVERSO DE AUDITORÍA ===");

        NodoAuditoria? actual = _cola;

        while (actual != null)
        {
            Console.WriteLine(
                $"{actual.Dato.FechaHora:yyyy-MM-dd HH:mm:ss} UTC | " +
                $"{actual.Dato.TipoOperacion} | " +
                $"Producto: {actual.Dato.ProductoId} | " +
                $"{actual.Dato.Referencia}"
            );

            actual = actual.Anterior;
        }

        Console.WriteLine(
            $"Total de registros: {TotalRegistros}");
    }

    /// <summary>
    /// Verifica que la lista tenga la misma cantidad de nodos
    /// al recorrerla hacia adelante y hacia atrás.
    /// </summary>
    public bool ValidarIntegridad()
    {
        int conteoAdelante = 0;
        int conteoAtras = 0;

        NodoAuditoria? actual = _cabeza;

        while (actual != null)
        {
            conteoAdelante++;
            actual = actual.Siguiente;
        }

        actual = _cola;

        while (actual != null)
        {
            conteoAtras++;
            actual = actual.Anterior;
        }

        return conteoAdelante == conteoAtras &&
               conteoAdelante == TotalRegistros;
    }
}