using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceOnPlane : MonoBehaviour
{

    public ARRaycastManager arRaycaster;
    public GameObject placeObject;
    GameObject spawnObject;

    public GameObject gpsManager;

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
            if (!spawnObject)
            {
                spawnObject = Instantiate(placeObject, hitPose.position, hitPose.rotation);
                gpsManager.SetActive(true);
            }
            else
            {
                spawnObject.transform.position = hitPose.position;
                spawnObject.transform.rotation = hitPose.rotation;
            }
        }
    }
}