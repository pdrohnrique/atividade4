using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 6f, -8f);
    public float smoothSpeed = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        // Acompanha a bolinha suavemente mantendo a distância do offset
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Faz a câmera olhar para o player
        transform.LookAt(target);
    }
}