using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class BathScene : MonoBehaviour
{
    public Text titleBarText;
    public GameObject bubblePrefab;
    public Transform bubbleParent;
    public GameObject[] bubbleArray;
    public GameObject titleBar, dogBodyImage, dogFaceImage, shiningImage;
    public GameObject CantBathAlertPanel, HowToBathPanel1, HowToBathPanel2, HowToBathPanel3;
    public Image background;
    public Sprite wetImage; // for background
    private bool bathTimePassed = true, bathHistoryClear = true;
    private int selectType; // 1: Bath, 2: Teeth
    private int dogCount = 0, dogLimit = 10, dogState = 0;
    private Button dogBody, dogFace;

    void Start()
    {
        bubbleArray = new GameObject[dogLimit];
        setSelectType(1); // Bath Only

        StartCoroutine(CheckBathTime());
        StartCoroutine(CheckBathHistory());
    }

    public void setSelectType(int type)
    {
        selectType = type;

        if (type == 1)
        {
            titleBarText.text = "목욕";
            dogBodyImage.SetActive(true);

            if (bathTimePassed && bathHistoryClear)
            {
                dogBody = dogBodyImage.GetComponent<Button>();
                dogBody.onClick.AddListener(OnClickDogBody);

                HowToBathPanel1.SetActive(true);
                Invoke(nameof(CloseAllAlert), 2.0f);
            }
            else
            {
                CantBathAlertPanel.SetActive(true);
                Invoke(nameof(CloseAllAlert), 2.0f);
            }
        }
        else
        {
            titleBarText.text = "양치";
            dogFaceImage.SetActive(true);
            dogFace = dogFaceImage.GetComponent<Button>();
            dogFace.onClick.AddListener(OnClickDogFace);
            // 알림창 나타내기
        }
        titleBar.SetActive(true);
    }

    void OnClickDogBody()
    {
        if (dogState == 0)
        { // 거품 내기
            if (dogCount < dogLimit)
            {
                Vector3 randomPosition = new(Random.Range(-2f, 2f), Random.Range(-5f, 1f));
                Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
                bubbleArray[dogCount++] = Instantiate(bubblePrefab, randomPosition, randomRotation, bubbleParent);
            }
            else
            {
                dogState = 1;
                HowToBathPanel2.SetActive(true);
                Invoke(nameof(CloseAllAlert), 2.0f);
            }
        }
        else if (dogState == 1)
        { // 물로 씻기
            if (dogCount > 0)
            {
                Destroy(bubbleArray[--dogCount]);
            }
            else
            {
                dogState = 2;
                background.sprite = wetImage;

                shiningImage.SetActive(true);
                HowToBathPanel3.SetActive(true);
                Invoke(nameof(CloseAllAlert), 2.0f);

                dogBody.onClick.RemoveListener(OnClickDogBody);
                StartCoroutine(DoBath());
            }
        }
    }

    void OnClickDogFace()
    {
        StartCoroutine(BrushTeeth());
    }

    void CloseAllAlert()
    {
        CantBathAlertPanel.SetActive(false);
        HowToBathPanel1.SetActive(false);
        HowToBathPanel2.SetActive(false);
        HowToBathPanel3.SetActive(false);
    }

    IEnumerator CheckBathTime()
    {
        string email = PlayerPrefs.GetString("email");
        // string url = ServerSettings.SERVER_URL + "/api/member/get/" + email;
        string url = ServerSettings.SERVER_URL + "/api/member/get?email=" + email; // TEST CODE

        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            Debug.Log("BathScene CheckBathTime Success"); // Debug Code

            string jsonString = www.downloadHandler.text;
            var response = JsonUtility.FromJson<APIResponse<MemberGetResponseClass>>(jsonString);

            System.TimeSpan dateDiff = System.DateTime.Now.Date - System.DateTime.Parse(response.data.createTime).Date;
            Debug.Log("입양일로부터 " + (dateDiff.Days + 1) + "일째"); // Debug Code
            if (dateDiff.Days + 1 > 30)
            {
                bathTimePassed = true;
            }
        }
        else
        {
            Debug.Log("BathScene CheckBathTime Error!"); // Debug Code
        }
    }

    IEnumerator CheckBathHistory()
    {
        string email = PlayerPrefs.GetString("email");
        // string url = ServerSettings.SERVER_URL + "/api/bath/list" + email;
        string url = ServerSettings.SERVER_URL + "/api/bath/list?email=" + email; // TEST CODE

        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            Debug.Log("BathScene CheckBathHistory Success"); // Debug Code

            string jsonString = www.downloadHandler.text;
            var response = JsonUtility.FromJson<APIResponse<SignUpResponseDtoClass>>(jsonString);
            if (response.status == "FAIL")
            {
                bathHistoryClear = true;
            }
        }
        else
        {
            Debug.Log("BathScene CheckBathHistory Error!"); // Debug Code
        }
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
