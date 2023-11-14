using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    public Text MainTitleText;
    public GameObject actionModal;

    public GameObject UserLocation;

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
        TimeSpan dateDiff = DateTime.Now.Date - DateTime.Parse(createTime).Date;
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

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("MainScene - 종료!!!!!\n입장시간:" + PlayerPrefs.GetString("enterTime"));

            PlayerPrefs.SetString("exitTime", DateTime.Now.ToString("yyyy.MM.dd.HH:mm"));

            Debug.Log("퇴장시간:" + PlayerPrefs.GetString("exitTime"));

            DateTime enterTime = DateTime.Parse(PlayerPrefs.GetString("enterTime"));
            DateTime exitTime = DateTime.Parse(PlayerPrefs.GetString("exitTime"));
            TimeSpan timeDiff = exitTime - enterTime;

            int diffMinute = timeDiff.Days * 24 * 60 + timeDiff.Hours * 60 + timeDiff.Minutes;

            Debug.Log("OnApplicationPause - diffMinute: " + diffMinute);
            RefreshTokenManager.Instance.UpdateTotalTime(diffMinute);
        }
        else
        {
            Debug.Log("RefreshTokenManager - OnApplicationPause");
            PlayerPrefs.SetString("enterTime", DateTime.Now.ToString("yyyy.MM.dd.HH:mm"));
            Debug.Log("새로운 입장 시간:" + PlayerPrefs.GetString("enterTime"));
        }
    }
}
