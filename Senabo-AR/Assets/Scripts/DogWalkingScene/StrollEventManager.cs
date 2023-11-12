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
    private bool distanceEventTrigger;
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


    // ================돌발 이벤트 발생 함수================
    // 1. 다른 강아지를 만났을 때
    IEnumerator SuddenEncounter(int delayTime)
    {
        delayTime = 1;
        yield return new WaitForSeconds(delayTime);

        dogManager.updateStrollEventCheck(true);    // 강아지가 정지하도록
        EventStatusManager.SwitchDogEvent(true);    // 애니메이션이 변경 안되도록
        
        arObjectController.setDogEventTrigger();    // otherDog가 나오게 설정
        userGestureManager.SetActive(true);         // 손동작 인식 on

        // 진동 알림
        for (int i = 0; i < 10; i++)
        {
            // 일정 거리 이상 떨어진 경우
            if (distanceEventTrigger)
            {
                distanceEventTrigger = false;
                yield break;
            }

            // 사용자가 핸드폰을 n번 당겼을 경우
            if (gestureEventTrigger)
            {
                EventStatusManager.SwitchDogEvent(false); // 애니메이션이 변경될 수 있게 설정
                dogManager.updateStrollEventCheck(false); // 강아지가 움직일 수 있게 설정
                userGestureManager.SetActive(false);      // 사용자 손동작 인식 off
                gestureEventTrigger = false;
            }

            dogAnimator.handleDogSuddenEvent("WelshBark");
            Handheld.Vibrate(); // 0.5초간 진동이 울림
            yield return new WaitForSeconds(1); 
        }

        // 사용자가 이벤트에 잘 대처하지 못했을 경우 실행
        gestureEventTrigger = false;
        dogManager.updateStrollEventCheck(false);
        EventStatusManager.SwitchDogEvent(false);
        userGestureManager.SetActive(false);
    }


    // 2, 땅에 떨어진 이물질을 주워 먹으려 할 때
    IEnumerator SuddenEat(int delayTime)
    {
        delayTime = 15;
        yield return new WaitForSeconds(delayTime);

        dogManager.updateStrollEventCheck(true);
        EventStatusManager.SwitchDogEvent(true);
        userGestureManager.SetActive(true);

        // 진동 알림
        for (int i = 0; i < 10; i++)
        {
            // event를 해결했을 경우
            if (gestureEventTrigger)
            {
                Debug.Log("주워먹는 이벤트 해제!");
                break;
            }
            dogAnimator.handleDogSuddenEvent("WelshEat");
            Handheld.Vibrate(); // 0.5초간 진동이 울림
            yield return new WaitForSeconds(1);
        }

        gestureEventTrigger = false;
        dogManager.updateStrollEventCheck(false);
        EventStatusManager.SwitchDogEvent(false);
        userGestureManager.SetActive(false);
    }

    // 3. 주저 앉아서 움직이지 않으려 할 때
    IEnumerator SuddenStop(int delayTime)
    {
        delayTime = 30;
        yield return new WaitForSeconds(delayTime);

        dogManager.updateStrollEventCheck(true);
        EventStatusManager.SwitchDogEvent(true);

        // 진동 알림
        for (int i = 0; i < 4; i++)
        {

            dogAnimator.handleDogSuddenEvent("WelshSit"); 
            Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }

        dogManager.updateStrollEventCheck(false);
        EventStatusManager.SwitchDogEvent(false);
    }

    // 4. 배변 활동
    IEnumerator SuddenPoop(int delayTime)
    {
        delayTime = 45;
        yield return new WaitForSeconds(delayTime);

        dogManager.updateStrollEventCheck(true);
        EventStatusManager.SwitchDogEvent(true);

        // 진동 알림
        for (int i = 0; i < 4; i++)
        {
            dogAnimator.handleDogSuddenEvent("WelshPoop");
            Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }

        dogManager.updateStrollEventCheck(false);
        EventStatusManager.SwitchDogEvent(false);
    }

    // ============= 내부 함수 ===================

}