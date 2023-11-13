using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventStatusManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static bool isDogEventOn;

    private static bool isDogStopResolved;
    private static bool isDogPoopResolved;

    private static int currentStress = 0;

    void Start()
    {
        isDogEventOn = false;

        isDogStopResolved = true;
        isDogPoopResolved = true;   
    }

    //���� ��Ȳ �߻� ������ Void Update�� isDogEventOn ��½� True�� ���� ��µ�.
    public static void SwitchDogEvent(bool eventStatus)
    {
        isDogEventOn = eventStatus;
    }
    public static bool GetDogEvent()
    {
        return isDogEventOn;
    }

    public static void IncreaseStress()
    {
        currentStress++;
        Debug.Log("stress : " + currentStress);
    }


    public static void SwitchDogStopResolved(bool eventStatus)
    {
        isDogStopResolved = eventStatus;
    }

    public static void SwitchDogPoopResolved(bool eventStatus)
    {
        isDogPoopResolved = eventStatus;
    }


    public static bool GetDogStopResolved()
    {
        return isDogStopResolved;
    }

    public static bool GetDogPoopResolved()
    {
        return isDogPoopResolved;
    }
}
