using UnityEngine;
 

// =====================================================================
//  LLAVE
//  Collider2D del GameObject → Is Trigger activado
//  Animator con animación de idle/flotación en loop (estado por defecto).
// =====================================================================
public class Llave : MonoBehaviour
{
    [Header("Efectos visuales (opcional)")]
    public GameObject efectoAlRecoger;
 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
 
        if (GameManager.Instancia != null)
            GameManager.Instancia.RecogerLlave();
 
        PlayerController jugador = other.GetComponent<PlayerController>();
        jugador?.ReproducirSonidoLlave();
 
        if (efectoAlRecoger != null)
            Instantiate(efectoAlRecoger, transform.position, Quaternion.identity);
 
        Destroy(gameObject);
    }
}