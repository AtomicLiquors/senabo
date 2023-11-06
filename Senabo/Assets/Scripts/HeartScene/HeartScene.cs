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
    public Image dogImage, heartImage;
    public int heartType; // TEST CODE
    private Button dogBody;
    private int heartCount = 0, heartLimit = 3;
    private bool heartUploading = false;

    void Start()
    {
        dogBody = dogImage.GetComponent<Button>();
        dogBody.onClick.AddListener(giveHeart);
    }

    void giveHeart()
    {
        if (!heartUploading)
        {
            Debug.Log(heartCount + "회 교감 진행"); // Debug Code
            heartUploading = true;
            heartCount++;
            heartImage.enabled = true;
            StartCoroutine(UploadAfterDelay(1.0f));
        }

        if (heartCount > heartLimit)
        {
            dogBody.onClick.RemoveListener(giveHeart);
        }
    }

    IEnumerator UploadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(Upload());
        heartUploading = false;
        heartImage.enabled = false;
    }

    string getHeartType()
    {
        switch (heartType)
        {
            case HeartType.WAIT:
                return "WAIT";
            case HeartType.SIT:
                return "SIT";
            case HeartType.HAND:
                return "HAND";
            case HeartType.PAT:
                return "PAT";
            case HeartType.TUG:
                return "TUG";
            case HeartType.WALK:
                return "WALK";
        }
        return ""; // SHOULD NEVER RUN
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", PlayerPrefs.GetString("email"));

        // string url = ServerSettings.SERVER_URL + "/api/communication/save/" + getHeartType();
        string url = ServerSettings.SERVER_URL + "/api/communication/save/" + getHeartType() + "?email=" + PlayerPrefs.GetString("email"); // TEST CODE
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("HeartScene Error! " + request.error);
        }
        else
        {
            Debug.Log("HeartScene Success!");
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
