using UnityEngine;

public class ReflectAuto : MonoBehaviour
{
    public Vector3 velocity = new Vector3(2f, -3f, 0f);

    private void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    void OnCollisionEnter(Collision col)
    {
        Vector3 normal = col.contacts[0].normal.normalized;

        Vector3 reflect = Vector3.Reflect(velocity, normal);

        velocity = reflect;
    }
}
