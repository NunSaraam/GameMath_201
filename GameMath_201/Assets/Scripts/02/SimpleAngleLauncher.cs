using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class SimpleAngleLauncher : MonoBehaviour
{
    public TMP_InputField angleInputField;
    public GameObject spherePrefab;
    public Transform firepoint;
    public float force = 15f;

    public void Launch()
    {
        float angle = float.Parse(angleInputField.text);
        float rad = angle * Mathf.Deg2Rad;

        Vector3 dir = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));

        GameObject sphere = Instantiate(spherePrefab, firepoint.position, quaternion.identity);
        Rigidbody rb = sphere.GetComponent<Rigidbody>();

        rb.AddForce((dir + Vector3.up * 0.3f).normalized * force,ForceMode.Impulse);
    }
}
