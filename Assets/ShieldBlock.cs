using UnityEngine;

public class ShieldBlock : MonoBehaviour
{
    public int hitsToDestroy = 1; 

    private int hitCount = 0;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("AlienBullet"))
        {
            hitCount++;
            if (hitCount >= hitsToDestroy)
            {
                Destroy(gameObject);
            }

            Destroy(collision.gameObject);
        }
    }
}
