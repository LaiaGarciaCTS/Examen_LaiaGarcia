using UnityEngine;
 
// Adjunta este script a la llave del mapa.
// El Collider2D del GameObject debe tener "Is Trigger" activado.
 
public class Llave : MonoBehaviour
{
    [Header("Efectos visuales (opcional)")]
    public GameObject efectoAlRecoger;
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
 
        // Registrar en el GameManager que el jugador tiene la llave
        if (GameManager.Instancia != null)
            GameManager.Instancia.RecogerLlave();
 
        // Sonido de llave
        CharacterController jugador = other.GetComponent<CharacterController>();
        if (jugador != null)
            jugador.ReproducirSonidoLlave();
 
        // Efecto visual
        if (efectoAlRecoger != null)
            Instantiate(efectoAlRecoger, transform.position, Quaternion.identity);
 
        Destroy(gameObject);
    }
}