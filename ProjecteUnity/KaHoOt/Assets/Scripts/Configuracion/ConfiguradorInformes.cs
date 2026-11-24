using System.IO;
using UnityEngine;

public class ConfiguradorInformes : MonoBehaviour//Clase que prepara las dependencias de la escena de informes.
{
    [SerializeField] private VistaInformes vista;//Referencia a la vista de informes.
    [SerializeField] private VistaContenidoInforme visor;//Referencia al visor del contenido de los informes.
    [SerializeField] private ControladorInformes controlador;//Referencia al controlador de informes.

    private void Start()//Prepara los repositorios al entrar en la escena.
    {
        if (vista != null && controlador != null &&
            visor != null)//Verifica las referencias del Inspector.
        {
            string carpeta = Path.Combine(
                Application.persistentDataPath,
                "Informes"
            );//Obtiene la carpeta donde se guardan los informes.

            IRepositorioInformes repositorio =
                new RepositorioInformesTexto(carpeta);//Prepara la consulta de los archivos de texto.

            IRegistradorErrores registradorErrores =
                new GestorErrores(
                    new RegistradorErroresTexto(carpeta)
                );//Prepara el registro de errores.

            controlador.Iniciar(
                repositorio,
                vista,
                visor,
                registradorErrores
            );//Entrega las dependencias e inicia la carga.
        }
        else
        {
            Debug.LogError("Asigna la vista, el visor y el controlador en ConfiguradorInformes.", this);
        }
    }
}