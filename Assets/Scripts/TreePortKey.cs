using UnityEngine;
using HolidayMaze;

public class TreePortKey : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the player and if the cooldown is not active
        if (other.CompareTag("Player"))
        {
            // Call the RemoveGifts method from the UIManager Singleton
            MazeRenderer.Instance.BeginLevel();
        }
    }
}