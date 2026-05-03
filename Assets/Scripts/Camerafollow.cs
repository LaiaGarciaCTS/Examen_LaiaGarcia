using UnityEngine;
 
// =====================================================================
//  CAMERA FOLLOW — Solo eje X
//  Coloca este script en la Main Camera.
//
//  La cámara sigue al jugador solo en el eje X, con límites izquierdo
//  y derecho. El eje Y permanece fijo donde esté la cámara al inicio.
//
//  Pasos en Unity:
//    1. Selecciona la Main Camera.
//    2. Añade este script.
//    3. Arrastra el GameObject del jugador al campo "objetivo".
//    4. Ajusta limiteIzquierdo y limiteDerecho según el tamaño del mapa.
//    5. Ajusta suavizado (0 = instantáneo, valores mayores = más suave).
// =====================================================================
 
public class CameraFollow : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform objetivo;             // Arrastra aquí el jugador
 
    [Header("Suavizado")]
    [Range(0f, 0.3f)]
    public float suavizado = 0.1f;         // Tiempo de suavizado en segundos
 
    [Header("Offset horizontal")]
    public float offsetX = 0f;             // Desplazamiento extra en X (opcional)
 
    [Header("Límites del mapa en X")]
    public float limiteIzquierdo = -20f;
    public float limiteDerecho   =  20f;
 
    // Y fija: se toma de la posición inicial de la cámara
    private float yFija;
    private float zFija;
    private float velocidadX = 0f;
 
    void Start()
    {
        yFija = transform.position.y;
        zFija = transform.position.z;
    }
 
    void LateUpdate()
    {
        if (objetivo == null) return;
 
        // Posición X objetivo con offset
        float targetX = objetivo.position.x + offsetX;
 
        // Aplicar límites
        float mitadAnchoCamara = Camera.main.orthographicSize * Camera.main.aspect;
        targetX = Mathf.Clamp(targetX,
                              limiteIzquierdo + mitadAnchoCamara,
                              limiteDerecho   - mitadAnchoCamara);
 
        // Suavizado en X
        float nuevoX = Mathf.SmoothDamp(transform.position.x, targetX, ref velocidadX, suavizado);
 
        // Aplicar solo X; Y y Z permanecen fijas
        transform.position = new Vector3(nuevoX, yFija, zFija);
    }
 
    // Dibuja los límites en el editor para verlos fácilmente
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        float h = 10f;
        Gizmos.DrawLine(new Vector3(limiteIzquierdo, -h, 0), new Vector3(limiteIzquierdo, h, 0));
        Gizmos.DrawLine(new Vector3(limiteDerecho,   -h, 0), new Vector3(limiteDerecho,   h, 0));
    }
}