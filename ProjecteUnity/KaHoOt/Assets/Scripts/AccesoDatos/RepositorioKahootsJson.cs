using System.IO;

// Busca los archivos y coordina su lectura y validación.
public class RepositorioKahootsJson : IRepositorioKahoots
{
    private readonly string carpeta;
    private readonly LectorKahoots lector;
    private readonly ValidadorKahoots validador;

    public RepositorioKahootsJson(
        string carpeta,
        LectorKahoots lector,
        ValidadorKahoots validador)
    {
        // Las dependencias se reciben al construir el repositorio.
        this.carpeta = carpeta;
        this.lector = lector;
        this.validador = validador;
    }

    public string[] ObtenerRutas() // Obtiene las rutas de los archivos JSON en la carpeta especificada.
    {
        return Directory.GetFiles(carpeta, "*.json"); // Devuelve un arreglo de rutas de archivos JSON en la carpeta.
    }

    public KahootDatos Cargar(string ruta) // Carga y valida un cuestionario Kahoot desde un archivo JSON.
    {
        KahootDatos datos = lector.LeerArchivo(ruta);

        // Solo entrega cuestionarios que superan la validación.
        validador.Validar(datos);

        return datos;
    }
}