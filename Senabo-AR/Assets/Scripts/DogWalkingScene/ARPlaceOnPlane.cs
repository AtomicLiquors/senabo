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

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        // 매 프레임에서 특정 함수를 실행합니다.
        //UpdateCenterObject(); 카메라 중앙에 오브젝트 생성.
        //PlaceObjectByTouch();
        
        if (!spawnObject)
        {
            //PlaceObjectByTouch();
            UpdateCenterObject();
        }
    
        
    }

    private void PlaceObjectByTouch()
    {
        if(Input.touchCount> 0)
        {
            Touch touch = Input.GetTouch(0); // 0번째 손가락으로 터치한 정보를 반환받는다.

            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;
                if (!spawnObject)
                {
                    spawnObject = Instantiate(placeObject, hitPose.position, hitPose.rotation);
                }
                else
                {
                    spawnObject.transform.position = hitPose.position;
                    spawnObject.transform.rotation = hitPose.rotation;
                    // 두 번째 프레임부터는 이미 spawn된 오브젝트의 위치와 rotation만 업데이트시켜준다.
                }
            }
        }

        
    }

    private void UpdateCenterObject()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes);

        if(hits.Count > 0 )
        {
            Pose placementPose = hits[0].pose;
            placeObject.SetActive(true);
            placeObject.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        /*
        else
        {
            placeObject.SetActive(true); // false로 세팅할 경우 평면이 인식되지 않았을 때 배치할 오브젝트가 사라지게 합니다.
        }*/
    }
}
