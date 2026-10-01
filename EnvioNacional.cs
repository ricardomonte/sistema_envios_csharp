namespace SistemaCourier;

public class EnvioNacional : EnvioBase
{
    public string RegionDestino { get; set; }

    public EnvioNacional(string idSeguimiento, string tipoPaquete, string descripcionContenido, double pesoKilos, double costoBase, string regionDestino) 
        : base(idSeguimiento, tipoPaquete, descripcionContenido, pesoKilos, costoBase) {

        RegionDestino = regionDestino;
    }

    public override void MostrarFichaEnvio() {
        Console.WriteLine($"---------{this.GetType().Name}---------");
        base.MostrarFichaEnvio();
        Console.WriteLine($"Region de destino: {RegionDestino}");
    }
}