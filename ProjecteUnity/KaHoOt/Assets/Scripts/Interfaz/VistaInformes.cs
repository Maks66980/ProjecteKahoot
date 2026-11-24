using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VistaInformes : MonoBehaviour//Clase que muestra la lista de informes y comunica la selección.
{
    [SerializeField] private Transform contenido;//Referencia al Content de la lista.
    [SerializeField] private FilaInforme filaPrefab;//Referencia al prefab de cada informe.
    [SerializeField] private TMP_Text textoEstado;//Referencia al texto de estado.

    private readonly List<FilaInforme> filas = new List<FilaInforme>();//Contiene las filas creadas.

    public event Action<string> InformeSeleccionado;//Evento que comunica la ruta del informe elegido.

    public void MostrarInformes(string[] rutas)//Método que crea una fila por cada informe recibido.
    {
        if (contenido != null && filaPrefab != null &&
            textoEstado != null && rutas != null)//Verifica las referencias y los datos.
        {
            LimpiarFilas();//Elimina las filas mostradas anteriormente.

            if (rutas.Length == 0)//Comprueba si hay informes disponibles.
            {
                textoEstado.text = "No hi ha informes disponibles.";
            }
            else
            {
                textoEstado.text = string.Empty;

                foreach (string ruta in rutas)//Recorre las rutas recibidas.
                {
                    FilaInforme fila = Instantiate(filaPrefab, contenido);//Crea la fila dentro del Content.
                    fila.Configurar(ruta);//Muestra el nombre del informe.
                    fila.Seleccionado += NotificarSeleccion;//Escucha la selección de esta fila.
                    filas.Add(fila);//Guarda la referencia para poder eliminarla después.
                }
            }
        }
        else
        {
            Debug.LogError("Revisa las referencias y las rutas de VistaInformes.", this);
        }
    }

    public void MostrarMensaje(string mensaje)//Método que muestra un mensaje sobre el estado de la lista.
    {
        if (textoEstado != null)
        {
            textoEstado.text = mensaje;
        }
    }

    private void NotificarSeleccion(string ruta)//Método que comunica el informe seleccionado.
    {
        InformeSeleccionado?.Invoke(ruta);
    }

    private void LimpiarFilas()//Método que elimina las filas y sus suscripciones.
    {
        foreach (FilaInforme fila in filas)
        {
            if (fila != null)//Verifica que la fila siga existiendo.
            {
                fila.Seleccionado -= NotificarSeleccion;
                fila.gameObject.SetActive(false);//Retira la fila del layout inmediatamente.
                Destroy(fila.gameObject);
            }
        }

        filas.Clear();//Vacía las referencias de las filas eliminadas.
    }

    private void OnDestroy()//Elimina las suscripciones cuando se destruye la vista.
    {
        foreach (FilaInforme fila in filas)
        {
            if (fila != null)
            {
                fila.Seleccionado -= NotificarSeleccion;
            }
        }
    }
}