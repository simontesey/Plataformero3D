using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Puntos de recorrido")]
    public Transform puntoA;
    public Transform puntoB;

    [Header("Configuracion")]
    public float velocidad = 2f;
    public bool empezarEnA = true;

    private Vector3 destino;

    void Start()
    {
        if (puntoA == null || puntoB == null)
        {
            Debug.LogWarning("MovingPlatform: asigna puntoA y puntoB en el Inspector.");
            return;
        }

        transform.position = empezarEnA ? puntoA.position : puntoB.position;
        destino = empezarEnA ? puntoB.position : puntoA.position;
    }

    void Update()
    {
        if (puntoA == null || puntoB == null) return;

        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);

        if (Vector3.Distance(transform.position, destino) < 0.05f)
        {
            destino = (destino == puntoA.position) ? puntoB.position : puntoA.position;
        }
    }
}