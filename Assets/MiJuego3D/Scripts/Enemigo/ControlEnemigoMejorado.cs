using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class ControlEnemigoMejorado : MonoBehaviour
{
    public enum EstadoIA { Patrullando, Persiguiendo }
    public EstadoIA estadoActual = EstadoIA.Patrullando;
    public int vidasActual;
    public int vidasMax;
    public int puntuacionEnemigo;
    public float velocidadPatrulla = 3.5f;
    public float velocidadPersecucion = 5.0f;
    public float rangoVision = 15f;
    public float rangoAtaque = 10f;
    public float radioPatrulla = 15f;
    private ControlArma arma;
    private GameObject objetivo;
    private NavMeshAgent agente;
    private float tiempoAtascado = 0f;
    private Renderer[] renderizadores;
    private Color[] coloresOriginales;

    void Start()
    {
        arma = GetComponent<ControlArma>();
        objetivo = GameObject.FindGameObjectWithTag("Jugador");
        agente = GetComponent<NavMeshAgent>();

        renderizadores = GetComponentsInChildren<Renderer>();
        coloresOriginales = new Color[renderizadores.Length];
        for (int i = 0; i < renderizadores.Length; i++)
        {
            coloresOriginales[i] = renderizadores[i].material.color;
        }

        BuscarNuevoPuntoAleatorio();
    }

    private void Update()
    {
        if (objetivo == null) return;

        float distanciaAlJugador = Vector3.Distance(transform.position, objetivo.transform.position);

        if (distanciaAlJugador <= rangoVision)
        {
            estadoActual = EstadoIA.Persiguiendo;
        }
        else if (estadoActual == EstadoIA.Persiguiendo)
        {
            estadoActual = EstadoIA.Patrullando;
            BuscarNuevoPuntoAleatorio();
        }

        switch (estadoActual)
        {
            case EstadoIA.Patrullando:
                ComportamientoPatrulla();
                break;
            case EstadoIA.Persiguiendo:
                ComportamientoPersecucion(distanciaAlJugador);
                break;
        }
    }

    private void ComportamientoPatrulla()
    {
        agente.speed = velocidadPatrulla;
        tiempoAtascado += Time.deltaTime;

        if ((!agente.pathPending && agente.remainingDistance <= 1.5f) || tiempoAtascado > 4f)
        {
            BuscarNuevoPuntoAleatorio();
        }
    }

    private void ComportamientoPersecucion(float distanciaAlJugador)
    {
        tiempoAtascado = 0f;
        agente.speed = velocidadPersecucion;
        agente.SetDestination(objetivo.transform.position);

        Vector3 direccion = (objetivo.transform.position - transform.position).normalized;
        direccion.y = 0;
        transform.rotation = Quaternion.LookRotation(direccion);

        if (distanciaAlJugador <= rangoAtaque)
        {
            if (arma.PuedeDisparar()) arma.Disparar();
        }
    }

    private void BuscarNuevoPuntoAleatorio()
    {
        tiempoAtascado = 0f;
        Vector3 direccionAleatoria = Random.insideUnitSphere * radioPatrulla;
        direccionAleatoria += transform.position;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(direccionAleatoria, out hit, radioPatrulla, NavMesh.AllAreas))
        {
            agente.SetDestination(hit.position);
        }
    }

    public void QuitarVidasEnemigo(int cantidad)
    {
        vidasActual -= cantidad;
        ControlJuego.instancia.PonerPuntuacion(puntuacionEnemigo);

        if (vidasActual > 0)
        {
            StartCoroutine(EfectoDanio());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator EfectoDanio()
    {
        for (int i = 0; i < renderizadores.Length; i++)
        {
            if (renderizadores[i] != null) renderizadores[i].material.color = Color.red;
        }

        yield return new WaitForSeconds(0.15f);

        for (int i = 0; i < renderizadores.Length; i++)
        {
            if (renderizadores[i] != null) renderizadores[i].material.color = coloresOriginales[i];
        }
    }
}