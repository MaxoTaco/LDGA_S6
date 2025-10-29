using System.Collections;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public RatController ratController;
    public float restartAfterDeath = 2;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) StartCoroutine(Caught());
    }

    IEnumerator Caught()
    {
        // HERE: all sounds & animations play before restarting

        //Debug.Log("Caught!");
        ratController.SetFreezeMovement(true);
        
        yield return new WaitForSeconds(restartAfterDeath);

        //Debug.Log("Restarting");
        StartCoroutine(GetComponentInParent<CatBehavior>().cameraChange.Restart());
    }
}