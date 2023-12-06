using UnityEngine;
using System.Collections;

public class GiftCollider : MonoBehaviour
{
    [SerializeField] private float timeTillNextGiftGrab;
    private bool isCooldown = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the player and if the cooldown is not active
        if (other.CompareTag("Player") && !isCooldown)
        {
            // Call the RemoveGifts method from the UIManager Singleton
            UIManager.Instance.RemoveGifts();

            // Start the cooldown coroutine
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        // Set cooldown to true
        isCooldown = true;

        // Wait for TimeUntilNextGiftGrab seconds
        yield return new WaitForSeconds(timeTillNextGiftGrab);

        // Reset cooldown
        isCooldown = false;
    }
}