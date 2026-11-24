using UnityEngine;

// Resultado de la pregunta que se comunica a los suscriptores.
public readonly struct RespuestaEvaluada
{
    public bool EsCorrecta { get; } // Indica si la respuesta elegida es correcta.
    public string RespuestaCorrecta { get; } // Texto de la respuesta correcta.
    public bool TiempoAgotado { get; } // Indica si se agotó el tiempo para responder.

    public int PuntosObtenidos { get; } // Puntos obtenidos en la pregunta.

    public float SegundosEmpleados { get; } // Tiempo empleado en la pregunta, incluida su duración completa si se agota.

    public RespuestaEvaluada(
        bool esCorrecta,
        string respuestaCorrecta,
        bool tiempoAgotado = false,
        int puntosObtenidos = 0,
        float segundosEmpleados = 0f)
    {
        EsCorrecta = esCorrecta;
        RespuestaCorrecta = respuestaCorrecta;
        TiempoAgotado = tiempoAgotado;
        PuntosObtenidos = puntosObtenidos;
        SegundosEmpleados = segundosEmpleados;
    }
}