using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0, 45 * Time.deltaTime, 0);
    }
}
