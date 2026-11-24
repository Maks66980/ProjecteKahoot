using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class VistaClasificacion : MonoBehaviour//Clase que muestra los resultados en la lista de clasificación.
{
    [SerializeField] private Transform contenido;//Referencia al Content de la lista.
    [SerializeField] private FilaResultado filaPrefab;//Referencia al prefab utilizado para mostrar cada resultado.
    [SerializeField] private TMP_Text textoEstado;//Referencia al texto que muestra el estado de la lista.
    [SerializeField] private AvisoInformes avisoInformes;//Referencia al aviso que permite consultar los informes.


    [SerializeField] private TMP_Dropdown dropdownCuestionarios;//Referencia al desplegable que muestra los cuestionarios disponibles.

    public event Action<int> CuestionarioSeleccionado;//Evento que comunica el índice del cuestionario seleccionado.

    public void MostrarResultados(List<ResultadoPartida> resultados)//Método que crea las filas con los resultados recibidos.
    {
        if (contenido != null &&
            filaPrefab != null &&
            textoEstado != null &&
            resultados != null)//Verifica que las referencias y la lista de resultados estén asignadas.
        {
            LimpiarFilas();//Elimina las filas del cuestionario mostrado anteriormente.

            if (resultados.Count == 0)//Verifica si el cuestionario todavía no tiene partidas guardadas.
            {
                textoEstado.text = "Encara no hi ha resultats.";
            }
            else
            {
                textoEstado.text = string.Empty;//Limpia el mensaje cuando hay resultados disponibles.

                for (int i = 0; i < resultados.Count; i++)//Recorre los resultados en el orden recibido.
                {
                    FilaResultado fila = Instantiate(
                        filaPrefab,
                        contenido
                    );//Crea una fila dentro del Content.

                    fila.Configurar(resultados[i], i + 1);//Muestra los datos de la partida y su posición empezando por uno.
                }
            }
        }
        else
        {
            Debug.LogError(
                "Revisa las referencias y los resultados de VistaClasificacion.",
                this
            );
        }
    }



    private void OnEnable()//Suscribe el método que recibe los cambios del desplegable.
    {
        if (dropdownCuestionarios != null)//Verifica que el desplegable esté asignado.
        {
            dropdownCuestionarios.onValueChanged.AddListener(NotificarSeleccion);
        }
    }

    private void OnDisable()//Elimina la suscripción cuando se desactiva la vista.
    {
        if (dropdownCuestionarios != null)
        {
            dropdownCuestionarios.onValueChanged.RemoveListener(NotificarSeleccion);
        }
    }

    public void MostrarCuestionarios(List<string> titulos)//Método que rellena el desplegable con los títulos recibidos.
    {
        if (dropdownCuestionarios != null && titulos != null)//Verifica que el desplegable y la lista estén asignados.
        {
            dropdownCuestionarios.ClearOptions();//Elimina las opciones mostradas anteriormente.

            if (titulos.Count > 0)//Verifica si hay cuestionarios disponibles.
            {
                dropdownCuestionarios.AddOptions(titulos);//Añade los títulos al desplegable.
            }
            else
            {
                dropdownCuestionarios.AddOptions(
                    new List<string> { "No hi ha qüestionaris" }
                );//Muestra un mensaje cuando no hay cuestionarios disponibles.
            }

            dropdownCuestionarios.interactable = titulos.Count > 0;//Permite utilizar el desplegable cuando hay cuestionarios.
            dropdownCuestionarios.SetValueWithoutNotify(0);//Selecciona la primera opción sin emitir el evento.
            dropdownCuestionarios.RefreshShownValue();//Actualiza el texto visible del desplegable.
        }
        else
        {
            Debug.LogError(
                "Revisa el desplegable y los títulos de VistaClasificacion.",
                this
            );
        }
    }

    private void NotificarSeleccion(int indice)//Método que comunica el cambio de cuestionario a los suscriptores.
    {
        CuestionarioSeleccionado?.Invoke(indice);
    }



    public void MostrarMensaje(string mensaje)//Método que limpia la clasificación y muestra un mensaje al usuario.
    {
        if (contenido != null && textoEstado != null)//Verifica que el contenido y el texto estén asignados.
        {
            LimpiarFilas();//Elimina los resultados mostrados anteriormente.
            textoEstado.text = mensaje;//Muestra el mensaje recibido.
        }
        else
        {
            Debug.LogError("Asigna el contenido y el texto de estado.", this);
        }
    }



    public void MostrarAvisoInformes(string mensaje)//Método que muestra dónde consultar los errores de carga.
    {
        if (avisoInformes != null)
        {
            avisoInformes.Mostrar(mensaje);
        }
    }

    private void LimpiarFilas()//Método que elimina las filas existentes antes de mostrar otra clasificación.
    {
        for (int i = contenido.childCount - 1; i >= 0; i--)//Recorre las filas desde la última hasta la primera.
        {
            GameObject fila = contenido.GetChild(i).gameObject;//Obtiene el objeto de la fila.

            fila.SetActive(false);//Desactiva la fila para retirarla del layout inmediatamente.
            Destroy(fila);//Elimina el objeto al terminar el frame.
        }
    }
}