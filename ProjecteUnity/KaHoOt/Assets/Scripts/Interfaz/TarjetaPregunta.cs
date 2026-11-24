using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TarjetaPregunta : MonoBehaviour//Clase que recoge los datos de una pregunta y comunica su eliminación.
{
    [SerializeField] private TMP_Text numeroPregunta;//Referencia al número de la pregunta.
    [SerializeField] private TMP_InputField enunciado;//Referencia al campo del enunciado.
    [SerializeField] private TMP_InputField[] respuestas = new TMP_InputField[4];//Referencias a las cuatro respuestas.
    [SerializeField] private TMP_Dropdown respuestaCorrecta;//Referencia al selector de la respuesta correcta.
    [SerializeField] private TMP_InputField duracion;//Referencia al campo de duración.
    [SerializeField] private Button botonEliminar;//Referencia al botón de eliminar.

    public event Action<TarjetaPregunta> EliminacionSolicitada;//Evento que comunica qué tarjeta se quiere eliminar.

    private void Awake()//Conecta el botón con la solicitud de eliminación.
    {
        if (botonEliminar != null)
        {
            botonEliminar.onClick.AddListener(SolicitarEliminacion);
        }
    }

    public void ConfigurarNumero(int numero)//Método que actualiza el número visible de la pregunta.
    {
        if (numeroPregunta != null)
        {
            numeroPregunta.text = "Pregunta " + numero;
        }
    }

    public PreguntaDatos ObtenerDatos()//Método que recoge los valores de los campos de la tarjeta.
    {
        if (enunciado == null || respuestas == null ||
            respuestas.Length != 4 || respuestaCorrecta == null ||
            duracion == null)//Verifica las referencias necesarias para recoger los datos.
        {
            throw new InvalidOperationException(
                "Revisa los campos asignados en TarjetaPregunta."
            );
        }

        string[] opciones = new string[4];//Prepara las cuatro respuestas de la pregunta.

        for (int i = 0; i < respuestas.Length; i++)
        {
            if (respuestas[i] == null)//Comprueba que cada campo esté asignado.
            {
                throw new InvalidOperationException(
                    "Falta asignar un campo de respuesta en TarjetaPregunta."
                );
            }
            else
            {
                opciones[i] = respuestas[i].text.Trim();//Recoge la respuesta sin espacios en los extremos.
            }
        }

        int segundos;

        if (!int.TryParse(duracion.text, out segundos))//Comprueba que la duración sea un número entero.
        {
            throw new FormatException(
                "La durada de la pregunta ha de ser un nombre enter."
            );
        }

        PreguntaDatos datos = new PreguntaDatos
        {
            enunciado = enunciado.text.Trim(),
            respuestas = opciones,
            respuestaCorrecta = respuestaCorrecta.value,
            duracionSegundos = segundos
        };//Construye los datos utilizando el mismo formato que los cuestionarios.

        return datos;
    }

    private void SolicitarEliminacion()//Método que comunica la tarjeta que se quiere eliminar.
    {
        EliminacionSolicitada?.Invoke(this);
    }

    private void OnDestroy()//Elimina la suscripción cuando se destruye la tarjeta.
    {
        if (botonEliminar != null)
        {
            botonEliminar.onClick.RemoveListener(SolicitarEliminacion);
        }
    }
}
