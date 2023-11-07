using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventStatusManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static bool isDogEventOn;
    void Start()
    {
        isDogEventOn = false;
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
}
