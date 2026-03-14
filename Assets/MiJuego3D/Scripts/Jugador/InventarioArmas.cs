using UnityEngine;

public class InventarioArmas : MonoBehaviour
{
    public GameObject[] armas;
    private int armaActual = 0;

    void Start()
    {
        EquiparArma(armaActual);
    }

    void Update()
    {
        int armaAnterior = armaActual;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (armaActual >= armas.Length - 1) armaActual = 0;
            else armaActual++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (armaActual <= 0) armaActual = armas.Length - 1;
            else armaActual--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && armas.Length > 0) armaActual = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && armas.Length > 1) armaActual = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && armas.Length > 2) armaActual = 2;

        if (armaAnterior != armaActual)
        {
            EquiparArma(armaActual);
        }
    }

    private void EquiparArma(int indice)
    {
        for (int i = 0; i < armas.Length; i++)
        {
            armas[i].SetActive(i == indice);
        }

        ControlArma armaActiva = GetComponentInChildren<ControlArma>();
        if (armaActiva != null && ControlHUD.instancia != null)
        {
            ControlHUD.instancia.actualizarBalasTexto(armaActiva.municionActual, armaActiva.municionMax);
        }
    }
}