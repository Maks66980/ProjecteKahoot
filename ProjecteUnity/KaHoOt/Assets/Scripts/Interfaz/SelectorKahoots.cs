using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Muestra los cuestionarios disponibles y permite seleccionar uno para jugar.
public class SelectorKahoots : MonoBehaviour
{
    [SerializeField] private Transform contenido;
    [SerializeField] private TarjetaKahoot tarjetaPrefab;
    [SerializeField] private TMP_Text textoSeleccion;
    [SerializeField] private AvisoInformes avisoInformes;//Referencia al aviso que permite consultar los informes.
    [SerializeField] private TMP_InputField nombreJugador;
    [SerializeField] private Button botonJugar;

    private KahootDatos kahootSeleccionado;

    private IRegistradorErrores registradorErrores;//Referencia al gestor que registra los errores de lectura.

    // El selector utiliza el contrato del repositorio sin construir su implementación.
    private IRepositorioKahoots repositorio;

    // Notifica la intención de jugar sin conocer la sesión ni la navegación.
    public event Action<KahootDatos, string> PartidaSolicitada;

    public void Inicializar(IRepositorioKahoots repositorio,IRegistradorErrores registradorErrores)//Método que recibe las dependencias y carga los cuestionarios.
    {
        this.repositorio = repositorio;//Guarda el repositorio recibido.
        this.registradorErrores = registradorErrores;//Guarda el gestor de errores recibido.
        ActualizarLista();//Carga los cuestionarios y crea sus tarjetas.
    }


    // Permite volver a consultar los archivos sin cambiar de escena.
    public void ActualizarLista()
    {
        if (contenido != null &&
            tarjetaPrefab != null &&
            repositorio != null)
        {
            // Al actualizar, la selección anterior deja de ser válida.
            kahootSeleccionado = null;

            if (avisoInformes != null)//Oculta el aviso anterior antes de volver a leer los cuestionarios.
            {
                avisoInformes.Ocultar();
            }
            ActualizarEstadoBoton(); // Actualiza la disponibilidad del botón de jugar.

            if (textoSeleccion != null)
            {
                textoSeleccion.text = "Selecciona un qüestionari";
            }

            LimpiarTarjetas(); // Limpiamos las tarjetas existentes antes de cargar los cuestionarios
            CargarCuestionarios(); // Cargamos los cuestionarios desde el repositorio y creamos sus tarjetas
        }
        else
        {
            Debug.LogError(
                "Falta asignar Content, el prefab o el repositorio."
            );
        }
    }

    private void SeleccionarKahoot(KahootDatos datos) // Selecciona el cuestionario y muestra su título.
    {
        kahootSeleccionado = datos; // Guarda el cuestionario seleccionado.
        ActualizarEstadoBoton(); // Actualiza la disponibilidad del botón de jugar.

        if (textoSeleccion != null) // Muestra el título del cuestionario seleccionado.
        {
            textoSeleccion.text = "Seleccionat: " + datos.titulo;
        }

        Debug.Log("Cuestionario seleccionado: " + datos.titulo);
    }

    private void LimpiarTarjetas() // Elimina las tarjetas de la lista.
    {
        // Recorremos las tarjetas desde el final.
        for (int i = contenido.childCount - 1; i >= 0; i--)
        {
            GameObject tarjeta = contenido.GetChild(i).gameObject;

            // Destroy se completa al terminar el frame.
            // La desactivamos para retirarla inmediatamente del layout.
            tarjeta.SetActive(false);
            Destroy(tarjeta);
        }
    }

    // Solo permite jugar cuando existe una selección y un nombre válido.
    public void ActualizarEstadoBoton()
    {
        if (botonJugar != null)
        {
            botonJugar.interactable =
                kahootSeleccionado != null &&
                nombreJugador != null &&
                !string.IsNullOrWhiteSpace(nombreJugador.text);
        }
    }
    public void SolicitarPartida() // Solicita una partida con el cuestionario y el nombre indicados.
    {
        if (kahootSeleccionado != null &&
            nombreJugador != null &&
            !string.IsNullOrWhiteSpace(nombreJugador.text))
        {
            // Avisa al configurador con el cuestionario y el nombre del jugador.
            PartidaSolicitada?.Invoke(
                kahootSeleccionado,
                nombreJugador.text.Trim()
            );
        }
        else
        {
            Debug.LogWarning(
                "Selecciona un cuestionario y escribe tu nombre."
            );
        }
    }
    private void MostrarAvisoInformes(string mensaje)//Método que indica dónde consultar el informe del error.
    {
        if (avisoInformes != null)
        {
            avisoInformes.Mostrar(mensaje);
        }
    }

    private void CargarCuestionarios() // Carga los cuestionarios y crea una tarjeta para cada uno.
    {
        try
        {
            string[] archivos = repositorio.ObtenerRutas(); // Obtiene las rutas de los cuestionarios.

            foreach (string ruta in archivos) // Recorre los archivos disponibles.
            {
                try
                {
                    // El repositorio entrega los datos del cuestionario después de validarlos.
                    KahootDatos datos = repositorio.Cargar(ruta);

                    TarjetaKahoot tarjeta = Instantiate(
                        tarjetaPrefab,
                        contenido
                    ); // Añade una tarjeta al contenido de la lista.

                    tarjeta.Configurar(datos); // Muestra los datos del cuestionario en la tarjeta.

                    // El selector escucha la selección de esta tarjeta.
                    tarjeta.Seleccionada += SeleccionarKahoot;
                }
                catch (Exception error)
                {
                    registradorErrores.Registrar(
                        "Leer el cuestionario " + ruta,
                        error
                    );//Registra el archivo que ha fallado y permite cargar los siguientes.

                    MostrarAvisoInformes("Hi ha qüestionaris JSON que no s\'han pogut llegir.");
                }
            }
        }
        catch (Exception error)
        {
            registradorErrores.Registrar(
                "Acceder a la carpeta de cuestionarios",
                error
            );//Registra el fallo al consultar los archivos disponibles.

            MostrarAvisoInformes("No s\'han pogut consultar els qüestionaris JSON.");
        }
    }
}
