using UnityEngine;
using UnityEngine.SceneManagement;

// Cambia de escena y permite salir de la aplicación.
public class NavegacionEscenas : MonoBehaviour
{
    public void CambiarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void Salir()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }
}