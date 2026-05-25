using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRaycast : MonoBehaviour
{
    public float rayDistance = 100f;
    float moveInput;
    public CameraOrbit cam;

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveInput = input.x;
        cam.moveInput = moveInput;
    }

    public void OnClick(InputValue value)
    {
        if (!value.isPressed) return;

        if (GameManager.Instance != null && GameManager.Instance.isBallsMoving) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb != null)
            {
                bool is1PBall = rb.gameObject.CompareTag("Player1");
                bool is2PBall = rb.gameObject.CompareTag("Player2");

                if ((GameManager.Instance.is1PTurn && is1PBall) ||
                    (!GameManager.Instance.is1PTurn && is2PBall))
                {
                    Vector3 hitPoint = hit.point;
                    Vector3 center = rb.transform.position;
                    Vector3 forceDirection = center - hitPoint;
                    forceDirection.y = 0f;
                    forceDirection.Normalize();

                    rb.AddForce(forceDirection * 10f, ForceMode.Impulse);

                    if (GameManager.Instance != null)
                        GameManager.Instance.OnTurnStarted();
                }
            }
        }
    }
}
