using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    public CameraChange cameraChange;
    public Transform defaultPosition;
    public List<Transform> roomOnePositions;
    public List<Transform> roomTwoPositions;
    public List<Transform> roomThreePositions;

    List<List<Transform>> allPositions = new List<List<Transform>>();

    void Start()
    {
        allPositions.Add(roomOnePositions);
        allPositions.Add(roomTwoPositions);
        allPositions.Add(roomThreePositions);

        StartCoroutine(CatCycle());
    }
    
    IEnumerator CatCycle()
    {
        while (true)
        {
            // determine which room player is in
            var roomIndex = cameraChange.RoomIndex;
            var currentPositionsList = new List<Transform>(allPositions[roomIndex]);

            // remove current position (doesn't go to same hole twice in a row)
            foreach (Transform transform in currentPositionsList)
            {
                if (this.transform.position == transform.position)
                {
                    currentPositionsList.Remove(transform);
                    break;
                }
            }
            //Debug.Log("Possible positions: " + currentPositionsList.Count);

            // reset cat position
            transform.position = defaultPosition.position;

            yield return new WaitForSeconds(1);

            // move cat to random position
            var randomIndex = Random.Range(0, currentPositionsList.Count);
            transform.position = currentPositionsList[randomIndex].position;
            //Debug.Log("Current position: " + currentPositionsList[randomIndex].name);

            yield return new WaitForSeconds(3);
        }
        
    }
}