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
            // ======= otherDog�� myDog �տ� ���ֺ����� ��ġ ���� ==========
            Vector3 offset = myDog.transform.forward * 2.0f; // myDog���� �ణ ������ ��ġ

            // myDog�� ���� ȸ���� ��������
            Quaternion myDogRotation = myDog.transform.rotation;

            // myDog�� ���� ȸ������ 180�� ȸ���� ���Ͽ� otherDog�� myDog�� �ٶ󺸵��� ȸ�� ����
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

        // myDog�� ��Ȱ�� ������ ���
        if (!myDog.activeInHierarchy)
        {
            float rotationSpeed = 30f; // Adjust the speed of rotation as needed

            // ó�� Ŭ���� ������ ���� RayCast�� ��� �ϰ�, ����� �ν����� ���
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