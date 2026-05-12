using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KaiSa : MonoBehaviour
{
    public GameObject missailePrefab;
    public Transform target;

    public void OnAttack(InputValue value)
    {
        if (!value.isPressed) return;

        Shooting();
    }

    public void Shooting()
    {
        for (int i = 0; i < 10; i++)
        {
            Bezier bezier = Instantiate(missailePrefab, transform.position, Quaternion.identity).GetComponent<Bezier>();

            bezier.point0 = this.transform;
            bezier.point3 = target;
            bezier.StartShooting();
        }
    }

}
