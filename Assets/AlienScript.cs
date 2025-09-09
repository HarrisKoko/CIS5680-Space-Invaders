using UnityEngine;

public class Alien : MonoBehaviour
{
    public int scoreValue = 10;  // points awarded when killed

    void OnCollisionEnter(Collision collision)
    {
        // Only respond to player bullets
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            // Notify GameController to add score

            // Destroy the alien
            Destroy(gameObject);

            // Destroy the bullet
            Destroy(collision.gameObject);
        }
    }

    public void Die()
    {
        // Play explosion VFX or sound here
        Destroy(gameObject);
    }
}
