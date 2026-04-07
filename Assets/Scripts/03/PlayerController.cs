using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float moveSpeed = 5f;

    private Vector2 moveInput;
    private Vector3 normalizeVector;

    public bool isLeftParry = false;
    public bool isRightParry = false;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLeftParry(InputValue value)
    {
        isLeftParry = value.isPressed;
    }

    public void OnRightParry(InputValue value)
    {
        isRightParry = value.isPressed;
    }

    private void Update()
    {
        PlayerMovement();
        PlayerRotation();
    }

    void PlayerMovement()
    {
        Vector3 direction = new Vector3(0, 0, moveInput.y);

        float sqrMagnitude = direction.x * direction.x + direction.y * direction.y + direction.z * direction.z;
        float magnitude = Mathf.Sqrt(sqrMagnitude);

        //0으로 나누기 방지
        if (magnitude > 0)
        {
            normalizeVector = direction / magnitude;
        }
        else
        {
            normalizeVector = Vector3.zero;
        }

        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    void PlayerRotation()
    {
        Quaternion rotation = Quaternion.Euler(0f, moveInput.x * rotationSpeed * Time.deltaTime, 0f);
        transform.rotation = rotation * transform.rotation;
    }
}
