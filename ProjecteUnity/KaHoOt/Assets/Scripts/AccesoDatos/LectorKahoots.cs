using System.IO;
using System.Text;
using UnityEngine;

// Lee un cuestionario JSON y lo convierte en datos de la partida.
public class LectorKahoots
{
    public KahootDatos LeerArchivo(string ruta)
    {
        // UTF-8 conserva los acentos de los enunciados y las respuestas.
        string contenido = File.ReadAllText(ruta, Encoding.UTF8);

        KahootDatos datos = JsonUtility.FromJson<KahootDatos>(contenido);

        return datos;
    }
}
