using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float height = 10f; // Height above the player

    void LateUpdate()
    {
        if (player != null)
        {
            // Set the camera's position to the player's position, but offset by 'height' units above
            transform.position = new Vector3(player.position.x, player.position.y + height, player.position.z);

            // Optional: Set the camera to look at the player
            // transform.LookAt(player);
        }
    }
}