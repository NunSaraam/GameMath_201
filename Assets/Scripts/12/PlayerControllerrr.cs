using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerrr : MonoBehaviour
{
    [Header("Skill Prefabs")]
    public GameObject bombPrefab;
    public GameObject minePrefab;

    [Header("Skill Settings")]
    public float mineSpawnDistance = 3f;

    public float bombSpawnForwardOffset = 1.5f;
    public float bombSpawnUpOffset = 1.0f;

    public void OnSkillQ(InputValue value)
    {
        if (value.isPressed)
        {
            Vector3 spawnPos = transform.position
                             + (transform.forward * bombSpawnForwardOffset)
                             + (Vector3.up * bombSpawnUpOffset);

            GameObject q = Instantiate(bombPrefab, spawnPos, transform.rotation);

            QSkill qSkill = q.GetComponent<QSkill>();
            if (qSkill != null)
            {
                Vector3 fireDirection = transform.forward;
                qSkill.Initialize(fireDirection);
            }
        }
    }

    public void OnSkillW(InputValue value)
    {
        if (value.isPressed)
        {
            Vector3 spawnPos = transform.position + (transform.forward * mineSpawnDistance);
            Instantiate(minePrefab, spawnPos, Quaternion.identity);
        }
    }
}
