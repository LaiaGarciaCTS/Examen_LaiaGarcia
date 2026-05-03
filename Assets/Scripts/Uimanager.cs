using UnityEngine;
using TMPro;
 
//  UI MANAGER
 
public class UIManager : MonoBehaviour
{
    public static UIManager Instancia { get; private set; }
 
    public TextMeshProUGUI textMonedas;
    public TextMeshProUGUI textVidas;
 
    void Awake()
    {
        if (Instancia != null && Instancia != this) { Destroy(gameObject); return; }
        Instancia = this;
    }
 
    public void ActualizarMonedas(int cantidad)
    {
        if (textMonedas != null)
            textMonedas.text = "Coins: " + cantidad;
    }
 
    public void ActualizarVidas(int vidas)
    {
        if (textVidas != null)
            textVidas.text = "Vida" + vidas;
    }
}