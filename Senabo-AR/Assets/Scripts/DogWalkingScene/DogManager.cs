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

    private bool strollEventCheck;
    public void updateStrollEventCheck(bool check)
    {
        strollEventCheck = check;
    }

    // Start is called before the first frame update
    void Start()
    {
        dogAnimator = dogAnimationManager.GetComponent<DogAnimationManager>();
        hits = new List<ARRaycastHit>();
    }

    // Update is called once per frame
    void Update()
    {
        // ���� �̺�Ʈ �߻� ���� ��
        if (strollEventCheck)
        {
            myDog.GetComponent<Rigidbody>().velocity = Vector3.zero; // �ӵ� ����
            myDog.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // ȸ�� ����
            return;
        }

        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));   // ī�޶��� �߾����� ray�� ���
        arRaycaster.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon); // ����� �ν��Ͽ� hits�� ���� �Է�

        // ����� �ν� ���� ��
        if (hits.Count > 0)
        {
            Pose hitPose = hits[0].pose;
            // myDog.transform.position = hitPose.position;

            // �̵� ���� ����
            Vector3 moveDirection = hitPose.position - myDog.transform.position;
            moveDirection.y = 0; // ���ϴ� ������ �̵��ϵ��� y ���� 0���� ����

            // �̵��ϴ� ���(�̵� �Ÿ��� ���� ������ Ŭ ���)
            if (moveDirection.magnitude > stoppingDistance) // magnitude�� 3���� ������ ũ��/����
            {
                // �̵� �������� ȸ��
                Quaternion newRotation = Quaternion.LookRotation(moveDirection);
                myDog.GetComponent<Rigidbody>().MoveRotation(newRotation);

                // �Ÿ��� �� ��
                if (moveDirection.magnitude > 1.3f)
                {
                    dogAnimator.handleDogMovement("WelshRun");
                    myDog.GetComponent<Rigidbody>().AddForce(moveDirection.normalized * 4f);
                }
                // �Ÿ��� ª�� ��
                else
                {
                    dogAnimator.handleDogMovement("WelshWalk");
                    myDog.GetComponent<Rigidbody>().AddForce(moveDirection.normalized * 2f);
                }
            }
            // �̵����ϴ� ���(�ʹ� ª�� �Ÿ��� �̵� X)
            else
            {
                // ���� �Ÿ��� ������ ���
                dogAnimator.handleDogMovement("WelshIdle");
                myDog.GetComponent<Rigidbody>().velocity = Vector3.zero; // �ӵ� ����
                myDog.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // ȸ�� ����
            }
        }
        // ����� �ν� �� ���� ��
        else
        {
            dogAnimator.handleDogMovement("WelshIdle");
            myDog.GetComponent<Rigidbody>().velocity = Vector3.zero; // �ӵ� ����
            myDog.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // ȸ�� ����
        }

    }
    }
