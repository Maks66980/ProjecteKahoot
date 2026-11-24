using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// La tarjeta necesita un botón para recibir los clics.
[RequireComponent(typeof(Button))]
public class TarjetaKahoot : MonoBehaviour
{
    [SerializeField] private TMP_Text texto;

    private KahootDatos datos;

    // Avisa de la selección sin depender directamente de la clase del selector.
    public event Action<KahootDatos> Seleccionada;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(NotificarSeleccion);
    }

    public void Configurar(KahootDatos datos) // Muestra el título y el número de preguntas.
    {
        this.datos = datos;

        if (texto != null) // Comprueba la referencia del texto.
        {
            texto.text = datos.titulo + "\n" +
                         datos.preguntas.Length + " preguntes"; // Texto visible en la tarjeta.
        }
        else
        {
            Debug.LogError("Falta asignar el texto de la tarjeta."); // Informa de la referencia que falta.
        }
    }

    private void NotificarSeleccion() // Notifica la selección del cuestionario.
    {
        if (datos != null)
        {
            // Solo avisa si existe algún componente escuchando el evento.
            Seleccionada?.Invoke(datos);
        }
    }
}
