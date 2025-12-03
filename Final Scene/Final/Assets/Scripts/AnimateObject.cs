using UnityEngine;

public class AnimateObject : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void Animate()
    {
        Debug.Log("animating");

        anim.Play("toy_box_open");
    }
}
