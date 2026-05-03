using UnityEngine;
 
//  GROUND SENSOR
 
public class GroundSensor : MonoBehaviour
{
    private bool isGrounded;
 
    public bool IsGrounded()
    {
        return isGrounded;
    }
 
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isGrounded = true;
        }
    }
 
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isGrounded = true;
        }
 
        if (collision.gameObject.layer == 7)
        {
            EnemyController ec = collision.gameObject.GetComponent<EnemyController>();
            if (ec != null)
                ec.RecibirDaño(ec.vida);
        }
    }
 
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isGrounded = false;
        }
    }
}