using UnityEngine;

public class Alien : MonoBehaviour
{
    public int scoreValue = 10;  // points awarded when killed

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            // Add score
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(scoreValue);

            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }


    public void Die()
    {
        // Play explosion VFX or sound here
        Destroy(gameObject);
    }
}
