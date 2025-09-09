using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AlienManager : MonoBehaviour
{
    [Header("Alien Setup")]
    public GameObject alienPrefab;
    public int rows = 5;
    public int cols = 11;
    public float spacing = 10f;

    [Header("Movement")]
    public float baseSpeed = 1.5f;
    public float maxSpeed = 8f;
    public float dropDistance = 0.5f;
    private Vector3 moveDirection = Vector3.right;

    [Header("Firing")]
    public GameObject alienBulletPrefab;
    public float fireInterval = 2f;
    private float lastFireTime = 0f;

    private List<GameObject> aliens = new List<GameObject>();

    void Start()
    {
        SpawnAliens();
    }

    void Update()
    {
        UpdateSpeed();
        MoveAliens();
        HandleAlienFiring();
    }

    void SpawnAliens()
    {
        aliens.Clear();
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 pos = new Vector3(col * spacing, 0f, row * spacing);
                GameObject alien = Instantiate(alienPrefab, pos, Quaternion.identity, transform);
                aliens.Add(alien);
            }
        }
    }

    private bool edgeHit = false;

    void MoveAliens()
    {
        transform.position += moveDirection * baseSpeed * Time.deltaTime;

        bool hitEdge = false;

        foreach (GameObject alien in aliens)
        {
            if (alien != null)
            {
                Vector3 viewportPos = Camera.main.WorldToViewportPoint(alien.transform.position);
                if (viewportPos.x < 0.05f || viewportPos.x > 0.95f)
                {
                    hitEdge = true;
                    break; // only need one alien to trigger
                }
            }
        }

        if (hitEdge && !edgeHit)
        {
            moveDirection = -moveDirection;
            transform.position += Vector3.back * dropDistance;
            edgeHit = true; // mark that we've already dropped
        }
        else if (!hitEdge)
        {
            edgeHit = false; // reset when back in bounds
        }
    }


    void UpdateSpeed()
    {
        int aliveCount = aliens.Count(a => a != null);
        float t = 1f - ((float)aliveCount / (rows * cols));
        baseSpeed = Mathf.Lerp(1.5f, maxSpeed, t);
    }

    void HandleAlienFiring()
    {
        if (Time.time - lastFireTime >= fireInterval)
        {
            FireFromColumn();
            lastFireTime = Time.time;
        }
    }

    void FireFromColumn()
    {
        int col = Random.Range(0, cols);
        GameObject bottomAlien = null;

        // Loop from bottom row (row = 0) to top
        for (int row = 0; row < rows; row++)
        {
            int index = row * cols + col;
            if (index < aliens.Count && aliens[index] != null)
            {
                bottomAlien = aliens[index];
                break;
            }
        }

        if (bottomAlien != null)
        {
            Vector3 spawnPos = bottomAlien.transform.position + Vector3.back * 0.5f;
            Quaternion spawnRot = Quaternion.Euler(90f, 0f, 0f);

            GameObject bullet = Instantiate(alienBulletPrefab, spawnPos, spawnRot);
            BulletScript bulletScript = bullet.GetComponent<BulletScript>();
            if (bulletScript != null)
                bulletScript.isPlayerBullet = false; // mark as alien bullet
        }
    }
}
