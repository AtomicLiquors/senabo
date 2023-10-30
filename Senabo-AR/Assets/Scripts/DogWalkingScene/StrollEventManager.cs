using System;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private int[] randomTimes;
    private double countTimes;

    // Start is called before the first frame update
    void Start()
    {
        countTimes = 0;
        randomTimes = new int[4];

        // 1분부터 30분까지 랜덤한 값 4개를 randomTimes 배열에 입력
        for (int i = 0; i < randomTimes.Length; i++)
        {
            randomTimes[i] = UnityEngine.Random.Range(1, 61); // 1부터 60까지의 랜덤한 값 생성
        }
    }

    // Update is called once per frame
    void Update()
    {
        countTimes += Time.deltaTime;

        // 시간을 분 단위로 변환
        int currentMinutes = (int)(countTimes / 60);

        // 랜덤한 시간과 현재 시간이 일치하는지 확인하여 이벤트 실행
        for (int i = 0; i < randomTimes.Length; i++)
        {
            if (currentMinutes == randomTimes[i])
            {
                Debug.Log("이벤트 발생! 현재 시간: " + currentMinutes + "분, 랜덤 시간: " + randomTimes[i] + "분");
            }
        }

    }


}
