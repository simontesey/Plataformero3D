using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform jugador; 

    [Header("Suavizado")]
    public float suavizadoPosicion = 8f; 

    [Header("Rotacion con click derecho")]
    public float sensibilidadMouse = 3f;
    public float limiteAnguloMinimo = -20f; 
    public float limiteAnguloMaximo = 70f;  

    private float distancia;
    private float yaw;   
    private float pitch; 

    void Start()
    {
        if (jugador == null)
        {
            Debug.LogWarning("CameraFollow: falta asignar el Transform del jugador.");
            return;
        }

        
        Vector3 offset = transform.position - jugador.position;
        distancia = offset.magnitude;

        Vector3 direccion = offset.normalized;
        pitch = Mathf.Asin(direccion.y) * Mathf.Rad2Deg;
        yaw = Mathf.Atan2(direccion.x, direccion.z) * Mathf.Rad2Deg;
    }

    void LateUpdate()
    {
        if (jugador == null) return;

        Mouse mouse = Mouse.current;
        if (mouse != null && mouse.rightButton.isPressed)
        {
            Vector2 deltaMouse = mouse.delta.ReadValue();
            yaw += deltaMouse.x * sensibilidadMouse * Time.deltaTime;
            pitch -= deltaMouse.y * sensibilidadMouse * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, limiteAnguloMinimo, limiteAnguloMaximo);
        }

       
        Quaternion rotacion = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 offsetRotado = rotacion * new Vector3(0f, 0f, -distancia);

        Vector3 posicionObjetivo = jugador.position + offsetRotado;
        transform.position = Vector3.Lerp(transform.position, posicionObjetivo, suavizadoPosicion * Time.deltaTime);
        transform.LookAt(jugador);
    }
}
