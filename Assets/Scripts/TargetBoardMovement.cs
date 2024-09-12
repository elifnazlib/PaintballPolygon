using UnityEngine;

public class TargetBoardMovement : MonoBehaviour
{ 
    private Vector3 endPointPosition;

    public Vector3 EndPointPosition
    {
        get {return endPointPosition; }
        set {endPointPosition = value; }
    }
    [SerializeField] private float desiredDuration;
    private Vector3 startPosition;
    private float elapsedTime;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Movement logic of the target board
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / desiredDuration;

        transform.position = Vector3.Lerp(startPosition, endPointPosition, percentageComplete);
    }


}
