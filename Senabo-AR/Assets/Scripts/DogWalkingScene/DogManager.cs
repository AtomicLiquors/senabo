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
        // ī�޶��� �߾����� ray�� ���
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        // ����� �ν��Ͽ� hits�� ���� �Է�
        arRaycaster.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon);

        if (hits.Count > 0)
        {
            Pose hitPose = hits[0].pose;
            // myDog.transform.position = hitPose.position;

            // �̵� ���� ���
            Vector3 moveDirection = hitPose.position - myDog.transform.position;
            moveDirection.y = 0; // ���ϴ� ������ �̵��ϵ��� y ���� 0���� ����

            // magnitude�� 3���� ������ ũ��/����
            // �̵��ϴ� ���(�̵� �Ÿ��� ���� ������ Ŭ ���)
            if (moveDirection.magnitude > stoppingDistance)
            {
                // �̵� �������� ȸ��
                Quaternion newRotation = Quaternion.LookRotation(moveDirection);
                myDog.GetComponent<Rigidbody>().MoveRotation(newRotation);

                // �Ÿ��� �� ��
                if (moveDirection.magnitude > 1.5f)
                {
                    dogAnimator.handleDogMovement("WelshRun");
                    myDog.GetComponent<Rigidbody>().AddForce(moveDirection.normalized * 3f);
                }
                // �Ÿ��� ª�� ��
                else
                {
                    dogAnimator.handleDogMovement("WelshWalk");
                    myDog.GetComponent<Rigidbody>().AddForce(moveDirection.normalized * 2f);
                }
            }
            // �̵����ϴ� ���
            else
            {
                // ���� �Ÿ��� ������ ���
                dogAnimator.handleDogMovement("WelshIdle");
                myDog.GetComponent<Rigidbody>().velocity = Vector3.zero; // �ӵ� ����
                myDog.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // ȸ�� ����
            }

            //myDog.GetComponent<Rigidbody>().AddForce(myDog.transform.forward * 10f);

        }


    }
    }
