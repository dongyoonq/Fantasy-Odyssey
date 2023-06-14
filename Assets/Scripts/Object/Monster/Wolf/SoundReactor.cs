using System.Collections;
using UnityEngine;

public class SoundReactor: MonoBehaviour, IHearable
{
    public float rotSpeed;

    public void Hear(Transform source)
    {
        StartCoroutine(LookAtRoutine(source));
    }

    IEnumerator LookAtRoutine(Transform source)
    {
        float rate = 0f;
        Vector3 dir = (source.position - transform.position).normalized;

        while (rate < 0.5f)
        {
            rate += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rate);
            yield return null;
        }


    }
}