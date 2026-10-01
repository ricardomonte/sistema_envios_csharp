namespace SistemaCourier;

using System.IO;
using System.Linq;
using System.Threading.Tasks; 
using System.Text.Json;   

public class EnvioCollection {
    private static List<EnvioBase> _listaEnvios = new List<EnvioBase>();
    private static readonly JsonSerializerOptions _opciones = new JsonSerializerOptions { WriteIndented = true };

    public static async Task<bool> CreateAsync(EnvioBase envio) {
        Console.WriteLine("\n[Creando envio...]");
        await Task.Delay(1200);
        bool envioExiste = _listaEnvios.Any(
            e => e.IdSeguimiento.Equals(
                envio.IdSeguimiento, StringComparison.OrdinalIgnoreCase
            )
        );
        if (envioExiste) {
            Console.WriteLine($"El numero de envio {envio.IdSeguimiento} ya esta registrado");
            return false;
        }

        _listaEnvios.Add(envio);
        await SaveToFileAsync();
        Console.WriteLine("Se ha guardado con exito");

        return true;
    }

    public static async Task<EnvioBase?> ReadByIdAsync(string envioId) {
        Console.WriteLine("\n[Buscando envio...]");
        await Task.Delay(800);
        EnvioBase? envio = _listaEnvios.FirstOrDefault(
            e => e.IdSeguimiento.Equals(
                envioId, StringComparison.OrdinalIgnoreCase
            )
        );
        return envio;
    }

    public static async Task<bool> UpdateAsync(string envioId, string tipoPaquete, string descripcionContenido, double pesoKilos, double costoBase) {
        Console.WriteLine("\n[Actualizando envio...]");
        await Task.Delay(1200);

        EnvioBase? envio = _listaEnvios.FirstOrDefault(
            e => e.IdSeguimiento.Equals(
                envioId, StringComparison.OrdinalIgnoreCase
            )
        );

        if (envio == null) {
            Console.WriteLine($"No existe envio con numero {envioId}");
            return false;
        };

        if (!string.IsNullOrWhiteSpace(tipoPaquete)) envio.TipoPaquete = tipoPaquete;
        if (!string.IsNullOrWhiteSpace(descripcionContenido)) envio.DescripcionContenido = descripcionContenido;
        if (pesoKilos > 0) envio.PesoKilos = pesoKilos;
        if (costoBase > 0) envio.CostoBase = costoBase;

        await SaveToFileAsync();
        Console.WriteLine($"Se ha actualizado con exito el envio con numero de seguimiento: {envio.IdSeguimiento}");

        return true;
    }

    public static async Task<bool> DeleteAsync(string envioId) {
        Console.WriteLine("\n[Eliminando envio...]");
        await Task.Delay(800);
        EnvioBase? envio = _listaEnvios.FirstOrDefault(
            e => e.IdSeguimiento.Equals(
                envioId, StringComparison.OrdinalIgnoreCase
            )
        );

        if (envio == null) {
            Console.WriteLine($"No existe envio con numero {envioId}");
            return false;
        };
        _listaEnvios.Remove(envio);
        await SaveToFileAsync();
        Console.WriteLine($"Se ha eliminado con exito el envio con numero de seguimiento: {envio.IdSeguimiento}");
        return true;
    }

    public static async Task<List<EnvioBase>> SearchByFilterAsync(string filtro) {
        Console.WriteLine("\n[Filtrando envios...]");
        await Task.Delay(800);
 
        if (string.IsNullOrWhiteSpace(filtro))
            return new List<EnvioBase>();
 
        string termino = filtro.ToLower();
 
        List<EnvioBase> resultado = _listaEnvios.Where(e =>
        {
            if (e.TipoPaquete.ToLower().Contains(termino)) return true;
            if (e is EnvioNacional en && en.RegionDestino.ToLower().Contains(termino)) return true;
            if (e is EnvioInternacional ei && ei.PaisDestino.ToLower().Contains(termino)) return true;
            return false;
        }).ToList();
 
        return resultado;
    }

    public static async Task ListAllAsync() {
        Console.WriteLine("\n[Cargando listado completo...]");
        await Task.Delay(1000);

        if (_listaEnvios.Count == 0)
        {
            Console.WriteLine("No hay envios registrados en el sistema.");
            return;
        };

        Console.WriteLine($"\n===== LISTADO DE ENVIOS ({_listaEnvios.Count}) =====\n");


        foreach (EnvioBase envio in _listaEnvios)
        {
            envio.MostrarFichaEnvio();
        };
    }

    public static async Task SaveToFileAsync() {
        try
        {
            string jsonString = JsonSerializer.Serialize(_listaEnvios, _opciones);
            await File.WriteAllTextAsync("registro_envios.json", jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"No se pudo guardar el archivo: {ex.Message}");
        }
    }

    public static async Task LoadFromFileAsync() {
        try
        {
            if (!File.Exists("registro_envios.json")) return;
 
            string json = await File.ReadAllTextAsync("registro_envios.json");
            List<EnvioBase>? datos = JsonSerializer.Deserialize<List<EnvioBase>>(json, _opciones);
 
            if (datos != null) _listaEnvios = datos;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"No se pudo cargar el archivo: {ex.Message}");
        }
    }
}