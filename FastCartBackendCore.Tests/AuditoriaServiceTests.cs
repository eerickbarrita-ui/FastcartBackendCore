using FastCartBackendCore.Services;

namespace FastCartBackendCore.Tests;

public class AuditoriaServiceTests
{
    [Fact]
    public void ValidarIntegridad_ListaVacia_RetornaTrue()
    {
        AuditoriaService auditoria = new AuditoriaService();

        bool resultado = auditoria.ValidarIntegridad();

        Assert.True(resultado);
        Assert.Equal(0, auditoria.TotalRegistros);
    }

    [Fact]
    public void RegistrarEvento_MultiplesEventos_IncrementaTotalRegistros()
    {
        AuditoriaService auditoria = new AuditoriaService();

        auditoria.RegistrarEvento(
            "INSERT",
            1001,
            "Producto Laptop agregado.");

        auditoria.RegistrarEvento(
            "UPDATE",
            1001,
            "Precio de Laptop actualizado.");

        auditoria.RegistrarEvento(
            "DELETE",
            1001,
            "Producto Laptop eliminado.");

        Assert.Equal(3, auditoria.TotalRegistros);
        Assert.True(auditoria.ValidarIntegridad());
    }

    [Fact]
    public void ImprimirHistorial_MuestraEventosEnOrdenCronologico()
    {
        AuditoriaService auditoria = new AuditoriaService();

        auditoria.RegistrarEvento(
            "INSERT",
            1001,
            "Primer evento");

        auditoria.RegistrarEvento(
            "UPDATE",
            1002,
            "Segundo evento");

        StringWriter salida = new StringWriter();
        TextWriter salidaOriginal = Console.Out;

        try
        {
            Console.SetOut(salida);

            auditoria.ImprimirHistorial();
        }
        finally
        {
            Console.SetOut(salidaOriginal);
        }

        string resultado = salida.ToString();

        int posicionPrimerEvento =
            resultado.IndexOf("Primer evento");

        int posicionSegundoEvento =
            resultado.IndexOf("Segundo evento");

        Assert.True(posicionPrimerEvento >= 0);
        Assert.True(posicionSegundoEvento >= 0);

        Assert.True(
            posicionPrimerEvento < posicionSegundoEvento);
    }

    [Fact]
    public void ImprimirHistorialInverso_MuestraEventosEnOrdenInverso()
    {
        AuditoriaService auditoria = new AuditoriaService();

        auditoria.RegistrarEvento(
            "INSERT",
            1001,
            "Primer evento");

        auditoria.RegistrarEvento(
            "UPDATE",
            1002,
            "Segundo evento");

        StringWriter salida = new StringWriter();
        TextWriter salidaOriginal = Console.Out;

        try
        {
            Console.SetOut(salida);

            auditoria.ImprimirHistorialInverso();
        }
        finally
        {
            Console.SetOut(salidaOriginal);
        }

        string resultado = salida.ToString();

        int posicionPrimerEvento =
            resultado.IndexOf("Primer evento");

        int posicionSegundoEvento =
            resultado.IndexOf("Segundo evento");

        Assert.True(posicionPrimerEvento >= 0);
        Assert.True(posicionSegundoEvento >= 0);

        Assert.True(
            posicionSegundoEvento < posicionPrimerEvento);
    }

    [Fact]
    public void InventarioLista_AuditoriaNula_LanzaArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => new InventarioLista(null!));
    }

    [Fact]
    public void ValidarIntegridad_MultiplesRegistros_RetornaTrue()
    {
        AuditoriaService auditoria = new AuditoriaService();

        for (int i = 1; i <= 10; i++)
        {
            auditoria.RegistrarEvento(
                "INSERT",
                1000 + i,
                $"Evento número {i}");
        }

        bool resultado = auditoria.ValidarIntegridad();

        Assert.True(resultado);
        Assert.Equal(10, auditoria.TotalRegistros);
    }
}
