using System.Collections;
using UnityEngine;
using Random = System.Random;

// This script controls the creations and randomizations of target boards

public class TargetBoardSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject targetBoardPrefab; // Prefab of the target board
    [SerializeField] private GameObject[] startPoints; // Possible start points of target board
    [SerializeField] private GameObject[] endPoints; // Possible end points of target board
    [SerializeField] private float maxDurationForLerp = 10f, minDurationForLerp = 1f; // Minimum and maximum desired duration for lerp operation of target board
    [SerializeField] private float minWaitDurationForCreations = 1f, maxWaitDurationForCreations = 6f; // Minimum and maximum durations between target boards' creations
    [SerializeField] private float minScaleForInstantiation = 0.2f, maxScaleForInstantiation = 0.5f; // Minimum and maximum scales for target board instantiations
    private Random random; // Random instance from System.Random

    void Start()
    {
        random = new Random(); // Initialization of Random instance
        StartCoroutine(SpawnTargetBoard()); // Starting the coroutine SpawnTargetBoard()
    }

    // This method randomizes the variables according to min-max values and instantiates the target boards 
    IEnumerator SpawnTargetBoard(){
        int randomStartPointIndex  = random.Next(0, startPoints.Length); // Index of random start point
        int randomEndPointIndex  = random.Next(0, endPoints.Length); // Index of random end point
        float randomDurationForLerp = UnityEngine.Random.Range(minDurationForLerp, maxDurationForLerp); // Random duration for lerp (Used UnityEngine.Random.Range() to generate random floats)
        float randomDurationForCreation = UnityEngine.Random.Range(minWaitDurationForCreations, maxWaitDurationForCreations); // Random duration for creation (Used UnityEngine.Random.Range() to generate random floats)
        float randomScaleForCreation = UnityEngine.Random.Range(minScaleForInstantiation, maxScaleForInstantiation); // Random scale for creation (Used UnityEngine.Random.Range() to generate random floats)
        
        targetBoardPrefab.gameObject.transform.localScale = new Vector3(randomScaleForCreation, randomScaleForCreation, randomScaleForCreation); // Setting the scale of the target board prefab
        GameObject createdTargetBoard = Instantiate(targetBoardPrefab, startPoints[randomStartPointIndex].transform.position, targetBoardPrefab.transform.rotation); // Instatiation of the target board with assigned prefab, random start point, and rotation of prefab

        TargetBoard createdTargetBoardsTBM = createdTargetBoard.GetComponent<TargetBoard>(); // Storing the TargetBoard script of created target board to use it later (for better performance)
        createdTargetBoardsTBM.EndPointPosition = endPoints[randomEndPointIndex].transform.position; // Setting the end point variable of the TargetBoard.cs
        createdTargetBoardsTBM.DesiredDuration = randomDurationForLerp; // Assigning the random speed of the target board
        createdTargetBoardsTBM.AreVariablesAssigned = true; // To prevent the case "TargetBoard.cs can reach the Lerp method before the created target board's end point position is assigned"

        Debug.Log($"Waiting for {randomDurationForCreation} seconds."); // Debugging
        
        yield return new WaitForSeconds(randomDurationForCreation); // Waiting for some time before creating the other target board
        StartCoroutine(SpawnTargetBoard()); // Continuing the creation loop

        // TODO: If the target board got shot; fall down, stop scoring, wait a while, and destroy
        // TODO: If the target board does not get shot; ???
        // TODO: Object Pooling for optimization
        // TODO: Set the optimal min max values for randomizations
        
    }
}
