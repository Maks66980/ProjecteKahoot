using System.IO;
using UnityEngine;

public class ConfiguradorCreadorKahoot : MonoBehaviour//Clase que prepara las dependencias del creador de cuestionarios.
{
    [SerializeField] private VistaCreadorKahoot vista;//Referencia a la vista del creador.
    [SerializeField] private ControladorCreadorKahoot controlador;//Referencia al controlador de guardado.

    private void Start()//Prepara el guardado al entrar en la escena.
    {
        if (vista != null && controlador != null)
        {
            string carpetaKahoots = Path.Combine(
                Application.persistentDataPath,
                "Kahoots"
            );//Utiliza la misma carpeta que el selector de cuestionarios.

            string carpetaInformes = Path.Combine(
                Application.persistentDataPath,
                "Informes"
            );//Obtiene la carpeta de informes de errores.

            IEscritorKahoots escritor = new EscritorKahootsJson(
                carpetaKahoots,
                new ValidadorKahoots()
            );

            IRegistradorErrores registradorErrores = new GestorErrores(
                new RegistradorErroresTexto(carpetaInformes)
            );

            controlador.Iniciar(vista, escritor, registradorErrores);
        }
        else
        {
            Debug.LogError("Asigna la vista y el controlador en ConfiguradorCreadorKahoot.", this);
        }
    }
}
