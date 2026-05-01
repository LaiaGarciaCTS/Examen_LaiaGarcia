using UnityEngine;
using TMPro;
 
// =====================================================================
//  UI MANAGER
//  Coloca este script en un GameObject vacío llamado "UIManager"
//  dentro del Canvas de tu escena.
//
//  En el Inspector arrastra:
//    - textMonedas → un TextMeshProUGUI con texto inicial "Coins: 0"
//    - textVidas   → un TextMeshProUGUI con texto inicial "❤ 3"
// =====================================================================
 
public class UIManager : MonoBehaviour
{
    public static UIManager Instancia { get; private set; }
 
    [Header("HUD - Texto")]
    public TextMeshProUGUI textMonedas;
    public TextMeshProUGUI textVidas;
 
    void Awake()
    {
        if (Instancia != null && Instancia != this) { Destroy(gameObject); return; }
        Instancia = this;
    }
 
    /// <summary>Actualiza el contador de monedas en pantalla.</summary>
    public void ActualizarMonedas(int cantidad)
    {
        if (textMonedas != null)
            textMonedas.text = "Coins: " + cantidad;
    }
 
    /// <summary>Actualiza el contador de vidas en pantalla.</summary>
    public void ActualizarVidas(int vidas)
    {
        if (textVidas != null)
            textVidas.text = "❤ " + vidas;
    }
}