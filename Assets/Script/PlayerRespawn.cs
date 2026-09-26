using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerRespawn : MonoBehaviour
{
    [Header("Caida / muerte")]
    public float alturaMinima = -10f; //si el jugador cae por debajo de esta Y, respawnea

    [Header("Inicio de nivel")]
    public string tagInicioNivel = "InicioNivel"; //tag del GameObject que marca donde arranca el nivel

    private CharacterController controller;
    private Vector3 posicionCheckpoint;
    private Quaternion rotacionCheckpoint;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        
        GameObject inicioNivel = GameObject.FindGameObjectWithTag(tagInicioNivel);

        if (inicioNivel != null)
        {
            posicionCheckpoint = inicioNivel.transform.position;
            rotacionCheckpoint = inicioNivel.transform.rotation;
            Respawnear(); //posiciona al jugador ahi apenas arranca la escena
        }
        else
        {
            //Si no hay marcador, usa la posicion inicial del jugador como antes
            posicionCheckpoint = transform.position;
            rotacionCheckpoint = transform.rotation;
        }
    }

    void Update()
    {
        if (transform.position.y < alturaMinima)
        {
            Respawnear();
        }
    }

    public void SetCheckpoint(Vector3 posicion, Quaternion rotacion)
    {
        posicionCheckpoint = posicion;
        rotacionCheckpoint = rotacion;
    }

    public void Respawnear()
    {
        
        controller.enabled = false;
        transform.position = posicionCheckpoint;
        transform.rotation = rotacionCheckpoint;
        controller.enabled = true;
    }
}