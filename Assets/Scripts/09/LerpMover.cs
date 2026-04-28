using UnityEngine;

public class LerpMover : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;

    [SerializeField] private float duration = 2f;
    [SerializeField] private float t = 0f;

    private void Update()
    {
        if (t < 1f)
        {
            //실습 1
            //t += Time.deltaTime / duration;

            //실습 2
            t = Mathf.PingPong(Time.time / duration, 1f);

            Vector3 a = startPos.position;
            Vector3 b = endPos.position;
            Vector3 p = (1f - t) * a + t * b;

            transform.position = p;
        }
    }
}
