using System;
using System.IO;
using UnityEngine;

public class ControladorInformes : MonoBehaviour//Clase que coordina la carga de los informes y su presentación.
{
    private IRepositorioInformes repositorio;//Referencia al repositorio de informes.
    private VistaInformes vista;//Referencia a la vista de la lista.
    private VistaContenidoInforme visor;//Referencia a la vista que muestra el contenido del informe.
    private IRegistradorErrores registradorErrores;//Referencia al gestor de errores.

    public void Iniciar(
        IRepositorioInformes repositorio,
        VistaInformes vista,
        VistaContenidoInforme visor,
        IRegistradorErrores registradorErrores)//Método que recibe las dependencias y carga la lista.
    {
        if (repositorio != null && vista != null &&
            visor != null && registradorErrores != null)//Verifica que las dependencias estén asignadas.
        {
            if (this.vista != null)//Elimina la suscripción de una inicialización anterior.
            {
                this.vista.InformeSeleccionado -= AbrirInforme;
            }

            this.repositorio = repositorio;
            this.vista = vista;
            this.visor = visor;
            this.registradorErrores = registradorErrores;

            this.vista.InformeSeleccionado += AbrirInforme;//Escucha la selección de un informe.

            ActualizarLista();
        }
        else
        {
            Debug.LogError("Faltan dependencias en ControladorInformes.", this);
        }
    }

    public void ActualizarLista()//Método que obtiene las rutas y actualiza las filas.
    {
        if (repositorio != null && vista != null &&
            registradorErrores != null)//Comprueba que el controlador esté preparado.
        {
            try
            {
                string[] rutas = repositorio.ObtenerRutas();//Obtiene los informes disponibles.
                vista.MostrarInformes(rutas);//Muestra una fila por cada informe.
            }
            catch (Exception error)
            {
                vista.MostrarMensaje("No s'ha pogut carregar la llista d'informes.");

                registradorErrores.Registrar(
                    "Cargar la lista de informes",
                    error
                );//Registra el fallo al consultar la carpeta.
            }
        }
        else
        {
            Debug.LogError("Inicializa ControladorInformes antes de actualizar la lista.", this);
        }
    }

    private void AbrirInforme(string ruta)//Método que lee el informe seleccionado y lo muestra.
    {
        try
        {
            string contenido = repositorio.Leer(ruta);//Obtiene el contenido completo del archivo.
            string nombre = Path.GetFileNameWithoutExtension(ruta);//Obtiene el nombre para el título.

            visor.Mostrar(nombre, contenido);//Abre el visor con el informe seleccionado.
            vista.MostrarMensaje(string.Empty);//Limpia los mensajes anteriores.
        }
        catch (Exception error)
        {
            vista.MostrarMensaje("No s'ha pogut obrir l'informe seleccionat.");

            registradorErrores.Registrar(
                "Leer el informe " + ruta,
                error
            );//Registra el fallo al leer el archivo.
        }
    }

    private void OnDestroy()//Elimina la suscripción cuando se destruye el controlador.
    {
        if (vista != null)
        {
            vista.InformeSeleccionado -= AbrirInforme;
        }
    }
}