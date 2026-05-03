using UnityEngine;
 
//  GROUND SENSOR
 
public class GroundSensor : MonoBehaviour
{
    // Variable privada: accede desde el mismo script
    private bool isGrounded;
 
    // API pública para que PlayerController pueda consultarla
    public bool IsGrounded()
    {
        return isGrounded;
    }
 
    // Para cuando inicias el juego no se ejecuta pero mientras juegas sí
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isGrounded = true;
        }
    }
 
    // Para asegurar que el objeto pueda saltar siempre mientras esté en suelo
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isGrounded = true;
        }
 
        // Mario mata al enemigo pisándolo (layer 7)
        if (collision.gameObject.layer == 7)
        {
            EnemyController ec = collision.gameObject.GetComponent<EnemyController>();
            if (ec != null)
                ec.RecibirDaño(ec.vida);
        }
    }
 
    // Para cuando el objeto está en el aire NO pueda saltar
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isGrounded = false;
        }
    }
}