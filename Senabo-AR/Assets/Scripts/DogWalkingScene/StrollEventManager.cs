using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

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

    DogAnimationManager dogAnimator;

    [SerializeField]
    private GameObject userGestureManager;

    private bool gestureEventTrigger;
    public void updateGestureEventTrigger(){
        this.gestureEventTrigger = true;
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

        dogManager.updateStrollEventCheck(true);
        EventStatusManager.SwitchDogEvent(true);
        
        arObjectController.setDogEventTrigger();
        userGestureManager.SetActive(true);

        // ���� �˸�
        for (int i = 0; i < 10; i++)
        {
            // event�� �ذ����� ���
            if (gestureEventTrigger)
            {
                Debug.Log("¢�� �̺�Ʈ ����!");
                gestureEventTrigger = false;
                break;
            }
            dogAnimator.handleDogSuddenEvent("WelshBark");
            Handheld.Vibrate(); // 0.5�ʰ� ������ �︲
            yield return new WaitForSeconds(1); 
        }

        dogManager.updateStrollEventCheck(false);
        EventStatusManager.SwitchDogEvent(false);
        userGestureManager.SetActive(false);
    }


    // 2, ���� ������ �̹����� �ֿ� ������ �� ��
    IEnumerator SuddenEat(int delayTime)
    {
        delayTime = 15;
        yield return new WaitForSeconds(delayTime);

        dogManager.updateStrollEventCheck(true);
        EventStatusManager.SwitchDogEvent(true);
        userGestureManager.SetActive(true);

        // ���� �˸�
        for (int i = 0; i < 10; i++)
        {
            // event�� �ذ����� ���
            if (gestureEventTrigger)
            {
                Debug.Log("�ֿ��Դ� �̺�Ʈ ����!");
                gestureEventTrigger = false;
                break;
            }
            dogAnimator.handleDogSuddenEvent("WelshEat");
            Handheld.Vibrate(); // 0.5�ʰ� ������ �︲
            yield return new WaitForSeconds(1);
        }

        dogManager.updateStrollEventCheck(false);
        EventStatusManager.SwitchDogEvent(false);
        userGestureManager.SetActive(false);
    }

    // 3. ���� �ɾƼ� �������� ������ �� ��
    IEnumerator SuddenStop(int delayTime)
    {
        delayTime = 30;
        yield return new WaitForSeconds(delayTime);

        dogManager.updateStrollEventCheck(true);
        EventStatusManager.SwitchDogEvent(true);

        // ���� �˸�
        for (int i = 0; i < 4; i++)
        {

            dogAnimator.handleDogSuddenEvent("WelshSit"); 
            Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }

        dogManager.updateStrollEventCheck(false);
        EventStatusManager.SwitchDogEvent(false);
    }

    // 4. �躯 Ȱ��
    IEnumerator SuddenPoop(int delayTime)
    {
        delayTime = 45;
        yield return new WaitForSeconds(delayTime);

        dogManager.updateStrollEventCheck(true);
        EventStatusManager.SwitchDogEvent(true);

        // ���� �˸�
        for (int i = 0; i < 4; i++)
        {
            dogAnimator.handleDogSuddenEvent("WelshPoop");
            Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }

        dogManager.updateStrollEventCheck(false);
        EventStatusManager.SwitchDogEvent(false);
    }

    // ============= ���� �Լ� ===================

}