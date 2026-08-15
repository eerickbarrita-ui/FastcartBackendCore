using FastCartBackendCore.Models;

namespace FastCartBackendCore.Services;

public class ColaDespacho
{
    private NodoCola? _frente;
    private NodoCola? _fin;
    private int _totalEncolados;

    public bool EstaVacia()
    {
        return _frente == null;
    }

    public int TotalEncolados
    {
        get { return _totalEncolados; }
    }

    public void EncolarPedido(Pedido nuevoPedido)
    {
        if (nuevoPedido == null)
        {
            throw new ArgumentNullException(
                nameof(nuevoPedido),
                "El pedido no puede ser nulo.");
        }

        NodoCola nuevoNodo = new NodoCola(nuevoPedido);

        if (EstaVacia())
        {
            _frente = nuevoNodo;
            _fin = nuevoNodo;
        }
        else
        {
            _fin!.Siguiente = nuevoNodo;
            _fin = nuevoNodo;
        }

        _totalEncolados++;

        Console.WriteLine(
            $"[COLA] Pedido #{nuevoPedido.IdPedido} " +
            $"(SKU: {nuevoPedido.SKU}) encolado. " +
            $"Total en cola: {_totalEncolados}");
    }

    public Pedido? DespacharPedido(
        InventarioLista inventario,
        AuditoriaService auditoria)
    {
        ArgumentNullException.ThrowIfNull(inventario);
        ArgumentNullException.ThrowIfNull(auditoria);

        if (EstaVacia())
        {
            Console.WriteLine(
                "[COLA] ERROR: La cola está vacía. " +
                "No hay pedidos para despachar.");

            return null;
        }

        Pedido pedidoDespachado = _frente!.Dato;

        Producto producto;

        try
        {
            producto =
                inventario.BuscarPorSKU(pedidoDespachado.SKU);
        }
        catch (KeyNotFoundException)
        {
            auditoria.RegistrarEvento(
                "DESPACHO_FALLIDO",
                pedidoDespachado.SKU,
                $"Pedido #{pedidoDespachado.IdPedido}: " +
                $"SKU {pedidoDespachado.SKU} no encontrado.");

            Console.WriteLine(
                $"[COLA] ERROR: SKU {pedidoDespachado.SKU} " +
                "no encontrado en el inventario.");

            return null;
        }

        if (producto.Stock < pedidoDespachado.Cantidad)
        {
            auditoria.RegistrarEvento(
                "STOCK_INSUFICIENTE",
                pedidoDespachado.SKU,
                $"Pedido #{pedidoDespachado.IdPedido}. " +
                $"Disponible: {producto.Stock}. " +
                $"Requerido: {pedidoDespachado.Cantidad}.");

            Console.WriteLine(
                $"[COLA] ERROR: Stock insuficiente para SKU " +
                $"{pedidoDespachado.SKU}. " +
                $"Disponible: {producto.Stock}. " +
                $"Requerido: {pedidoDespachado.Cantidad}.");

            return null;
        }

        _frente = _frente.Siguiente;

        if (_frente == null)
        {
            _fin = null;
        }

        _totalEncolados--;

        int nuevoStock =
            producto.Stock - pedidoDespachado.Cantidad;

        inventario.ModificarStock(
            pedidoDespachado.SKU,
            nuevoStock,
            "DESPACHO");

        auditoria.RegistrarEvento(
            "DESPACHO_EXITOSO",
            pedidoDespachado.SKU,
            $"Pedido #{pedidoDespachado.IdPedido} despachado. " +
            $"Cantidad: {pedidoDespachado.Cantidad}. " +
            $"Stock restante: {nuevoStock}. " +
            $"Cliente: {pedidoDespachado.Cliente}.");

        Console.WriteLine(
            $"[COLA] Pedido #{pedidoDespachado.IdPedido} " +
            $"despachado correctamente.");

        return pedidoDespachado;
    }
}