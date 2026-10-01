namespace SistemaCourier;
using System.Threading.Tasks; 

public class EnvioInternacional : EnvioBase, IAduana
{
    public string PaisDestino { get; set; }
    public string CodigoArancelario {get; set; }

    public EnvioInternacional(string idSeguimiento, string tipoPaquete, string descripcionContenido, double pesoKilos, double costoBase, string paisDestino, string codigoArancelario = "PENDIENTE") 
        : base(idSeguimiento, tipoPaquete, descripcionContenido, pesoKilos, costoBase) {

        PaisDestino = paisDestino;
        CodigoArancelario = codigoArancelario;
    }

    public async Task DeclararImpuestosAsync() {

        await Task.Delay(2000);

        if ( CodigoArancelario == "PENDIENTE") {
            throw new Exception("Error: No se pueden calcular tasas. Codigo arancelario no asignado.");
        }
        CostoBase = CostoBase + 15000;
        Console.WriteLine($"Exito Impuesto aduanero, aplicado. Costo de Envio: {CostoBase}");
    }

    public override void MostrarFichaEnvio() {
        Console.WriteLine($"---------{this.GetType().Name}---------");
        base.MostrarFichaEnvio();
        Console.WriteLine($"Pais de destino: {PaisDestino}");
    }
}