using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogLeadDrawer : MonoBehaviour
{
    // Start is called before the first frame update
    LineRenderer lineRenderer;
    Vector3 dogPos, cameraPos, cameraPos1;

    GameObject roTlqkfwhwrkxdmsrotoRlsus;
    void Start()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.activeInHierarchy)
                print(go.name + " is an active object");
        }

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = .05f;
        lineRenderer.endWidth = .05f;

        roTlqkfwhwrkxdmsrotoRlsus = GameObject.Find("WelshCorgi(Clone)");
        if(!roTlqkfwhwrkxdmsrotoRlsus)
            roTlqkfwhwrkxdmsrotoRlsus = GameObject.Find("WelshCorgi");
        /*
        while (welshAnim == null)
        {
            welshAnim = GameObject.Find("WelshCorgi(Clone)").GetComponentInChildren<Animator>();
        }*/
    }

    void Update() {


        //dogPos = ModifyDogPos(dogPrefab.GetComponent<Transform>().position);
        //dogPos = dogPrefab.GetComponent<Transform>().position;
        print(roTlqkfwhwrkxdmsrotoRlsus);
        cameraPos = Camera.current.ViewportToWorldPoint(new Vector3(0.5f, 0f, Camera.current.nearClipPlane));
        
    }

    Vector3 ModifyDogPos(Vector3 originalDogPos)
    {
        float z = originalDogPos.z + 0.3f;
        return new Vector3(originalDogPos.x, originalDogPos.y, z);
    }
}
