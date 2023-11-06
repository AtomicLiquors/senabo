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

        // 1분부터 60분까지 랜덤한 값 4개를 randomTimes 배열에 입력
        for (int i = 0; i < randomTimes.Length; i++)
        {
            randomTimes[i] = UnityEngine.Random.Range(1, 31); // 1부터 60까지의 랜덤한 값 생성
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

    // ================돌발 이벤트 발생 함수================
    // 1. 다른 강아지를 만났을 때
    IEnumerator SuddenEncounter(int delayTime)
    {
       EventStatusManager.SwitchDogEvent(true);
        yield return new WaitForSeconds(delayTime);

        dogAnimator.handleDogSuddenEvent("WelshBark");
        arObjectController.setDogEventTrigger();
        
        // 진동 알림
        for (int i = 0; i < 2; i++)
        {
            Handheld.Vibrate(); // 0.5초간 진동이 울림
            yield return new WaitForSeconds(1); 
        }

        EventStatusManager.SwitchDogEvent(false);
    }


    // 2, 땅에 떨어진 이물질을 주워 먹으려 할 때
    IEnumerator SuddenEat(int delayTime)
    {
        EventStatusManager.SwitchDogEvent(true);
        dogAnimator.handleDogSuddenEvent("WelshEat");
        yield return new WaitForSeconds(delayTime);
        // 진동 알림
        for (int i = 0; i < 2; i++)
        {
            Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }
        EventStatusManager.SwitchDogEvent(false);
    }

    // 3. 주저 앉아서 움직이지 않으려 할 때
    IEnumerator SuddenStop(int delayTime)
    {

        EventStatusManager.SwitchDogEvent(true);
        dogAnimator.handleDogSuddenEvent("WelshSit");
        yield return new WaitForSeconds(delayTime);
        // 진동 알림
        for (int i = 0; i < 2; i++)
        {
            Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }
        EventStatusManager.SwitchDogEvent(false);
    }

    // 4. 배변 활동
    IEnumerator SuddenPoop(int delayTime)
    {

        EventStatusManager.SwitchDogEvent(true);
        dogAnimator.handleDogSuddenEvent("WelshPoop");
        yield return new WaitForSeconds(delayTime);
        // 진동 알림
        for (int i = 0; i < 2; i++)
        {
            Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }
        EventStatusManager.SwitchDogEvent(false);
    }

    // ============= 내부 함수 ===================

}