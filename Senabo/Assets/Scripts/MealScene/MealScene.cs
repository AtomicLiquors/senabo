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
    // public Sprite mealSprite;
    private int clickedCount = 0, mealState = 0, changeLimit = 1;
    private Button button;
    void Start()
    {
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
            Debug.Log("MealScene CheckFeed Error! " + request.error);
        }
        else
        {
            Debug.Log("MealScene CheckFeed Success!");

            string jsonString = request.downloadHandler.text;
            var response = JsonUtility.FromJson<APIResponse<MealSceneClass>>(jsonString);

            if (response.data.possibleYn)
            {
                Debug.Log("배식 가능 상태"); // Debug Code
                button = mealImage.GetComponent<Button>();
                button.onClick.AddListener(OnClickMeal);
            }
            else
            {
                Debug.Log("배식 불가 상태"); // Debug Code
                mealImage.color = new Color32(0, 30, 225, 100);
                mealState = 1;
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
        clickedCount++;
        if (mealState == 0 && clickedCount > changeLimit)
        {
            mealImage.color = new Color32(0, 30, 225, 100);
            mealState = 1;

            StartCoroutine(GiveFeed());
            button.onClick.RemoveListener(OnClickMeal);
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
