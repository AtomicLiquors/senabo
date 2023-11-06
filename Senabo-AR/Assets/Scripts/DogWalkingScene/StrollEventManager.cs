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
    private GameObject dogObject;

    [SerializeField]
    GameObject dogAnimationManager;

    DogAnimationManager dogAnimator;

  //  Animator welshAnim;


    // Start is called before the first frame update
    void Start()
    {
        //if (welshAnim == null)
         //   welshAnim = dogObject.GetComponentInChildren<Animator>();

        dogAnimator = dogAnimationManager.GetComponent<DogAnimationManager>(); if (dogAnimator == null)
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

    // Update is called once per frame
    void Update()
    {
    }

    // ================���� �̺�Ʈ �߻� �Լ�================
    // 1. �ٸ� �������� ������ ��
    IEnumerator SuddenEncounter(int delayTime)
    {
       EventStatusManager.SwitchDogEvent(true);
        yield return new WaitForSeconds(delayTime);

        dogAnimator.handleDogSuddenEvent("WelshBark");
        arObjectController.setDogEventTrigger();
        
        // ���� �˸�
        for (int i = 0; i < 2; i++)
        {
            Handheld.Vibrate(); // 0.5�ʰ� ������ �︲
            yield return new WaitForSeconds(1); 
        }

        EventStatusManager.SwitchDogEvent(false);
    }


    // 2, ���� ������ �̹����� �ֿ� ������ �� ��
    IEnumerator SuddenEat(int delayTime)
    {
        EventStatusManager.SwitchDogEvent(true);
        dogAnimator.handleDogSuddenEvent("WelshEat");
        yield return new WaitForSeconds(delayTime);
        // ���� �˸�
        for (int i = 0; i < 2; i++)
        {
            Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }
        EventStatusManager.SwitchDogEvent(false);
    }

    // 3. ���� �ɾƼ� �������� ������ �� ��
    IEnumerator SuddenStop(int delayTime)
    {

        EventStatusManager.SwitchDogEvent(true);
        dogAnimator.handleDogSuddenEvent("WelshSit");
        yield return new WaitForSeconds(delayTime);
        // ���� �˸�
        for (int i = 0; i < 2; i++)
        {
            Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }
        EventStatusManager.SwitchDogEvent(false);
    }

    // 4. �躯 Ȱ��
    IEnumerator SuddenPoop(int delayTime)
    {

        EventStatusManager.SwitchDogEvent(true);
        dogAnimator.handleDogSuddenEvent("WelshPoop");
        yield return new WaitForSeconds(delayTime);
        // ���� �˸�
        for (int i = 0; i < 2; i++)
        {
            Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }
        EventStatusManager.SwitchDogEvent(false);
    }

    // ============= ���� �Լ� ===================

}