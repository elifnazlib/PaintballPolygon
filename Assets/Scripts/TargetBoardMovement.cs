using UnityEngine;

// This script is used to control the movement of the target board.
public class TargetBoardMovement : MonoBehaviour
{ 
    private bool areVariablesAssigned = false;  // Is used to prevent the synchronization problem in TargetBoardSpawnManager.cs (for end point and desired duration)

    public bool AreVariablesAssigned
    {
        get {return areVariablesAssigned; }
        set {areVariablesAssigned = value; }
    }

    private Vector3 endPointPosition;   // end point position of the target board.

    public Vector3 EndPointPosition
    {
        get {return endPointPosition; }
        set {endPointPosition = value; }
    }
    private float desiredDuration;  // desired duration for the target board movement.

    public float DesiredDuration
    {
        get {return desiredDuration; }
        set {desiredDuration = value; }
    }
    private Vector3 startPosition;  // start point position of the target board.
    private float elapsedTime;  // elapsed time of the Lerp.

    void Start()
    {
        startPosition = transform.position; // assigning the initial position of target board at the start.
    }

    void Update()
    {
        if(areVariablesAssigned){ // TODO: This line may need to change during object pooling implementation
            // Movement logic of the target board (https://www.youtube.com/watch?v=MyVY-y_jK1I)
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / desiredDuration;

            transform.position = Vector3.Lerp(startPosition, endPointPosition, percentageComplete);
        }
    }

    // TODO: Target boards shall move forward and back 
}
