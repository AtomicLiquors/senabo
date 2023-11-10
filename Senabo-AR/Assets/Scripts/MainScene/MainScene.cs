using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class MainSceneClass
{
    public long id;
    public string dogName;
    public string email;
    public string species;
    public char sex;
    public int affection;
    public int stressLevel;
    public double houseLatitude;
    public double houseLongitude;
    public int totalTime;
    public string uid;
    public string deviceToken;
    public string exitTime;
    public string enterTime;
    public string createTime;
    public string updateTime;
    public int days;
}

public class MainScene : MonoBehaviour
{
    public Text MainTitleText;
    public GameObject actionModal;

    void Start()
    {
        StartCoroutine(WebRequestGET());
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

    IEnumerator WebRequestGET()
    {
        string email = PlayerPrefs.GetString("email");
        // string url = ServerSettings.SERVER_URL + "/api/member/get/" + email;
        string url = ServerSettings.SERVER_URL + "/api/member/get?email=" + email; // TEST CODE

        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            string jsonString = www.downloadHandler.text;
            var response = JsonUtility.FromJson<APIResponse<MainSceneClass>>(jsonString);

            TimeSpan dateDiff = DateTime.Now.Date - DateTime.Parse(response.data.createTime).Date;
            MainTitleText.text = $"{PlayerPrefs.GetString("dogName")}{GetVerb(PlayerPrefs.GetString("dogName"))} 함께한 지 {dateDiff.Days + 1}일 째";
            //MainTitleText.text = response.data.dogName + "와(과) 함께한 지 " + (dateDiff + 1) + "일 째"; // TEST CODE
        }
        else
        {
            MainTitleText.text = "강아지와 함께한 지 0일 째";
            Debug.Log("WebRequest Error Occured"); // Debug Code
        }
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
        // SceneManager.LoadScene("OwnerDictScene");
        Debug.Log("견주 사전으로 이동");
    }

    public void LoadProfileScene()
    {
        SceneManager.LoadScene("ProfileScene");
    }

    public void LoadSettingScene()
    {
        //SceneManager.LoadScene("SettingScene");
        Debug.Log("환경 설정으로 이동");
    }

}
