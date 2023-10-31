using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalkTimer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Text totalWalkTime;


    private float elapsedTime = 0f;
    void Start()
    {
        totalWalkTime.text = "√‚πﬂ";
        StartCoroutine(UpdateElapsedTime());
    }

    // Update is called once per frame

    IEnumerator UpdateElapsedTime()
    {
        while (true)
        {
            // Check if the object is activated

                // Update elapsed time
                elapsedTime += Time.deltaTime;

                // Do something with the elapsed time, e.g., print it
                totalWalkTime.text = String.Join("", elapsedTime.ToString("F2"),"∫–");
            

            yield return new WaitForSeconds(1); // Yielding null will make the coroutine wait for the next frame
        }
    }
}
