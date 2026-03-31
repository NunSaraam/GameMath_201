using UnityEngine;

public class SystemRandomSeed : MonoBehaviour
{
    private void Start()
    {
        System.Random rnd = new System.Random(1234);

        for (int i = 0; i < 5; i++)
        {
            Debug.Log(rnd.Next(1, 7));
        }
    }
}
