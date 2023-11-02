using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectController : MonoBehaviour
{
    public ARRaycastManager arRaycaster;
    public GameObject gpsManager;
    public GameObject dogLeadSpawner;
    public GameObject walkTimer;
    public GameObject dogRotator;

    [SerializeField]
    private GameObject myDog;

    [SerializeField]
    private GameObject otherDog;

    private static List<ARRaycastHit> arHits = new List<ARRaycastHit>();


    // 화면에 다른 강아지가 등장하는 이벤트 발생을 확인하는 변수
    private bool dogEventTrigger;
    public void setDogEventTrigger()
    {
        dogEventTrigger = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (dogEventTrigger)
        {
            // ======= otherDog를 myDog 앞에 마주보도록 위치 설정 ==========
            Vector3 offset = myDog.transform.forward * 2.0f; // myDog에서 약간 떨어진 위치

            // myDog의 현재 회전값 가져오기
            Quaternion myDogRotation = myDog.transform.rotation;

            // myDog의 현재 회전값에 180도 회전을 더하여 otherDog가 myDog를 바라보도록 회전 설정
            Quaternion lookRotation = myDogRotation * Quaternion.Euler(0, 180, 0);

            Instantiate(otherDog, myDog.transform.position + offset, lookRotation);
            dogEventTrigger = false;
        }

        if (Input.touchCount==0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);
        Vector2 touchPosition = touch.position;

        // myDog가 비활성 상태인 경우
        if (!myDog.activeInHierarchy)
        {
            float rotationSpeed = 30f; // Adjust the speed of rotation as needed

            // 처음 클릭한 상태일 때만 RayCast를 쏘도록 하고, 평면을 인식했을 경우
            if (touch.phase == TouchPhase.Began && arRaycaster.Raycast(touchPosition, arHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = arHits[0].pose;
                myDog.SetActive(true);
                myDog.transform.position = hitPose.position;

                gpsManager.SetActive(true);
                dogLeadSpawner.SetActive(true);
                walkTimer.SetActive(true);

                // dogRotator.SetActive(true);
                myDog.transform.rotation = Quaternion.Euler(rotationSpeed += Time.deltaTime * 10, 0, 0);
            }
        }
    }
}