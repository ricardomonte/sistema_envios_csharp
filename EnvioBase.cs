namespace SistemaCourier;
using System.Text.Json.Serialization;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$tipoEnvio")]
[JsonDerivedType(typeof(EnvioNacional), "nacional")]
[JsonDerivedType(typeof(EnvioInternacional), "internacional")]
public abstract class EnvioBase
{
    public string IdSeguimiento { get; set; }
    public string TipoPaquete { get; set; }
    public string DescripcionContenido { get; set; }
    public double PesoKilos { get; set; }
    public double CostoBase { get; set; }
    
    public EnvioBase(string idSeguimiento, string tipoPaquete, string descripcionContenido, double pesoKilos, double costoBase) {
        IdSeguimiento = idSeguimiento;
        TipoPaquete = tipoPaquete;
        DescripcionContenido = descripcionContenido;
        PesoKilos = pesoKilos;
        CostoBase = costoBase;
    }

    public virtual void MostrarFichaEnvio() {
        Console.WriteLine($"Numero de seguimiento: {IdSeguimiento}");
        Console.WriteLine($"Tipo de paquete: {TipoPaquete}");
        Console.WriteLine($"Descripcion: {DescripcionContenido}");
        Console.WriteLine($"Peso: {PesoKilos} Kg");
        Console.WriteLine($"Costo base: ${CostoBase}");
    }
}