using System.Collections;
using UnityEngine;

// This script is used to control the movement of the target board.
public class TargetBoard : MonoBehaviour
{ 
    private bool areVariablesAssigned = false;  // Is used to prevent the synchronization problem in TargetBoardSpawnManager.cs (for end point and desired duration)
    private bool canUpdateScore = true;  // Is used to prevent the multiple score updates for the same target board
    private bool isShot = false; // Is used to prevent the movement of LERP after getting shot
    private float timeToDisappearAfterLerp; // Time to wait after reaching the destination
    private bool isDestroyProcessStarted = false; // Is used to prevent the multiple destroy calls for the same target board
    
    public float TimeToDisappearAfterLerp
    {
        get {return timeToDisappearAfterLerp; }
        set {timeToDisappearAfterLerp = value; }
    }
    
    public bool AreVariablesAssigned
    {
        get {return areVariablesAssigned; }
        set {areVariablesAssigned = value; }
    }
    
    public bool CanUpdateScore
    {
        get {return canUpdateScore; }
        set {canUpdateScore = value; }
    }

    public bool IsShot 
    {
        get {return isShot; }
        set {isShot = value; }
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
        if(areVariablesAssigned && !isShot) { // TODO: This line may need to change during object pooling implementation
            // Movement logic of the target board (https://www.youtube.com/watch?v=MyVY-y_jK1I)
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / desiredDuration;

            transform.position = Vector3.Lerp(startPosition, endPointPosition, percentageComplete);
            if (percentageComplete >= 1f && !isDestroyProcessStarted)
            {
                isDestroyProcessStarted = true;
                StartCoroutine(Destroyer()); // Destroying the target board after reaching the end point
            }
        }
    }

    IEnumerator Destroyer()
    {
        yield return new WaitForSeconds(timeToDisappearAfterLerp);
        Destroy(gameObject); // Destroying the target board after waiting for some time
    }

    // TODO: Target boards shall move forward and back 
}
