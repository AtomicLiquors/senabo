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

    Animator welshAnim;

    // Start is called before the first frame update
    void Start()
    {
        if (welshAnim == null)
            welshAnim = dogObject.GetComponentInChildren<Animator>();

        randomTimes = new int[4];

        // 1분부터 60분까지 랜덤한 값 4개를 randomTimes 배열에 입력
        for (int i = 0; i < randomTimes.Length; i++)
        {
            randomTimes[i] = UnityEngine.Random.Range(1, 31); // 1부터 60까지의 랜덤한 값 생성
        }

        StartCoroutine(suddenEncounter(randomTimes[0]));
        StartCoroutine(suddenEat(randomTimes[1]));
        StartCoroutine(suddenStop(randomTimes[2]));
        StartCoroutine(suddenPoop(randomTimes[3]));
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ================돌발 이벤트 발생 함수================
    // 1. 다른 강아지를 만났을 때
    IEnumerator suddenEncounter(int delayTime)
    {
        delayTime = 1;
        welshAnim.SetTrigger("WelshBark");
        yield return new WaitForSeconds(delayTime);

        arObjectController.setDogEventTrigger();
        
        // 진동 알림
        for (int i = 0; i < 2; i++)
        {
            Handheld.Vibrate(); // 0.5초간 진동이 울림
            yield return new WaitForSeconds(1); 
        }


    }


    // 2, 땅에 떨어진 이물질을 주워 먹으려 할 때
    IEnumerator suddenEat(int delayTime)
    {
        welshAnim.SetTrigger("WelshEat");
        yield return new WaitForSeconds(delayTime);
        // 진동 알림
        for (int i = 0; i < 2; i++)
        {
            Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }
    }

    // 3. 주저 앉아서 움직이지 않으려 할 때
    IEnumerator suddenStop(int delayTime)
    {
        welshAnim.SetTrigger("WelshSit");
        yield return new WaitForSeconds(delayTime);
        // 진동 알림
        for (int i = 0; i < 2; i++)
        {
            //Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }
    }

    // 4. 배변 활동
    IEnumerator suddenPoop(int delayTime)
    {
        welshAnim.SetTrigger("WelshPoop");
        yield return new WaitForSeconds(delayTime);
        // 진동 알림
        for (int i = 0; i < 2; i++)
        {
           // Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }
    }

    // ============= 내부 함수 ===================

}