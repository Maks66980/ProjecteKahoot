using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfiguradorClasificacion : MonoBehaviour//Clase que prepara los repositorios e inicia la clasificación.
{
    [SerializeField] private VistaClasificacion vista;//Referencia a la vista de la clasificación.
    [SerializeField] private ControladorClasificacion controlador;//Referencia al controlador de la clasificación.

    private IRegistradorErrores registradorErrores;//Referencia al gestor que registra los errores de la clasificación.

    private void Start()//Prepara los cuestionarios y las dependencias al entrar en la escena.
    {
        if (vista != null && controlador != null)//Verifica que la vista y el controlador estén asignados.
        {
            string carpetaOrigen = Path.Combine(
                Application.streamingAssetsPath,
                "Kahoots"
            );//Obtiene la carpeta de los cuestionarios iniciales.

            string carpetaCuestionarios = Path.Combine(
                Application.persistentDataPath,
                "Kahoots"
            );//Obtiene la carpeta de los cuestionarios del usuario.

            string carpetaResultados = Path.Combine(
                Application.persistentDataPath,
                "Resultats"
            );//Obtiene la carpeta donde se guardan los resultados XML.

            string carpetaInformes = Path.Combine(
                Application.persistentDataPath,
                "Informes"
            );//Obtiene la carpeta de informes.

            registradorErrores = new GestorErrores(
                new RegistradorErroresTexto(carpetaInformes)
            );//Prepara el gestor utilizado para registrar los errores.

            PreparadorCarpetaKahoots preparador = new PreparadorCarpetaKahoots();//Crea el preparador de la carpeta de cuestionarios.

            try
            {
                preparador.Preparar(
                    carpetaOrigen,
                    carpetaCuestionarios
                );//Copia los cuestionarios iniciales que todavía faltan.
            }
            catch (Exception error)
            {
                registradorErrores.Registrar(
                    "Preparar la carpeta de cuestionarios para la clasificación",
                    error
                );//Registra el fallo al preparar los cuestionarios.

                vista.MostrarAvisoInformes("S\'ha produït un error amb els qüestionaris.");
            }

            try
            {
                IRepositorioKahoots repositorioCuestionarios =
                    new RepositorioKahootsJson(
                        carpetaCuestionarios,
                        new LectorKahoots(),
                        new ValidadorKahoots()
                    );//Prepara el repositorio que lee y valida los cuestionarios.

                IRepositorioResultados repositorioResultados =
                    new RepositorioResultadosXml(
                        carpetaResultados
                    );//Prepara el repositorio que carga los resultados XML.

                ClasificadorResultados clasificador =
                    new ClasificadorResultados();//Crea el clasificador que ordena los puntos y los tiempos.

                List<KahootDatos> cuestionarios =
                    CargarCuestionarios(repositorioCuestionarios);//Carga los cuestionarios disponibles.

                controlador.Iniciar(
                    cuestionarios,
                    repositorioResultados,
                    clasificador,
                    vista,
                    registradorErrores
                );//Entrega al controlador las dependencias y el gestor de errores.
            }
            catch (Exception error)
            {
                vista.MostrarMensaje(
                    "No s'ha pogut preparar la classificació."
                );

                registradorErrores.Registrar(
                    "Preparar la clasificación",
                    error
                );//Registra el fallo al cargar las dependencias o los cuestionarios.

                vista.MostrarAvisoInformes("Consulta els informes per veure el detall de l\'error.");
            }
        }
        else
        {
            Debug.LogError(
                "Asigna la vista y el controlador en ConfiguradorClasificacion.",
                this
            );
        }
    }

    private List<KahootDatos> CargarCuestionarios(
        IRepositorioKahoots repositorio)//Método que obtiene los cuestionarios válidos del repositorio.
    {
        List<KahootDatos> cuestionarios =
            new List<KahootDatos>();//Crea la lista de cuestionarios disponibles.

        foreach (string ruta in repositorio.ObtenerRutas())//Recorre los archivos de cuestionarios.
        {
            try
            {
                KahootDatos cuestionario =
                    repositorio.Cargar(ruta);//Lee y valida el cuestionario.

                cuestionarios.Add(cuestionario);//Añade el cuestionario válido a la lista.
            }
            catch (Exception error)
            {
                registradorErrores.Registrar(
                    "Leer el cuestionario " + ruta,
                    error
                );//Registra el archivo incorrecto y permite cargar los siguientes.

                vista.MostrarAvisoInformes("Hi ha qüestionaris JSON que no s\'han pogut llegir.");
            }
        }

        return cuestionarios;
    }
}