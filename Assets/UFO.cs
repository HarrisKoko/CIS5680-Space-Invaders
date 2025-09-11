using UnityEngine;

public class UFO : MonoBehaviour
{
    public float speed = 10f;           
    public int scoreValue = 100;        
    public float lifeTime = 10f;        
    public bool movingRight = true;     

    private float timer = 0f;

    void Update()
    {
        // Move horizontally based on direction
        float dir = movingRight ? 1f : -1f;
        transform.position += Vector3.right * dir * speed * Time.deltaTime;

        // Destroy UFO after lifeTime
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        // Destroy if out of screen bounds
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPos.x < -0.1f || viewportPos.x > 1.1f)
        {
            Destroy(gameObject);
        }
    }
    public GameObject deathExplosion;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(scoreValue);
            Instantiate(deathExplosion, gameObject.transform.position,
            Quaternion.AngleAxis(-90, Vector3.right));
            Destroy(gameObject);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
