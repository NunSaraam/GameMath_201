using UnityEngine;

public class DistributionTest : MonoBehaviour
{
    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            int count = PoissonDistribution(3f);
            Debug.Log($"Munute {i + 1} : {count} events");
        }
    }

    int PoissonDistribution(float lambda)
    {
        int k = 0;
        float p = 1f;
        float L = Mathf.Exp(-lambda);

        while (p > L)
        {
            k++;
            p *= Random.value;
        }
        return k - 1;
    }
}
