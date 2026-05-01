using UnityEngine;


// =====================================================================
//  MONEDA
//  Collider2D del GameObject → Is Trigger activado
//  Animator con animación de giro en loop (estado por defecto).
// =====================================================================
public class Moneda : MonoBehaviour
{
    [Header("Valor")]
    public int valor = 1;
 
    [Header("Efectos visuales (opcional)")]
    public GameObject efectoAlRecoger;
 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
 
        if (GameManager.Instancia != null)
            GameManager.Instancia.AñadirMoneda(valor);
 
        PlayerController jugador = other.GetComponent<PlayerController>();
        jugador?.ReproducirSonidoMoneda();
 
        if (efectoAlRecoger != null)
            Instantiate(efectoAlRecoger, transform.position, Quaternion.identity);
 
        Destroy(gameObject);
    }
}