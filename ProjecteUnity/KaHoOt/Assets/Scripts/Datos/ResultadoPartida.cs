using UnityEngine;

// Representa el resultado guardado de un jugador para mostrarlo en la clasificación.
public class ResultadoPartida
{
    public string Jugador { get; }
    public int Puntos { get; }
    public float Segundos { get; }

    public ResultadoPartida(string jugador, int puntos, float segundos)
    {
        Jugador = jugador;
        Puntos = puntos;
        Segundos = segundos;
    }
}