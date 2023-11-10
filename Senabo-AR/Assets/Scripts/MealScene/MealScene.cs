using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

[System.Serializable]
public class MealSceneClass
{
    public bool possibleYn;
    public string lastFeedDateTime;
    public string nowDateTime;
}

public class MealScene : MonoBehaviour
{
    public Image mealImage;
    // public Sprite mealSprite; // 꽉 찬 밥그릇 이미지
    public GameObject MealFedAlertPanel;
    private bool mealable = false;
    private Button button;
    void Start()
    {
        MealFedAlertPanel.SetActive(false);

        button = mealImage.GetComponent<Button>();
        button.onClick.AddListener(OnClickMeal);

        StartCoroutine(CheckFeed());
    }

    IEnumerator CheckFeed()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", PlayerPrefs.GetString("email"));

        // string url = ServerSettings.SERVER_URL + "/api/feed/check";
        string url = ServerSettings.SERVER_URL + "/api/feed/check?email=" + PlayerPrefs.GetString("email"); // TEST CODE
        UnityWebRequest request = new UnityWebRequest(url, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("MealScene CheckFeed Error! " + request.error); // Debug Code
        }
        else
        {
            Debug.Log("MealScene CheckFeed Success!"); // Debug Code

            string jsonString = request.downloadHandler.text;
            var response = JsonUtility.FromJson<APIResponse<MealSceneClass>>(jsonString);

            if (response.data.possibleYn)
            {
                Debug.Log("배식 가능 상태"); // Debug Code
                mealable = true;
            }
            else
            {
                Debug.Log("배식 불가 상태"); // Debug Code
            }
        }
    }

    IEnumerator GiveFeed()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", PlayerPrefs.GetString("email"));

        // string url = ServerSettings.SERVER_URL + "/api/feed/save";
        string url = ServerSettings.SERVER_URL + "/api/feed/save?email=" + PlayerPrefs.GetString("email"); // TEST CODE
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("MealScene GiveFeed Error! " + request.error);
        }
        else
        {
            Debug.Log("MealScene GiveFeed Success!");
        }
    }

    void OnClickMeal()
    {
        if (mealable)
        {
            // 배식 가능 상태, 배식 진행
            // mealImage.sprite = mealSprite;

            StartCoroutine(GiveFeed());
            mealable = false;
        }
        else
        {
            // 배식 불가 상태, 해당 사항 알림
            MealFedAlertPanel.SetActive(true);
            Invoke(nameof(DestroyModal), 2.0f);
        }
    }

    void DestroyModal()
    {
        MealFedAlertPanel.SetActive(false);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
