using UnityEngine;
using UnityEngine.UI;

public class Barradevida : MonoBehaviour
{
    public Image barraVida;
    public Player player;
    public int vidaMaxima; 
    void Start()
    {
        player= GameObject.FindWithTag("Player").GetComponent<Player>();
        vidaMaxima = player.vida;
    }

    void Update()
    {
        barraVida.fillAmount = Mathf.Clamp01((float)player.vida / vidaMaxima);

    }
}
