using TMPro;
using UnityEngine;

public class FilaResultado : MonoBehaviour//Clase que muestra los datos de una partida en la clasificación.
{
    [SerializeField] private TMP_Text textoPosicion;//Referencia al texto de la posición.
    [SerializeField] private TMP_Text textoJugador;//Referencia al texto del nombre del jugador.
    [SerializeField] private TMP_Text textoPuntos;//Referencia al texto de los puntos obtenidos.
    [SerializeField] private TMP_Text textoTiempo;//Referencia al texto del tiempo empleado.

    public void Configurar(ResultadoPartida resultado, int posicion)//Método que muestra el resultado de la partida y su posición en la clasificación.
    {
        if (resultado != null &&
            textoPosicion != null &&
            textoJugador != null &&
            textoPuntos != null &&
            textoTiempo != null)//Verifica que el resultado y los textos estén asignados.
        {
            textoPosicion.text = posicion.ToString();//Muestra la posición de la partida.
            textoJugador.text = resultado.Jugador;//Muestra el nombre del jugador.
            textoPuntos.text = resultado.Puntos + " punts";//Muestra los puntos obtenidos.
            textoTiempo.text = resultado.Segundos.ToString("0.0") + " s";//Muestra el tiempo con un decimal.
        }
        else
        {
            Debug.LogError(
                "Revisa el resultado y los textos asignados en FilaResultado.",
                this
            );
        }
    }
}