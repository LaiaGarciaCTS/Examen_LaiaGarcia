using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterController : MonoBehaviour

{

//Movimiento y dirección
public Vector3 startPosition;

public float movementSpeed = 5;

public int direction = 5;

private InputAction moveAction;

private Vector2 moveDirection;

//Para hacer Flip
private SpriteRenderer renderer;


void Awake()
{
    renderer = GetComponent<SpriteRenderer>();

    if(moveDirection.x > 0)
    {
        renderer.flipX = false;
    }

    else if(moveDirection.x < 0)
    {
        renderer.flipX = true;
    }
}


void Start()
{
    transform.position = startPosition;

    startPosition = new Vector3(0, 0, 0);

    moveAction = InputSystem.actions["Move"];

    moveDirection = moveAction.ReadValue<Vector2>();

    transform.position = new Vector3(transform.position.x + moveDirection.x * movementSpeed * Time.deltaTime, transform.position.y, transform.position.z);
}


void Update()
{
    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + direction, transform.position.y), movementSpeed * Time.deltaTime);
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