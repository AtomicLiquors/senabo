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

        // 1�к��� 60�б��� ������ �� 4���� randomTimes �迭�� �Է�
        for (int i = 0; i < randomTimes.Length; i++)
        {
            randomTimes[i] = UnityEngine.Random.Range(1, 31); // 1���� 60������ ������ �� ����
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

    // ================���� �̺�Ʈ �߻� �Լ�================
    // 1. �ٸ� �������� ������ ��
    IEnumerator suddenEncounter(int delayTime)
    {
        delayTime = 1;
        welshAnim.SetTrigger("WelshBark");
        yield return new WaitForSeconds(delayTime);

        arObjectController.setDogEventTrigger();
        
        // ���� �˸�
        for (int i = 0; i < 2; i++)
        {
            Handheld.Vibrate(); // 0.5�ʰ� ������ �︲
            yield return new WaitForSeconds(1); 
        }


    }


    // 2, ���� ������ �̹����� �ֿ� ������ �� ��
    IEnumerator suddenEat(int delayTime)
    {
        welshAnim.SetTrigger("WelshEat");
        yield return new WaitForSeconds(delayTime);
        // ���� �˸�
        for (int i = 0; i < 2; i++)
        {
            Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }
    }

    // 3. ���� �ɾƼ� �������� ������ �� ��
    IEnumerator suddenStop(int delayTime)
    {
        welshAnim.SetTrigger("WelshSit");
        yield return new WaitForSeconds(delayTime);
        // ���� �˸�
        for (int i = 0; i < 2; i++)
        {
            //Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }
    }

    // 4. �躯 Ȱ��
    IEnumerator suddenPoop(int delayTime)
    {
        welshAnim.SetTrigger("WelshPoop");
        yield return new WaitForSeconds(delayTime);
        // ���� �˸�
        for (int i = 0; i < 2; i++)
        {
           // Handheld.Vibrate();
            yield return new WaitForSeconds(1);
        }
    }

    // ============= ���� �Լ� ===================

}