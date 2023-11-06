using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DogManager : MonoBehaviour
{
    public ARRaycastManager arRaycaster;
    public GameObject myDog;
    List<ARRaycastHit> hits;
    float stoppingDistance = 0.8f;

    [SerializeField]
    GameObject dogAnimationManager;

    DogAnimationManager dogAnimator;

    // Start is called before the first frame update
    void Start()
    {
        dogAnimator = dogAnimationManager.GetComponent<DogAnimationManager>();
        hits = new List<ARRaycastHit>();
    }

    // Update is called once per frame
    void Update()
    {
        // 카메라의 중앙으로 ray를 쏜다
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        // 평면을 인식하여 hits에 값을 입력
        arRaycaster.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon);

        if (hits.Count > 0)
        {
            Pose hitPose = hits[0].pose;
            // myDog.transform.position = hitPose.position;

            // 이동 방향 계산
            Vector3 moveDirection = hitPose.position - myDog.transform.position;
            moveDirection.y = 0; // 원하는 축으로 이동하도록 y 값을 0으로 설정

            // magnitude는 3차원 백터의 크기/길이
            // 이동하는 경우(이동 거리가 정한 값보다 클 경우)
            if (moveDirection.magnitude > stoppingDistance)
            {
                // 이동 방향으로 회전
                Quaternion newRotation = Quaternion.LookRotation(moveDirection);
                myDog.GetComponent<Rigidbody>().MoveRotation(newRotation);

                // 거리가 멀 때
                if (moveDirection.magnitude > 1.5f)
                {
                    dogAnimator.handleDogMovement("WelshRun");
                    myDog.GetComponent<Rigidbody>().AddForce(moveDirection.normalized * 3f);
                }
                // 거리가 짧을 때
                else
                {
                    dogAnimator.handleDogMovement("WelshWalk");
                    myDog.GetComponent<Rigidbody>().AddForce(moveDirection.normalized * 2f);
                }
            }
            // 이동안하는 경우
            else
            {
                // 멈출 거리에 도달한 경우
                dogAnimator.handleDogMovement("WelshIdle");
                myDog.GetComponent<Rigidbody>().velocity = Vector3.zero; // 속도 중지
                myDog.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // 회전 중지
            }

            //myDog.GetComponent<Rigidbody>().AddForce(myDog.transform.forward * 10f);

        }


    }
    }
