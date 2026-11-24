using System.Collections.Generic;

// Contrato para guardar y consultar los resultados de un cuestionario.
public interface IRepositorioResultados
{
    void Guardar(
        string idCuestionario,
        string jugador,
        int puntos,
        float segundos
    );

    List<ResultadoPartida> Cargar(string idCuestionario);
}