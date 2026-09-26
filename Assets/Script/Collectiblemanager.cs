using UnityEngine;
using TMPro; // si usas Texto UI clasico (no TextMeshPro), cambia esto por: using UnityEngine.UI;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instancia { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI textoContador; // si usas Text clasico, cambia el tipo por "Text"
    public string formato = "Coleccionables: {0} / {1}";

    [Header("Configuracion")]
    public int totalColeccionables = 0; // si lo dejas en 0, se calcula automaticamente al iniciar

    private int coleccionablesRecogidos = 0;

    void Awake()
    {
        // Patron singleton simple: solo puede existir un CollectibleManager en la escena
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
    }

    void Start()
    {
        if (totalColeccionables <= 0)
        {
            totalColeccionables = FindObjectsByType<Collectible>(FindObjectsSortMode.None).Length;
        }

        ActualizarUI();
    }

    public void RecogerColeccionable()
    {
        coleccionablesRecogidos++;
        ActualizarUI();
    }

    void ActualizarUI()
    {
        if (textoContador != null)
        {
            textoContador.text = string.Format(formato, coleccionablesRecogidos, totalColeccionables);
        }
    }

    public int CantidadRecogida => coleccionablesRecogidos;
}