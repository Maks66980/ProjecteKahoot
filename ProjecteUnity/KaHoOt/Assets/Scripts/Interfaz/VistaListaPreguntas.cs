using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VistaListaPreguntas : MonoBehaviour//Clase que gestiona las tarjetas del creador de cuestionarios.
{
    [SerializeField] private Transform contenido;//Referencia al Content de la lista.
    [SerializeField] private TarjetaPregunta tarjetaPrefab;//Referencia al prefab de pregunta.
    [SerializeField] private Button botonAgregar;//Referencia al botón de añadir preguntas.
    [SerializeField] private ScrollRect scrollPreguntas;//Referencia al desplazamiento de la lista.

    private readonly List<TarjetaPregunta> tarjetas = new List<TarjetaPregunta>();//Contiene las tarjetas del cuestionario.

    private void Awake()//Prepara las tarjetas existentes y conecta el botón de añadir.
    {
        if (contenido != null && tarjetaPrefab != null &&
            botonAgregar != null && scrollPreguntas != null)//Comprueba las referencias de la lista.
        {
            TarjetaPregunta[] existentes =
                contenido.GetComponentsInChildren<TarjetaPregunta>();//Obtiene las tarjetas que ya están en la escena.

            foreach (TarjetaPregunta tarjeta in existentes)
            {
                tarjetas.Add(tarjeta);
                tarjeta.EliminacionSolicitada += EliminarPregunta;
            }

            botonAgregar.onClick.AddListener(AgregarPregunta);
            ActualizarNumeros();

            if (tarjetas.Count == 0)//Crea la primera pregunta cuando la lista está vacía.
            {
                AgregarPregunta();
            }
        }
        else
        {
            Debug.LogError("Asigna las referencias de VistaListaPreguntas.", this);
        }
    }

    private void AgregarPregunta()//Método que crea una nueva tarjeta dentro de la lista.
    {
        TarjetaPregunta tarjeta = Instantiate(tarjetaPrefab, contenido);
        tarjeta.EliminacionSolicitada += EliminarPregunta;
        tarjetas.Add(tarjeta);

        ActualizarNumeros();

        Canvas.ForceUpdateCanvases();//Actualiza la altura de la lista antes de desplazarla.
        scrollPreguntas.StopMovement();
        scrollPreguntas.verticalNormalizedPosition = 0f;//Desplaza la lista hasta el final.
    }

    private void EliminarPregunta(TarjetaPregunta tarjeta)//Método que elimina la tarjeta seleccionada.
    {
        if (tarjeta != null && tarjetas.Remove(tarjeta))//Comprueba que la tarjeta pertenezca a esta lista.
        {
            tarjeta.EliminacionSolicitada -= EliminarPregunta;
            tarjeta.gameObject.SetActive(false);//Retira la tarjeta del layout inmediatamente.
            Destroy(tarjeta.gameObject);

            ActualizarNumeros();
        }
    }

    private void ActualizarNumeros()//Método que mantiene la numeración después de añadir o eliminar preguntas.
    {
        for (int i = 0; i < tarjetas.Count; i++)
        {
            tarjetas[i].ConfigurarNumero(i + 1);
        }
    }

    public PreguntaDatos[] ObtenerPreguntas()//Método que recoge los datos de todas las tarjetas.
    {
        PreguntaDatos[] preguntas = new PreguntaDatos[tarjetas.Count];

        for (int i = 0; i < tarjetas.Count; i++)
        {
            preguntas[i] = tarjetas[i].ObtenerDatos();
        }

        return preguntas;
    }

    private void OnDestroy()//Elimina las suscripciones cuando se destruye la vista.
    {
        if (botonAgregar != null)
        {
            botonAgregar.onClick.RemoveListener(AgregarPregunta);
        }

        foreach (TarjetaPregunta tarjeta in tarjetas)
        {
            if (tarjeta != null)
            {
                tarjeta.EliminacionSolicitada -= EliminarPregunta;
            }
        }
    }
}
