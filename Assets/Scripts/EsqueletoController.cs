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
                animator.SetBool("Caminar", false); // Apagamos caminar mientras salta
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
/* EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE


/*Particulas
private ParticleSystem _walkParticles;

Void Awake()
{
    _walkParticles =GetComponentInChildren(ParticleSystem)();
}

Void Update()
{
    if (moveDirection.x >0)
    {
        if (!_walkParticles.isPlaying && sensor.isGrounded)
        {
        _walkParticles.Play();
        }
    }

    else if (moveDirection.x <0)
    {
        if (!_walkParticles.isPlaying && sensor.isGrounded)
        {
            _walkParticles.Play();
        }
    }

    else
    {
        if(_walkParticles.isPlaying)
        {
            _walkParticles = Stop();
        }
    }
}

Poner que no se ejecuten las particulas cuando salta.
if (jumpAction.wasPressedThisFrame() && sensor.isGrounded)
{
    rBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    _walkParticles.Stop();
}


Se ejecutan las particulas al aterrizar cuando saltas.
Script en el Ground Sensor.
public ParticleSystem _jumpParticles;


Void OnTriggerEnter2D(Collider2D collision)
{
    if(collision.gameObject)
    {
        _jumpParticles.Play();
    }
}
Poner las variables publicas para asignar manualmente las particulas.


Hacer que cuando caiga las particulas no se ejecuten, asi solo se ejecutaran al aterrizar al saltar y al moverse.
if(!sensor.isGrounded && _walkParticles.isPlaying)
{
    _walkParticles.Stop();
}



}*/