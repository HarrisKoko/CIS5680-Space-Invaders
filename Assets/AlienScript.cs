using UnityEngine;

public class Alien : MonoBehaviour
{
    public int scoreValue = 10; 
    public AudioClip deathSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(scoreValue);

            Die();

            Destroy(collision.gameObject);
        }
    }
    public GameObject deathExplosion;
    public void Die()
    {
        if (deathSound != null)
        {
            // Create a temporary GameObject for the sound
            GameObject tempAudio = new GameObject("TempAudio");
            tempAudio.transform.position = transform.position;

            AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
            audioSource.clip = deathSound;
            audioSource.volume = 100f;        
            audioSource.spatialBlend = 0f;    
            audioSource.Play();

            Destroy(tempAudio, deathSound.length); 
        }
        else
        {
            Debug.Log("NO AUDIOSOURCE");
        }
        Instantiate(deathExplosion, gameObject.transform.position,
        Quaternion.AngleAxis(-90, Vector3.right));
        Destroy(gameObject);
        Destroy(gameObject);
    }

}
