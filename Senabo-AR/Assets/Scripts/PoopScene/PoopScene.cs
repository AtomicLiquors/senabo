using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PoopScene : MonoBehaviour
{
    public Image poopPadImage;
    public GameObject poopPadObject, plasticBagObject, shiningObject;
    public Sprite[] poopPadSprites; // clean, fold1, fold2, fold3, dirt1, dirt2
    public GameObject NoPoopPanel, PoopCleanPanel1, PoopCleanPanel2, PoopCleanPanel3;
    private bool isPoop = false;
    private int padCount = 0, padState = 0, padLimit = 3;
    private Button button, button2;

    void Start()
    {
        // StartCoroutine(CreatePoop()); // TEST CODE
        StartCoroutine(CheckIsPoop());
    }

    void AfterJudgePoop()
    {
        poopPadObject.SetActive(true);

        if (isPoop)
        {
            PoopCleanPanel1.SetActive(true);
            Invoke(nameof(CloseAllPanel), 1.0f);

            SetPoopPadImage();
            button = poopPadImage.GetComponent<Button>();
            button.onClick.AddListener(OnClickPoopPad);
            button2 = plasticBagObject.GetComponent<Button>();
            button2.onClick.AddListener(OnClickPoopPad);
        }
        else
        {
            shiningObject.SetActive(true);
            NoPoopPanel.SetActive(true);
            Invoke(nameof(CloseAllPanel), 2.0f);
        }
    }

    void SetPoopPadImage()
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next(2); // dirt sprite: 2
        poopPadImage.sprite = poopPadSprites[randomNumber + 4];
    }

    void OnClickPoopPad()
    {
        if (padState < 3)
        {
            padCount++;
            if (padCount >= padLimit)
            {
                poopPadImage.sprite = poopPadSprites[++padState];
                padCount = 0;

                if (padState == 3)
                {
                    PoopCleanPanel2.SetActive(true);
                    Invoke(nameof(CloseAllPanel), 1.0f);

                    poopPadObject.transform.position = new Vector3(0, 2);
                    plasticBagObject.SetActive(true);
                }
            }
        }
        else
        {
            if (padCount == 0)
            {
                // 1. 접힌 패드 버리기 (넣어지는 애니메이션)
                poopPadObject.SetActive(false);
            }
            else if (padCount == 1)
            {
                // 2. 쓰레기 봉투 치우기 (아래로 내려가는 애니메이션)
                plasticBagObject.SetActive(false);

                poopPadObject.SetActive(true);
                poopPadImage.sprite = poopPadSprites[0];
                poopPadObject.transform.position = new Vector3(3.5f, -3.5f);
            }
            else if (padCount == 2)
            {
                // 3. 새 패드 꺼내기 (위로 올라오는 애니메이션)
                poopPadObject.transform.position = new Vector3(0, 0);

                PoopCleanPanel3.SetActive(true);
                Invoke(nameof(CloseAllPanel), 2.0f);
                shiningObject.SetActive(true);

                button.onClick.RemoveListener(OnClickPoopPad);
                button2.onClick.RemoveListener(OnClickPoopPad);

                StartCoroutine(CleanPoop());
            }

            padCount++;
        }
    }

    void CloseAllPanel()
    {
        NoPoopPanel.SetActive(false);
        PoopCleanPanel1.SetActive(false);
        PoopCleanPanel2.SetActive(false);
        PoopCleanPanel3.SetActive(false);
    }

    IEnumerator CreatePoop()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", PlayerPrefs.GetString("email"));

        // string url = ServerSettings.SERVER_URL + "/api/poop/save";
        string url = ServerSettings.SERVER_URL + "/api/poop/save?email=" + PlayerPrefs.GetString("email"); // TEST CODE
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("PoopScene CreatePoop Error! " + request.error);
        }
        else
        {
            Debug.Log("PoopScene CreatePoop Success!");
        }
    }

    IEnumerator CheckIsPoop()
    {
        string email = PlayerPrefs.GetString("email");
        // string url = ServerSettings.SERVER_URL + "/api/feed/latest" + email;
        string url = ServerSettings.SERVER_URL + "/api/feed/latest?email=" + email; // TEST CODE

        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            Debug.Log("PoopScene CheckIsPoop Success"); // Debug Code

            string jsonString = www.downloadHandler.text;
            var response = JsonUtility.FromJson<APIResponse<FeedLatestDtoClass>>(jsonString);

            System.DateTimeOffset dateCreateTime = System.DateTimeOffset.ParseExact(response.data.createTime, "yyyy-MM-ddTHH:mm:ss.fffZ", null);
            System.DateTimeOffset date3HoursAgo = System.DateTimeOffset.UtcNow.AddHours(-3);

            if (dateCreateTime < date3HoursAgo && response.data.createTime == response.data.updateTime)
            {
                isPoop = true;
            }
        }
        else
        {
            // isPoop = true; // TEST CODE
            Debug.Log("PoopScene CheckIsPoop Error"); // Debug Code
        }

        AfterJudgePoop();
    }

    IEnumerator CleanPoop()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", PlayerPrefs.GetString("email"));

        // string url = ServerSettings.SERVER_URL + "/api/poop/clean";
        string url = ServerSettings.SERVER_URL + "/api/poop/clean?email=" + PlayerPrefs.GetString("email"); // TEST CODE
        UnityWebRequest request = new UnityWebRequest(url, "PATCH");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("PoopScene CleanPoop Error! " + request.error);
        }
        else
        {
            Debug.Log("PoopScene CleanPoop Success!");
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
