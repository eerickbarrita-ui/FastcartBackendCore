using FastCartBackendCore.Models;

namespace FastCartBackendCore.Services;

public class AuditoriaService
{
    private NodoAuditoria? _cabeza;
    private NodoAuditoria? _cola;

    public int TotalRegistros { get; private set; }

    public void RegistrarEvento(string tipoOperacion, int productoId, string referencia)
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
            nuevoNodo.Anterior = _cola;
            _cola!.Siguiente = nuevoNodo;
            _cola = nuevoNodo;
        }

        TotalRegistros++;
    }

    public void ImprimirHistorial()
    {
        if (_cabeza == null)
        {
            Console.WriteLine("No existen registros de auditoría.");
            return;
        }

        Console.WriteLine("\n=== HISTORIAL DE AUDITORÍA ===");

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

        Console.WriteLine($"Total de registros: {TotalRegistros}");
    }

    public void ImprimirHistorialInverso()
    {
        if (_cola == null)
        {
            Console.WriteLine("No existen registros de auditoría.");
            return;
        }

        Console.WriteLine("\n=== HISTORIAL INVERSO DE AUDITORÍA ===");

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

        Console.WriteLine($"Total de registros: {TotalRegistros}");
    }
}