using UnityEngine;

public class CameraSlerp : MonoBehaviour
{
    Quaternion originRot;
    public Transform target;
    float speed = 2f;

    private void Update()
    {
        Quaternion lookRot = Quaternion.LookRotation(target.position - transform.position);

        float t = 1f - Mathf.Exp(-speed * Time.deltaTime);
        transform.rotation = ManualSlerp(transform.rotation, lookRot, t);
    }

    Quaternion ManualSlerp(Quaternion form, Quaternion to, float t)
    {
        float dot = Quaternion.Dot(form, to);

        if (dot < 0f)
        {
            to = new Quaternion(-to.x, -to.y, -to.z, -to.w);
            dot = -dot;
        }

        if (1f - dot < .01f)
        {
            Quaternion lerp = new Quaternion(
                Mathf.Lerp(form.x, to.x, t),
                Mathf.Lerp(form.y, to.y, t),
                Mathf.Lerp(form.z, to.z, t),
                Mathf.Lerp(form.w, to.w, t)
            );
            return lerp.normalized;
        }

        float theta = Mathf.Acos(dot);
        float sinTheta = Mathf.Sin(theta);

        float ratioA = Mathf.Sin((1f - t) * theta) / sinTheta;
        float ratioB = Mathf.Sin(t * theta) / sinTheta;

        Quaternion result = new Quaternion(
            ratioA * form.x + ratioB * to.x,
            ratioA * form.y + ratioB * to.y,
            ratioA * form.z + ratioB * to.z,
            ratioA * form.w + ratioB * to.w
        );
        return result.normalized;
    }
}
