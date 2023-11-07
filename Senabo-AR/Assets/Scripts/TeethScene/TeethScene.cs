using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class TeethScene : MonoBehaviour
{
    public Image background, dogImage;
    // public Sprite cleanTeethSprite;
    private int clickedCount = 0, changeLimit = 5;
    private Button button;

    void Start()
    {
        button = dogImage.GetComponent<Button>();
        button.onClick.AddListener(OnClickDog);
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", PlayerPrefs.GetString("email"));

        // string url = ServerSettings.SERVER_URL + "/api/brushing-teeth/save";
        string url = ServerSettings.SERVER_URL + "/api/brushing-teeth/save?email=" + PlayerPrefs.GetString("email"); // TEST CODE
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("TeethScene Error! " + request.error);
        }
        else
        {
            Debug.Log("TeethScene Success!");
        }
    }

    void OnClickDog()
    {
        clickedCount++;
        if (clickedCount > changeLimit)
        {
            StartCoroutine(Upload());
            button.onClick.RemoveListener(OnClickDog);
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
