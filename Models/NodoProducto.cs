using FastCartBackendCore.Models;

namespace FastCartBackendCore.Models;

public class NodoProducto
{
    public Producto Data { get; set; }

    public NodoProducto? Siguiente { get; set; }

    public NodoProducto(Producto producto)
    {
        Data = producto;
        Siguiente = null;
    }
}