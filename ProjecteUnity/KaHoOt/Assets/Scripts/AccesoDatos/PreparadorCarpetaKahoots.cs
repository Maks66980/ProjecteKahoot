using System.IO;

// Copia los cuestionarios iniciales que faltan en la carpeta del usuario.
public class PreparadorCarpetaKahoots
{
    public void Preparar(string carpetaOrigen,string carpetaDestino)
    {
        Directory.CreateDirectory(carpetaDestino); // Crea la carpeta de destino si no existe.

        string[] archivos = Directory.GetFiles(carpetaOrigen, "*.json"); // Obtiene todos los archivos JSON de la carpeta de origen.

        foreach (string archivoOrigen in archivos)
        {
            string nombreArchivo = Path.GetFileName(archivoOrigen); // Obtiene el nombre del archivo.
            string archivoDestino = Path.Combine(carpetaDestino, nombreArchivo); // Construye la ruta de destino.
            // Copia únicamente los cuestionarios que todavía no existen.
            if (!File.Exists(archivoDestino)) // Verifica si el archivo ya existe en la carpeta de destino.
            {
                File.Copy(archivoOrigen, archivoDestino); // Copia el archivo de origen al destino.
            }
        }
            
    }
}
