using FastCartBackendCore.Models;
using FastCartBackendCore.Services;

Console.Title = "FastCart Backend Core - Motor Logístico v4.0";

// ============================================================
// SERVICIOS CENTRALES
// ============================================================

AuditoriaService auditoria = new AuditoriaService();
InventarioLista inventario = new InventarioLista(auditoria);
ColaDespacho colaDespacho = new ColaDespacho();
PilaDevoluciones pilaDevoluciones = new PilaDevoluciones();

// ============================================================
// PROVEEDORES BASE
// ============================================================

Proveedor proveedor1 = new Proveedor
{
    IdProveedor = 1,
    NombreCorporativo = "Proveedor Norte"
};

Proveedor proveedor2 = new Proveedor
{
    IdProveedor = 2,
    NombreCorporativo = "Proveedor Centro"
};

Proveedor proveedor3 = new Proveedor
{
    IdProveedor = 3,
    NombreCorporativo = "Proveedor Sur"
};

// ============================================================
// CATÁLOGO BASE
// ============================================================

Producto[] productos =
{
    new Producto
    {
        SKU = 1001,
        Nombre = "Laptop",
        Precio = 15999.00,
        Stock = 8,
        DatosProveedor = proveedor1
    },
    new Producto
    {
        SKU = 1002,
        Nombre = "Monitor",
        Precio = 4299.00,
        Stock = 15,
        DatosProveedor = proveedor2
    },
    new Producto
    {
        SKU = 1003,
        Nombre = "Teclado",
        Precio = 899.00,
        Stock = 30,
        DatosProveedor = proveedor3
    },
    new Producto
    {
        SKU = 1004,
        Nombre = "Mouse",
        Precio = 499.00,
        Stock = 40,
        DatosProveedor = proveedor1
    },
    new Producto
    {
        SKU = 1005,
        Nombre = "Impresora",
        Precio = 3199.00,
        Stock = 12,
        DatosProveedor = proveedor2
    },
    new Producto
    {
        SKU = 1006,
        Nombre = "Webcam",
        Precio = 1299.00,
        Stock = 18,
        DatosProveedor = proveedor3
    },
    new Producto
    {
        SKU = 1007,
        Nombre = "Router",
        Precio = 1899.00,
        Stock = 22,
        DatosProveedor = proveedor1
    },
    new Producto
    {
        SKU = 1008,
        Nombre = "Tablet",
        Precio = 7499.00,
        Stock = 10,
        DatosProveedor = proveedor2
    },
    new Producto
    {
        SKU = 1009,
        Nombre = "Smartphone",
        Precio = 11999.00,
        Stock = 14,
        DatosProveedor = proveedor3
    },
    new Producto
    {
        SKU = 1010,
        Nombre = "Bocina",
        Precio = 999.00,
        Stock = 25,
        DatosProveedor = proveedor1
    },
    new Producto
    {
        SKU = 1011,
        Nombre = "Audifonos",
        Precio = 1499.00,
        Stock = 35,
        DatosProveedor = proveedor2
    },
    new Producto
    {
        SKU = 1012,
        Nombre = "Disco SSD",
        Precio = 2199.00,
        Stock = 20,
        DatosProveedor = proveedor3
    },
    new Producto
    {
        SKU = 1013,
        Nombre = "Memoria RAM",
        Precio = 1699.00,
        Stock = 28,
        DatosProveedor = proveedor1
    },
    new Producto
    {
        SKU = 1014,
        Nombre = "Proyector",
        Precio = 8999.00,
        Stock = 6,
        DatosProveedor = proveedor2
    },
    new Producto
    {
        SKU = 1015,
        Nombre = "Microfono",
        Precio = 2499.00,
        Stock = 16,
        DatosProveedor = proveedor3
    }
};

// Carga inicial del catálogo utilizando la lista enlazada.
foreach (Producto producto in productos)
{
    inventario.InsertarOrdenado(producto);
}

// ============================================================
// MENÚ MAESTRO
// ============================================================

bool ejecutando = true;

while (ejecutando)
{
    Console.WriteLine();
    Console.WriteLine("╔════════════════════════════════════════════════╗");
    Console.WriteLine("║       FASTCART BACKEND CORE - FASE 4          ║");
    Console.WriteLine("║          MOTOR LOGÍSTICO v4.0                 ║");
    Console.WriteLine("╠════════════════════════════════════════════════╣");
    Console.WriteLine("║ FASE 1 Y 2 - CATÁLOGO / LISTA ENLAZADA       ║");
    Console.WriteLine("║  1. Mostrar catálogo                          ║");
    Console.WriteLine("║  2. Buscar producto por SKU                   ║");
    Console.WriteLine("║  3. Modificar precio                          ║");
    Console.WriteLine("║  4. Eliminar producto                         ║");
    Console.WriteLine("╠════════════════════════════════════════════════╣");
    Console.WriteLine("║ FASE 3 - AUDITORÍA BIDIRECCIONAL              ║");
    Console.WriteLine("║  5. Ver historial cronológico                 ║");
    Console.WriteLine("║  6. Ver historial inverso                     ║");
    Console.WriteLine("║  7. Validar integridad de auditoría            ║");
    Console.WriteLine("╠════════════════════════════════════════════════╣");
    Console.WriteLine("║ FASE 4 - COLA FIFO / PILA LIFO                ║");
    Console.WriteLine("║  8. Encolar pedido                            ║");
    Console.WriteLine("║  9. Despachar pedido                          ║");
    Console.WriteLine("║ 10. Registrar devolución                      ║");
    Console.WriteLine("║ 11. Procesar devolución                       ║");
    Console.WriteLine("║ 12. Estado de cola y pila                     ║");
    Console.WriteLine("╠════════════════════════════════════════════════╣");
    Console.WriteLine("║  0. Salir                                     ║");
    Console.WriteLine("╚════════════════════════════════════════════════╝");

    int opcion = LeerEntero("Seleccione una opción: ");

    Console.WriteLine();

    try
    {
        switch (opcion)
        {
            // ====================================================
            // FASE 1 Y 2
            // ====================================================

            case 1:
                Console.WriteLine("=== CATÁLOGO DE PRODUCTOS ===");
                Console.WriteLine();

                inventario.MostrarProductos();
                break;

            case 2:
            {
                Console.WriteLine("=== BÚSQUEDA POR SKU ===");

                int sku = LeerEntero("SKU: ");

                Producto producto =
                    inventario.BuscarPorSKU(sku);

                Console.WriteLine();
                Console.WriteLine("Producto encontrado:");
                Console.WriteLine($"SKU: {producto.SKU}");
                Console.WriteLine($"Nombre: {producto.Nombre}");
                Console.WriteLine($"Precio: ${producto.Precio:F2}");
                Console.WriteLine($"Stock: {producto.Stock}");
                Console.WriteLine(
                    $"Proveedor: {producto.DatosProveedor.NombreCorporativo}");

                break;
            }

            case 3:
            {
                Console.WriteLine("=== MODIFICAR PRECIO ===");

                int sku =
                    LeerEntero("SKU: ");

                double nuevoPrecio =
                    LeerDouble("Nuevo precio: $");

                inventario.ModificarPrecio(
                    sku,
                    nuevoPrecio);

                Console.WriteLine(
                    "Precio actualizado correctamente.");

                break;
            }

            case 4:
            {
                Console.WriteLine("=== ELIMINAR PRODUCTO ===");

                int sku =
                    LeerEntero("SKU: ");

                bool eliminado =
                    inventario.EliminarPorSKU(sku);

                if (eliminado)
                {
                    Console.WriteLine(
                        $"Producto con SKU {sku} eliminado.");
                }
                else
                {
                    Console.WriteLine(
                        $"No existe un producto con SKU {sku}.");
                }

                break;
            }

            // ====================================================
            // FASE 3
            // ====================================================

            case 5:
                Console.WriteLine(
                    "=== HISTORIAL CRONOLÓGICO ===");

                auditoria.ImprimirHistorial();
                break;

            case 6:
                Console.WriteLine(
                    "=== HISTORIAL INVERSO ===");

                auditoria.ImprimirHistorialInverso();
                break;

            case 7:
            {
                Console.WriteLine(
                    "=== VALIDACIÓN DE INTEGRIDAD ===");

                bool integridad =
                    auditoria.ValidarIntegridad();

                Console.WriteLine(
                    integridad
                        ? "La lista de auditoría es estructuralmente válida."
                        : "ERROR: La lista presenta inconsistencias.");

                Console.WriteLine(
                    $"Total de registros: {auditoria.TotalRegistros}");

                break;
            }

            // ====================================================
            // FASE 4 - COLA FIFO
            // ====================================================

            case 8:
            {
                Console.WriteLine(
                    "=== ENCOLAR NUEVO PEDIDO (FIFO) ===");

                int idPedido =
                    LeerEntero("ID del pedido: ");

                int sku =
                    LeerEntero("SKU del producto: ");

                int cantidad =
                    LeerEntero("Cantidad: ");

                string cliente =
                    LeerTexto("Cliente: ");

                if (cantidad <= 0)
                {
                    Console.WriteLine(
                        "La cantidad debe ser mayor que cero.");

                    break;
                }

                // Comprobamos primero que el SKU exista.
                inventario.BuscarPorSKU(sku);

                Pedido pedido =
                    new Pedido(
                        idPedido,
                        sku,
                        cantidad,
                        cliente);

                colaDespacho.EncolarPedido(pedido);

                break;
            }

            case 9:
            {
                Console.WriteLine(
                    "=== DESPACHAR PEDIDO (FIFO) ===");

                Pedido? pedido =
                    colaDespacho.DespacharPedido(
                        inventario,
                        auditoria);

                if (pedido != null)
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        $"Pedido #{pedido.IdPedido} procesado.");

                    Producto actualizado =
                        inventario.BuscarPorSKU(
                            pedido.SKU);

                    Console.WriteLine(
                        $"Stock actual del SKU {pedido.SKU}: " +
                        $"{actualizado.Stock}");
                }

                break;
            }

            // ====================================================
            // FASE 4 - PILA LIFO
            // ====================================================

            case 10:
            {
                Console.WriteLine(
                    "=== REGISTRAR DEVOLUCIÓN (LIFO) ===");

                int idDevolucion =
                    LeerEntero("ID de devolución: ");

                int sku =
                    LeerEntero("SKU: ");

                int cantidad =
                    LeerEntero("Cantidad devuelta: ");

                string cliente =
                    LeerTexto("Cliente: ");

                string motivo =
                    LeerTexto("Motivo: ");

                if (cantidad <= 0)
                {
                    Console.WriteLine(
                        "La cantidad debe ser mayor que cero.");

                    break;
                }

                // Validación previa del SKU.
                inventario.BuscarPorSKU(sku);

                Devolucion devolucion =
                    new Devolucion
                    {
                        IdDevolucion = idDevolucion,
                        SKU = sku,
                        Cantidad = cantidad,
                        Cliente = cliente,
                        Motivo = motivo,
                        FechaHora = DateTime.UtcNow
                    };

                pilaDevoluciones.PushDevolucion(
                    devolucion);

                break;
            }

            case 11:
            {
                Console.WriteLine(
                    "=== PROCESAR DEVOLUCIÓN (LIFO) ===");

                Devolucion? devolucion =
                    pilaDevoluciones.PopDevolucion(
                        inventario,
                        auditoria);

                if (devolucion != null)
                {
                    Console.WriteLine();

                    Producto actualizado =
                        inventario.BuscarPorSKU(
                            devolucion.SKU);

                    Console.WriteLine(
                        $"Stock actual del SKU {devolucion.SKU}: " +
                        $"{actualizado.Stock}");
                }

                break;
            }

            case 12:
                Console.WriteLine(
                    "=== ESTADO DEL MOTOR LOGÍSTICO ===");

                Console.WriteLine(
                    $"Pedidos pendientes en cola: " +
                    $"{colaDespacho.TotalEncolados}");

                Console.WriteLine(
                    $"Devoluciones pendientes en pila: " +
                    $"{pilaDevoluciones.TotalDevoluciones}");

                Console.WriteLine(
                    $"Registros de auditoría: " +
                    $"{auditoria.TotalRegistros}");

                Console.WriteLine(
                    $"Integridad de auditoría: " +
                    $"{(auditoria.ValidarIntegridad() ? "OK" : "ERROR")}");

                break;

            case 0:
                Console.WriteLine(
                    "Cerrando FastCart Backend Core...");

                ejecutando = false;
                break;

            default:
                Console.WriteLine(
                    "Opción no válida.");
                break;
        }
    }
    catch (KeyNotFoundException ex)
    {
        Console.WriteLine(
            $"ERROR: {ex.Message}");
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine(
            $"ERROR: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(
            $"ERROR: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            $"ERROR NO CONTROLADO: {ex.Message}");
    }

    if (ejecutando)
    {
        Console.WriteLine();
        Console.WriteLine(
            "Presione ENTER para regresar al menú...");

        Console.ReadLine();

        Console.Clear();
    }
}

// ============================================================
// MÉTODOS AUXILIARES DEL MENÚ
// ============================================================

static int LeerEntero(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);

        string? entrada =
            Console.ReadLine();

        if (int.TryParse(
                entrada,
                out int resultado))
        {
            return resultado;
        }

        Console.WriteLine(
            "Valor inválido. Ingrese un número entero.");
    }
}

static double LeerDouble(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);

        string? entrada =
            Console.ReadLine();

        if (double.TryParse(
                entrada,
                out double resultado))
        {
            return resultado;
        }

        Console.WriteLine(
            "Valor inválido. Ingrese un número.");
    }
}

static string LeerTexto(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);

        string? entrada =
            Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(entrada))
        {
            return entrada.Trim();
        }

        Console.WriteLine(
            "El texto no puede estar vacío.");
    }
}