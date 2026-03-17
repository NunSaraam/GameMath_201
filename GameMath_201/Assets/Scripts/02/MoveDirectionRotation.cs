using UnityEngine;
using UnityEngine.InputSystem;

public class MoveDirectionRotation : MonoBehaviour
{
    public int moveSpeed = 5;

    private Vector2 moveInput;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        MoveMent();
        LookAt();
    }

    public void MoveMent()
    {
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);

        float sqrMagnitude = direction.x * direction.x + direction.y * direction.y + direction.z * direction.z;
        float magnitude = Mathf.Sqrt(sqrMagnitude);

        //0으로 나누기 방지
        if (magnitude > 0)
        {
            direction = direction / magnitude;
        }
        else
        {
            direction = Vector3.zero;
        }

        transform.position += (direction * moveSpeed * Time.deltaTime);
    }
    public void LookAt()
    {
        if (moveInput.sqrMagnitude > .01f)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
}

