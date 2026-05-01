using UnityEngine;
 
// Adjunta este script a cada moneda del mapa.
// El Collider2D del GameObject debe tener "Is Trigger" activado.
 
public class Moneda : MonoBehaviour
{
    [Header("Valor")]
    public int valor = 1;
 
    [Header("Efectos visuales (opcional)")]
    public GameObject efectoAlRecoger;
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
 
        // Sumar al GameManager
        if (GameManager.Instancia != null)
            GameManager.Instancia.AñadirMoneda(valor);
 
        // Sonido de moneda
        CharacterController jugador = other.GetComponent<CharacterController>();
        if (jugador != null)
            jugador.ReproducirSonidoMoneda();
 
        // Efecto visual
        if (efectoAlRecoger != null)
            Instantiate(efectoAlRecoger, transform.position, Quaternion.identity);
 
        Destroy(gameObject);
    }
}