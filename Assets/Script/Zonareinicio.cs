using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class ZonaReinicio : MonoBehaviour
{
    [Header("Configuracion")]
    public string tagJugador = "Player";

    void Reset()
    {
        
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tagJugador)) return;

        
        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.buildIndex);
    }
}
