using UnityEngine;

public class GiftPickup : MonoBehaviour
{
    // This function is called when another object enters a trigger collider attached to this object
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Access the UIManager singleton and call the FoundGift method
            UIManager.Instance.FoundGift();

            // Destroy this game object
            Destroy(gameObject);
        }
    }
}