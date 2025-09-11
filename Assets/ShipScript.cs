using UnityEngine;

public class Ship : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public float bulletOffset = 1.5f;
    public int lives = 3;

    [Header("Shooting")]
    public float fireCooldown = 0.5f;
    private float lastFireTime = 0f;

    void Start()
    {
        GameManager.Instance?.UpdateLivesUI(lives);
    }

    void Update()
    {
        // Ship movement (left/right)
        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector3 move = new Vector3(moveInput, 0f, 0f);
        transform.position += move * moveSpeed * Time.deltaTime;

        // Clamp X position to viewport
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        viewportPos.x = Mathf.Clamp(viewportPos.x, 0.05f, 0.95f);
        transform.position = Camera.main.ViewportToWorldPoint(viewportPos);

        // Fire bullet with cooldown
        if (Input.GetButtonDown("Fire1") && Time.time - lastFireTime >= fireCooldown)
        {
            Vector3 spawnPos = transform.position + new Vector3(0f, 0f, bulletOffset);
            Quaternion spawnRot = Quaternion.Euler(90f, 0f, 0f);
            Instantiate(bulletPrefab, spawnPos, spawnRot);
            lastFireTime = Time.time;
        }
    }
    public AudioClip explosionSound;
    public GameObject deathExplosion;
    public void LoseLife()
    {
        lives--;
        GameManager.Instance?.UpdateLivesUI(lives); 

        if (explosionSound != null)
        {
 
            GameObject tempAudio = new GameObject("TempAudio");
            tempAudio.transform.position = transform.position;

            AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
            audioSource.clip = explosionSound;
            audioSource.volume = 100f;       
            audioSource.spatialBlend = 0f;   
            audioSource.Play();

            Destroy(tempAudio, explosionSound.length);
        }
        else
        {
            Debug.Log("NO AUDIOSOURCE");
        }

        if (lives <= 0)
            Die();
    }




    public void Die()
    {

        Instantiate(deathExplosion, gameObject.transform.position,
        Quaternion.AngleAxis(-90, Vector3.right));
        Destroy(gameObject);
        Destroy(gameObject);
    }


}
