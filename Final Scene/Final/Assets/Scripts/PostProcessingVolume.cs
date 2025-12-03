using UnityEngine;

public class PostProcessingVolume : MonoBehaviour
{
    private float distance = 3f;

    Vector3 start;
    Vector3 target;
    float duration;
    float t;
    bool moving = false;

    public void FadeOut(float seconds)
    {
        start = transform.position;
        target = start + Vector3.up * distance;

        duration = seconds;
        t = 0f;
        moving = true;
    }

    void Update()
    {
        if (!moving) return;

        t += Time.deltaTime / duration;
        transform.position = Vector3.Lerp(start, target, t);

        if (t >= 1f)
            moving = false;
    }
}
