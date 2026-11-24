using System;
using System.IO;
using System.Text;
using UnityEngine;

public class EscritorKahootsJson : IEscritorKahoots//Clase que guarda cuestionarios en archivos JSON.
{
    private readonly string carpeta;//Contiene la carpeta de destino.
    private readonly ValidadorKahoots validador;//Referencia al validador de cuestionarios.

    public EscritorKahootsJson(
        string carpeta,
        ValidadorKahoots validador)//Recibe la carpeta y el validador.
    {
        this.carpeta = carpeta;
        this.validador = validador;
    }

    public string Guardar(KahootDatos datos)//Método que valida y escribe el cuestionario.
    {
        validador.Validar(datos);//Comprueba los datos antes de crear el archivo.

        Directory.CreateDirectory(carpeta);//Crea la carpeta si todavía no existe.

        string nombreArchivo =
            "Kahoot_" + Guid.NewGuid().ToString("N") + ".json";//Genera un nombre independiente del título.

        string ruta = Path.Combine(carpeta, nombreArchivo);//Construye la ruta del archivo.

        string contenido = JsonUtility.ToJson(
            datos,
            true
        );//Convierte los datos a JSON con formato legible.

        using (FileStream archivo = new FileStream(
            ruta,
            FileMode.CreateNew,
            FileAccess.Write))//Crea un archivo nuevo sin sobrescribir otro cuestionario.
        {
            using (StreamWriter escritor = new StreamWriter(
                archivo,
                new UTF8Encoding(false)))//Prepara la escritura en UTF-8 sin firma.
            {
                escritor.Write(contenido);//Escribe el cuestionario completo.
            }
        }

        return ruta;
    }
}
