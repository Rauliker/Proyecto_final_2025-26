using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private List<Transform> teleportPoints = new();
    [SerializeField] private int cantidad = 5;

    private int contador = 0;
    private Transform ultimoTeleport;

    private void Start()
    {
        // Añade la posición inicial como punto de teletransporte
        if (!teleportPoints.Contains(transform))
            teleportPoints.Add(transform);

        ultimoTeleport = transform;
    }

    public void SumarCantidad()
    {
        if (teleportPoints.Count == 0) return;

        // Sumar una llamada
        contador++;

        // Si aún no llegó al límite, no teletransporta
        if (contador < cantidad) return;

        // Reiniciar contador
        contador = 0;

        // Si solo hay un punto, teletransporta sin comprobar
        if (teleportPoints.Count == 1)
        {
            transform.position = teleportPoints[0].position;
            ultimoTeleport = teleportPoints[0];
            return;
        }

        // Elegir un punto distinto al último
        Transform destino;
        do
        {
            destino = teleportPoints[Random.Range(0, teleportPoints.Count)];
        }
        while (destino == ultimoTeleport);

        // Teletransportar
        transform.position = destino.position;
        ultimoTeleport = destino;
    }


}
