using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrollContentSceneChanges : MonoBehaviour
{
    public void SceneChangeYoung()
    {
        SceneManager.LoadScene("YoungPuppyOwnerDictDetailScene");

    }

    public void SceneChangeFood()
    {
        SceneManager.LoadScene("FoodFeedingOwnerDictDetailScene");

    }

    public void SceneChangeBath()
    {
        SceneManager.LoadScene("BathOwnerDictDetailScene");

    }

    public void SceneChangeWalking()
    {
        SceneManager.LoadScene("WalkingOwnerDictDetailScene");

    }

    public void SceneChangeSnack()
    {
        SceneManager.LoadScene("SnackTimeOwnerDictDetailScene");

    }

    public void SceneChangeHospital()
    {
        SceneManager.LoadScene("HospitalTimeOwnerDictDetailScene");

    }

    public void SceneChangeIshouldcare()
    {
        SceneManager.LoadScene("IShouldCareOwnerDictDetailScene");

    }

    public void SceneChangeEmergency()
    {
        SceneManager.LoadScene("EmergencyIssueOwnerDictDetailScene");

    }



}
