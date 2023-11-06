using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class BathScene : MonoBehaviour
{
    public Image background, dogImage;
    public Sprite wetImage;
    private int clickedCount = 0, bathState = 0, changeLimit = 10;

    void Start()
    {
        Button button = dogImage.GetComponent<Button>();
        button.onClick.AddListener(OnClickDog);
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", PlayerPrefs.GetString("email"));

        // string url = ServerSettings.SERVER_URL + "/api/bath/save";
        string url = ServerSettings.SERVER_URL + "/api/bath/save?email=" + PlayerPrefs.GetString("email"); // TEST CODE
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("BathScene Error! " + request.error);
        }
        else
        {
            Debug.Log("BathScene Success!");
        }
    }

    void OnClickDog()
    {
        clickedCount++;
        if(bathState == 0 && clickedCount > changeLimit) {
            background.sprite = wetImage;
            bathState = 1;

            StartCoroutine(Upload());
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
