using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<GameObject> armas = new List<GameObject>();
    private Recoger objetoRecogible;
    private PlayerMovement3D movimiento;

    public BotonesConfig botones;

    public int vida = 100;

    void Awake()
    {
        movimiento = GetComponent<PlayerMovement3D>();
        CargarBotones();
    }

    void CargarBotones()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("botones");
        if (jsonText != null)
        {
            botones = JsonUtility.FromJson<BotonesConfig>(jsonText.text);
            Debug.Log("Botones cargados correctamente " + botones);
        }
        else
        {
            Debug.LogError("No se pudo cargar el archivo botones.json");
        }
    }

    void Update()
    {
        // Movimiento
        Vector2 input = ObtenerInputMovimiento();
        bool jump = Input.GetKeyDown(ToKeyCode(botones.Saltar));
        movimiento.Move(input, jump);

        // Disparar
        if (botones.Disparar.StartsWith("Mouse"))
        {
            int botonMouse = int.Parse(botones.Disparar.Substring(5));
            if (Input.GetMouseButtonDown(botonMouse) && armas.Count > 0)
                armas[0].GetComponent<Shoot>()?.Disparar();
        }
        else
        {
            if (Input.GetKeyDown(ToKeyCode(botones.Disparar)) && armas.Count > 0)
                armas[0].GetComponent<Shoot>()?.Disparar();
        }

        // Recoger arma
        if (Input.GetKeyDown(ToKeyCode(botones.Recoger)) && objetoRecogible != null)
        {
            RecogerArma(objetoRecogible);
        }
    }

    public void SetObjetoRecogible(Recoger recoger)
    {
        objetoRecogible = recoger;
    }

    public void ClearObjetoRecogible(Recoger recoger)
    {
        if (objetoRecogible == recoger)
            objetoRecogible = null;
    }

    void RecogerArma(Recoger recoger)
    {
        GameObject arma = recoger.gameObject;
        armas.Add(arma);

        Transform camTransform = transform.Find("Main Camera");
        Transform positionWeapon = camTransform.Find("Posicion Armas");

        arma.transform.SetParent(positionWeapon);
        arma.transform.SetPositionAndRotation(
            positionWeapon.position,
            positionWeapon.rotation * Quaternion.Euler(0f, 270f, 0f)
        );

        arma.GetComponent<Collider>().enabled = false;
        objetoRecogible = null;
    }

    Vector2 ObtenerInputMovimiento()
    {
        float x = 0f, z = 0f;
        if (Input.GetKey(ToKeyCode(botones.MoverArriba))) z += 1f;
        if (Input.GetKey(ToKeyCode(botones.MoverAbajo))) z -= 1f;
        if (Input.GetKey(ToKeyCode(botones.MoverDerecha))) x += 1f;
        if (Input.GetKey(ToKeyCode(botones.MoverIzquierda))) x -= 1f;
        return new Vector2(x, z).normalized;
    }

    KeyCode ToKeyCode(string tecla)
    {
        try
        {
            return (KeyCode)System.Enum.Parse(typeof(KeyCode), tecla, true);
        }
        catch
        {
            Debug.LogError("Tecla inválida: " + tecla);
            return KeyCode.None;
        }
    }

    public void RecibirDanio(int danio)
    {
        vida -= danio;
        Debug.Log("Diana golpeada. Vida restante: " + vida);

        if (vida <= 0)
        {
            Destruir();
        }
    }

    void Destruir()
    {
        Debug.Log("Diana destruida");
        Destroy(gameObject);
    }

}
