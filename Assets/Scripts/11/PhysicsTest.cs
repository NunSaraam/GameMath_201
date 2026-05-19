using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsTest : MonoBehaviour
{
    public float forcePower = 10;
    private Rigidbody rb;
    [SerializeField] private float speed;
    private bool isSprint;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnSprint(InputValue value)
    {
        isSprint = value.isPressed;
    }

    private void FixedUpdate()
    {
        if (isSprint)
        {
            rb.AddForce(Vector3.forward * forcePower, ForceMode.Force);
        }
    }

    private void Update()
    {
        speed = rb.linearVelocity.magnitude;
    }
}
