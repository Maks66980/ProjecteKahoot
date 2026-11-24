public interface IRepositorioKahoots // Define un contrato para un repositorio de Kahoots.
{
    string[] ObtenerRutas();

    KahootDatos Cargar(string ruta);
}