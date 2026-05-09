using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
// =====================================================================
//  UI MANAGER — Nivel
//
//  Colocar en un GameObject vacío "UIManager" dentro del Canvas del nivel.
//
//  En el Inspector conectar:
//    - textMonedas   → TextMeshProUGUI  "Coins: 0"
//    - iconosVida[]  → GameObjects de corazones (uno por vida)
//    - barraVidaFill → Image (Image Type=Filled, Method=Horizontal)
//    - textVidas     → TextMeshProUGUI  "3 / 3"
// =====================================================================
 
public class UIManager : MonoBehaviour
{
    public static UIManager Instancia { get; private set; }
 
    [Header("Monedas")]
    public TextMeshProUGUI textMonedas;
 
    [Header("Vidas - Opcion A: iconos corazon")]
    public GameObject[] iconosVida;
 
    [Header("Vidas - Opcion B: barra de vida")]
    public Image barraVidaFill;
    private int vidasMaximas = 3;
 
    [Header("Vidas - Opcion C: texto numerico")]
    public TextMeshProUGUI textVidas;
 
    void Awake()
    {
        if (Instancia != null && Instancia != this) { Destroy(gameObject); return; }
        Instancia = this;
    }
 
    void Start()
    {
        ActualizarMonedas(0);
    }
 
    // ------------------------------------------------------------------
    //  MONEDAS
    // ------------------------------------------------------------------
    public void ActualizarMonedas(int cantidad)
    {
        if (textMonedas != null)
            textMonedas.text = "Coins: " + cantidad;
    }
 
    // ------------------------------------------------------------------
    //  VIDAS
    // ------------------------------------------------------------------
    public void InicializarVidas(int maxVidas)
    {
        vidasMaximas = maxVidas;
        ActualizarVidas(maxVidas);
    }
 
    public void ActualizarVidas(int vidasActuales)
    {
        // Opcion A: iconos
        if (iconosVida != null && iconosVida.Length > 0)
        {
            for (int i = 0; i < iconosVida.Length; i++)
                if (iconosVida[i] != null)
                    iconosVida[i].SetActive(i < vidasActuales);
        }
 
        // Opcion B: barra
        if (barraVidaFill != null && vidasMaximas > 0)
            barraVidaFill.fillAmount = (float)vidasActuales / vidasMaximas;
 
        // Opcion C: texto
        if (textVidas != null)
            textVidas.text = vidasActuales + " / " + vidasMaximas;
    }
}