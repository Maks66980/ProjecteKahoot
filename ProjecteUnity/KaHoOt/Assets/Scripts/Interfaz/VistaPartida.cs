using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Muestra la pregunta, el contador, el resultado de la respuesta y la puntuación.
public class VistaPartida : MonoBehaviour
{
    [SerializeField] private TMP_Text textoPregunta; // Referencia al texto de la pregunta.
    [SerializeField] private TMP_Text textoProgreso; // Referencia al texto del progreso de la partida.
    [SerializeField] private TMP_Text textoTiempo; // Referencia al texto del tiempo restante.
    [SerializeField] private TMP_Text[] textosRespuestas = new TMP_Text[4]; // Referencia a los textos de las respuestas.
    [SerializeField] private TMP_Text textoFeedback; // Mensaje que indica el resultado de la respuesta.
    [SerializeField] private GameObject panelPuntos; // Referencia al panel que muestra los puntos obtenidos.
    [SerializeField] private TMP_Text tituloPuntos; // Título de la pantalla de puntuación.
    [SerializeField] private TMP_Text textoPuntosPregunta; // Puntos obtenidos en la pregunta.
    [SerializeField] private TMP_Text textoPuntosTotales; // Puntuación total de la partida.
    [SerializeField] private GameObject botonMenuPuntos; // Botón para volver al menú al terminar la partida.

    public void MostrarPregunta(
        PreguntaDatos pregunta,
        int numero,
        int total) // Muestra la pregunta y sus respuestas en la interfaz.
    {
        if (textoPregunta != null &&
            textoProgreso != null &&
            textoTiempo != null &&
            textosRespuestas != null &&
            textosRespuestas.Length == 4) // Verifica que los textos estén asignados correctamente.
        {
            // Al empezar otra pregunta, retiramos la pantalla de puntos.
            if (panelPuntos != null)
            {
                panelPuntos.SetActive(false);
            }

            CambiarEstadoRespuestas(true);

            textoPregunta.text = pregunta.enunciado; // Muestra el enunciado de la pregunta.

            textoProgreso.text =
                "Pregunta " + numero + " / " + total; // Muestra el progreso de la partida.

            textoTiempo.text =
                "Temps: " + pregunta.duracionSegundos + " s"; // Muestra el tiempo restante.

            for (int i = 0; i < textosRespuestas.Length; i++) // Itera sobre los textos de las respuestas.
            {
                if (textosRespuestas[i] != null) // Verifica que el texto de la respuesta esté asignado.
                {
                    textosRespuestas[i].text = pregunta.respuestas[i]; // Muestra la respuesta correspondiente.
                }
                else
                {
                    Debug.LogError(
                        "Falta asignar el texto de la respuesta " + i
                    );
                }
            }

            SeleccionarPrimeraRespuesta();//Recupera la selección al mostrar cada pregunta.
        }
        else
        {
            Debug.LogError(
                "Revisa los textos asignados en VistaPartida."
            );
        }
    }
    public void MostrarTiempo(float segundosRestantes) // Actualiza el texto del tiempo restante en la interfaz.
    {
        if (textoTiempo != null) // Verifica que el texto de tiempo esté asignado.
        {
           int segundosVisibles = Mathf.CeilToInt(segundosRestantes); // Redondea hacia arriba para mostrar cero cuando se agota el tiempo.
            textoTiempo.text = "Temps: " + segundosVisibles + " s"; // Muestra el tiempo restante en la interfaz.
        }
    }
    // Muestra el resultado de la evaluación y bloquea las opciones.
    public void MostrarEvaluacion(RespuestaEvaluada evaluacion)
    {
        CambiarEstadoRespuestas(false);

        if(textoFeedback != null)
        {
            if(evaluacion.TiempoAgotado)
            {
                textoFeedback.text = "Temps esgotat. Resposta correcta: " + evaluacion.RespuestaCorrecta;
            }
            else if (evaluacion.EsCorrecta)
            {
                textoFeedback.text = "Correcte!";
            }
            else
            {
                textoFeedback.text = "Incorrecte. Resposta correcta: " + evaluacion.RespuestaCorrecta;
            }
        }
    }

    // Los textos asignados son hijos de los botones de respuesta.
    private void CambiarEstadoRespuestas(bool activas)
    {
        if (textosRespuestas != null)
        {
            foreach (TMP_Text texto in textosRespuestas)
            {
                if (texto != null)
                {
                    Button boton = texto.GetComponentInParent<Button>();

                    if (boton != null)
                    {
                        boton.interactable = activas;
                    }
                }
            }
        }
    }

    public void MostrarPuntos(int puntos, int total)
    {
        if (ComprobarReferenciasPuntos())
        {
            tituloPuntos.text = "Puntuació";
            textoPuntosPregunta.text = "+" + puntos + " punts";
            textoPuntosTotales.text = "Total · " + total + " punts";

            // El botón de menú se muestra al finalizar el cuestionario.
            botonMenuPuntos.SetActive(false);
            panelPuntos.SetActive(true);
            SeleccionarBoton(null);//Retira la selección de la respuesta bloqueada durante la puntuación.
        }
    }

    public void MostrarFinPartida(float segundosTotales)
    {
        if (ComprobarReferenciasPuntos())
        {
            tituloPuntos.text = "Partida acabada";
            textoFeedback.text = string.Empty;
            textoPuntosPregunta.text = "Temps total · " + segundosTotales.ToString("0.0") + " s";
            botonMenuPuntos.SetActive(true);
            panelPuntos.SetActive(true);
            SeleccionarBoton(botonMenuPuntos.GetComponent<Button>());//Permite volver al menú con teclado o mando.
        }
    }

    private void SeleccionarPrimeraRespuesta()//Método que busca la primera respuesta disponible para navegar.
    {
        Button primeraRespuesta = null;

        if (textosRespuestas != null)
        {
            foreach (TMP_Text texto in textosRespuestas)
            {
                if (primeraRespuesta == null && texto != null)
                {
                    Button boton = texto.GetComponentInParent<Button>();

                    if (boton != null && boton.isActiveAndEnabled && boton.IsInteractable())
                    {
                        primeraRespuesta = boton;
                    }
                }
            }
        }

        SeleccionarBoton(primeraRespuesta);
    }

    private void SeleccionarBoton(Button boton)//Método que actualiza el elemento seleccionado del EventSystem.
    {
        EventSystem sistema = EventSystem.current;

        if (sistema != null)
        {
            sistema.SetSelectedGameObject(null);//Limpia la selección anterior aunque sea el mismo botón.

            if (boton != null && boton.isActiveAndEnabled && boton.IsInteractable())
            {
                sistema.SetSelectedGameObject(boton.gameObject);
            }
        }
    }

    private bool ComprobarReferenciasPuntos()
    {
        bool configurado = panelPuntos != null &&
                            tituloPuntos != null &&
                            textoFeedback != null &&
                            textoPuntosPregunta != null &&
                            textoPuntosTotales != null &&
                            botonMenuPuntos != null;

        if (!configurado)
        {
            Debug.LogError("Asigna el panel de puntos, sus textos y el botón de menú en VistaPartida.");
        }

        return configurado;
    }
}
