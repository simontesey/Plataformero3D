using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectible : MonoBehaviour
{
    [Header("Configuracion")]
    public string tagJugador = "Player";

    [Header("Efectos opcionales")]
    public GameObject efectoRecoleccion; // opcional: particulas/sonido al recogerlo
    public AudioClip sonidoRecoleccion;

    [Header("Animacion simple (opcional)")]
    public bool rotarConstantemente = true;
    public float velocidadRotacion = 90f; // grados por segundo

    void Reset()
    {
        // Se asegura de que el collider sea un Trigger al agregar el script
        GetComponent<Collider>().isTrigger = true;
    }

    void Update()
    {
        if (rotarConstantemente)
        {
            transform.Rotate(0f, velocidadRotacion * Time.deltaTime, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tagJugador)) return;

        if (CollectibleManager.Instancia != null)
        {
            CollectibleManager.Instancia.RecogerColeccionable();
        }
        else
        {
            Debug.LogWarning("Collectible: no se encontro un CollectibleManager en la escena.");
        }

        if (efectoRecoleccion != null)
        {
            Instantiate(efectoRecoleccion, transform.position, Quaternion.identity);
        }

        if (sonidoRecoleccion != null)
        {
            AudioSource.PlayClipAtPoint(sonidoRecoleccion, transform.position);
        }

        Destroy(gameObject);
    }
}