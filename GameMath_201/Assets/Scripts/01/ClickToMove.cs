using UnityEngine;
using UnityEngine.InputSystem;

public class ClickToMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    private Vector2 mouseScreenPosition;
    private Vector3 targetPosition;

    private bool isMoving = false;
    private bool isSprinting = false;

    public void OnPoint(InputValue value)
    {
        mouseScreenPosition = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);            //레이 경로에 있는 모든 물체를 탐색

            foreach (RaycastHit hit in hits)            //모든 물체에 한해 반복
            {
                if (hit.collider.gameObject != gameObject)                  //물체가 내가 아닐때만
                {
                    targetPosition = hit.point;
                    targetPosition.y = transform.position.y;
                    isMoving = true;

                    break;      //탐색 했으니 foreach 반복 중단
                }
            }
        }
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }


    private void Update()
    {
        if (!isSprinting)
        {
            if (isMoving)
            {
                Vector3 direction = new Vector3(targetPosition.x - transform.position.x, 0, targetPosition.z - transform.position.z);

                float sqrMagnitude = direction.x * direction.x + direction.y * direction.y + direction.z * direction.z;
                float magnitude = Mathf.Sqrt(sqrMagnitude);

                if (magnitude > 0)
                    direction = direction / magnitude;

                transform.position += direction * moveSpeed * Time.deltaTime;

                if (magnitude < .1f)
                {
                    isMoving = false;
                }
            }
        }
        else
        {
            if (isMoving)
            {
                Vector3 direction = new Vector3(targetPosition.x - transform.position.x, 0, targetPosition.z - transform.position.z);

                float sqrMagnitude = direction.x * direction.x + direction.y * direction.y + direction.z * direction.z;
                float magnitude = Mathf.Sqrt(sqrMagnitude);

                if (magnitude > 0)
                    direction = direction / magnitude;

                transform.position += direction * sprintSpeed * Time.deltaTime;

                if (magnitude < .1f)
                {
                    isMoving = false;
                }
            }
        }
    }

    private void Movemnt()
    {

    }
}
