using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MealScene : MonoBehaviour
{
    public Image mealImage;
    // public Sprite mealSprite;
    private int clickedCount = 0, mealState = 0, changeLimit = 1;
    void Start()
    {
        Button button = mealImage.GetComponent<Button>();
        button.onClick.AddListener(OnClickMeal);
    }

    IEnumerator Upload()
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
            Debug.Log("MealScene Error! " + request.error);
        }
        else
        {
            Debug.Log("MealScene Success!");
        }
    }

    void OnClickMeal()
    {
        clickedCount++;
        if (mealState == 0 && clickedCount > changeLimit)
        {
            mealImage.color = new Color32(0, 30, 225, 100);
            mealState = 1;

            StartCoroutine(Upload());
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
