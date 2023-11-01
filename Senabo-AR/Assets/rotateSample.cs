using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateSample : MonoBehaviour
{
    public GameObject rotateObject;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotationSpeed = 30f; // Adjust the speed of rotation as needed
        Debug.Log("Before : " + rotateObject.transform.rotation.x);

        /*if (rotateObject.activeInHierarchy)
            rotateObject.transform.rotation = Quaternion.Euler(rotationSpeed += Time.deltaTime * 10, 0, 0);
        */
        Debug.Log("After : " + rotateObject.transform.rotation.x);

    }
}
