using UnityEngine;

public class Alien : MonoBehaviour
{
    public int scoreValue = 10;  // points awarded when killed

    void OnCollisionEnter(Collision collision)
    {
        // Only respond to player bullets
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            // Notify GameController (optional) to add score
            //GameController.Instance?.AddScore(scoreValue);

            // Destroy the alien
            Destroy(gameObject);

            // Destroy the bullet
            Destroy(collision.gameObject);
        }
    }

    public void Die()
    {
        // Optional: play explosion VFX or sound here
        Destroy(gameObject);
    }
}
