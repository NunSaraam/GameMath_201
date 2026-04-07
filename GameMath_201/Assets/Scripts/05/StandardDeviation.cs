using System.Linq;
using UnityEngine;

public class StandardDeviation : MonoBehaviour
{
    public int sample;
    public int min;
    public int max;

    public float mean = 10.0f;
    public float stddev = 1.0f;
    private void Start()
    {
        StandardDev();
    }
    void StandardDev()
    {
        int n = sample;
        float[] samples = new float[n];
        for (int i = 0; i < n; i++)
        {
            samples[i] = Random.Range(min, max);
        }

        float mean = samples.Average();
        float sumOfSquares = samples.Sum(x => Mathf.Pow(x -  mean, 2));
        float stdDev = Mathf.Sqrt(sumOfSquares / n);

        Debug.Log($"평균 : {mean}, 표준편차 : {stdDev}");
    }

    float GenerateGaussian(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;

        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);

        return mean + stdDev * randStdNormal;
    }

    public void Generate()
    {
        Debug.Log(GenerateGaussian(mean, stddev));
    }
}
