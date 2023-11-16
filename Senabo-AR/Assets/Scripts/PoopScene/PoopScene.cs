using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PoopScene : MonoBehaviour
{
    public Image poopPadImage;

    public Sprite[] poopPadSprites;

    private int clickedCount = 0, spriteIndex = 0, changeLimit = 3;
    private Button button;

    void Start()
    {
        StartCoroutine(CreatePoop());

        button = poopPadImage.GetComponent<Button>();
        button.onClick.AddListener(OnClickPoopPad);
    }

    void OnClickPoopPad()
    {
        clickedCount++;
        // Debug.Log("click 횟수: " + clickedCount + ", 사진 순번: " + spriteIndex); // Debug Code
        if (clickedCount > changeLimit && spriteIndex < poopPadSprites.Length)
        {
            poopPadImage.sprite = poopPadSprites[spriteIndex++];
            clickedCount = 0;
        }

        if (spriteIndex == poopPadSprites.Length)
        {
            button.onClick.RemoveListener(OnClickPoopPad);
            StartCoroutine(CleanPoop());
        }
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
