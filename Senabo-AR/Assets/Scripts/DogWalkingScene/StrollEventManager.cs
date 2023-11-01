using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class StrollEventManager : MonoBehaviour
{
    private int[] randomTimes;

    public GameObject spawnObject;

    public bool dogEventTrigger;

    // Start is called before the first frame update
    void Start()
    {
        randomTimes = new int[4];

        // 1분부터 60분까지 랜덤한 값 4개를 randomTimes 배열에 입력
        for (int i = 0; i < randomTimes.Length; i++)
        {
            randomTimes[i] = UnityEngine.Random.Range(1, 31); // 1부터 60까지의 랜덤한 값 생성
        }

        StartCoroutine(function1(randomTimes[0]));
        StartCoroutine(function2(randomTimes[1]));
        StartCoroutine(function3(randomTimes[2]));
        StartCoroutine(function4(randomTimes[3]));
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ================돌발 이벤트 발생 함수================
    // 1. 다른 강아지를 만났을 때
    IEnumerator function1(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        dogEventTrigger = true;
        // 진동 알림
        for (int i = 0; i < 2; i++)
        {
            Handheld.Vibrate(); // 0.5초간 진동이 울림
            yield return new WaitForSeconds(1); 
        }
    }


    // 2, 땅에 떨어진 이물질을 주워 먹으려 할 때
    IEnumerator function2(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        dogEventTrigger = false;
        // 진동 알림
        for (int i = 0; i < 2; i++)
        {
            Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }

    }

    // 3. 주저 앉아서 움직이지 않으려 할 때
    IEnumerator function3(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        dogEventTrigger = true;
        // 진동 알림
        for (int i = 0; i < 2; i++)
        {
            //Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }
    }

    // 4. 배변 활동
    IEnumerator function4(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        dogEventTrigger = false;
        // 진동 알림
        for (int i = 0; i < 2; i++)
        {
           // Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }
    }

    // ============= 내부 함수 ===================

}