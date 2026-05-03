using UnityEngine;
 
//  CAMERA CONTROLLER
 
public class CameraFollow : MonoBehaviour
{
    private Transform cameraTarget;
 
    public Vector3 cameraOffset = new Vector3(0f, 5f, -10f);
    public Vector3 minCameraPosition;
    public Vector3 maxCameraPosition;
 
    //  AWAKE
    void Awake()
    {
        cameraTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
 
    //  UPDATE
    void Update()
    {
        if (cameraTarget != null)
        {
            Vector3 desiredPosition = cameraTarget.position + cameraOffset;
 
            float clampX = Mathf.Clamp(desiredPosition.x, minCameraPosition.x, maxCameraPosition.x);
            float clampY = Mathf.Clamp(desiredPosition.y, minCameraPosition.y, maxCameraPosition.y);
 
            Vector3 clampedPosition = new Vector3(clampX, clampY, desiredPosition.z);
 
            transform.position = clampedPosition;
        }
    }
 
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(minCameraPosition.x, -20f, 0), new Vector3(minCameraPosition.x, 20f, 0));
        Gizmos.DrawLine(new Vector3(maxCameraPosition.x, -20f, 0), new Vector3(maxCameraPosition.x, 20f, 0));
    }
}