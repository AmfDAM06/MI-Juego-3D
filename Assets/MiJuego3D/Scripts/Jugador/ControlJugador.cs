using System;
using UnityEngine;
using UnityEngine.Rendering;

public class ControlJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad;
    public float fuerzaSalto;

    [Header("Camara")]
    public float sensibilidadRaton;
    public float maxVistaX;
    public float minVistaX;
    private float rotacionX;

    [Header("Vidas")]
    public int vidasActual;
    public int vidasMax;

    private Camera camara;
    private Rigidbody fisica;

    private ControlArma arma;

    public void Start()
    {
        Time.timeScale = 1.0f;
        
        ControlHUD.instancia.actualizaBarraVida(vidasActual, vidasMax);
        ControlHUD.instancia.actualizarBalasTexto(arma.municionActual, arma.municionMax);
        ControlHUD.instancia.actualizarPuntuacion(0);
    }

    private void Awake()
    {
        camara = Camera.main;
        fisica = GetComponent<Rigidbody>();
        arma = GetComponent<ControlArma>();

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
            if(arma.PuedeDisparar())arma.Disparar();
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

        rotacionX = Mathf.Clamp(rotacionX,minVistaX,maxVistaX);

        camara.transform.localRotation = Quaternion.Euler(-rotacionX, 0,0);
        transform.eulerAngles += Vector3.up * y;

     }

    private void Movimiento()
    {
        float x = Input.GetAxis("Horizontal") * velocidad;
        float z = Input.GetAxis("Vertical") * velocidad;

        Vector3 direccion = transform.right * x + transform.forward * z;
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
        Debug.Log("GAME OVER!!!");
        ControlHUD.instancia.establecerVentanaFinJuego(false);
    }

    internal void IncrementaVida(int cantidad)
    {
        vidasActual = Mathf.Clamp(vidasActual + cantidad, 0, vidasMax);
        ControlHUD.instancia.actualizaBarraVida(vidasActual, vidasMax);
    }

    internal void IncrementarBalas(int cantidad)
    {
        arma.municionActual = Mathf.Clamp(arma.municionActual + cantidad, 0, arma.municionMax);
        ControlHUD.instancia.actualizarBalasTexto(arma.municionActual, arma.municionMax);
    }
}
