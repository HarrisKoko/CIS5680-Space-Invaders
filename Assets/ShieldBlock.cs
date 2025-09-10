using UnityEngine;

public class ShieldBlock : MonoBehaviour
{
    public int hitsToDestroy = 1;  // how many hits before disappearing

    private int hitCount = 0;

    void OnCollisionEnter(Collision collision)
    {
        // Detect bullets (player or alien)
        if (collision.gameObject.CompareTag("AlienBullet"))
        {
            hitCount++;
            if (hitCount >= hitsToDestroy)
            {
                Destroy(gameObject);
            }

            // Optionally destroy the bullet
            Destroy(collision.gameObject);
        }
    }
}
