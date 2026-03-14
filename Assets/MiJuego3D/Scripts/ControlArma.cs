using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ControlArma : MonoBehaviour
{
    public int municionActual;
    public int municionMax;
    public bool municionInfinita;
    public int dañoArma;
    public float velocidadBala;
    public float frecuenciaDisparo;

    private PoolObjetos balaPool;
    public Transform puntoSalida;
    private float ultimoTiempoDisparo;
    private bool esJugador;
    private AudioSource audioDisparo;

    private void Awake()
    {
        if (transform.root.CompareTag("Jugador"))
            esJugador = true;

        balaPool = GetComponent<PoolObjetos>();
        audioDisparo = GetComponent<AudioSource>();
    }

    public bool PuedeDisparar()
    {
        if (Time.time - ultimoTiempoDisparo >= frecuenciaDisparo)
            if (municionActual > 0 || municionInfinita)
                return true;
        return false;
    }

    public void Disparar()
    {
        ultimoTiempoDisparo = Time.time;
        municionActual--;

        audioDisparo.Play();

        GameObject bala = balaPool.getObjeto();
        bala.transform.position = puntoSalida.position;
        bala.transform.rotation = puntoSalida.rotation;

        bala.GetComponent<Rigidbody>().linearVelocity = puntoSalida.forward * velocidadBala;
        bala.GetComponent<ControlBala>().cantidadVida = dañoArma;

        if (esJugador)
            ControlHUD.instancia.actualizarBalasTexto(municionActual, municionMax);
    }
}