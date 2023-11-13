using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using static ItemSpawner;

public class StrollEventManager : MonoBehaviour
{
    private int[] randomTimes;

    [SerializeField]
    private ARObjectController arObjectController;

    [SerializeField]
    private DogManager dogManager;

    [SerializeField]
    private GameObject dogObject;

    [SerializeField]
    GameObject dogAnimationManager;

    [SerializeField]
    GameObject itemSpawner;

    DogAnimationManager dogAnimator;

    [SerializeField]
    private GameObject userGestureManager;

    ItemSpawner itemSpawnerScript;


    private bool gestureEventTrigger;   // ������� ��� ���� ���� üũ
    private bool distanceEventTrigger;  // myDog�� otherDog�� �Ÿ� üũ(�ָ� �������� ��� true)
    public void updateGestureEventTrigger(){
        this.gestureEventTrigger = true;
    }
    public void updateDistanceEventTrigger()
    {
        this.distanceEventTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //if (welshAnim == null)
         //   welshAnim = dogObject.GetComponentInChildren<Animator>();

        dogAnimator = dogAnimationManager.GetComponent<DogAnimationManager>(); 
        if (dogAnimator == null)
        {
            Debug.LogError("DogAnimationManager component not found on dogAnimationManager GameObject!");
        }

        randomTimes = new int[4];

        // 1�к��� 60�б��� ������ �� 4���� randomTimes �迭�� �Է�
        for (int i = 0; i < randomTimes.Length; i++)
        {
            randomTimes[i] = UnityEngine.Random.Range(1, 31); // 1���� 60������ ������ �� ����
        }

        StartCoroutine(SuddenEncounter(randomTimes[0]));
        StartCoroutine(SuddenEat(randomTimes[1]));
        StartCoroutine(SuddenStop(randomTimes[2]));
        StartCoroutine(SuddenPoop(randomTimes[3]));
    }


    // ================���� �̺�Ʈ �߻� �Լ�================
    // 1. �ٸ� �������� ������ ��
    IEnumerator SuddenEncounter(int delayTime)
    {
        delayTime = 1;
        yield return new WaitForSeconds(delayTime);

        dogManager.updateStrollEventCheck(true);    // ������ ������ ����
        EventStatusManager.SwitchDogEvent(true);    // �ִϸ��̼� ����
        
        arObjectController.setDogEventTrigger();    // otherDog�� ������ ����
        userGestureManager.SetActive(true);         // �յ��� �ν� on

        // ���� �˸�
        for (int i = 0; i < 10; i++)
        {
            // ���� �Ÿ� �̻� ������ ���
            if (distanceEventTrigger)
            {
                distanceEventTrigger = false;
                // �����ൿ ��ó ���� ������ ó��
                yield break;
            }

            // ����ڰ� �ڵ����� n�� ����� ���
            if (gestureEventTrigger)
            {
                EventStatusManager.SwitchDogEvent(false); // �ִϸ��̼��� ����� �� �ְ� ����
                dogManager.updateStrollEventCheck(false); // �������� ������ �� �ְ� ����
                userGestureManager.SetActive(false);      // ����� �յ��� �ν� off
                gestureEventTrigger = false;
            }

            dogAnimator.handleDogSuddenEvent("WelshBark");
            Handheld.Vibrate(); // 0.5�ʰ� ������ �︲
            yield return new WaitForSeconds(1); 
        }

        gestureEventTrigger = false;
        dogManager.updateStrollEventCheck(false);
        EventStatusManager.SwitchDogEvent(false);
        userGestureManager.SetActive(false);
        // �����ൿ ��ó ���� ������ ó��
        EventStatusManager.IncreaseStress();
    }


    // 2, ���� ������ �̹����� �ֿ� ������ �� ��
    IEnumerator SuddenEat(int delayTime)
    {
        delayTime = 15;
        yield return new WaitForSeconds(delayTime);

        dogManager.updateStrollEventCheck(true);        // ������ ������ ����
        EventStatusManager.SwitchDogEvent(true);        // �ִϸ��̼� ����
        userGestureManager.SetActive(true);             // �յ��� �ν� on

        // ���� �˸�
        for (int i = 0; i < 10; i++)
        {
            // ����ڰ� �ڵ����� n�� ����� ���
            if (gestureEventTrigger)
            {
                gestureEventTrigger = false;
                dogManager.updateStrollEventCheck(false);  // �������� ������ �� �ְ� ����
                EventStatusManager.SwitchDogEvent(false);  // �ִϸ��̼��� ����� �� �ְ� ����
                userGestureManager.SetActive(false);       // ����� �յ��� �ν� off

                // �����ൿ ��ó ���� ������ ó��
                yield break;
            }
            dogAnimator.handleDogSuddenEvent("WelshEat");
            Handheld.Vibrate(); // 0.5�ʰ� ������ �︲
            yield return new WaitForSeconds(1);
        }

        gestureEventTrigger = false;
        dogManager.updateStrollEventCheck(false);
        EventStatusManager.SwitchDogEvent(false);
        userGestureManager.SetActive(false);
        // �����ൿ ��ó ���� ������ ó��
        EventStatusManager.IncreaseStress();
    }


    // 3. ���� �ɾƼ� �������� ������ �� ��
    IEnumerator SuddenStop(int delayTime)
    {
        delayTime = 30;
        yield return new WaitForSeconds(delayTime);

        dogManager.updateStrollEventCheck(true);
        EventStatusManager.SwitchDogEvent(true);
        EventStatusManager.SwitchDogStopResolved(false);

        // ���� �˸�
        for (int i = 0; i < 4; i++)
        {
            if (EventStatusManager.GetDogStopResolved())
            {
                dogManager.updateStrollEventCheck(false); 
                EventStatusManager.SwitchDogEvent(false);
                yield break;
            }
            dogAnimator.handleDogSuddenEvent("WelshSit"); 
            Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }

        dogManager.updateStrollEventCheck(false);
        EventStatusManager.SwitchDogEvent(false);
        EventStatusManager.SwitchDogStopResolved(true);
        // ���� ó��
        EventStatusManager.IncreaseStress();
    }

    // 4. �躯 Ȱ��
    IEnumerator SuddenPoop(int delayTime)
    {
        delayTime = 45;
        yield return new WaitForSeconds(delayTime);

        if(itemSpawnerScript == null) itemSpawnerScript = itemSpawner.GetComponent<ItemSpawner>();

        dogManager.updateStrollEventCheck(true);
        EventStatusManager.SwitchDogEvent(true);
        EventStatusManager.SwitchDogPoopResolved(false);
        itemSpawnerScript.HandleSpawnAction(ItemType.Poop);

        // ���� �˸�
        for (int i = 0; i < 4; i++)
        {
            if (EventStatusManager.GetDogPoopResolved())
            {
                itemSpawnerScript.HandleRemoveAction(ItemType.Poop);
                dogManager.updateStrollEventCheck(false);
                EventStatusManager.SwitchDogEvent(false);
                yield break;
            }
            dogAnimator.handleDogSuddenEvent("WelshPoop");
            Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }

        dogManager.updateStrollEventCheck(false);
        EventStatusManager.SwitchDogEvent(false);
        EventStatusManager.SwitchDogPoopResolved(true);
        // ���� ó��
        EventStatusManager.IncreaseStress();
    }

    // ============= ���� �Լ� ===================

}