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
    public GameObject strollEventManager;
    public GameObject dogAnimationManager;
    public GameObject dogManager;

    [SerializeField]
    private GameObject myDog;

    [SerializeField]
    private GameObject otherDog;

    private static List<ARRaycastHit> arHits = new List<ARRaycastHit>();

    private Gyroscope gyro;
    private bool myDogCheck;

    // ȭ�鿡 �ٸ� �������� �����ϴ� �̺�Ʈ �߻��� Ȯ���ϴ� ����
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
            // ======= otherDog�� �����Ͽ� myDog �տ� ���ֺ����� ��ġ ���� ========== //
            Vector3 offset = myDog.transform.forward * 2.0f; 

            otherDog.SetActive(true);
            otherDog.transform.position = myDog.transform.position + offset;
            float rotationSpeed = 180f;
            otherDog.transform.rotation = Quaternion.Euler(0, rotationSpeed += Time.deltaTime * 10, 0);
            dogEventTrigger = false;
        }

        if (Input.touchCount==0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);
        Vector2 touchPosition = touch.position;

        // myDog�� ��Ȱ�� ������ ���
        if (!myDog.activeInHierarchy)
        {
            // ó�� Ŭ���� ������ ���� RayCast�� ��� �ϰ�, ����� �ν����� ���
            if (touch.phase == TouchPhase.Began && arRaycaster.Raycast(touchPosition, arHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = arHits[0].pose;
                myDog.SetActive(true);
                myDog.transform.position = hitPose.position;

               // dogRotator.SetActive(true);
                dogLeadSpawner.SetActive(true);
                walkTimer.SetActive(true);
                strollEventManager.SetActive(true);
                dogAnimationManager.SetActive(true);
                gpsManager.SetActive(true);
                dogManager.SetActive(true);
            }
        }
    }
}