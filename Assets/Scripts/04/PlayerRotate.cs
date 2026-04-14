using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotate : MonoBehaviour
{
    public float rotationSpeed = 100f;

    private Vector2 moveInput;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {


        // 회전 실습 2
        /*
        float rotation = moveInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);     */

        // 회전 실습 3
        /*
        Quaternion rotation = Quaternion.Euler(0f, moveInput.x * rotationSpeed * Time.deltaTime, 0f);
        transform.rotation = rotation * transform.rotation;     */
    }

}
