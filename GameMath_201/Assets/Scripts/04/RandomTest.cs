using UnityEngine;

public class RandomTest : MonoBehaviour
{
    private void Start()
    {
        float chance = Random.value;
        int dice = Random.Range(1, 7);

        System.Random sysRandom = new System.Random();
        int number = sysRandom.Next(1, 7);

        Debug.Log("Unity Random(Random.value) : " + chance);
        Debug.Log("Unity Random(Random.Range) : " + dice);
        Debug.Log("System Random(Next) : " + number);
    }
}
