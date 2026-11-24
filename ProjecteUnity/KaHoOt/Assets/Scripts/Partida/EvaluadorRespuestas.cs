using UnityEngine;
using System;

// Evalúa la respuesta o el tiempo agotado y calcula los puntos de la pregunta.
public class EvaluadorRespuestas
{
    private readonly PreguntaDatos pregunta; // Pregunta que se está evaluando.
    private readonly BusEventosPartida bus; // Bus que comunica el resultado de la pregunta.

    public float TiempoRestante { get; private set; } // Tiempo restante para responder la pregunta, en segundos.
    public bool Finalizada { get; private set; } // Indica si la pregunta ha terminado.

    public EvaluadorRespuestas(
        PreguntaDatos pregunta,
        BusEventosPartida bus)
    {
        this.pregunta = pregunta;
        this.bus = bus;
        TiempoRestante = pregunta.duracionSegundos;
    } // La pregunta comienza con su duración completa.

    public void Evaluar(int indiceElegido) // Comprueba la opción elegida y cierra la pregunta.
    {
        if (!Finalizada &&
            indiceElegido >= 0 &&
            indiceElegido < pregunta.respuestas.Length)
        {
            bool esCorrecta = indiceElegido == pregunta.respuestaCorrecta;
            CerrarPregunta(esCorrecta, false);
        } // Ignora las respuestas fuera de rango o posteriores al cierre.
    }

    public void AvanzarTiempo(float segundos) // Descuenta el tiempo transcurrido y cierra la pregunta si se agota.
    {
        if (!Finalizada && segundos > 0f)
        {
            TiempoRestante = Math.Max(0f, TiempoRestante - segundos);

            if (TiempoRestante <= 0f)
            {
                CerrarPregunta(false, true);
            }
        }
    }

    private void CerrarPregunta(bool esCorrecta, bool tiempoAgotado)
    {
        // Marcamos el cierre antes de avisar a los suscriptores.
        Finalizada = true;

        string respuestaCorrecta =
            pregunta.respuestas[pregunta.respuestaCorrecta];

        int puntos = 0;

        if (esCorrecta)
        {
            // Un acierto vale 500 puntos más un extra por el tiempo restante.
            float proporcion = TiempoRestante / pregunta.duracionSegundos;
            puntos = 500 + (int)Math.Round(500 * proporcion);
        }

        float segundosEmpleados = pregunta.duracionSegundos - TiempoRestante;

        bus.Publish(new RespuestaEvaluada(
            esCorrecta,
            respuestaCorrecta,
            tiempoAgotado,
            puntos,
            segundosEmpleados
        ));
    }
}