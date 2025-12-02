using UnityEngine;

public class PostProcessingVolume : MonoBehaviour
{
    public float distance = 3f;
    public float speed = 1f;

    Vector3 target;
    bool moving = false;

    public void FadeOut()
    {
        target = transform.position + Vector3.up * distance;
        moving = true;
    }

    void Update()
    {
        if (!moving) return;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position == target)
            moving = false;
    }
}
