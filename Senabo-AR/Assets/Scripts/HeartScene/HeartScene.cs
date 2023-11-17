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
    public Button[] heartButtons;
    private int heartType = 0;
    private Button dogBody;
    private int[] heartCount = new int[5];
    private int heartLimit = 3;
    private bool heartUploading = false;

    void Start()
    {
        dogBody = dogImage.GetComponent<Button>();
        dogBody.onClick.AddListener(GiveHeart);
    }

    public void OffAlertPanel()
    {
        alertPanel.SetActive(false);
    }

    public void SetHeartType(int type)
    {
        heartType = type;
        OffAlertPanel();
    }

    void GiveHeart()
    {
        if (heartType == 0)
        {
            alertPanel.SetActive(true);
            Invoke(nameof(OffAlertPanel), 2.0f);
        }
        else
        {
            int current = heartType / 100 - 1;
            if (heartButtons[current].interactable && !heartUploading)
            {
                heartCount[current]++;
                heartUploading = true;
                heartImage.SetActive(true);
                StartCoroutine(UploadAfterDelay(1.0f));

                Debug.Log(GetHeartType() + " 교감을 " + heartCount[current] + "회 진행"); // Debug Code

                if (heartCount[current] >= heartLimit)
                {
                    heartButtons[current].interactable = false;
                }
            }
        }
    }

    IEnumerator UploadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(Upload());
        heartUploading = false;
        heartImage.SetActive(false);
    }

    string GetHeartType()
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

        // string url = ServerSettings.SERVER_URL + "/api/communication/save/" + GetHeartType();
        string url = ServerSettings.SERVER_URL + "/api/communication/save/" + GetHeartType() + "?email=" + PlayerPrefs.GetString("email"); // TEST CODE
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
