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

        // 1�к��� 60�б��� ������ �� 4���� randomTimes �迭�� �Է�
        for (int i = 0; i < randomTimes.Length; i++)
        {
            randomTimes[i] = UnityEngine.Random.Range(1, 31); // 1���� 60������ ������ �� ����
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

        dogEventTrigger = true;
        // ���� �˸�
        for (int i = 0; i < 2; i++)
        {
            Handheld.Vibrate(); // 0.5�ʰ� ������ �︲
            yield return new WaitForSeconds(1); 
        }
    }


    // 2, ���� ������ �̹����� �ֿ� ������ �� ��
    IEnumerator function2(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        dogEventTrigger = false;
        // ���� �˸�
        for (int i = 0; i < 2; i++)
        {
            Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }

    }

    // 3. ���� �ɾƼ� �������� ������ �� ��
    IEnumerator function3(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        dogEventTrigger = true;
        // ���� �˸�
        for (int i = 0; i < 2; i++)
        {
            //Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }
    }

    // 4. �躯 Ȱ��
    IEnumerator function4(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        dogEventTrigger = false;
        // ���� �˸�
        for (int i = 0; i < 2; i++)
        {
           // Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }
    }

    // ============= ���� �Լ� ===================

}