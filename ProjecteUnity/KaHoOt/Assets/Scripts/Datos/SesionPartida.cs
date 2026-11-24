using System;
using UnityEngine;

// Conserva el cuestionario y el jugador de la partida actual.
[CreateAssetMenu(
    fileName = "SesionPartida",
    menuName = "KaHoOt/Sesion de partida"
)]
public class SesionPartida : ScriptableObject
{
    public KahootDatos Cuestionario { get; private set; } // El cuestionario de la partida actual.
    public string NombreJugador { get; private set; } = string.Empty; // El nombre del jugador de la partida actual.

    public void Preparar(KahootDatos cuestionario, string nombreJugador) // Prepara la sesión de partida con el cuestionario y el nombre del jugador.
    {
        if (cuestionario == null) // Comprueba que haya un cuestionario.
        {
            throw new ArgumentNullException(nameof(cuestionario));
        }
        else if (string.IsNullOrWhiteSpace(nombreJugador)) // Comprueba que el jugador tenga nombre.
        {
            throw new ArgumentException(
                "El nombre del jugador no puede estar vacío.",
                nameof(nombreJugador)
            );
        }
        else // Guarda los datos de la sesión.
        {
            Cuestionario = cuestionario;

            // Eliminamos los espacios al principio y al final del nombre.
            NombreJugador = nombreJugador.Trim();
        }
    }

    public void Limpiar()
    {
        Cuestionario = null;
        NombreJugador = string.Empty;
    }
}
