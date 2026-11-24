using System;
using System.IO;
using System.Text;

public class RepositorioInformesTexto : IRepositorioInformes//Clase que consulta los informes guardados en archivos de texto.
{
    private readonly string carpeta;//Contiene la ruta de la carpeta de informes.

    public RepositorioInformesTexto(string carpeta)//Recibe la carpeta donde se buscan los informes.
    {
        this.carpeta = carpeta;
    }

    public string[] ObtenerRutas()//Método que obtiene los informes disponibles y los ordena por su nombre.
    {
        string[] rutas = Array.Empty<string>();//Prepara un array vacío por si todavía no existe la carpeta.

        if (Directory.Exists(carpeta))//Verifica que exista la carpeta de informes.
        {
            rutas = Directory.GetFiles(
                carpeta,
                "*.txt"
            );//Obtiene las rutas de los archivos de texto.

            Array.Sort(rutas, StringComparer.Ordinal);//Ordena las rutas por el nombre de los informes.
            Array.Reverse(rutas);//Coloca primero las fechas más recientes de los nombres generados.
        }

        return rutas;
    }

    public string Leer(string ruta)//Método que lee todo el contenido del informe seleccionado.
    {
        string contenido = File.ReadAllText(
            ruta,
            Encoding.UTF8
        );//Lee el archivo utilizando la misma codificación del registrador.

        return contenido;
    }
}