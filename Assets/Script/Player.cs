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
    private Teleport teleport;

    public bool Fps;
    public Transform camaraFPS;
    public Transform camaraTPS;
    public Transform armasFPS;
    public GameObject model;
    public BotonesConfig botones;

    public TextMeshProUGUI textAmmo;
    public TextMeshProUGUI textPoints;

    public GameObject positionWeapon;

    public Animator animator;

    private int armaActualIndex = 0;

    public GameObject UIDefault;

    public GameObject Opciones;

    [Header("Apuntar")]
    public Camera playerCamera;
    public float normalFOV = 60f;
    public float aimFOV = 35f;
    public float aimSpeed = 8f;

    public bool apuntando = false;


    public int points = 0;
    public int vida = 100;
    private bool pausa=false;
    public static Player Instance; // Singleton

    public Image sangre;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // evitar duplicados
            return;
        }
        Instance = this;

        movimiento = GetComponent<PlayerMovement3D>();
        CargarBotones();
        Fps = false;
    }

    void Start()
    {
        sangre.enabled = false;
        UIDefault.SetActive(true);
        Opciones.SetActive(false);  
        apuntando = false;
        EquiparArmasIniciales();
        ActualizarTextPoints();
    }

    void EquiparArma(int index)
    {
        if (armas.Count == 0) return;

        // Limitar índice
        armaActualIndex = Mathf.Clamp(index, 0, armas.Count - 1);

        // Activar solo el arma seleccionada
        for (int i = 0; i < armas.Count; i++)
        {
            if (armas[i] != null)
                armas[i].SetActive(i == armaActualIndex);
        }

        // Actualizar UI de munición
        Shoot shoot = armas[armaActualIndex].GetComponent<Shoot>();
        if (shoot != null)
            shoot.ActualizarTexto();

        if (armas[armaActualIndex].GetComponent<Recoger>().tipoArma == TiposArmas.FUSIL)
        {

            animator.SetBool("haveRifle", true);
        }
        else
        {
            animator.SetBool("haveRifle", false);

        }
        armas[armaActualIndex].GetComponent<Shoot>().CancelarRecarga();
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
            //if (!armaClon) null;

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pausa();
        }
        if (pausa) return;
        if (Input.GetKeyDown(KeyCode.P))
        {
            FristPerson();
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            armaActualIndex++;
            if (armaActualIndex >= armas.Count) armaActualIndex = 0;
            EquiparArma(armaActualIndex);
        }
        else if (scroll < 0f)
        {
            armaActualIndex--;
            if (armaActualIndex < 0) armaActualIndex = armas.Count - 1;
            EquiparArma(armaActualIndex);
        }

        Vector2 input = ObtenerInputMovimiento();
        bool jump = Input.GetKeyDown(ToKeyCode(botones.Saltar));
        bool sprint = Input.GetKey(ToKeyCode(botones.Correr));

        movimiento.Move(input, jump, sprint);
        ActualizarAnimaciones(input, sprint, jump);
        
        if (armas.Count > 0)
        {
            apuntando = InputApuntar();
            ActualizarApuntado();

            
            Shoot shoot = armas[armaActualIndex].GetComponent<Shoot>();

            if (shoot.fireMode == Shoot.FireMode.Single)
            {
                if (InputDownDisparo())
                    shoot.Disparar();
            }
            else
            {
     
                if (InputDownDisparo())
                {
                    shoot.StartShooting();
                }

                if (InputUpDisparo())
                {
                    shoot.StopShooting();
                }
            }
        }

        if (Input.GetKeyDown(ToKeyCode(botones.Recargar)) && armas.Count > 0)
        {
            armas[armaActualIndex].GetComponent<Shoot>()?.Recargar();
        }

        if (Input.GetKeyDown(ToKeyCode(botones.Recoger)) && objetoRecogible != null)
        {
            RecogerArma(objetoRecogible);
        }

        if (Input.GetKeyDown(ToKeyCode(botones.Anadir)) && objetoRecogible != null)
        {
            AumentarDano(objetoRecogible);
        }
    }

    public void FristPerson()
    {
        // Activar o desactivar el modelo del jugador
        model.SetActive(Fps);

        // Cambiar posición de cámara
        if (Fps!=true)
        {
            Fps = true;
            // Volver a primera persona
            playerCamera.transform.SetParent(camaraFPS);
            playerCamera.transform.localPosition = Vector3.zero;
            playerCamera.transform.localRotation = Quaternion.identity;
            //armasFPS.SetParent(playerCamera.transform);

            // Mover armas a posición FPS
            foreach (var arma in armas)
            {
                arma.transform.SetParent(armasFPS);
                arma.transform.localPosition = Vector3.zero;
                arma.transform.localRotation = Quaternion.Euler(0, 270, 0);
                Recoger recogerArma = arma.GetComponent<Recoger>();
                if (recogerArma.invertir)
                {
                    arma.transform.localRotation *= Quaternion.Euler(0, 180f, 0);
                }

            }
        }
        else
        {
            Fps = false;
            // Pasar a tercera persona
            playerCamera.transform.SetParent(camaraTPS);
            playerCamera.transform.localPosition = Vector3.zero;
            playerCamera.transform.localRotation = Quaternion.identity;

            // Mover armas a posición TPS
            foreach (var arma in armas)
            {
                arma.transform.SetParent(positionWeapon.transform);
                arma.transform.localPosition = Vector3.zero;
                arma.transform.localRotation = Quaternion.identity;
                Recoger recogerArma = arma.GetComponent<Recoger>();
                if (recogerArma.invertir)
                {
                    arma.transform.localRotation *= Quaternion.Euler(0, 180f, 0);
                }
            }
        }
    }

    public void Pausa()
    {
        pausa = !pausa;

        Time.timeScale = pausa ? 0f : 1f;

        UIDefault.SetActive(!pausa);
        Opciones.SetActive(pausa);

        Cursor.visible = pausa;
        Cursor.lockState = pausa ? CursorLockMode.None : CursorLockMode.Locked;
    }

    void ActualizarApuntado()
    {
        if (playerCamera == null) return;

        float objetivoFOV = apuntando ? aimFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            objetivoFOV,
            Time.deltaTime * aimSpeed
        );
        animator.SetBool("IsAim", apuntando);
    }

    public void AnimationShoot(TiposArmas tipo)
    {
        if (TiposArmas.PISTOLA==tipo)
        {
            animator.SetTrigger("ShootPistol");
        }
        if (TiposArmas.FUSIL == tipo)
        {
            animator.SetTrigger("ShootFusil");
        }

    }

    bool InputApuntar()
    {
        if (botones.Apuntar.StartsWith("Mouse"))
        {
            int boton = int.Parse(botones.Apuntar.Replace("Mouse", ""));
            return Input.GetMouseButton(boton);
        }

        return Input.GetKey(ToKeyCode(botones.Disparar));
    }

    bool InputDisparo()
    {
        if (botones.Disparar.StartsWith("Mouse"))
        {
            int boton = int.Parse(botones.Disparar.Replace("Mouse", ""));
            return Input.GetMouseButton(boton); 
        }

        return Input.GetKey(ToKeyCode(botones.Disparar)); 
    }
    bool InputDownDisparo()
    {
        if (botones.Disparar.StartsWith("Mouse"))
        {
            int boton = int.Parse(botones.Disparar.Replace("Mouse", ""));
            return Input.GetMouseButtonDown(boton);
        }

        return Input.GetKey(ToKeyCode(botones.Disparar));
    }
    bool InputUpDisparo()
    {
        if (botones.Disparar.StartsWith("Mouse"))
        {
            int boton = int.Parse(botones.Disparar.Replace("Mouse", ""));
            return Input.GetMouseButtonUp(boton);
        }

        return Input.GetKey(ToKeyCode(botones.Disparar));
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

            Shoot armaExistenteDisparo = armaExistente.GetComponent<Shoot>();
            if ((points - recoger.puntos)<0 || armaExistenteDisparo.ammo== armaExistenteDisparo.maxAmmo) return;
            int totalmunicion = armaExistenteDisparo.ammo + 10;

            if (totalmunicion >= armaExistenteDisparo.maxAmmo)
            {
                totalmunicion = armaExistenteDisparo.maxAmmo;
            }

            armaExistenteDisparo.ammo = totalmunicion;


            armaExistenteDisparo.ActualizarTexto();
            AddPoints(-recoger.puntos);
            recoger.puntosAumentar += 10;
            //armaExistente.GetComponent<Teleport>().SumarCantidad();
            recoger.GetComponent<Teleport>().SumarCantidad();



            return;
        }


        recoger.ActualizarTieneArma();
        recoger.gameObject.GetComponent<Collider>().enabled = false;
        recoger.gameObject.GetComponent<Collider>().enabled = true;

        GameObject armaClon = Instantiate(recoger.gameObject);
        armaClon.transform.SetParent(positionWeapon.transform);
        AplicarPosicionYRotacionArma(armaClon);
        Recoger recogerArma = armaClon.GetComponent<Recoger>();

        float offsetX = 0f;

        offsetX = recogerArma.posocionMango.localPosition.x;
        armaClon.transform.localPosition = new Vector3(
            -offsetX,
            0f,
            0f
        );
        

        if (recogerArma.invertir)
        {
            armaClon.transform.localRotation *= Quaternion.Euler(0, 180f, 0);
        }

        Collider col = armaClon.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        recoger.enabled = false;

        armas.Add(armaClon);
        objetoRecogible = null;
        textAmmo.enabled = true;
        EquiparArma(armaActualIndex);
    }
    void AplicarPosicionYRotacionArma(GameObject arma)
    {
        Recoger recoger = arma.GetComponent<Recoger>();

        // El padre SIEMPRE es TPS
        arma.transform.SetParent(positionWeapon.transform);

        if (Fps!=true)
        {
            // POSICIÓN FPS
            arma.transform.localPosition = armasFPS.localPosition;

            // ROTACIÓN FPS ORIGINAL
            arma.transform.localRotation = Quaternion.Euler(0, 0, 0);

            // Ajuste del mango
            if (recoger != null && recoger.posocionMango != null)
            {
                float offsetX = recoger.posocionMango.localPosition.x;
                arma.transform.localPosition += new Vector3(-offsetX, 0, 0);
            }
        }
        else
        {
            arma.transform.SetParent(armasFPS);
            arma.transform.localPosition = Vector3.zero;
            arma.transform.localRotation = Quaternion.Euler(0, 270, 0);
        }
    }


    void AumentarDano(Recoger recoger)
    {
        GameObject armaExistente = ObtenerArmaConTag(recoger.gameObject.tag);

        if (armaExistente != null)
        {
            if (points < recoger.puntosAumentar) return; // puntos suficientes?

            Shoot armaExistenteDisparo = armaExistente.GetComponent<Shoot>();
            // Aumentar daño según el valor del objeto recogible
            armaExistenteDisparo.damage += recoger.aumentoDano;

            // Restar los puntos del jugador
            AddPoints(-recoger.puntosAumentar);

            // Opcional: incrementar la "capacidad" de mejora del objeto recogible
            recoger.puntosAumentar += 10;
            recoger.GetComponent<Teleport>().SumarCantidad();
            ActualizarTextPoints();
            return;
        }

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

        } else 
        {
            sangre.enabled = true;
            yield return new WaitForSeconds(0.5f);
            sangre.enabled = false;
        }

        yield break;
    }

    void ActualizarAnimaciones(Vector2 input, bool isSprinting, bool isJumping)
    {
        float speed = input.magnitude;

        animator.SetFloat("Speed", speed);
        //animator.SetBool("Jumping", isJumping);
        animator.SetBool("IsRunning", isSprinting && speed > 0.1f);
        animator.SetBool("IsAim", apuntando);

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
