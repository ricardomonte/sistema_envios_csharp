namespace SistemaCourier;

using System.Threading.Tasks;


class Program {
    static async Task Main(string[] args) {
        await EnvioCollection.LoadFromFileAsync();
 
        string opcion;
        do {
            Console.WriteLine("\n==================================================");
            Console.WriteLine("SISTEMA COURIER - CONTROL DE ENVIOS");
            Console.WriteLine("==================================================");
            Console.WriteLine(" 1. Registrar Envio Nacional (Destino Local) o internacional");
            Console.WriteLine(" 2. Buscar Envio por Codigo de Seguimiento");
            Console.WriteLine(" 3. Actualizar Datos de Peso/Descripcion");
            Console.WriteLine(" 4. Eliminar (Dar de Baja) Orden de Envio");
            Console.WriteLine(" 5. Filtrar Envios por Tipo de Paquete o Destino");
            Console.WriteLine(" 6. Listar Todos los Envios en Transito");
            Console.WriteLine(" 0. Salir de la Aplicacion");
            Console.Write("Seleccione una opcion: ");
            opcion = Console.ReadLine() ?? "";
 
            try {
                switch (opcion) {
                    case "1": await Dialogos.CargarDialogoCreate(); break;
                    case "2": await Dialogos.CargarDialogoRead(); break;
                    case "3": await Dialogos.CargarDialogoUpdate(); break;
                    case "4": await Dialogos.CargarDialogoDelete(); break;
                    case "5": await Dialogos.CargarDialogoSearchByFilter(); break;
                    case "6": await Dialogos.ListAll(); break;
                    case "0":
                        Console.WriteLine("\nSaliendo del sistema...");
                        Environment.Exit(0);
                        break;
                    default: Console.WriteLine("[ERROR] Opcion no valida."); break;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"\n[ERROR] Ocurrio un problema al procesar la operacion: {ex.Message}");
            }
 
            if (opcion != "0") {
                Console.WriteLine("\nPresione una tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
 
        } while (opcion != "0");
    }
}
