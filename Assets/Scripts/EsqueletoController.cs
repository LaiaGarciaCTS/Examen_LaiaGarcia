using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterController : MonoBehaviour

{

private Rigidbody2D rBody2D;

public Vector3 cameraOffset;
public Vector3 minCameraPosition;
public Vector3 maxCameraPosition;
//public GameObject cameraTarget;


//Movimiento y direccion
public Vector3 startPosition;

public float movementSpeed = 5;

public int direction = 5;

private InputAction moveAction;

private Vector2 moveDirection;

//Para hacer Flip
private SpriteRenderer renderer;

//Animaciones
private Animator animator;

//Salto
private InputAction jumpAction;
public float jumpForce = 10;

//Salto NO infinito
private GroundSensor sensor;



void Awake()
{
    rBody2D = GetComponent<Rigidbody2D>();
    renderer = GetComponent<SpriteRenderer>();
    sensor = GetComponentInChildren<GroundSensor>();
    animator = GetComponent<Animator>();

    moveAction = InputSystem.actions["Move"];
    jumpAction = InputSystem.actions["Jump"];

    


    if(moveDirection.x > 0)
    {
        renderer.flipX = false;
        animator.SetBool("Animacion caminar", true);
    }

    else if(moveDirection.x < 0)
    {
        renderer.flipX = true;
        animator.SetBool("Animacion caminar", true);
    }

    else
    {
        animator.SetBool("Animacion caminar", false);
    }
}


void Start()
{
    transform.position = startPosition;

    startPosition = new Vector3(0, 0, 0);

    moveAction = InputSystem.actions["Move"];

    moveDirection = moveAction.ReadValue<Vector2>();

    transform.position = new Vector3(transform.position.x + moveDirection.x * movementSpeed * Time.deltaTime, transform.position.y, transform.position.z);

    rBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
}


void Update()
{
    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + direction, transform.position.y), movementSpeed * Time.deltaTime);

    //transform.position = new Vector3(cameraTarget.position.x, 0, 0) + cameraOffset;
}


void FixedUpdate()
{
    rBody2D.linearVelocity = new Vector2(moveDirection.x * movementSpeed, rBody2D.linearVelocity.y);
}

/*
//Salto NO infinito
void OnTriggerEnter2D(Collider2D collision)
{
    if(collision.gameObject.layer == 6)
    {
        isGrounded = true;
    }
}

void OnTriggerStay2D(Collider2D collision)
{
    if(collision.gameObject.layer == 6)
    {
        isGrounded = true;
    }
}

void OnTriggerExit2D(Collider2D collision)
{
    if(collision.gameObject.layer == 6)
    {
        isGrounded = false;
    }
}





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


*/
}