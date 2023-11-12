using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogOffscreenIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject dogObject;

    [SerializeField]
    GameObject dogIndicator;

    Vector3 screenPoint; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dogObject.activeInHierarchy)
        {

        }
            //screenPoint= Camera.main.WorldToViewportPoint(projectile.transform)
    }
}
