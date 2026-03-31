using UnityEngine;

public class CriticalManager : MonoBehaviour
{
    public int totalHits = 0;
    public int critHits = 0;
    public float targetRate = .1f;

    public bool RollCrit()
    {
        totalHits++;
        float currentRate = 0f;
        if (critHits > 0)
        {
            currentRate = (float) critHits / totalHits;
        }

        if (currentRate < targetRate && (float)(critHits + 1) / totalHits <= targetRate)
        {
            Debug.Log("Critical!, (Forced)");
            critHits++;
            return true;
        }

        if (currentRate > targetRate && (float)critHits / totalHits >= targetRate)
        {
            Debug.Log("Default. (Forced)");
            return false;
        }

        if (Random.value < targetRate)
        {
            Debug.Log("Critical!, (Base)");
            critHits++;
            return true;
        }

        Debug.Log("Default. (Base)");
        return false;
    }

    public void SimulateCritical()
    {
        RollCrit();
        Debug.Log("Total Hits : " + totalHits);
        Debug.Log("Critical Hits : " + critHits);
        Debug.Log("Current Critical Rate : " + (float)critHits / totalHits);
    }
}
