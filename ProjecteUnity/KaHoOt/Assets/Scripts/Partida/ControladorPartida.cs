using System.Collections;
using UnityEngine;

// Coordina las respuestas, la cuenta atrás y el paso entre preguntas.
public class ControladorPartida : MonoBehaviour
{
    [SerializeField] private TemporizadorPregunta temporizador;

    private VistaPartida vista;
    private BusEventosPartida bus; // Referencia al bus de eventos de la partida.
    private ProgresoPartida progreso;
    private EvaluadorRespuestas evaluador; // Referencia al evaluador de respuestas.
    private float segundosPuntos;
    private Coroutine esperaPuntos;

    public void Iniciar(
        ProgresoPartida progreso,
        BusEventosPartida bus,
        VistaPartida vista,
        float segundosPuntos)
    {
        // Una sola suscripción y una sola cuenta atrás al iniciar.
        if (temporizador == null)
        {
            Debug.LogError("Asigna el temporizador en ControladorPartida.");
        }
        else if (this.bus == null)
        {
            this.progreso = progreso;
            this.bus = bus;
            this.vista = vista;
            this.segundosPuntos = segundosPuntos;

            this.bus.Subscribe<RespuestaEvaluada>(AlEvaluarRespuesta);
            MostrarPreguntaActual();
        }
    }

    private void ActualizarTiempo(float transcurrido)
    {
        evaluador.AvanzarTiempo(transcurrido);
        vista.MostrarTiempo(evaluador.TiempoRestante);
    }

    private void OnDisable()
    {
        if (temporizador != null)
        {
            temporizador.Detener();
        }

        if (esperaPuntos != null)
        {
            StopCoroutine(esperaPuntos);
            esperaPuntos = null;
        }
    }

    public void Responder(int indice) // Evalúa la opción elegida por el jugador.
    {
        if (evaluador != null && !evaluador.Finalizada)
        {
            // Comprobamos el tiempo exacto al pulsar, entre dos actualizaciones.
            temporizador.Medir();
            evaluador.Evaluar(indice);

            if (evaluador.Finalizada)
            {
                temporizador.Detener();
            }
        }
    }

    private void OnDestroy()
    {
        // Retiramos la suscripción al salir de la escena.
        if (bus != null)
        {
            bus.Unsubscribe<RespuestaEvaluada>(AlEvaluarRespuesta);
        }
    }
    private void MostrarPreguntaActual() // Muestra la pregunta actual en la vista y prepara el evaluador.
    {
        PreguntaDatos pregunta = progreso.PreguntaActual;

        // Cada pregunta comienza con su propia duración.
        evaluador = new EvaluadorRespuestas(pregunta, bus);
        vista.MostrarPregunta(
            pregunta,
            progreso.NumeroPregunta,
            progreso.TotalPreguntas
        );

        temporizador.Iniciar(() => evaluador.TiempoRestante, ActualizarTiempo);
    }

    private void AlEvaluarRespuesta(RespuestaEvaluada evaluacion) // Acumula el resultado y muestra la puntuación antes de avanzar.
    {
        progreso.RegistrarRespuesta(evaluacion.PuntosObtenidos,evaluacion.SegundosEmpleados);

        vista.MostrarEvaluacion(evaluacion);
        vista.MostrarPuntos(evaluacion.PuntosObtenidos, progreso.PuntosTotales);

        esperaPuntos = StartCoroutine(EsperarYAvanzar());
    }

    private IEnumerator EsperarYAvanzar()
    {
        yield return new WaitForSecondsRealtime(segundosPuntos);
        esperaPuntos = null;

        if (progreso.Avanzar())
        {
            MostrarPreguntaActual();
        }
        else
        {
            vista.MostrarFinPartida(progreso.SegundosTotales);
            bus.Publish(new PartidaTerminada(progreso.PuntosTotales, progreso.SegundosTotales));
        }
    }
}
