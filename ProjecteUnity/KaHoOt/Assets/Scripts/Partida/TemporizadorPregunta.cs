using System;
using System.Collections;
using UnityEngine;

// Mide el tiempo real de la pregunta y comunica cada intervalo transcurrido.
public class TemporizadorPregunta : MonoBehaviour
{
    private Coroutine cuentaAtras;
    private Action<float> alMedir;
    private double ultimaMedicion;

    public void Iniciar(Func<float> leerTiempoRestante, Action<float> alMedir)
    {
        Detener();
        this.alMedir = alMedir;
        ultimaMedicion = Time.realtimeSinceStartupAsDouble;
        cuentaAtras = StartCoroutine(ContarTiempo(leerTiempoRestante));
    }

    public void Medir()
    {
        if (alMedir != null)
        {
            // Descontamos el tiempo real transcurrido entre dos mediciones.
            double ahora = Time.realtimeSinceStartupAsDouble;
            float transcurrido = (float)(ahora - ultimaMedicion);
            ultimaMedicion = ahora;

            alMedir(transcurrido);
        }
    }

    public void Detener()
    {
        if (cuentaAtras != null)
        {
            StopCoroutine(cuentaAtras);
            cuentaAtras = null;
        }

        alMedir = null;
    }

    private IEnumerator ContarTiempo(Func<float> leerTiempoRestante)
    {
        float restante = leerTiempoRestante();

        while (restante > 0f && alMedir != null)
        {
            // Ajustamos la última espera al tiempo que queda en la pregunta.
            float espera = Mathf.Min(1f, restante);
            yield return new WaitForSecondsRealtime(espera);
            Medir();
            restante = leerTiempoRestante();
        }

        cuentaAtras = null;
        alMedir = null;
    }

    private void OnDisable()
    {
        Detener();
    }
}
