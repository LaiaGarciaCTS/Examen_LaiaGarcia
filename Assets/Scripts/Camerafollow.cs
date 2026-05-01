using UnityEngine;
 
// =====================================================================
//  CAMERA FOLLOW
//  Coloca este script en la Main Camera.
//  La cámara sigue al jugador con suavizado y puede tener límites de mapa.
// =====================================================================
 
public class CameraFollow : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform objetivo;             // Arrastra el GameObject del jugador
 
    [Header("Suavizado")]
    [Range(0f, 1f)]
    public float suavizado = 0.12f;        // 0 = instantáneo, 1 = muy lento
 
    [Header("Offset")]
    public Vector3 offset = new Vector3(0f, 1f, -10f);  // La Z debe ser negativa en 2D
 
    [Header("Límites del mapa (opcional)")]
    public bool usarLimites = false;
    public float limiteMinX = -10f;
    public float limiteMaxX =  10f;
    public float limiteMinY = -5f;
    public float limiteMaxY =  5f;
 
    private Vector3 velocidadActual = Vector3.zero;
 
    void LateUpdate()
    {
        if (objetivo == null) return;
 
        Vector3 posicionObjetivo = objetivo.position + offset;
 
        if (usarLimites)
        {
            posicionObjetivo.x = Mathf.Clamp(posicionObjetivo.x, limiteMinX, limiteMaxX);
            posicionObjetivo.y = Mathf.Clamp(posicionObjetivo.y, limiteMinY, limiteMaxY);
        }
 
        // Mantener la Z de la cámara
        posicionObjetivo.z = transform.position.z;
 
        transform.position = Vector3.SmoothDamp(
            transform.position,
            posicionObjetivo,
            ref velocidadActual,
            suavizado
        );
    }
}