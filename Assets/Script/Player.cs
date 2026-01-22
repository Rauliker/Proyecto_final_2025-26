using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<GameObject> armas = new List<GameObject>();
    private Recoger objetoRecogible;
    private PlayerMovement3D movimiento;

    public BotonesConfig botones;

    public TextMeshProUGUI textAmmo;
    public TextMeshProUGUI textPoints;

    public GameObject positionWeapon;

    public Animator animator;
    

    public int points = 0;
    public int vida = 100;

    void Awake()
    {
        movimiento = GetComponent<PlayerMovement3D>();
        CargarBotones();
    }

    void Start()
    {
        EquiparArmasIniciales();
        ActualizarTextPoints();
    }

    void EquiparArmasIniciales()
    {
        if (armas.Count == 0) return;

        Transform camTransform = transform.Find("Main Camera");
        if (camTransform == null) return;

        Transform posArmas = camTransform.Find("Posicion Armas");
        if (posArmas == null) return;

        List<GameObject> armasClonadas = new List<GameObject>();

        foreach (GameObject arma in armas)
        {
            if (arma == null) continue;
            GameObject armaClon = Instantiate(arma);
            armaClon.transform.SetParent(posArmas);
            armaClon.transform.SetLocalPositionAndRotation(
                Vector3.zero,
                Quaternion.Euler(0f, 270f, 0f)
            );

            Recoger recoger = armaClon.GetComponent<Recoger>();
            if (recoger != null) recoger.enabled = false;
            

            armasClonadas.Add(armaClon);
        }

        armas = armasClonadas;
        Debug.Log("Armas iniciales equipadas.");
    }

    void CargarBotones()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("botones");
        if (jsonText != null)
        {
            botones = JsonUtility.FromJson<BotonesConfig>(jsonText.text);
        }
        else
        {
            Debug.LogError("No se pudo cargar botones.json");
        }
    }

    void Update()
    {
        Vector2 input = ObtenerInputMovimiento();
        bool jump = Input.GetKeyDown(ToKeyCode(botones.Saltar));
        bool sprint = Input.GetKey(ToKeyCode(botones.Correr));

        movimiento.Move(input, jump, sprint);
        ActualizarAnimaciones(input, sprint, jump);


        if (InputDisparo() && armas.Count > 0)
        {
            armas[0].GetComponent<Shoot>()?.Disparar();


        }

        if (Input.GetKeyDown(ToKeyCode(botones.Recargar)) && armas.Count > 0)
        {
            armas[0].GetComponent<Shoot>()?.Recargar();
        }

        if (Input.GetKeyDown(ToKeyCode(botones.Recoger)) && objetoRecogible != null)
        {
            RecogerArma(objetoRecogible);
        }
    }

    public void AnimationShoot()
    {
        animator.SetTrigger("ShootPistol");

    }

    bool InputDisparo()
    {
        if (botones.Disparar.StartsWith("Mouse"))
        {
            int boton = int.Parse(botones.Disparar.Replace("Mouse", ""));
            return Input.GetMouseButtonDown(boton);
        }

        return Input.GetKeyDown(ToKeyCode(botones.Disparar));
    }

    GameObject ObtenerArmaConTag(string tag)
    {
        foreach (GameObject arma in armas)
        {
            if (arma != null && arma.CompareTag(tag))
            {
                return arma;
            }
        }
        return null;
    }



    void RecogerArma(Recoger recoger)
    {
        GameObject armaExistente = ObtenerArmaConTag(recoger.gameObject.tag);

        if (armaExistente != null)
        {
            if ((points - recoger.puntos)<0) return;
            Shoot armaExistenteDisparo = armaExistente.GetComponent<Shoot>();
            int totalmunicion= armaExistenteDisparo.ammo + recoger.ammo;
            if (totalmunicion >= armaExistenteDisparo.maxAmmo)
            {
                totalmunicion = armaExistenteDisparo.maxAmmo;
            }
            armaExistenteDisparo.ammo = totalmunicion;

            armaExistenteDisparo.ActualizarTexto();
            AddPoints(-recoger.puntos);
            return;
        }


        recoger.ActualizarTieneArma();

        GameObject armaClon = Instantiate(recoger.gameObject);
        armaClon.transform.SetParent(positionWeapon.transform);
        armaClon.transform.localPosition = Vector3.zero;
        armaClon.transform.localRotation = Quaternion.identity;
        armaClon.transform.localScale = Vector3.one * 0.03f;

        Collider col = armaClon.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        recoger.enabled = false;

        armas.Add(armaClon);
        objetoRecogible = null;
        textAmmo.enabled = true;
    }

    Vector2 ObtenerInputMovimiento()
    {
        float x = 0, z = 0;

        if (Input.GetKey(ToKeyCode(botones.MoverArriba))) z++;
        if (Input.GetKey(ToKeyCode(botones.MoverAbajo))) z--;
        if (Input.GetKey(ToKeyCode(botones.MoverDerecha))) x++;
        if (Input.GetKey(ToKeyCode(botones.MoverIzquierda))) x--;

        return new Vector2(x, z).normalized;
    }

    KeyCode ToKeyCode(string tecla)
    {
        if (System.Enum.TryParse(tecla, true, out KeyCode key))
            return key;

        Debug.LogError("Tecla inválida: " + tecla);
        return KeyCode.None;
    }

    public IEnumerator RecibirDanio(int danio, float delay)
    {


        yield return new WaitForSeconds(delay);
        vida -= danio;

        if (vida <= 0)
        {
            EndLevel.Instance.FinishGame(points);

        }
    }

    void ActualizarAnimaciones(Vector2 input, bool isSprinting, bool isJumping)
    {
        float speed = input.magnitude;

        animator.SetFloat("Speed", speed);
        animator.SetBool("Jumping", isJumping);
        animator.SetBool("IsRunning", isSprinting && speed > 0.1f);
    }


    public void AddPoints(int puntos)
    {
        points += puntos;
        ActualizarTextPoints();
    }

    void ActualizarTextPoints()
    {
        if (textPoints != null)
        {
            textPoints.text = LocalizationManager.Instance.GetTranslation("PUNTOS") + ": " + points;
        }
    }

    public void SetObjetoRecogible(Recoger recoger) => objetoRecogible = recoger;

    public void ClearObjetoRecogible(Recoger recoger)
    {
        if (objetoRecogible == recoger)
            objetoRecogible = null;
    }
}
