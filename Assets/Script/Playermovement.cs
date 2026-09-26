using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadMovimiento = 6f;
    public float velocidadRotacion = 120f; //grados por segundo al girar A/D

    [Header("Salto")]
    public float fuerzaSalto = 8f;
    public float gravedad = -20f;

    private CharacterController controller;
    private Vector3 velocidadVertical;
    private bool estaEnSuelo;

    [Header("Plataformas moviles")]
    public float radioDeteccionPlataforma = 0.6f;
    public LayerMask capaPlataformas; 
    public bool mostrarDebug = true; 

    private Transform plataformaActual;
    private Vector3 posicionAnteriorPlataforma;

    [Header("Dash")]
    public float velocidadDash = 20f;
    public float duracionDash = 0.2f;   //cuanto dura el dash en segundos
    public float cooldownDash = 1f;     //cuanto hay que esperar para volver a dashear

    private bool estaDasheando = false;
    private float tiempoRestanteDash = 0f;
    private float tiempoRestanteCooldown = 0f;
    private Vector3 direccionDash;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        
        if (capaPlataformas.value == 0)
            capaPlataformas = ~0;
    }

    void Update()
    {
        var teclado = Keyboard.current;
        if (teclado == null) return; 

        estaEnSuelo = controller.isGrounded;

        
        if (estaEnSuelo && velocidadVertical.y < 0f)
            velocidadVertical.y = -2f;

        
        DetectarPlataforma();

        
        if (plataformaActual != null)
        {
            Vector3 deltaPlataforma = plataformaActual.position - posicionAnteriorPlataforma;
            controller.Move(deltaPlataforma);
            posicionAnteriorPlataforma = plataformaActual.position;
        }

        //Rotacion (A / D)
        float rotacionInput = 0f;
        if (teclado.aKey.isPressed) rotacionInput -= 1f;
        if (teclado.dKey.isPressed) rotacionInput += 1f;
        transform.Rotate(0f, rotacionInput * velocidadRotacion * Time.deltaTime, 0f);

        //Dash (Shift)
        if (tiempoRestanteCooldown > 0f)
            tiempoRestanteCooldown -= Time.deltaTime;

        bool teclaDashPresionada = teclado.leftShiftKey.wasPressedThisFrame || teclado.rightShiftKey.wasPressedThisFrame;
        if (teclaDashPresionada && !estaDasheando && tiempoRestanteCooldown <= 0f)
        {
            estaDasheando = true;
            tiempoRestanteDash = duracionDash;
            direccionDash = transform.forward; 
        }

        if (estaDasheando)
        {
            controller.Move(direccionDash * velocidadDash * Time.deltaTime);
            tiempoRestanteDash -= Time.deltaTime;

            if (tiempoRestanteDash <= 0f)
            {
                estaDasheando = false;
                tiempoRestanteCooldown = cooldownDash;
            }
        }

        //Movimiento(W / S)
        
        if (!estaDasheando)
        {
            float movimientoInput = 0f;
            if (teclado.wKey.isPressed) movimientoInput += 1f;
            if (teclado.sKey.isPressed) movimientoInput -= 1f;

            Vector3 direccionMovimiento = transform.forward * movimientoInput;
            controller.Move(direccionMovimiento.normalized * velocidadMovimiento * Time.deltaTime * Mathf.Abs(movimientoInput));
        }

        //Salto (Espacio)
        if (teclado.spaceKey.wasPressedThisFrame && estaEnSuelo)
        {
            velocidadVertical.y = fuerzaSalto;
        }

        //Gravedad
        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);
    }

    void DetectarPlataforma()
    {
        
        Vector3 origen = new Vector3(transform.position.x, controller.bounds.min.y + 0.1f, transform.position.z);
        float distancia = controller.skinWidth + 0.3f;

        bool hayImpacto = Physics.SphereCast(origen, radioDeteccionPlataforma, Vector3.down, out RaycastHit hit, distancia, capaPlataformas);

        if (mostrarDebug)
        {
            if (!estaEnSuelo)
                Debug.Log("Debug plataforma: no esta en el suelo (estaEnSuelo = false), no se hace el SphereCast.");
            else if (!hayImpacto)
                Debug.Log("Debug plataforma: el SphereCast no toco nada. Revisa el radio, la distancia o la LayerMask.");
            else
                Debug.Log("Debug plataforma: el SphereCast toco '" + hit.collider.name + "' (layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer) + ")");
        }

        if (estaEnSuelo && hayImpacto)
        {
            MovingPlatform plataforma = hit.collider.GetComponent<MovingPlatform>();

            if (plataforma == null && mostrarDebug)
                Debug.Log("Debug plataforma: toco '" + hit.collider.name + "' pero ese objeto NO tiene el script MovingPlatform encima.");

            if (plataforma != null)
            {
                if (plataformaActual != plataforma.transform)
                {
                    plataformaActual = plataforma.transform;
                    posicionAnteriorPlataforma = plataformaActual.position;
                }
                return;
            }
        }

        plataformaActual = null;
    }

    void OnDrawGizmos()
    {
        if (!mostrarDebug || controller == null) return;

        Vector3 origen = new Vector3(transform.position.x, controller.bounds.min.y + 0.1f, transform.position.z);
        float distancia = controller.skinWidth + 0.3f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origen, radioDeteccionPlataforma);
        Gizmos.DrawWireSphere(origen + Vector3.down * distancia, radioDeteccionPlataforma);
    }
}