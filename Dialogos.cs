namespace SistemaCourier;

using System.Threading.Tasks;


public class Dialogos {

    public static async Task CargarDialogoCreate() {
        Console.WriteLine("\n===== REGISTRAR NUEVO ENVIO =====");
        Console.WriteLine("1. Envio Nacional");
        Console.WriteLine("2. Envio Internacional");
        Console.Write("Seleccione el tipo de envio: ");
        string opcion = Console.ReadLine() ?? "";

        if (opcion != "1" && opcion != "2") {
            Console.WriteLine("Opcion no valida. Se cancela el registro.");
            return;
        }

        Console.Write("ID de seguimiento : ");
        string id = Console.ReadLine() ?? "";

        if(string.IsNullOrWhiteSpace(id)) {
            Console.WriteLine("ID de seguimiento no valido. Se cancela el registro.");
            return;
        }

        Console.Write("Tipo de paquete (Documento/Caja/Fragil...) : ");
        string tipoPaquete = Console.ReadLine() ?? "";

        Console.Write("Descripcion del Contenido : ");
        string descripcion = Console.ReadLine() ?? "";

        double peso = LeerDouble("Peso en kg : ");
        double precio = LeerDouble("Costo base : ");


        EnvioBase envio;

        if (opcion == "1") {

            Console.Write("Region de destino : ");
            string destino = Console.ReadLine() ?? "";

            envio = new EnvioNacional (id, tipoPaquete, descripcion, peso, precio, destino);
        }
        else {
            Console.Write("Pais de destino : ");
            string pais = Console.ReadLine() ?? "";

            Console.Write("Codigo de aduana : ");
            string codigoAduana = Console.ReadLine() ?? "";

            if(string.IsNullOrWhiteSpace(codigoAduana)) codigoAduana = "PENDIENTE";

            envio = new EnvioInternacional (id, tipoPaquete, descripcion, peso, precio, pais, codigoAduana);
        }

        bool creado = await EnvioCollection.CreateAsync(envio);

        if (creado && envio is IAduana aduana) {
            Console.Write("Declarar impuestos aduaneros ahora? (s/n) : ");
            string respuesta = Console.ReadLine() ?? "";

            if(respuesta.Trim().ToLower() == "s") {
                try {
                    Console.WriteLine("[Declarando impuestos...]");
                    await aduana.DeclararImpuestosAsync();
                    await EnvioCollection.SaveToFileAsync();
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

    public static async Task CargarDialogoRead() {
        Console.WriteLine("\n===== BUSCAR ENVIO CODIGO DE SEGUIMIENTO =====");
        Console.Write("Ingrese el ID de seguimiento : ");
        string id = Console.ReadLine() ?? "";

        EnvioBase? envio = await EnvioCollection.ReadByIdAsync(id);

        if (envio == null)
            Console.WriteLine("No se encontro ningun envio con ese ID.");
        else
            envio.MostrarFichaEnvio(); 
    }

    public static async Task CargarDialogoUpdate() {
        Console.WriteLine("\n===== ACTUALIZAR DATOS DE ENVIO =====");
        Console.Write("Ingrese el ID de seguimiento : ");
        string envioId = Console.ReadLine() ?? "";

        EnvioBase? envio = await EnvioCollection.ReadByIdAsync(envioId);

        if (envio == null) {
            Console.WriteLine("No se encontro ningun envio con ese ID.");
            return;
        }

        Console.WriteLine("\nDatos actuales del envio:");
        envio.MostrarFichaEnvio();
        Console.WriteLine("(Deje en blanco o escriba 0 para conservar el valor actual)\n");

        Console.Write($"Nuevo tipo de paquete [{envio.TipoPaquete}] : ");
        string tipoPaquete = Console.ReadLine() ?? "";

        Console.Write($"Nueva descripcion del contenido [{envio.DescripcionContenido}] : ");
        string descripcionContenido = Console.ReadLine() ?? "";

        double pesoKilos = LeerDoubleOpcional($"Nuevo peso en kg [{envio.PesoKilos}] : ");
        double costoBase = LeerDoubleOpcional($"Nuevo precio $ [{envio.CostoBase}] : ");

        await EnvioCollection.UpdateAsync(envioId, tipoPaquete, descripcionContenido, pesoKilos, costoBase);
    }

    public static async Task CargarDialogoDelete() {
        Console.WriteLine("\n===== ELIMINAR ORDEN DE ENVIO =====");
        Console.Write("Ingrese el ID de seguimiento : ");
        string id = Console.ReadLine() ?? "";

        Console.Write($"Confirma eliminar el envio {id}? (s/n) : ");
        string respuesta = Console.ReadLine() ?? "";

        if (respuesta.Trim().ToLower() != "s") {
            Console.WriteLine("Operacion cancelada.");
            return;
        }

        await EnvioCollection.DeleteAsync(id);
    }

    public static async Task CargarDialogoSearchByFilter() {
        Console.WriteLine("\n===== BUSCAR POR TIPO DE PAQUETE O DESTINO =====");
        Console.Write("Ingrese el termino de busqueda : ");
        string filtro = Console.ReadLine() ?? "";

        List<EnvioBase> resultados = await EnvioCollection.SearchByFilterAsync(filtro);

        if (resultados.Count == 0) {
            Console.WriteLine($"No se encontraron envios que coincidan con \"{filtro}\".");
            return;
        }

        Console.WriteLine($"\nSe encontraron {resultados.Count} coincidencia(s):\n");
        foreach (EnvioBase envio in resultados)
        {
            envio.MostrarFichaEnvio();
        }
    }

    public static async Task ListAll() {
        await EnvioCollection.ListAllAsync();
    }

    private static double LeerDouble(string mensaje) {
        double valor;
        do {
            Console.Write(mensaje);
            if (double.TryParse(Console.ReadLine(), out valor) && valor > 0) return valor;
            Console.WriteLine("Ingrese un numero mayor que 0.");
        } while (true);
    }

    private static double LeerDoubleOpcional(string mensaje) {
        Console.Write(mensaje);
        return double.TryParse(Console.ReadLine(), out double valor) ? valor : 0;
    }
}
