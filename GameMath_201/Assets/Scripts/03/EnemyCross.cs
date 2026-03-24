using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCross : MonoBehaviour
{
    public Transform target;
    public float viewAngle = 60f;
    public float viewDistance = 5f;
    public float dashSpeed = 15f;
    public float rotationAngle = 30f;

    bool isDashing = false;
    Rigidbody rb;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        Vector3 forward = transform.forward;
        Vector3 dirToTarget = (target.position - transform.position).normalized;

        float targetDistance = (target.position - transform.position).magnitude;

        float dot = DotProduct(forward, dirToTarget);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (!isDashing)
        {
            transform.Rotate(Vector3.up * rotationAngle * Time.deltaTime);

            if (targetDistance < viewDistance && angle < viewAngle)
            {
                isDashing = true;
            }
        }
        else
        {
            Vector3 moveVelocity = dirToTarget * dashSpeed;
            
            rb.linearVelocity = new Vector3(moveVelocity.x, moveVelocity.y, moveVelocity.z);

            if (targetDistance < 1f)
            {
                CheckParry();
            }
        }

        //실습 1
        /*
        Vector3 crossProduct = Vector3.Cross(forward, dirToTarget);

        if (crossProduct.y > .1f)
        {
            Debug.Log("적이 오른쪽에 있습니다.");
        }
        else if (crossProduct.y < -.1f)
        {
            Debug.Log("적이 왼쪽에 있습니다.");
        } */
        //실습 2    
        /*
        Vector3 crossProduct = CrossProduct(forward, dirToTarget);

        if (crossProduct.y > .1f)
        {
            Debug.Log("적이 오른쪽에 있습니다.");
        }
        else if (crossProduct.y < -.1f)
        {
            Debug.Log("적이 왼쪽에 있습니다.");
        }       */
    }

    void CheckParry()
    {
        PlayerController pc = target.GetComponent<PlayerController>();
        if (pc.isLeftParry || pc.isRightParry)
        {
            Vector3 forward = transform.forward;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            Vector3 crossProduct = CrossProduct(forward, dirToTarget);

            if (crossProduct.y > .1f && pc.isRightParry)
            {
                GameObject.Destroy(this.gameObject);
            }
            else if (crossProduct.y < -.1f && pc.isLeftParry)
            {
                GameObject.Destroy(this.gameObject);
            }

        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    Vector3 CrossProduct(Vector3 A, Vector3 B)
    {
        return new Vector3(
            A.y * B.z - A.z * B.y,
            A.z * B.x - A.x * B.z,
            A.x * B.y - A.y * B.x
            );
    }       //외적

    float DotProduct(Vector3 A, Vector3 B)
    {
        return (A.x * B.x + A.y * B.y + A.z * B.z);
    }           //내적
}
