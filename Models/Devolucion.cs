namespace FastCartBackendCore.Models;

public class Devolucion
{
    public int IdDevolucion { get; set; }

    public int SKU { get; set; }

    public int Cantidad { get; set; }

    public string Cliente { get; set; } = string.Empty;

    public string Motivo { get; set; } = string.Empty;

    public DateTime FechaHora { get; set; } = DateTime.UtcNow;
}