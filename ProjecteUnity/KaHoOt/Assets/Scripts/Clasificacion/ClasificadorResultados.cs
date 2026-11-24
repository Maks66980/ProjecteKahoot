using System.Collections.Generic;

public class ClasificadorResultados//Clase que ordena los resultados de las partidas por puntos y tiempo.
{
    public List<ResultadoPartida> Ordenar(List<ResultadoPartida> resultados)//Método que devuelve una copia ordenada de los resultados.
    {
        List<ResultadoPartida> ordenados = new List<ResultadoPartida>(resultados);//Crea una copia de la lista recibida.

        ordenados.Sort(CompararResultados);//Ordena la lista utilizando la comparación de puntos y tiempo.

        return ordenados;
    }

    private int CompararResultados(ResultadoPartida primero,ResultadoPartida segundo)//Método que compara dos resultados para determinar su posición.
    {
        int comparacion = segundo.Puntos.CompareTo(primero.Puntos);//Compara los puntos para colocar primero la puntuación más alta.

        if (comparacion == 0)//Verifica si las dos partidas tienen la misma puntuación.
        {
            comparacion = primero.Segundos.CompareTo(segundo.Segundos);//Compara los tiempos para colocar primero el menor.
        }

        return comparacion;
    }
}