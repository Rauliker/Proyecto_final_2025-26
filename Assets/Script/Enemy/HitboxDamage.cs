using UnityEngine;

public class HitboxDamage : MonoBehaviour
{
    public GameObject enemy;
    public float damageMultiplier = 1f;

    private Enemy enemyScript;

    void Start()
    {
        // Si no está asignado manualmente, buscar el Enemy en el padre
        if (enemy == null)
        {
            Enemy padre = GetComponentInParent<Enemy>();

            if (padre != null)
            {
                enemy = padre.gameObject;
            }
            else
            {
                Debug.LogWarning("HitboxDamage: No se encontró Enemy en los padres");
            }
        }

        // Obtener el componente Enemy del GameObject asignado
        if (enemy != null)
        {
            enemyScript = enemy.GetComponent<Enemy>();
        }
    }

    public void Recibirdanio(int danio)
    {
        if (enemyScript == null)
        {
            Debug.LogWarning("Hitbox sin referencia válida al componente Enemy");
            return;
        }

        int finalDamage = Mathf.RoundToInt(danio * damageMultiplier);
        enemyScript.RecibirDanio(finalDamage);
    }
}
