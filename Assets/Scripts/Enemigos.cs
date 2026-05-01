using UnityEngine;

public class Enemigos : MonoBehaviour
{
    public Transform[] patrolPoints;

    private Vector3 moveDirection;

    private Rigidbody2D _rigidBody2D;

    private int direction = 1;

    private float movementSpeed = 2;

    public int patrolIndex = 0;

    private Transform playerPosition;

    public float detectionRange = 5;

    public float attackRange = 1;


    void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }


    void FixedUpdate()
    {
        //rigidBody2D.linearvelocity - comentar ésta línea.
        
        float distanceToPlayer = Vector3.Distance(playerPosition.position, transform.position);

        if(distanceToPlayer > detectionRange)
        {
            Patrol();
        }

        else if(distanceToPlayer < detectionRange && distanceToPlayer > attackRange)
        {
            FollowPlayer();
        }

        else if(distanceToPlayer < attackRange)
        {
            Attack();
        }
    }


    void Patrol()
    {
        float distanceToPoint = Vector3.Distance(transform.position, patrolPoints[patrolIndex].position);

        if(distanceToPoint < 1f)
        {
            if(patrolIndex == 0)
            {
                patrolIndex = 1;
            }

            else
            {
                patrolIndex = 0;
            }
        }

        Vector3 moveDirection = patrolPoints[patrolIndex].position - transform.position;
        Debug.Log(moveDirection);

        if(moveDirection.x < 0)
        {
            direction = -1;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        else if(moveDirection.x > 0)
        {
            direction = 1;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        _rigidBody2D.linearVelocity = new Vector2(direction * movementSpeed, _rigidBody2D.linearVelocity.y);
    }

    void FollowPlayer()
    {
        Vector3 moveDirection = playerPosition.position - transform.position;

        Movement(moveDirection);
    }

    void Movement(Vector3 moveDirection)
    {
        if(moveDirection.x <0)
        {
            direction = -1;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        else if(moveDirection.x >0)
        {
            direction = 1;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        _rigidBody2D.linearVelocity = new Vector2(direction * movementSpeed, _rigidBody2D.linearVelocity.y);
    }

    void Attack()
    {
        direction = 0;

        Debug.Log("Atacando");
    }

}