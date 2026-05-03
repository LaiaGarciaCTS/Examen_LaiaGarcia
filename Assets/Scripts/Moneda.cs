using UnityEngine;



//  MONEDA

public class Moneda : MonoBehaviour
{
    public int valor = 1;
 
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