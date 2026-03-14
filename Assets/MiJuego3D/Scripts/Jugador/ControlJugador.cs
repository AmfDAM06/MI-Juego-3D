using UnityEngine;

public class ControlJugador : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public float sensibilidadRaton;
    public float maxVistaX;
    public float minVistaX;
    private float rotacionX;
    public int vidasActual;
    public int vidasMax;
    private Camera camara;
    private Rigidbody fisica;

    public void Start()
    {
        Time.timeScale = 1.0f;
        ControlHUD.instancia.actualizaBarraVida(vidasActual, vidasMax);
        ControlHUD.instancia.actualizarPuntuacion(0);

        ControlArma armaActiva = GetComponentInChildren<ControlArma>();
        if (armaActiva != null)
            ControlHUD.instancia.actualizarBalasTexto(armaActiva.municionActual, armaActiva.municionMax);
    }

    private void Awake()
    {
        camara = Camera.main;
        fisica = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (ControlJuego.instancia.juegoPausado) return;

        Movimiento();
        VistaCamara();

        if (Input.GetButtonDown("Jump")) Salto();

        if (Input.GetButton("Fire1"))
        {
            ControlArma armaActiva = GetComponentInChildren<ControlArma>();
            if (armaActiva != null && armaActiva.PuedeDisparar()) armaActiva.Disparar();
        }
    }

    private void Salto()
    {
        Ray rayo = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(rayo, 1.1f))
            fisica.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
    }

    private void VistaCamara()
    {
        float y = Input.GetAxis("Mouse X") * sensibilidadRaton;
        rotacionX += Input.GetAxis("Mouse Y") * sensibilidadRaton;
        rotacionX = Mathf.Clamp(rotacionX, minVistaX, maxVistaX);
        camara.transform.localRotation = Quaternion.Euler(-rotacionX, 0, 0);
        transform.eulerAngles += Vector3.up * y;
    }

    private void Movimiento()
    {
        float x = Input.GetAxis("Horizontal") * velocidad;
        float z = Input.GetAxis("Vertical") * velocidad;
        Vector3 direccion = transform.right * x + transform.forward * z;
        direccion.y = fisica.linearVelocity.y;
        fisica.linearVelocity = direccion;
    }

    internal void QuitarVidasJugador(int cantidadVida)
    {
        vidasActual -= cantidadVida;
        ControlHUD.instancia.actualizaBarraVida(vidasActual, vidasMax);

        if (vidasActual <= 0)
            TerminaJugador();
    }

    private void TerminaJugador()
    {
        ControlHUD.instancia.establecerVentanaFinJuego(false);
    }

    internal void IncrementaVida(int cantidad)
    {
        vidasActual = Mathf.Clamp(vidasActual + cantidad, 0, vidasMax);
        ControlHUD.instancia.actualizaBarraVida(vidasActual, vidasMax);
    }

    internal void IncrementarBalas(int cantidad)
    {
        ControlArma armaActiva = GetComponentInChildren<ControlArma>();
        if (armaActiva != null)
        {
            armaActiva.municionActual = Mathf.Clamp(armaActiva.municionActual + cantidad, 0, armaActiva.municionMax);
            ControlHUD.instancia.actualizarBalasTexto(armaActiva.municionActual, armaActiva.municionMax);
        }
    }
}