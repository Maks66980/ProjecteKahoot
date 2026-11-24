using System;

// Recibe el resultado final y lo guarda para el jugador y el cuestionario de la partida.
public class GuardadorResultadosPartida
{
    private readonly IRepositorioResultados repositorio;
    private readonly string idCuestionario;
    private readonly string jugador;
    private readonly IRegistradorErrores registradorErrores;//Referencia al gestor de errores de guardado.

    public GuardadorResultadosPartida(
        IRepositorioResultados repositorio,
        string idCuestionario,
        string jugador,
        IRegistradorErrores registradorErrores)
    {
        this.repositorio = repositorio;
        this.idCuestionario = idCuestionario;
        this.jugador = jugador;
        this.registradorErrores = registradorErrores;
    }

    public void Guardar(PartidaTerminada partida)
    {
        try
        {
            repositorio.Guardar(idCuestionario, jugador, partida.Puntos, partida.Segundos);
        }
        catch (Exception error)
        {
            registradorErrores.Registrar(
                "Guardar los resultados XML del cuestionario " + idCuestionario,
                error
            );//Registra el fallo para poder consultarlo desde Informes.
        }
    }
}
