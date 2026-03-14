using System;
using UnityEngine;
public class ControlJuego : MonoBehaviour
{
    public int puntuacionParaGanar;
    public int puntuacionActual;
    public bool juegoPausado;
    public static ControlJuego instancia;
    public void Awake()
    {
        instancia = this;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            cambiarPausa();

        int numEnemigos = GameObject.FindGameObjectsWithTag("Enemigo").Length;

        if (numEnemigos <= 0) ganarJuego();

    }
    public void cambiarPausa()
    {
        juegoPausado = !juegoPausado;
        Time.timeScale = (juegoPausado) ? 0.0f : 1.0f;
        Cursor.lockState = (juegoPausado) ? CursorLockMode.None : CursorLockMode.Locked;
        
        ControlHUD.instancia.CambiarEstadoVentanaPausa(juegoPausado);

    }
    public void PonerPuntuacion(int puntuacion)
    {
        puntuacionActual += puntuacion;
        ControlHUD.instancia.actualizarPuntuacion(puntuacionActual);
        
        //if (puntuacionActual >= puntuacionParaGanar)
        //    ganarJuego();
    }
    public void ganarJuego()
    {
        ControlHUD.instancia.establecerVentanaFinJuego(true);
    }
}
