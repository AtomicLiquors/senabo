using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeartType
{
    public const int WAIT = 100;
    public const int SIT = 200;
    public const int HAND = 300;
    public const int PAT = 400;
    public const int TUG = 500;
    public const int WALK = 600;
}

public class HeartScene : MonoBehaviour
{
    public Image dogImage;
    public GameObject heartImage, alertPanel;
    public Sprite[] dogSprites; // normal
    public Sprite[] dogAnimations; // heart
    private int heartType = 0, typeNumber = 5;
    private bool heartUploading = false;
    private readonly int heartDelayTime = 2; // TEST VALUE
    private Button dogBody;

    void Start()
    {
        dogBody = dogImage.GetComponent<Button>();
        dogBody.onClick.AddListener(GiveHeart);
    }

    void Update()
    {
    }

    public void SetHeartType(int type)
    {
        heartType = type;
        OffAlertPanel();
        dogImage.sprite = dogSprites[GetHeartInteger()];

        Debug.Log("Heart Type: " + heartType); // Debug Code
    }

    void OffAlertPanel()
    {
        alertPanel.SetActive(false);
    }

    void GiveHeart()
    {
        int current = GetHeartInteger();
        if (current < 0 || current >= typeNumber)
        {
            alertPanel.SetActive(true);
            Invoke(nameof(OffAlertPanel), 2.0f);
            Debug.Log("Heart Type is " + current + ", can't communicate!"); // Debug Code !!!
        }
        else
        {
            if (!heartUploading)
            {
                heartUploading = true;
                heartImage.SetActive(true);

                // 교감하는 애니메이션
                // dogImage.sprite = dogAnimations[current];

                StartCoroutine(UploadAfterDelay(heartDelayTime));

                Debug.Log(GetHeartText() + " 교감 진행 시작"); // Debug Code
            }
            else
            {
                Debug.Log("교감 진행 중"); // Debug Code
            }
        }
    }

    IEnumerator UploadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(Upload());
        heartUploading = false;
        heartImage.SetActive(false);
        // dogImage.sprite = dogSprites[GetHeartInteger()];
    }

    int GetHeartInteger()
    {
        return (heartType / 100) - 1;
    }

    string GetHeartText()
    {
        return heartType switch
        {
            HeartType.WAIT => "WAIT",
            HeartType.SIT => "SIT",
            HeartType.HAND => "HAND",
            HeartType.PAT => "PAT",
            HeartType.TUG => "TUG",
            HeartType.WALK => "WALK",
            _ => "",// SHOULD NEVER RUN
        };
    }

    IEnumerator Upload()
    {
        string url = ServerSettings.SERVER_URL + "/api/communication/save";
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

        string accessToken = PlayerPrefs.GetString("accessToken");
        string jwtToken = $"Bearer {accessToken}";
        request.SetRequestHeader("Authorization", jwtToken);

        yield return request.SendWebRequest();

        if (request.error == null)
        {
            Debug.Log("HeartScene Success! " + request.error);
        }
        else
        {
            Debug.Log("HeartScene error!");

            if (request.responseCode == 403)
            {
                RefreshTokenManager.Instance.ReIssueRefreshToken();

                StartCoroutine(Upload());
            }
            else
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("HeartScene - 종료!!!!!\n입장시간:" + PlayerPrefs.GetString("enterTime"));

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
            Debug.Log("HeartScene - OnApplicationPause");
            PlayerPrefs.SetString("enterTime", DateTime.Now.ToString("yyyy.MM.dd.HH:mm"));
            Debug.Log("새로운 입장 시간:" + PlayerPrefs.GetString("enterTime"));
        }
    }
}
