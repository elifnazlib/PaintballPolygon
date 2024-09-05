using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBoardMovement : MonoBehaviour
{ 
    [SerializeField] private GameObject endPoint;
    private Vector3 endPosition;
    private Vector3 startPosition;
    [SerializeField] private float desiredDuration;
    private float elapsedTime;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / desiredDuration;

        transform.position = Vector3.Lerp(startPosition, endPoint.transform.position, percentageComplete);
    }
}
