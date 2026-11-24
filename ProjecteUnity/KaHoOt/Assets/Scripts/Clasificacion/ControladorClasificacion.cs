using System;
using System.Collections.Generic;
using UnityEngine;

public class ControladorClasificacion : MonoBehaviour//Clase que coordina la selección del cuestionario y la carga de su clasificación.
{
    private List<KahootDatos> cuestionarios;//Contiene los cuestionarios disponibles en el desplegable.
    private IRepositorioResultados repositorio;//Referencia al repositorio que carga los resultados.
    private ClasificadorResultados clasificador;//Referencia a la clase que ordena los resultados.
    private VistaClasificacion vista;//Referencia a la vista que muestra la clasificación.
    private IRegistradorErrores registradorErrores;//Referencia al gestor que registra los errores al cargar resultados.

    public void Iniciar(
        List<KahootDatos> cuestionarios,
        IRepositorioResultados repositorio,
        ClasificadorResultados clasificador,
        VistaClasificacion vista,
        IRegistradorErrores registradorErrores)//Método que recibe las dependencias y prepara la clasificación.
    {
        if (cuestionarios != null &&
            repositorio != null &&
            clasificador != null &&
            vista != null &&
            registradorErrores != null)//Verifica que los datos y las dependencias estén asignados.
        {
            if (this.vista != null)//Verifica si existía una suscripción de una inicialización anterior.
            {
                this.vista.CuestionarioSeleccionado -= MostrarCuestionario;
            }

            this.cuestionarios = new List<KahootDatos>(cuestionarios);//Guarda una copia de los cuestionarios recibidos.
            this.repositorio = repositorio;
            this.clasificador = clasificador;
            this.vista = vista;
            this.registradorErrores = registradorErrores;//Guarda el gestor de errores recibido.

            this.vista.CuestionarioSeleccionado += MostrarCuestionario;//Suscribe el método que recibe los cambios de selección.

            List<string> titulos = new List<string>();//Crea la lista de títulos para el desplegable.

            foreach (KahootDatos cuestionario in this.cuestionarios)//Recorre los cuestionarios disponibles.
            {
                titulos.Add(cuestionario.titulo);//Añade el título de cada cuestionario.
            }

            this.vista.MostrarCuestionarios(titulos);//Rellena las opciones del desplegable.

            if (this.cuestionarios.Count > 0)//Verifica si hay algún cuestionario disponible.
            {
                MostrarCuestionario(0);//Carga los resultados del primer cuestionario.
            }
            else
            {
                this.vista.MostrarMensaje("No hi ha qüestionaris disponibles.");
            }
        }
        else
        {
            Debug.LogError(
                "Faltan datos o dependencias en ControladorClasificacion.",
                this
            );
        }
    }

    private void MostrarCuestionario(int indice)//Método que carga y muestra los resultados del cuestionario seleccionado.
    {
        if (indice >= 0 && indice < cuestionarios.Count)//Verifica que el índice pertenece a la lista de cuestionarios.
        {
            try
            {
                string idCuestionario = cuestionarios[indice].id;//Obtiene el ID del cuestionario seleccionado.

                List<ResultadoPartida> resultados =
                    repositorio.Cargar(idCuestionario);//Carga las partidas guardadas en el XML.

                List<ResultadoPartida> ordenados =
                    clasificador.Ordenar(resultados);//Ordena las partidas por puntos y tiempo.

                vista.MostrarResultados(ordenados);//Muestra la clasificación del cuestionario.
            }
            catch (Exception error)
            {
                vista.MostrarMensaje(
                    "No s'han pogut carregar els resultats."
                );//Retira los resultados anteriores y muestra el error.

                registradorErrores.Registrar(
                    "Leer los resultados XML del cuestionario " +
                    cuestionarios[indice].id,
                    error
                );//Registra el cuestionario cuyos resultados no se han podido cargar.

                vista.MostrarAvisoInformes("No s\'han pogut llegir els resultats XML.");
            }
        }
    }

    private void OnDestroy()//Elimina la suscripción cuando se destruye el controlador.
    {
        if (vista != null)
        {
            vista.CuestionarioSeleccionado -= MostrarCuestionario;
        }
    }
}