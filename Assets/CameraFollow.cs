using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target-Einstellungen")]
    public Transform target; // Spieler (z. B. Player)

    [Header("Kamera-Offset")]
    public float zOffset = -10f; // typischer Wert für 2D-Kameras

    [Header("Kamera-Nachlauf")]
    [Range(0f, 10f)]
    public float smoothSpeed = 5f;

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Nur X und Y folgen – Z bleibt konstant
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, zOffset);

        // Weiches Nachziehen
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}
