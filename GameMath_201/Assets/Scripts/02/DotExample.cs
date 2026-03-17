using UnityEngine;

public class DotExample : MonoBehaviour
{
    public Transform player;
    public float viewAngle = 60f;
    public float viewDistance = 5f;
    private bool checkPlayer = false;
    private void Update()
    {
        //실습 3
        /*
        Vector3 toPlayer = player.position - transform.position;        //플레이어를 보는 방향
        toPlayer.y = 0;

        Vector3 forward = transform.forward;        //적의 앞 방향
        forward.y = 0;

        forward.Normalize();
        toPlayer.Normalize();

        float dot = Vector3.Dot(forward, toPlayer);         //내적

        if (dot > 0.2f)
        {
            Debug.Log("플레이어가 적 앞");
        }
        else if (Mathf.Abs(dot) < 0.2f)
        {
            Debug.Log("플레이어가 적 뒤");
        }
        else
        {
            Debug.Log("플레이어가 적 옆");
        }*/

        //실습 4
        /*
        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 forward = transform.forward;

        float dot = Vector3.Dot(forward, toPlayer);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (angle < viewAngle / 2)
        {
            Debug.Log("플레이어가 시야 안에 있음!");
        }*/

        //실습 5
        Vector3 toPlayer = (player.position - transform.position).normalized;
        float playerDistance = (player.position - transform.position).magnitude;
        Vector3 forward = transform.forward;

        float dot = DotProduct(forward, toPlayer);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (playerDistance < viewDistance)
        {
            if (angle < viewAngle / 2)
            {
                checkPlayer = true;
                transform.localScale = new Vector3(2, 2, 2);

                Debug.Log("플레이어가 시야 안에 있음!");
            }
        }
        else
        {
            checkPlayer = false;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public float DotProduct(Vector3 A, Vector3 B)
    {
        return (A.x * B.x + A.y * B.y + A.z * B.z);
    }
}
