using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float detectionRange = 2f;
    public GameObject canvas;

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);

        bool inRange = false;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                inRange = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Perform interaction.");
                    break;
                }
            }
        }
        canvas.SetActive(inRange);
    }

    // visualize detection range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
