using UnityEngine;

public class QSkill : MonoBehaviour
{
    public float speed = 10f;
    public float gravity = 9.81f;

    private Vector3 velocity;
    private int groundTouchCount = 0;
    private bool isInitialized = false;

    public void Initialize(Vector3 fireDirection)
    {
        velocity = fireDirection * speed + Vector3.up * 5f;
        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized) return;

        velocity.y -= gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Explode();
            return;
        }

        if (col.gameObject.CompareTag("Ground"))
        {
            groundTouchCount++;
            if (groundTouchCount >= 3)
            {
                Explode();
                return;
            }
        }

        Vector3 normal = col.contacts[0].normal.normalized;
        Vector3 reflect = velocity - 2 * Vector3.Dot(velocity, normal) * normal;

        velocity = reflect * 0.8f;
    }

    void Explode()
    {
        Destroy(gameObject);
    }
}
