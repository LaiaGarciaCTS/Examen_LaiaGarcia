using UnityEngine;
 

//  LLAVE

public class Llave : MonoBehaviour
{
    public GameObject efectoAlRecoger;
 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
 
        if (GameManager.Instancia != null)
            GameManager.Instancia.RecogerLlave();
        else
            Debug.LogWarning("GameManager no encontrado. Comprueba que esta en la escena.");
 
        PlayerController jugador = other.GetComponent<PlayerController>();
        jugador?.ReproducirSonidoLlave();
 
        if (efectoAlRecoger != null)
            Instantiate(efectoAlRecoger, transform.position, Quaternion.identity);
 
        Destroy(gameObject);
    }
}