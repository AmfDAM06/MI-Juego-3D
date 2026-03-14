using UnityEngine;

public class ControlArma : MonoBehaviour
{
    public int municionActual;
    public int municionMax;
    public bool municionInfinita;

    private PoolObjetos balaPool;
    public Transform puntoSalida;

    public float velocidadBala;

    public float frecuenciaDisparo;
    private float ultimoTiempoDisparo;
    private bool esJugador;

    

    private void Awake()
    {
        if(transform.tag == "Jugador")
            esJugador= true;
        balaPool = GetComponent<PoolObjetos>();
    }

    public bool PuedeDisparar()
    {
        if(Time.time - ultimoTiempoDisparo >= frecuenciaDisparo)
            if(municionActual > 0 ||municionInfinita == true)
                return true;
        return false;
    }

    public void Disparar()
    {
        ultimoTiempoDisparo = Time.time;
        municionActual--;

        GameObject bala = balaPool.getObjeto();
        bala.transform.position = puntoSalida.position;
        bala.transform.rotation = puntoSalida.rotation;

        bala.GetComponent<Rigidbody>().linearVelocity = puntoSalida.forward * velocidadBala;

        if (esJugador)
            ControlHUD.instancia.actualizarBalasTexto(municionActual, municionMax);
    }
}
