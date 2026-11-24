using System.IO;
using UnityEngine;

// Lee la sesión, prepara las dependencias e inicia el controlador de la partida.
public class ConfiguradorPartida : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private SesionPartida sesionPartida; // Referencia a la sesión de la partida.
    [SerializeField] private VistaPartida vista; // Referencia a la vista de la partida.
    [SerializeField] private ControladorPartida controlador; // Referencia al controlador de la partida.

    [Header("Configuración de puntos")]
    [SerializeField, Range(5f, 10f)] private float segundosPuntos = 7f;

    private BusEventosPartida bus;
    private GuardadorResultadosPartida guardadorResultados;

    private void Start()
    {
        if (sesionPartida != null && vista != null && controlador != null) // Verifica las referencias de la escena.
        {
            KahootDatos cuestionario = sesionPartida.Cuestionario; // Obtiene el cuestionario de la sesión.

            if (cuestionario != null &&
                cuestionario.preguntas != null &&
                cuestionario.preguntas.Length > 0) // Verifica que el cuestionario tenga preguntas.
            {
                ProgresoPartida progreso = new ProgresoPartida(cuestionario);
                bus = new BusEventosPartida();

                // Prepara el guardado de resultados para el jugador y el cuestionario actuales.
                string carpetaResultados = Path.Combine(Application.persistentDataPath, "Resultats");
                IRepositorioResultados repositorio = new RepositorioResultadosXml(carpetaResultados);
                string carpetaInformes = Path.Combine(
                    Application.persistentDataPath,
                    "Informes"
                );//Obtiene la carpeta donde se guardan los informes.

                IRegistradorErrores registradorErrores = new GestorErrores(
                    new RegistradorErroresTexto(carpetaInformes)
                );//Prepara el registro de errores del guardado.

                guardadorResultados = new GuardadorResultadosPartida(
                    repositorio,
                    cuestionario.id,
                    sesionPartida.NombreJugador,
                    registradorErrores
                );//Entrega al guardador sus dependencias.

                // El guardador escucha el fin de la partida desde el inicio.
                bus.Subscribe<PartidaTerminada>(guardadorResultados.Guardar);
                Debug.Log("Carpeta de resultados: " + carpetaResultados);

                controlador.Iniciar(progreso, bus, vista, segundosPuntos);

                Debug.Log(
                    "Jugador de la partida: " +
                    sesionPartida.NombreJugador
                );
            }
            else
            {
                Debug.LogError(
                    "No hay cuestionario preparado. Empieza desde el selector."
                );
            }
        }
        else
        {
            Debug.LogError(
                "Asigna la sesión, la vista y el controlador en ConfiguradorPartida."
            );
        }
    }

    private void OnDestroy()
    {
        if (bus != null && guardadorResultados != null)
        {
            bus.Unsubscribe<PartidaTerminada>(guardadorResultados.Guardar);
        }
    }
}
