using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public string nombre;
    void Start()
    {
        Invoke("CargarEscena", 4f); // espera 2 segundos
    }

    void CargarEscena()
    {
        SceneManager.LoadScene(nombre, LoadSceneMode.Single);
    }
}
