using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    public Transform centerTarget;

    [Header("공전 설정")]
    public float radius = 5f;       
    public float moveSpeed = 2f;        

    // 누적될 각도 변수
    private float currentAngle = 0f;

    void Update()
    {
        // 타겟이 할당되어 있을 때만 실행
        if (centerTarget != null)
        {
            // Time.deltaTime을 곱해 프레임에 상관없이 일정한 속도로 각도 증가
            currentAngle += moveSpeed * Time.deltaTime;

            // 삼각함수(sin, cos)를 이용해 X, Z 좌표 계산 (3D 공간의 가로/세로 평면)
            float x = centerTarget.position.x + Mathf.Cos(currentAngle) * radius;
            float z = centerTarget.position.z + Mathf.Sin(currentAngle) * radius;

            // 계산된 위치를 현재 오브젝트에 적용 (Y축은 그대로)
            transform.position = new Vector3(x, transform.position.y, z);
        }
    }
}
