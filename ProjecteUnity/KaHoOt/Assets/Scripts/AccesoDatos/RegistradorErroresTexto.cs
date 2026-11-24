using System;
using System.IO;
using System.Text;

public class RegistradorErroresTexto : IRegistradorErrores//Clase que guarda los informes de errores en archivos de texto.
{
    private readonly string carpeta;//Contiene la ruta de la carpeta de informes.

    public RegistradorErroresTexto(string carpeta)//Recibe la carpeta donde se guardan los informes.
    {
        this.carpeta = carpeta;
    }

    public void Registrar(string operacion, Exception error)//Método que crea un informe con los datos del error.
    {
        if (error == null)//Verifica que se haya recibido una excepción.
        {
            throw new ArgumentNullException(nameof(error));
        }

        Directory.CreateDirectory(carpeta);//Crea la carpeta de informes si todavía no existe.

        DateTime fecha = DateTime.Now;//Obtiene la fecha y la hora del registro.

        string nombreArchivo =
            "Informe_" +
            fecha.ToString("yyyy-MM-dd_HH-mm-ss-fff") +
            "_" + Guid.NewGuid().ToString("N") +
            ".txt";//Genera un nombre con fecha, hora y una clave para evitar coincidencias.

        string ruta = Path.Combine(
            carpeta,
            nombreArchivo
        );//Construye la ruta completa del informe.

        StringBuilder contenido = new StringBuilder();//Prepara el contenido del archivo.

        contenido.AppendLine("INFORME DE ERROR");
        contenido.AppendLine("Fecha y hora: " + fecha.ToString("dd/MM/yyyy HH:mm:ss.fff"));
        contenido.AppendLine("Operación: " + operacion);
        contenido.AppendLine("Tipo de error: " + error.GetType().Name);
        contenido.AppendLine("Mensaje: " + error.Message);
        contenido.AppendLine();
        contenido.AppendLine("Detalle de la excepción:");
        contenido.AppendLine(error.ToString());//Añade los detalles y la ubicación del error cuando están disponibles.

        File.WriteAllText(
            ruta,
            contenido.ToString(),
            new UTF8Encoding(false)
        );//Guarda el informe utilizando UTF-8.
    }
}