namespace SistemaCourier;
using System.Threading.Tasks; 

public interface IAduana {
    string CodigoArancelario { get; set; }

    Task DeclararImpuestosAsync();
}