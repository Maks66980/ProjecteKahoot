using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VistaContenidoInforme : MonoBehaviour//Clase que muestra el contenido de un informe y permite cerrar el visor.
{
    [SerializeField] private GameObject panelLista;//Referencia al panel que contiene la lista de informes.
    [SerializeField] private GameObject panelVisor;//Referencia al panel que muestra el informe.
    [SerializeField] private TMP_Text titulo;//Referencia al título del visor.
    [SerializeField] private TMP_Text textoContenido;//Referencia al texto del informe.
    [SerializeField] private ScrollRect scrollContenido;//Referencia al desplazamiento del contenido.
    [SerializeField] private Button botonCerrar;//Referencia al botón para volver a la lista.

    private void Awake()//Conecta el botón de cierre y prepara la pantalla inicial.
    {
        if (botonCerrar != null)
        {
            botonCerrar.onClick.AddListener(Cerrar);
        }

        Cerrar();//Deja visible la lista al entrar en la escena.
    }

    public void Mostrar(string nombre, string contenido)//Método que muestra el informe recibido.
    {
        if (panelLista != null && panelVisor != null &&
            titulo != null && textoContenido != null &&
            scrollContenido != null)//Verifica las referencias del visor.
        {
            titulo.text = nombre;
            textoContenido.richText = false;//Muestra las etiquetas del archivo como texto.
            textoContenido.text = contenido;

            panelLista.SetActive(false);//Oculta la lista mientras se consulta el informe.
            panelVisor.SetActive(true);

            Canvas.ForceUpdateCanvases();//Actualiza el tamaño del contenido antes de mover el scroll.
            scrollContenido.StopMovement();//Detiene el desplazamiento del informe anterior.
            scrollContenido.verticalNormalizedPosition = 1f;//Muestra el principio del informe.
        }
        else
        {
            Debug.LogError("Revisa las referencias de VistaContenidoInforme.", this);
        }
    }

    public void Cerrar()//Método que oculta el visor y vuelve a mostrar la lista.
    {
        if (panelVisor != null && panelLista != null)
        {
            panelVisor.SetActive(false);
            panelLista.SetActive(true);
        }
        else
        {
            Debug.LogError("Asigna los paneles de VistaContenidoInforme.", this);
        }
    }

    private void OnDestroy()//Elimina la suscripción cuando se destruye la vista.
    {
        if (botonCerrar != null)
        {
            botonCerrar.onClick.RemoveListener(Cerrar);
        }
    }
}