using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterController : MonoBehaviour


{
    private Rigidbody2D rBody2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public float movementSpeed = 5f;
    public float jumpForce = 10f;
    
    
    public GroundSensor sensor; 

    private Vector2 moveDirection;



    void Awake()
    {
        rBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. MOVIMIENTO
        float horizontal = Input.GetAxisRaw("Horizontal");
        moveDirection = new Vector2(horizontal, 0);

        // 2. FLIP
        if (horizontal > 0) spriteRenderer.flipX = false;
        else if (horizontal < 0) spriteRenderer.flipX = true;

        // 3. ANIMACIONES
        if (sensor != null)
        {
            bool tocandoSuelo = sensor.IsGrounded();

            if (!tocandoSuelo) //¿Está en el aire?
            {
                animator.SetBool("Saltar", true);
                animator.SetBool("Caminar", false); // Desactivamos caminar mientras salta
            }
            else //¿Está en el suelo?
            {
                animator.SetBool("Saltar", false);

                if (horizontal != 0) // ¿Se está moviendo?
                {
                    animator.SetBool("Caminar", true);
                }
                else //¿Está quieto?
                {
                    animator.SetBool("Caminar", false);
                }
            }
        }

        // 4. SALTO
        if (Input.GetKeyDown(KeyCode.Space) && sensor.IsGrounded())
        {
            rBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Movimiento físico
        rBody2D.linearVelocity = new Vector2(moveDirection.x * movementSpeed, rBody2D.linearVelocity.y);
    }
}
