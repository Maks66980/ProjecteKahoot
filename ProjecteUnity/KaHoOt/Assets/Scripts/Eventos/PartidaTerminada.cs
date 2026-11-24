using UnityEngine;

// Datos que se publican al completar el cuestionario.
public readonly struct PartidaTerminada
{
    public int Puntos { get; } // Puntuación total de la partida.
    public float Segundos { get; } // Suma del tiempo empleado en las preguntas.

    public PartidaTerminada(int puntos, float segundos)
    {
        Puntos = puntos;
        Segundos = segundos;
    }
}