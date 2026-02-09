using UnityEngine;
using static UnityEditor.ShaderData;

public class MenudePausa : MonoBehaviour
{
    public GameObject BotonPausa;
    private bool pausa = false;

    public GameObject menu;
    public GameObject UIDefault;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            Pausa();
        }
    }
    public void Pausa()
    {
        pausa = !pausa;
        Time.timeScale = pausa ? 0f : 1f;

        UIDefault.SetActive(!pausa);
        menu.SetActive(pausa);

        Cursor.visible = pausa;
        Cursor.lockState = pausa ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
