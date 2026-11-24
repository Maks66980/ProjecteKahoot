using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FilaInforme : MonoBehaviour//Clase que muestra el nombre de un informe y permite seleccionarlo.
{
    [SerializeField] private TMP_Text texto;//Referencia al texto del nombre del informe.

    private string ruta;//Contiene la ruta completa del informe.
    private Button boton;//Referencia al botón de la fila.

    public event Action<string> Seleccionado;//Evento que comunica la ruta del informe seleccionado.

    private void Awake()//Conecta el botón con el método que notifica la selección.
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(NotificarSeleccion);
    }

    public void Configurar(string ruta)//Método que guarda la ruta y muestra el nombre del informe.
    {
        this.ruta = ruta;//Guarda la ruta del archivo recibido.

        if (texto != null)//Verifica que el texto esté asignado.
        {
            texto.text = Path.GetFileNameWithoutExtension(ruta);//Muestra el nombre del archivo sin su extensión.
        }
        else
        {
            Debug.LogError("Asigna el texto de FilaInforme.", this);
        }
    }

    private void NotificarSeleccion()//Método que comunica el informe elegido al pulsar la fila.
    {
        if (!string.IsNullOrWhiteSpace(ruta))//Verifica que la fila tenga una ruta asignada.
        {
            Seleccionado?.Invoke(ruta);
        }
    }

    private void OnDestroy()//Elimina la suscripción cuando se destruye la fila.
    {
        if (boton != null)
        {
            boton.onClick.RemoveListener(NotificarSeleccion);
        }
    }
}