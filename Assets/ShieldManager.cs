using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    [Header("Shield Grid")]
    public int width = 5;         // number of blocks horizontally
    public int height = 3;        // number of blocks vertically
    public float blockSize = 1f; // size of each block
    public GameObject blockPrefab; // a simple cube or quad prefab

    [Header("Shield Instances")]
    public int shieldInstances = 3;    // shields to left/right of center
    public float shieldSpacing = 8f;   // distance between shield centers

    private GameObject[,] shieldBlocks;

    void Start()
    {
        SpawnMultipleShields();
    }

    void SpawnMultipleShields()
    {
        // center shield
        CreateShieldAtPosition(transform.position);

        for (int i = 1; i <= shieldInstances; i++)
        {
            // right side
            Vector3 rightPos = transform.position + new Vector3(i * shieldSpacing, 0f, 0f);
            CreateShieldAtPosition(rightPos);

            // left side
            Vector3 leftPos = transform.position + new Vector3(-i * shieldSpacing, 0f, 0f);
            CreateShieldAtPosition(leftPos);
        }
    }

    void CreateShieldAtPosition(Vector3 position)
    {
        shieldBlocks = new GameObject[width, height];

        Vector3 startPos = position;
        startPos.x -= (width - 1) * blockSize * 0.5f;
        startPos.z -= (height - 1) * blockSize * 0.5f;

        float curveStrength = 0.5f; // adjust how curved the shield is

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // apply curve along z-axis
                float curveOffset = Mathf.Sin((float)x / (width - 1) * Mathf.PI) * curveStrength;

                Vector3 blockPos = startPos + new Vector3(x * blockSize, 0f, z * blockSize + curveOffset);
                GameObject block = Instantiate(blockPrefab, blockPos, Quaternion.identity, transform);
                block.transform.localScale = Vector3.one * blockSize;
                block.AddComponent<BoxCollider>();
                block.AddComponent<ShieldBlock>(); // script handles collisions/destruction
                shieldBlocks[x, z] = block;
            }
        }
    }
}
