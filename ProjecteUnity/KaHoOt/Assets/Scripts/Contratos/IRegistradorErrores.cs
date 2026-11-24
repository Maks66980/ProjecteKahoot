using System;

public interface IRegistradorErrores//Define un contrato para guardar los errores de la aplicación.
{
    void Registrar(string operacion, Exception error);
}