using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    [Header("Configuracion")]
    public string tagJugador = "Player";

    [Header("Efectos opcionales")]
    public GameObject efectoActivacion;

    private bool yaActivado = false;

    void Reset()
    {
        
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tagJugador)) return;

        PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
        if (respawn == null) return;

        respawn.SetCheckpoint(transform.position, transform.rotation);

        if (!yaActivado && efectoActivacion != null)
        {
            Instantiate(efectoActivacion, transform.position, Quaternion.identity);
        }

        yaActivado = true;
    }
}

