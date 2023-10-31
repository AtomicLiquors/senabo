using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticMove : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float movementSpeed = 2.0f;

    private float journeyLength;
    private float startTime;
    void Start()
    {
        journeyLength = Vector3.Distance(startPoint.position, endPoint.position);
        startTime = Time.time;
    }

    void Update()
    {
        float distanceCovered = (Time.time - startTime) * movementSpeed;

        if(distanceCovered < journeyLength) {
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position=Vector3.Lerp(startPoint.position, endPoint.position, fractionOfJourney);
        } else {
            transform.position = startPoint.position;
            startTime = Time.time;
        }
    }
}
