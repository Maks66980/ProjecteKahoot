using System;

// Controla el avance del cuestionario y acumula los puntos y el tiempo.
public class ProgresoPartida
{
    private readonly PreguntaDatos[] preguntas;
    private int indicePregunta;

    public PreguntaDatos PreguntaActual => preguntas[indicePregunta];
    public int NumeroPregunta => indicePregunta + 1;
    public int TotalPreguntas => preguntas.Length;
    public int PuntosTotales { get; private set; } // Suma de los puntos obtenidos.
    public bool Finalizada { get; private set; } // Indica si se ha completado el cuestionario.
    public float  SegundosTotales { get; set; } // Suma del tiempo empleado en las preguntas.

    public ProgresoPartida(KahootDatos cuestionario)
    {
        if (cuestionario == null ||
            cuestionario.preguntas == null ||
            cuestionario.preguntas.Length == 0)
        {
            throw new ArgumentException(
                "La partida necesita un cuestionario con preguntas.",
                nameof(cuestionario)
            );
        }

        preguntas = cuestionario.preguntas;
    }

    public void RegistrarRespuesta(int puntosObtenidos, float segundosEmpleados) // Acumula los puntos y el tiempo de la pregunta terminada.
    {
        if (!Finalizada)
        {
            PuntosTotales += puntosObtenidos;
            SegundosTotales += segundosEmpleados;
        }
    }
    public bool Avanzar()
    {
        if (NumeroPregunta < TotalPreguntas)
        {
            indicePregunta++;
        }
        else
        {
            // Conservamos la última pregunta y el total al terminar.
            Finalizada = true;
        }

        return !Finalizada;
    }
}
