using UnityEngine;

public class Camara : MonoBehaviour
{
    //Movimiento camara
    private Transform cameraTarget;

    public Vector3 cameraOffset;
    public Vector3 minCameraPosition;
    public Vector3 maxCameraPosition;
    

    void Awake()
    {
        cameraTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    void Start()
    {
        
    }

    
    void Update()
    {
        Vector3 desiredPosition = cameraTarget.position + cameraOffset;

        float clampX = Mathf.Clamp(desiredPosition.x, minCameraPosition.x, maxCameraPosition.x);
        float clampY = Mathf.Clamp(desiredPosition.y, minCameraPosition.y, maxCameraPosition.y);

        Vector3 clampedPosition = new Vector3(clampX, clampY, desiredPosition.z);

        transform.position = clampedPosition;

    }
}
