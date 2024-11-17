using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    public Vector3 offset; // Offset to maintain relative to the player
    public float smoothSpeed = 0.125f; // Smoothness of the camera movement

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.LookAt(player); // Ensures the camera looks at the player
        }
    }
}
