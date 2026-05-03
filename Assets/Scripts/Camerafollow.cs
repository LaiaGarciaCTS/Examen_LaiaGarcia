using UnityEngine;
 
//  CAMERA CONTROLLER
//    - Camera Offset       X=0  Y=5  Z=-10
 
public class CameraFollow : MonoBehaviour
{
    // Según apuntes: private Transform cameraTarget
    private Transform cameraTarget;
 
    public Vector3 cameraOffset = new Vector3(0f, 5f, -10f);
    public Vector3 minCameraPosition;
    public Vector3 maxCameraPosition;
 
    // ==================================================================
    //  AWAKE — busca al jugador por tag
    // ==================================================================
    void Awake()
    {
        // Según apuntes: mejor usar FindGameObjectsWithTag que Find por nombre
        cameraTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
 
    // ==================================================================
    //  UPDATE — sigue al jugador con Mathf.Clamp
    // ==================================================================
    void Update()
    {
        // Según apuntes: if(cameraTarget != null) para que cuando Mario muera
        // la cámara deje de seguirle y se quede estática
        if (cameraTarget != null)
        {
            // Posición deseada: solo sigue eje X, Y fija según offset
            Vector3 desiredPosition = cameraTarget.position + cameraOffset;
 
            // Según apuntes: Mathf.Clamp — hace que no pueda pasar de los valores X e Y
            float clampX = Mathf.Clamp(desiredPosition.x, minCameraPosition.x, maxCameraPosition.x);
            float clampY = Mathf.Clamp(desiredPosition.y, minCameraPosition.y, maxCameraPosition.y);
 
            // La posición de la cámara delimitada
            Vector3 clampedPosition = new Vector3(clampX, clampY, desiredPosition.z);
 
            // Mueve la cámara dentro de los valores
            transform.position = clampedPosition;
        }
    }
 
    // Dibuja los límites en el editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(minCameraPosition.x, -20f, 0), new Vector3(minCameraPosition.x, 20f, 0));
        Gizmos.DrawLine(new Vector3(maxCameraPosition.x, -20f, 0), new Vector3(maxCameraPosition.x, 20f, 0));
    }
}