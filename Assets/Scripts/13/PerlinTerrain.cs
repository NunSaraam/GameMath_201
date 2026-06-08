using UnityEngine;

public class PerlinTerrain : MonoBehaviour
{
    public int width = 30;
    public int depth = 30;
    public float scale = 0.1f;
    public float heightMultiplier = 8f;

    public GameObject dirtPrefab;
    public GameObject grassPrefab;
    public GameObject waterPrefab;

    public int waterHeight = 3;

    private SimplePerlinNoise simpleNoise;
    private float offsetX;
    private float offsetZ;

    void Start()
    {
        simpleNoise = GetComponent<SimplePerlinNoise>();

        offsetX = Random.Range(-9999f, 9999f);
        offsetZ = Random.Range(-9999f, 9999f);

        Generate();
    }

    public void Generate()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float xCoord = (x + offsetX) * scale;
                float zCoord = (z + offsetZ) * scale;

                float noise = simpleNoise.Noise(xCoord, zCoord);

                int height = Mathf.RoundToInt(Mathf.Clamp01(noise) * heightMultiplier);

                CreateCube(x, z, height);
            }
        }
    }

    void CreateCube(int x, int z, int height)
    {
        for (int y = 0; y <= height; y++)
        {
            GameObject prefabToInstantiate = dirtPrefab;

            if (y == height)
            {
                prefabToInstantiate = grassPrefab;
            }

            Vector3 position = new Vector3(x, y, z);
            Instantiate(prefabToInstantiate, position, Quaternion.identity, transform);
        }

        if (height < waterHeight)
        {
            for (int y = height + 1; y <= waterHeight; y++)
            {
                Vector3 position = new Vector3(x, y, z);
                Instantiate(waterPrefab, position, Quaternion.identity, transform);
            }
        }
    }
}
