using UnityEngine;
 
// Adjunta este script a cada objeto coleccionable del mapa (monedas, gemas, etc.)
// Requiere un Collider2D en modo "Is Trigger" en el mismo GameObject.
 
public class Coleccionable : MonoBehaviour

{
    [Header("Tipo de coleccionable")]
    public TipoColeccionable tipo = TipoColeccionable.Moneda;
    public int valor = 1; // Puntos o monedas que otorga
 
    [Header("Efectos visuales (opcional)")]
    public GameObject efectoAlRecoger; // Partículas o animación de recogida
 
    //  DETECCIÓN
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Solo reacciona al jugador
        if (!other.CompareTag("Player")) return;
 
        // Avisar al GameManager
        if (GameManager.Instancia != null)
            GameManager.Instancia.AñadirPuntos(tipo, valor);
 
        // Avisar al personaje para que reproduzca el sonido
        CharacterController jugador = other.GetComponent<CharacterController>();
        if (jugador != null)
            jugador.RecogerColeccionable();
 
        // Efecto visual
        if (efectoAlRecoger != null)
            Instantiate(efectoAlRecoger, transform.position, Quaternion.identity);
 
        // Destruir el coleccionable
        Destroy(gameObject);
    }
}
 
// Enum para distinguir tipos de coleccionables fácilmente
public enum TipoColeccionable
{
    Moneda,
    Gema,
    Estrella,
    PowerUp
}