using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // O Player deve ficar aqui no inspector
    public float smoothSpeed = 0.125f;
    public Vector3 offset; // Ajustar a distância Z (ex: -10)

    void FixedUpdate()
    {
        if (target != null)
        {
            // Pega a posição desejada (X do player, mas mantendo Y e Z da câmera se quiser travar altura)
            Vector3 desiredPosition = new Vector3(target.position.x, transform.position.y, transform.position.z) + offset;

            // Interpolação suave (Lerp)
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;
        }
    }
}
