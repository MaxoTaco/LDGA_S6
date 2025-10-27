using System;
using System.Collections;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public Camera[] cameras;
    public GameObject[] startingPositions;
    public BoxCollider cameraTrigger;
    private CharacterController characterController;

    private int index = 0;

    public int RoomIndex => index;

    private void Awake()
    {
        index = 0;
        //transform.position = new Vector3(10f, 10, 10f);
        characterController = GetComponent<CharacterController>();

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == cameraTrigger.name)
        {

            cameras[index].gameObject.SetActive(false);

            index++;

            if (index == startingPositions.Length)
            {
                index = 0;
            }

            transform.position = startingPositions[index].transform.position;

            characterController.enabled = false;
            cameras[index].gameObject.SetActive(true);
            characterController.enabled = true;


            Debug.Log(startingPositions[index].gameObject.name);


        }
    }

    public IEnumerator Restart()
    {
        transform.position = startingPositions[index].transform.position;

        // bug where if you keep moving you aren't teleported back to start
        yield return new WaitForSeconds(0.1f);
        characterController.gameObject.GetComponent<PlayerController>().SetFreezeMovement(false);
    }
}
