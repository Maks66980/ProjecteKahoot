using System;
using UnityEngine;

public class GestorErrores : IRegistradorErrores//Clase que muestra los errores en la consola y solicita su informe.
{
    private readonly IRegistradorErrores registrador;//Referencia al registrador que guarda los informes.

    public GestorErrores(IRegistradorErrores registrador)//Recibe el registrador utilizado para guardar los errores.
    {
        this.registrador = registrador;
    }

    public void Registrar(string operacion, Exception error)//Método que informa del error y solicita su guardado.
    {
        if (error == null)//Verifica que se haya recibido una excepción.
        {
            throw new ArgumentNullException(nameof(error));
        }

        Debug.LogError(operacion + ": " + error.Message);//Muestra el error original en la consola.

        try
        {
            registrador.Registrar(operacion, error);//Guarda el informe con los datos de la excepción.
        }
        catch (Exception errorInforme)
        {
            Debug.LogError(
                "No se pudo guardar el informe: " + errorInforme.Message
            );//Informa del fallo de escritura y permite continuar la aplicación.
        }
    }
}