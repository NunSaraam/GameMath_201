using UnityEngine;

public class RandomTerrain : MonoBehaviour
{
    public int width = 30;
    public int depth = 30;
    public float scale = .1f;
    public int minheight = 0;
    public int maxheight = 8;

    public GameObject cubePrefab;

    int xOffset = 0;
    int zOffset = 0;


    private void Start()
    {
        xOffset = Random.Range(-9999, 9999);
        zOffset = Random.Range(-9999, 9999);

        Generate();
    }

    public void Generate()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float noise = Mathf.PerlinNoise((x+ xOffset) * scale, (z + zOffset) * scale);
                int height = Mathf.RoundToInt(noise * maxheight);
                CreateCube(x, z, height);
            }
        }
    }

    void CreateCube(int x, int z, int height)
    {
        for (int y = 0; y <= height; y++)
        {
            Vector3 position = new Vector3(x, y, z);

            Instantiate(cubePrefab, position, Quaternion.identity, transform);
        }
    }
}
