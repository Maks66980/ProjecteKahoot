using System;
using System.IO;
using UnityEngine;

// Prepara los cuestionarios y conecta el selector con la sesión y la navegación.
public class ConfiguradorSelector : MonoBehaviour
{
    [SerializeField] private SelectorKahoots selector; // Referencia al selector de Kahoots.
    [SerializeField] private SesionPartida sesionPartida; // Referencia a la sesión de partida.
    [SerializeField] private NavegacionEscenas navegacion; // Referencia a la navegación de escenas.

    private void Start()
    {
        // Todas las referencias deben estar asignadas antes de preparar el selector.
        if (selector != null && sesionPartida != null && navegacion != null)
        {
            string carpetaOrigen = Path.Combine(
                Application.streamingAssetsPath,
                "Kahoots"
            ); // Construye la ruta a la carpeta de Kahoots en StreamingAssets.

            string carpetaUsuario = Path.Combine(
                Application.persistentDataPath,
                "Kahoots"
            ); // Construye la ruta a la carpeta de cuestionarios del usuario.

            string carpetaInformes = Path.Combine(Application.persistentDataPath,"Informes");//Obtienemos la carpeta donde se guardan los informes.
            
            IRegistradorErrores registradorErrores = new GestorErrores(new RegistradorErroresTexto(carpetaInformes));//Preparamos el gestor de errores y su registrador de archivos.

            PreparadorCarpetaKahoots preparador = new PreparadorCarpetaKahoots();

            try
            {
                preparador.Preparar(carpetaOrigen, carpetaUsuario);
            } // Copia los cuestionarios iniciales a la carpeta del usuario.
            catch (Exception error)
            {
                registradorErrores.Registrar(
                    "Acceder a la carpeta de cuestionarios",
                    error
                );//Registra el fallo al consultar los archivos disponibles.
            }

            // Permite localizar la carpeta del usuario durante el desarrollo.
            Debug.Log("Carpeta de cuestionarios: " + carpetaUsuario);

            LectorKahoots lector = new LectorKahoots(); // Prepara el lector de cuestionarios.
            ValidadorKahoots validador = new ValidadorKahoots(); // Prepara la validación de cuestionarios.

            IRepositorioKahoots repositorio =
                new RepositorioKahootsJson(
                    carpetaUsuario,
                    lector,
                    validador
                ); // Conecta el repositorio con el lector y el validador.

            // El configurador atiende las solicitudes de inicio de partida.
            selector.PartidaSolicitada += IniciarPartida;

            // El selector recibe la dependencia mediante su interfaz.
            // Intentamos leer los archivos existentes aunque haya fallado la preparación.
            selector.Inicializar(repositorio, registradorErrores);//Entrega al selector el repositorio y el gestor de errores.
        }
        else
        {
            Debug.LogError(
                 "Asigna el selector, la sesión y la navegación en ConfiguradorSelector."
            );
        }
    }

    private void IniciarPartida(KahootDatos cuestionario, string nombreJugador) // Prepara la sesión y abre la escena de partida.
    {
        // Guardamos los datos antes de cambiar de escena.
        sesionPartida.Preparar(cuestionario, nombreJugador);

        navegacion.CambiarEscena("Partida");
    }

    private void OnDestroy()
    {
        // Dejamos de escuchar cuando se destruye el configurador.
        if (selector != null)
        {
            selector.PartidaSolicitada -= IniciarPartida;
        }
    }
}
