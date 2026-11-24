using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VistaCreadorKahoot : MonoBehaviour//Clase que recoge los campos del creador y muestra el resultado del guardado.
{
    [SerializeField] private TMP_InputField titulo;//Referencia al título del cuestionario.
    [SerializeField] private VistaListaPreguntas listaPreguntas;//Referencia a la lista de preguntas.
    [SerializeField] private Button botonGuardar;//Referencia al botón de guardar.
    [SerializeField] private TMP_Text textoEstado;//Referencia al mensaje de estado.

    public event Action GuardadoSolicitado;//Evento que comunica la solicitud de guardar el cuestionario.

    private void Awake()//Conecta el botón con la solicitud de guardado.
    {
        if (botonGuardar != null)
        {
            botonGuardar.onClick.AddListener(SolicitarGuardado);
        }
        else
        {
            Debug.LogError("Asigna el botón de guardar en VistaCreadorKahoot.", this);
        }
    }

    public string ObtenerTitulo()//Método que obtiene el título sin espacios en los extremos.
    {
        if (titulo == null)
        {
            throw new InvalidOperationException("Asigna el título en VistaCreadorKahoot.");
        }

        string nombre = titulo.text.Trim();
        return nombre;
    }

    public PreguntaDatos[] ObtenerPreguntas()//Método que solicita los datos a la lista de preguntas.
    {
        if (listaPreguntas == null)
        {
            throw new InvalidOperationException("Asigna la lista en VistaCreadorKahoot.");
        }

        PreguntaDatos[] preguntas = listaPreguntas.ObtenerPreguntas();
        return preguntas;
    }

    public void MostrarMensaje(string mensaje, bool esError)//Método que muestra el resultado de la operación.
    {
        if (textoEstado != null)
        {
            textoEstado.text = mensaje;

            if (esError)
            {
                textoEstado.color = new Color(1f, 0.85f, 0.35f);//Destaca los errores sobre el fondo morado.
            }
            else
            {
                textoEstado.color = new Color(0.65f, 1f, 0.7f);//Destaca la confirmación de guardado.
            }
        }
    }

    private void SolicitarGuardado()//Método que comunica la pulsación del botón de guardar.
    {
        GuardadoSolicitado?.Invoke();
    }

    private void OnDestroy()//Elimina la suscripción cuando se destruye la vista.
    {
        if (botonGuardar != null)
        {
            botonGuardar.onClick.RemoveListener(SolicitarGuardado);
        }
    }
}
