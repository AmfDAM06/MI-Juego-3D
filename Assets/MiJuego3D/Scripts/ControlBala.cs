using UnityEngine;

public class ControlBala : MonoBehaviour
{
    public GameObject particulasExplosion;
    public int cantidadVida;
    public float tiempoActivo;
    private float tiempoDisparo;

    public void OnEnable()
    {
        tiempoDisparo = Time.time;
    }

    private void Update()
    {
        if(Time.time - tiempoDisparo >= tiempoActivo) gameObject.SetActive(false);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jugador")) other.GetComponent<ControlJugador>().QuitarVidasJugador(cantidadVida);
        else if(other.CompareTag("Enemigo")) other.GetComponent<ControlEnemigo>().QuitarVidasEnemigo(cantidadVida);

        GameObject particulas = Instantiate(particulasExplosion, transform.position, Quaternion.identity);
        Destroy(particulas, 1f);

        gameObject.SetActive(false);

    }
}
