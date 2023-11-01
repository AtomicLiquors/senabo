using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectController : MonoBehaviour
{

    public ARRaycastManager arRaycaster;
    public GameObject myDog;
    public GameObject gpsManager;
    public GameObject dogLeadSpawner;
    public GameObject walkTimer;
    public GameObject dogRotator;

    private StrollEventManager strollEventManager;


    // Update is called once per frame
    void Update()
    {
        UpdateCenterObject();

        if (strollEventManager != null)
        {
            Debug.Log(strollEventManager.dogEventTrigger);
        }
    }

    /*
    private IEnumerator DogSpawner()
    {

    }*/

    private void UpdateCenterObject()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes);

        float rotationSpeed = 30f; // Adjust the speed of rotation as needed

        if (hits.Count > 0)
        {
            Pose hitPose = hits[0].pose;
            if (!myDog.activeInHierarchy)
            {
                myDog.SetActive(true);
                myDog.transform.position = hitPose.position;
                gpsManager.SetActive(true);
                dogLeadSpawner.SetActive(true);
                walkTimer.SetActive(true);
                //                dogRotator.SetActive(true);
                myDog.transform.rotation = Quaternion.Euler(rotationSpeed += Time.deltaTime * 10, 0, 0);

                strollEventManager = FindAnyObjectByType<StrollEventManager>();
            }
            else
            {
                myDog.transform.position = hitPose.position;
                myDog.transform.rotation = Quaternion.Euler(rotationSpeed += Time.deltaTime * 10, 0, 0);
            }
        }

    }
}