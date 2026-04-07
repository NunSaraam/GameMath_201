using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;

    private Vector3 normalizeVector;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0);

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
}
