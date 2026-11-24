using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Collections.Generic;

// Guarda todas las partidas de cada cuestionario en su propio XML.
public class RepositorioResultadosXml : IRepositorioResultados
{
    private readonly string carpeta;

    public RepositorioResultadosXml(string carpeta)
    {
        this.carpeta = carpeta;
    }

    public void Guardar(string idCuestionario, string jugador, int puntos, float segundos)
    {
        Directory.CreateDirectory(carpeta);

        string ruta = ObtenerRuta(idCuestionario);
        XmlDocument documento = new XmlDocument();

        if (File.Exists(ruta))
        {
            // Cargamos el historial antes de añadir una nueva partida.
            documento.Load(ruta);
        }
        else
        {
            XmlElement nuevaRaiz = documento.CreateElement("resultados");
            nuevaRaiz.SetAttribute("cuestionario", idCuestionario);
            documento.AppendChild(nuevaRaiz);
        }

        XmlElement raiz = documento.DocumentElement;

        if (raiz == null ||
            raiz.Name != "resultados" ||
            raiz.NamespaceURI.Length != 0 ||
            raiz.GetAttributeNode("cuestionario")?.Value != idCuestionario)
        {
            throw new InvalidDataException("El XML no corresponde al cuestionario.");
        }

        // Cada partida añade un nodo con el nombre, los puntos y el tiempo total.
        XmlElement partida = documento.CreateElement("partida");
        raiz.AppendChild(partida);

        XmlElement nombreJugador = documento.CreateElement("jugador");
        nombreJugador.InnerText = jugador;
        partida.AppendChild(nombreJugador);

        XmlElement puntuacion = documento.CreateElement("puntos");
        puntuacion.InnerText = XmlConvert.ToString(puntos);
        partida.AppendChild(puntuacion);

        XmlElement tiempo = documento.CreateElement("segundos");
        tiempo.InnerText = XmlConvert.ToString(segundos);
        partida.AppendChild(tiempo);

        string temporal = ruta + "." + Guid.NewGuid().ToString("N") + ".tmp";

        try
        {
            // Escribimos primero la copia completa para proteger el historial.
            documento.Save(temporal);

            if (File.Exists(ruta))
            {
                File.Replace(temporal, ruta, null);
            }
            else
            {
                File.Move(temporal, ruta);
            }
        }
        finally
        {
            if (File.Exists(temporal))
            {
                File.Delete(temporal);
            }
        }
    }



    // Lee las partidas guardadas del cuestionario y devuelve sus resultados.
    public List<ResultadoPartida> Cargar(string idCuestionario)
    {
        List<ResultadoPartida> resultados = new List<ResultadoPartida>();
        string ruta = ObtenerRuta(idCuestionario);

        // Si todavía no hay archivo, entregamos una lista vacía.
        if (File.Exists(ruta))
        {
            XmlDocument documento = new XmlDocument();
            documento.Load(ruta);

            XmlElement raiz = documento.DocumentElement;

            // Comprobamos que el archivo pertenece al cuestionario solicitado.
            if (raiz == null ||
                raiz.Name != "resultados" ||
                raiz.NamespaceURI.Length != 0 ||
                raiz.GetAttributeNode("cuestionario")?.Value != idCuestionario)
            {
                throw new InvalidDataException(
                    "El XML no corresponde al cuestionario."
                );
            }

            // Recorremos las partidas,
            foreach (XmlNode partida in raiz.ChildNodes)
            {
                if (partida.Name == "partida")
                {
                    XmlNode jugador = partida["jugador"];
                    XmlNode puntos = partida["puntos"];
                    XmlNode segundos = partida["segundos"];

                    if (jugador == null || puntos == null || segundos == null)
                    {
                        throw new InvalidDataException(
                            "Una partida del XML tiene datos incompletos."
                        );
                    }
                    else
                    {
                        // Convertimos los textos del XML a sus tipos correspondientes.
                        ResultadoPartida resultado = new ResultadoPartida(
                            jugador.InnerText,
                            XmlConvert.ToInt32(puntos.InnerText),
                            XmlConvert.ToSingle(segundos.InnerText)
                        );

                        resultados.Add(resultado);
                    }
                }
            }
        }

        return resultados;
    }

    private string ObtenerRuta(string idCuestionario)
    {
        string clave;

        using (SHA256 hash = SHA256.Create())
        {
            byte[] datos = Encoding.UTF8.GetBytes(idCuestionario);
            clave = BitConverter.ToString(hash.ComputeHash(datos)).Replace("-", "");
        }

        return Path.Combine(carpeta, clave + ".xml");
    }
}
