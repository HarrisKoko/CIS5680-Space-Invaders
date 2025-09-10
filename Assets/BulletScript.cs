using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 10f;
    public bool isPlayerBullet = true;

    void Update()
    {
        // Move bullet
        transform.position += Vector3.forward * (isPlayerBullet ? speed : -speed) * Time.deltaTime;

        // Destroy bullet if it's off screen
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPos.y < -0.1f || viewportPos.y > 1.1f || viewportPos.x < -0.1f || viewportPos.x > 1.1f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isPlayerBullet && collision.gameObject.CompareTag("Alien"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (!isPlayerBullet && collision.gameObject.CompareTag("Player"))
        {
            Ship playerShip = collision.gameObject.GetComponent<Ship>();
            if (playerShip != null)
                playerShip.LoseLife();

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
