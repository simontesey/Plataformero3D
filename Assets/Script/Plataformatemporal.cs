using UnityEngine;

public class PlataformaTemporal : MonoBehaviour
{
    [Header("Tiempos (segundos)")]
    public float tiempoVisible = 7f;
    public float tiempoOculta = 3f;

    [Header("Efectos opcionales")]
    public GameObject efectoDesaparicion; 
    public GameObject efectoReaparicion;  

    private Renderer[] renderers;
    private Collider[] colliders;
    private bool estaVisible = true;

    void Start()
    {
        // no se usa Destroy(gameObject) porque eso eliminaria el objeto para siempre.
        // se desactivan el/los Renderer y Collider para simular que "desaparece"
        // y despues se reactivan para que "reaparezca" en el mismo lugar.
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();

        Invoke(nameof(Desaparecer), tiempoVisible);
    }

    void Desaparecer()
    {
        estaVisible = false;
        SetActivo(false);

        if (efectoDesaparicion != null)
            Instantiate(efectoDesaparicion, transform.position, Quaternion.identity);

        Invoke(nameof(Reaparecer), tiempoOculta);
    }

    void Reaparecer()
    {
        estaVisible = true;
        SetActivo(true);

        if (efectoReaparicion != null)
            Instantiate(efectoReaparicion, transform.position, Quaternion.identity);

        Invoke(nameof(Desaparecer), tiempoVisible);
    }

    void SetActivo(bool activo)
    {
        foreach (Renderer r in renderers) r.enabled = activo;
        foreach (Collider c in colliders) c.enabled = activo;
    }
}