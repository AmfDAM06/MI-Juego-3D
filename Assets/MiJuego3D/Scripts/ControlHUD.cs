using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class ControlHUD : MonoBehaviour
{
    [Header("HUD")]
    public TextMeshProUGUI puntuacionTexto;
    public TextMeshProUGUI balasTexto;
    public Image barraVidas;
    [Header("Vetana Pausa")]
    public GameObject ventanaPausa;
    [Header("Ventana Fin Juego")]
    public GameObject ventanaFinJuego;
    public TextMeshProUGUI resultadoTexto;
    public static ControlHUD instancia;
    private void Awake()
    {
        instancia = this;
    }
    public void actualizaBarraVida(int vidaActual, int vidaMax)
    {
        barraVidas.fillAmount = (float)vidaActual / (float)vidaMax;
    }
    public void actualizarBalasTexto(int numBolasActual, int numBolasMax)
    {
        balasTexto.text = "Bolas : " + numBolasActual + " / " + numBolasMax;
    }
    public void actualizarPuntuacion(int puntuacion)
    {
        puntuacionTexto.text = puntuacion.ToString("00000");
    }
    public void CambiarEstadoVentanaPausa(bool pausa)
    {
        ventanaPausa.SetActive(pausa);
    }
    public void establecerVentanaFinJuego(bool ganado)
    {
        ventanaFinJuego.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0.0f;

        resultadoTexto.text = (ganado) ? "HAS GANADO!!" : "HAS PERDIDO!!";
        resultadoTexto.color = (ganado) ? Color.green : Color.red;
    }
    public void OnBotonMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void OnBotonVolver()
    {
        ControlJuego.instancia.cambiarPausa();
    }
    public void OnBotonEmpezar()
    {
        SceneManager.LoadScene("Nivel1");
    }
    public void onBotonSalir()
    {
        Application.Quit();
    }
}
