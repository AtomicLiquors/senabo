using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventStatusManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static bool isDogEventOn;
    private static bool isHarnessOn;
    void Start()
    {
        isDogEventOn = false;
        isHarnessOn = true;
    }

    public static void SwitchDogEvent(bool eventStatus)
    {
        isDogEventOn = eventStatus;
    }
    public static bool GetDogEvent()
    {
        return isDogEventOn;
    }

    public static void SwitchHarnessOn(bool eventStatus)
    {
        isHarnessOn = eventStatus;
    }
    public static bool GetHarnessOn()
    {
        return isHarnessOn;
    }
}
