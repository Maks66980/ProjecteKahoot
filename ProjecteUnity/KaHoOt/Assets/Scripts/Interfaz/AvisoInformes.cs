using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvisoInformes : MonoBehaviour//Clase que muestra un aviso y permite abrir la carpeta de informes.
{
    [SerializeField] private TMP_Text textoAviso;//Referencia al texto que explica el error.
    [SerializeField] private Button botonAbrir;//Referencia al botón que abre la carpeta de informes.

    private void Awake()//Conecta el botón con la apertura de la carpeta.
    {
        if (botonAbrir != null)
        {
            botonAbrir.onClick.AddListener(AbrirCarpeta);
        }
    }

    public void Mostrar(string mensaje)//Método que muestra el aviso recibido junto al botón de informes.
    {
        if (textoAviso != null)
        {
            textoAviso.text = mensaje;
        }

        gameObject.SetActive(true);
    }

    public void Ocultar()//Método que oculta el aviso cuando se vuelve a cargar la lista.
    {
        gameObject.SetActive(false);
    }

    private void AbrirCarpeta()//Método que abre la carpeta de informes en el explorador de archivos.
    {
        string carpeta = Path.Combine(Application.persistentDataPath, "Informes");

        try
        {
            if (Directory.Exists(carpeta))//Verifica que exista la carpeta antes de abrirla.
            {
                Uri direccion = new Uri(Path.GetFullPath(carpeta) + Path.DirectorySeparatorChar);
                Application.OpenURL(direccion.AbsoluteUri);
            }
            else
            {
                Mostrar("Encara no hi ha informes guardats.");
            }
        }
        catch (Exception error)
        {
            Mostrar("No s'ha pogut obrir la carpeta d'informes.");
            Debug.LogError("No se pudo abrir la carpeta de informes: " + error.Message, this);
        }
    }

    private void OnDestroy()//Elimina la suscripción cuando se destruye el aviso.
    {
        if (botonAbrir != null)
        {
            botonAbrir.onClick.RemoveListener(AbrirCarpeta);
        }
    }
}
