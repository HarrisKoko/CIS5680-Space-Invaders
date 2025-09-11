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

    [Header("UFO")]
    public GameObject ufoPrefab;
    public float ufoSpawnIntervalMin = 20f;
    public float ufoSpawnIntervalMax = 40f;
    private float nextUfoTime = 0f;

    [Header("Game Over / Level Cleared Settings")]
    public float deathZ = 0f; // Z position where aliens reaching triggers game over
    private bool gameEnded = false;

    private List<GameObject> aliens = new List<GameObject>();
    private bool edgeHit = false;

    private Vector3 ufoSpawnPosition = new Vector3(-20f, 0f, 10f);

    void Start()
    {
        SpawnAliens();
        ScheduleNextUFO();
    }

    void Update()
    {
        if (gameEnded) return; // stop updates if game over or level cleared

        UpdateSpeed();
        MoveAliens();
        HandleAlienFiring();
        HandleUFOSpawn();

        CheckAliensReachedBottom();
        CheckLevelCleared();
    }

    void SpawnAliens()
    {
        aliens.Clear();

        int zoneSize = Mathf.CeilToInt((float)rows / 3);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 pos = new Vector3(col * spacing, 0f, row * spacing);
                Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
                GameObject alien = Instantiate(alienPrefab, pos, rotation, transform);


                Alien alienScript = alien.GetComponent<Alien>();
                if (alienScript != null)
                {
                    if (row < zoneSize)
                        alienScript.scoreValue = 10;
                    else if (row < 2 * zoneSize)
                        alienScript.scoreValue = 20;
                    else
                        alienScript.scoreValue = 30;
                }

                aliens.Add(alien);
            }
        }
    }

    void MoveAliens()
    {
        transform.position += moveDirection * baseSpeed * Time.deltaTime;

        bool hitEdge = aliens.Any(a => a != null &&
            (Camera.main.WorldToViewportPoint(a.transform.position).x < 0.05f ||
             Camera.main.WorldToViewportPoint(a.transform.position).x > 0.95f));

        if (hitEdge && !edgeHit)
        {
            moveDirection = -moveDirection;
            transform.position += Vector3.back * dropDistance;
            edgeHit = true;
        }
        else if (!hitEdge)
        {
            edgeHit = false;
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
                bulletScript.isPlayerBullet = false;
        }
    }

    void HandleUFOSpawn()
    {
        if (ufoPrefab == null) return;

        if (Time.time >= nextUfoTime)
        {
            SpawnUFO();
            ScheduleNextUFO();
        }
    }

    void SpawnUFO()
    {
        Vector3 spawnPos = new Vector3(-20f, 0f, 10f);
        Quaternion spawnRot = Quaternion.Euler(90f, 0f, 0f);
        GameObject ufo = Instantiate(ufoPrefab, spawnPos, spawnRot);
        ufo.transform.localScale = Vector3.one;

        UFO ufoScript = ufo.GetComponent<UFO>();
        if (ufoScript != null)
            ufoScript.movingRight = true;
    }

    void ScheduleNextUFO()
    {
        nextUfoTime = Time.time + Random.Range(ufoSpawnIntervalMin, ufoSpawnIntervalMax);
    }

    void CheckAliensReachedBottom()
    {
        foreach (var alien in aliens)
        {
            if (alien != null && alien.transform.position.z <= deathZ)
            {
                gameEnded = true;
                GameManager.Instance.TriggerGameOver();
                break;
            }
        }
    }

    void CheckLevelCleared()
    {
        if (aliens.All(a => a == null))
        {
            gameEnded = true;
            GameManager.Instance.TriggerLevelCleared();
        }
    }
}
