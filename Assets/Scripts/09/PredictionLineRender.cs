using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(LineRenderer))]
public class PredictionLineRender : MonoBehaviour
{
    Vector3 originPos;

    public Transform startPos;
    public Transform endPos;
    [Range(1f, 5f)] public float extend = 1.5f;
    private LineRenderer lr;
    [SerializeField] private CameraSlerp cmaSlerp;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.widthMultiplier = .05f;
        lr.material = new Material(Shader.Find("Unlit/Color"))
        {
            color = Color.red
        };
    }

    public void OnRightClick(InputValue value)
    {
        if (!value.isPressed) return;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                lr.enabled = true;
                cmaSlerp.target = hit.collider.transform;
                endPos = hit.collider.transform;
            }
        }
        else
        {
            cmaSlerp.target = null;
            Origin();
        }
    }

    public void Origin()
    {
        endPos = null;
        lr.enabled = false;
    }

    private void Update()
    {
        if (!startPos || !endPos) return;
        Vector3 a = startPos.position;
        Vector3 b = endPos.position;
        Vector3 pred = Vector3.LerpUnclamped(a, b, extend);
        lr.SetPosition(0, a);
        lr.SetPosition(1, pred);
    }
}
