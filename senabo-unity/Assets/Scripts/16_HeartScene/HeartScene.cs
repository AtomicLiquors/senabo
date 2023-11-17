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
    public GameObject heartImage, dogAnimationGroup;
    public GameObject[] dogAnimations;

    private bool heartUploading = false;
    private bool communicating = false;

    private readonly float heartDelayTime = 2.0f; // TEST VALUE // 값 조정 필수 (애니메이션 별로 분리)
    private Button dogBody;

    void Start()
    {
        PlaySelectedAnimation(0);

        dogBody = dogAnimationGroup.GetComponent<Button>();
        dogBody.onClick.AddListener(GiveHeart);
    }

    void PlaySelectedAnimation(int index)
    {
        for (int i = 0; i < dogAnimations.Length; i++)
        {
            dogAnimations[i].SetActive(false);
        }
        dogAnimations[index].SetActive(true);
    }

    public void OnClickButton(int type)
    {
        if (communicating)
        {
            return;
        }

        communicating = true;

        PlaySelectedAnimation(type - 1);
        StartCoroutine(CommunicationAfterDelay(type));
    }

    void StopAnimation()
    {
        PlaySelectedAnimation(0);
    }

    public void GiveHeart()
    {
        if (heartUploading)
        {
            return;
        }

        heartUploading = true;
        heartImage.SetActive(true);

        StartCoroutine(UploadAfterDelay(heartDelayTime));
    }

    IEnumerator UploadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        heartUploading = false;
        heartImage.SetActive(false);
    }

    IEnumerator CommunicationAfterDelay(int type)
    {
        yield return new WaitForSeconds(heartDelayTime);

        StartCoroutine(Upload(type));
    }

    string GetHeartText(int type)
    {
        return type switch
        {
            1 => "WAIT",
            2 => "SIT",
            3 => "HAND",
            4 => "PAT",
            5 => "TUG",
            _ => "",// SHOULD NEVER RUN
        };
    }

    IEnumerator Upload(int type)
    {
        string url = $"{ServerSettings.SERVER_URL}/api/communication/save/{GetHeartText(type)}";
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

        string accessToken = PlayerPrefs.GetString("accessToken");
        string jwtToken = $"Bearer {accessToken}";
        request.SetRequestHeader("Authorization", jwtToken);

        yield return request.SendWebRequest();

        communicating = false;

        if (request.error == null)
        {
            Debug.Log("HeartScene Success! " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("HeartScene error!");

            if (request.responseCode == 403)
            {
                RefreshTokenManager.Instance.ReIssueRefreshToken();

                StartCoroutine(Upload(type));
            }
            else
            {
                SceneManager.LoadScene("MainScene");
            }
        }

        Invoke("StopAnimation", heartDelayTime);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("HeartScene - 종료!!!!!\n입장 시간:" + PlayerPrefs.GetString("enterTime"));

            PlayerPrefs.SetString("exitTime", DateTime.Now.ToString("yyyy.MM.dd.HH:mm"));

            Debug.Log("퇴장 시간:" + PlayerPrefs.GetString("exitTime"));

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
