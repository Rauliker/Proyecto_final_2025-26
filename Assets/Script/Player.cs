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
    void Start()
    {
        EquiparArmasIniciales();
    }

    void EquiparArmasIniciales()
    {
        if (armas.Count == 0) return; // Si no hay armas, no hacemos nada

        Transform camTransform = transform.Find("Main Camera");
        Transform positionWeapon = camTransform.Find("Posicion Armas");

        // Creamos una lista temporal para reemplazar los originales por clones
        List<GameObject> armasClonadas = new List<GameObject>();

        foreach (GameObject arma in armas)
        {
            if (arma == null) continue;

            // Creamos un clon del arma
            GameObject armaClon = Instantiate(arma);

            // La equipamos
            armaClon.transform.SetParent(positionWeapon);
            armaClon.transform.SetPositionAndRotation(
                positionWeapon.position,
                positionWeapon.rotation * Quaternion.Euler(0f, 270f, 0f)
            );

            // Desactivamos el collider para que no interfiera
            armaClon.GetComponent<Collider>().enabled = false;

            armasClonadas.Add(armaClon);
        }

        // Reemplazamos la lista original por los clones
        armas = armasClonadas;

        Debug.Log("Armas iniciales equipadas correctamente (con clones).");
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
        GameObject armaOriginal = recoger.gameObject;

        // Creamos un clon en lugar de usar el original
        GameObject armaClon = Instantiate(armaOriginal);

        armas.Add(armaClon);

        Transform camTransform = transform.Find("Main Camera");
        Transform positionWeapon = camTransform.Find("Posicion Armas");

        armaClon.transform.SetParent(positionWeapon);
        armaClon.transform.SetPositionAndRotation(
            positionWeapon.position,
            positionWeapon.rotation * Quaternion.Euler(0f, 270f, 0f)
        );

        armaClon.GetComponent<Collider>().enabled = false;
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
