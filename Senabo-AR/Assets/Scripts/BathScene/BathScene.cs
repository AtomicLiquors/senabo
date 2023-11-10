using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class BathScene : MonoBehaviour
{
    public Text titleBarText;
    public GameObject titleBar, dogBodyImage, dogFaceImage;
    public Image background;
    // public Sprite wetImage; // for background
    private int selectType; // 1: Bath, 2: Teeth
    private Button dogBody, dogFace;

    void Start()
    {
        setSelectType(1);
    }
    
    public void setSelectType(int type)
    {
        selectType = type;

        if (type == 1)
        {
            titleBarText.text = "목욕";
            dogBodyImage.SetActive(true);
            dogBody = dogBodyImage.GetComponent<Button>();
            dogBody.onClick.AddListener(OnClickDogBody);
        }
        else
        {
            titleBarText.text = "양치";
            dogFaceImage.SetActive(true);
            dogFace = dogFaceImage.GetComponent<Button>();
            dogFace.onClick.AddListener(OnClickDogFace);
        }
        titleBar.SetActive(true);
    }

    void OnClickDogBody()
    {
        // 목욕

        // A 횟수 동안 거품내는 모션
        // 강아지 주변에 거품 이미지 생성

        // B 횟수 동안 물로 씻는 모션
        // 샤워기로 물 뿌림 + 거품 소멸
        // 욕실 바닥에 물 생성

        // 다 씻은 후 강아지 주변에 반짝 거림
        // 다 씼었음 알리는 Alert 등장

        StartCoroutine(DoBath());
    }

    void OnClickDogFace()
    {
        StartCoroutine(BrushTeeth());
    }

    IEnumerator DoBath()
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
            Debug.Log("BathroomScene DoBath Error! " + request.error);
        }
        else
        {
            Debug.Log("BathroomScene DoBath Success!");
        }
    }

    IEnumerator BrushTeeth()
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
            Debug.Log("BathroomScene BrushTeeth Error! " + request.error);
        }
        else
        {
            Debug.Log("BathroomScene BrushTeeth Success!");
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
