namespace FastCartBackendCore.Models;

public class NodoAuditoria
{
    public LogMovimiento Dato { get; set; }

    public NodoAuditoria? Siguiente { get; set; }

    public NodoAuditoria? Anterior { get; set; }

    public NodoAuditoria(LogMovimiento dato)
    {
        Dato = dato;
        Siguiente = null;
        Anterior = null;
    }
}