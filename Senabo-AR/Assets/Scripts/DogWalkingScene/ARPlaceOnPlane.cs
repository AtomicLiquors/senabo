using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceOnPlane : MonoBehaviour
{

    public ARRaycastManager arRaycaster;
    public GameObject placeObject;
    public GameObject gpsManager;
    public GameObject dogLeadSpawner;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateCenterObject();

    }

    private void UpdateCenterObject()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            Pose hitPose = hits[0].pose;
            if (!placeObject.activeInHierarchy)
            {
                placeObject.SetActive(true);
                placeObject.transform.position = hitPose.position;
                placeObject.transform.rotation = hitPose.rotation;
                gpsManager.SetActive(true);
                dogLeadSpawner.SetActive(true);
            }
            else
            {
                placeObject.transform.position = hitPose.position;
                placeObject.transform.rotation = hitPose.rotation;
            }
        }
    }
}