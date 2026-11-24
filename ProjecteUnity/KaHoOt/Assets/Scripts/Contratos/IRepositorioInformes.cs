public interface IRepositorioInformes//Define un contrato para consultar los informes guardados.
{
    string[] ObtenerRutas();//Obtiene las rutas de los informes disponibles.

    string Leer(string ruta);//Obtiene el contenido completo del informe indicado.
}