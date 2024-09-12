using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class TargetBoardSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject targetBoardPrefab;
    [SerializeField] private GameObject[] startPoints;
    [SerializeField] private GameObject[] endPoints;
    private int maxSpeed = 10, minSpeed = 0;
    private Random random;

    void Start()
    {
        random = new Random();
        SpawnTargetBoard();
    }

    public void SpawnTargetBoard(){
        int randomStartPointIndex  = random.Next(0, startPoints.Length);
        int randomEndPointIndex  = random.Next(0, endPoints.Length);

        GameObject createdTargetBoard = Instantiate(targetBoardPrefab, startPoints[randomStartPointIndex].transform.position, Quaternion.identity);

        createdTargetBoard.GetComponent<TargetBoardMovement>().EndPointPosition = endPoints[randomEndPointIndex].transform.position;

        // TODO: Assign the random speed for the target board
        // TODO: TargetBoardMovement.cs can reach the Lerp method before the created target board's end point position is assigned
        // TODO: If the target board got shot; fall down, stop scoring, wait a while, and destroy
        // TODO: If the target board does not get shot; ???
        // TODO: Object Pooling for optimization
    }
}
