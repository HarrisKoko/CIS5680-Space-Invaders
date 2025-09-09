using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 10f;
    public bool isPlayerBullet = true;

    void Update()
    {
        transform.position += Vector3.forward * (isPlayerBullet ? speed : -speed) * Time.deltaTime;
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
