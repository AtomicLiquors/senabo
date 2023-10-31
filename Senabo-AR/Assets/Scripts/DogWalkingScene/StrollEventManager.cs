using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrollEventManager : MonoBehaviour
{
    private int[] randomTimes;

    // Start is called before the first frame update
    void Start()
    {
        randomTimes = new int[4];

        // 1�к��� 60�б��� ������ �� 4���� randomTimes �迭�� �Է�
        for (int i = 0; i < randomTimes.Length; i++)
        {
            randomTimes[i] = UnityEngine.Random.Range(1, 61) * 60; // 1���� 60������ ������ �� ����
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

    // ================���� �̺�Ʈ �߻� �Լ�================
    // 1. �ٸ� �������� ������ ��
    IEnumerator function1(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Debug.Log(delayTime);
    }

    // 2, ���� ������ �̹����� �ֿ� ������ �� ��
    IEnumerator function2(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Debug.Log(delayTime);
    }

    // 3. ���� �ɾƼ� �������� ������ �� ��
    IEnumerator function3(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Debug.Log(delayTime);
    }

    // 4. �躯 Ȱ��
    IEnumerator function4(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Debug.Log(delayTime);
    }
}
