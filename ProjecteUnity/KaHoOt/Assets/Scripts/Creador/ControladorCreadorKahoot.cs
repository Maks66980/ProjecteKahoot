using System;
using System.IO;
using UnityEngine;

public class ControladorCreadorKahoot : MonoBehaviour//Clase que coordina la recogida de datos y el guardado del cuestionario.
{
    private VistaCreadorKahoot vista;//Referencia a la vista del creador.
    private IEscritorKahoots escritor;//Referencia al escritor de cuestionarios.
    private IRegistradorErrores registradorErrores;//Referencia al gestor de errores.

    public void Iniciar(
        VistaCreadorKahoot vista,
        IEscritorKahoots escritor,
        IRegistradorErrores registradorErrores)//Método que recibe las dependencias y escucha el guardado.
    {
        if (vista != null && escritor != null && registradorErrores != null)
        {
            if (this.vista != null)//Elimina la suscripción de una inicialización anterior.
            {
                this.vista.GuardadoSolicitado -= GuardarCuestionario;
            }

            this.vista = vista;
            this.escritor = escritor;
            this.registradorErrores = registradorErrores;

            this.vista.GuardadoSolicitado += GuardarCuestionario;
        }
        else
        {
            Debug.LogError("Faltan dependencias en ControladorCreadorKahoot.", this);
        }
    }

    private void GuardarCuestionario()//Método que recoge el cuestionario y solicita su guardado.
    {
        try
        {
            KahootDatos datos = new KahootDatos
            {
                id = Guid.NewGuid().ToString("N"),//Asigna un identificador independiente del título.
                titulo = vista.ObtenerTitulo(),
                preguntas = vista.ObtenerPreguntas()
            };

            escritor.Guardar(datos);//Valida los datos y escribe el archivo JSON.

            vista.MostrarMensaje(
                "Qüestionari guardat. Torna al menú i prem Jugar per seleccionar-lo.",
                false
            );
        }
        catch (FormatException error)//Muestra los errores al convertir la duración de una pregunta.
        {
            vista.MostrarMensaje(error.Message, true);
        }
        catch (InvalidDataException error)//Muestra los datos que se deben corregir antes de guardar.
        {
            vista.MostrarMensaje(error.Message, true);
        }
        catch (Exception error)
        {
            vista.MostrarMensaje(
                "No s'ha pogut guardar el qüestionari. Consulta l'apartat Informes.",
                true
            );

            registradorErrores.Registrar(
                "Guardar el cuestionario del creador",
                error
            );//Registra los fallos de escritura o de configuración.
        }
    }

    private void OnDestroy()//Elimina la suscripción cuando se destruye el controlador.
    {
        if (vista != null)
        {
            vista.GuardadoSolicitado -= GuardarCuestionario;
        }
    }
}
