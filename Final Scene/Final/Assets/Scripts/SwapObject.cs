using UnityEngine;

public class SwapObject : MonoBehaviour
{
    // ALL objects that change should have this script
    // this should be idealy attached to the parent and should have two children (past object and present object)
    // the past object will already be active, and this script swaps the present (past -> present)
    // an object with this script should be assigned to its corresponding tag based on which number interaction it is

    public GameObject swapFrom;
    public GameObject swapTo;

    public void Swap()
    {
        swapFrom.SetActive(false);
        swapTo.SetActive(true);
    }
}
