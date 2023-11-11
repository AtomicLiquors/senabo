using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    public Text MainTitleText;
    public GameObject actionModal;

    void Start()
    {
        SetTitleDayCount();
    }

    string GetVerb(string dogName)
    {
        char lastLetter = dogName.ElementAt(dogName.Length - 1);

        // 한글의 제일 처음과 끝의 범위 밖일 경우 
        if (lastLetter < 0xAC00 || lastLetter > 0xD7A3)
        {
            return "와";
        }

        return (lastLetter - 0xAC00) % 28 > 0 ? "이와" : "와";
    }

    void SetTitleDayCount()
    {
        string createTime = PlayerPrefs.GetString("createTime");
        System.DateTime createDate = System.DateTime.ParseExact(createTime, "yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture);

        System.TimeSpan dateDiff = System.DateTime.Now.Date - createDate.Date;
        MainTitleText.text = $"{PlayerPrefs.GetString("dogName")}{GetVerb(PlayerPrefs.GetString("dogName"))} 함께한 지 {dateDiff.Days + 1}일 째";
    }

    public void LoadBathScene()
    {
        SceneManager.LoadScene("BathScene");
    }

    public void LoadMealScene()
    {
        SceneManager.LoadScene("MealScene");
    }

    public void LoadPoopScene()
    {
        SceneManager.LoadScene("PoopScene");
    }

    public void LoadDogWalking2DScene()
    {
        SceneManager.LoadScene("WalkCheckListScene");
    }

    public void LoadMoveHospitalScene()
    {
        ReceiptScene.type = ReceiptType.HospitalCost1;
        SceneManager.LoadScene("MoveHospitalScene");
    }

    public void LoadMoveGroomingScene()
    {
        ReceiptScene.type = ReceiptType.GroomingCost;
        SceneManager.LoadScene("MoveGroomingScene");
    }

    public void LoadHeartScene()
    {
        SceneManager.LoadScene("HeartScene");
    }

    public void LoadOwnerDictScene()
    {
        SceneManager.LoadScene("OwnerDictScene");
        Debug.Log("견주 사전으로 이동");
    }

    public void LoadProfileScene()
    {
        SceneManager.LoadScene("ProfileScene");
    }

    public void LoadSettingScene()
    {
        SceneManager.LoadScene("SettingScene");
        Debug.Log("환경 설정으로 이동");
    }

}
