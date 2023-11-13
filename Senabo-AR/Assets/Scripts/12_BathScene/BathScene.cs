using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
public class BathScene : MonoBehaviour
{
    public Text titleBarText;
    public GameObject bubblePrefab, dirtPrefab;
    public Transform bubbleParent, dirtParent;
    public GameObject[] bubbleArray, dirtArray;
    public GameObject titleBar, dogBodyImage, dogFaceImage, shiningImage;
    public GameObject CantBathAlertPanel, HowToBathPanel1, HowToBathPanel2, HowToBathPanel3, BrushTeethPanel1, BrushTeethPanel2;
    public Image background;
    public Sprite wetImage; // for background
    private bool bathTimePassed = false, bathHistoryClear = false, brushTeethPossible = false; // checking status
    private int selectType; // 1: Bath, 2: Teeth
    private int dogCount = 0, dogLimit = 10, dogState = 0;
    private Button dogBody, dogFace;

    void Start()
    {
        bubbleArray = new GameObject[dogLimit];
        dirtArray = new GameObject[dogLimit];

        CheckBathTime(); if(bathTimePassed) StartCoroutine(CheckBathHistory());
        StartCoroutine(CheckTeethPossible());
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
            CreateTeethDirt();
            
            dogFace = dogFaceImage.GetComponent<Button>();
            dogFace.onClick.AddListener(OnClickDogFace);

            BrushTeethPanel1.SetActive(true);
            Invoke(nameof(CloseAllAlert), 2.0f);
        }
        titleBar.SetActive(true);
    }

    void OnClickDogBody()
    {
        if (dogState == 0)
        { // 거품 내기
            if (dogCount < dogLimit)
            {
                Vector3 randomPosition = new(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-5f, 1f));
                Quaternion randomRotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
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

    void CreateTeethDirt()
    {
        while (dogCount < dogLimit)
        {
            Vector3 randomPosition = new(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-5f, 1f));
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
            dirtArray[dogCount++] = Instantiate(dirtPrefab, randomPosition, randomRotation, dirtParent);
        }
    }

    void OnClickDogFace()
    {
        if (dogCount > 0)
        {
            Destroy(dirtArray[--dogCount]);
        }
        else
        {
            shiningImage.SetActive(true);
            BrushTeethPanel2.SetActive(true);
            Invoke(nameof(CloseAllAlert), 2.0f);

            dogFace.onClick.RemoveListener(OnClickDogFace);
            StartCoroutine(BrushTeeth());
        }
    }

    void CloseAllAlert()
    {
        CantBathAlertPanel.SetActive(false);
        HowToBathPanel1.SetActive(false);
        HowToBathPanel2.SetActive(false);
        HowToBathPanel3.SetActive(false);
        BrushTeethPanel1.SetActive(false);
        BrushTeethPanel2.SetActive(false);
    }

    void CheckBathTime () {
        string createTime = PlayerPrefs.GetString("createTime");
        System.DateTime createDate = System.DateTime.ParseExact(createTime, "yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture);

        System.TimeSpan dateDiff = System.DateTime.Now.Date - createDate.Date;
        if(dateDiff.Days + 1 > 30) bathTimePassed = true;
    }

    IEnumerator CheckBathTimeByAPI()
    {
        string url = ServerSettings.SERVER_URL + "/api/member/get";

        UnityWebRequest www = UnityWebRequest.Get(url);
        string accessToken = PlayerPrefs.GetString("accessToken");
        string jwtToken = $"Bearer {accessToken}";

        www.SetRequestHeader("Authorization", jwtToken);
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

            if (www.responseCode == 403)
            {
                RefreshTokenManager.Instance.ReIssueRefreshToken();

                StartCoroutine(CheckBathTimeByAPI());
            }
            else
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    IEnumerator CheckBathHistory()
    {
        string url = ServerSettings.SERVER_URL + "/api/bath/list";

        UnityWebRequest www = UnityWebRequest.Get(url);
        string accessToken = PlayerPrefs.GetString("accessToken");
        string jwtToken = $"Bearer {accessToken}";

        www.SetRequestHeader("Authorization", jwtToken);
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

            if (www.responseCode == 403)
            {
                RefreshTokenManager.Instance.ReIssueRefreshToken();

                StartCoroutine(CheckBathHistory());
            }
            else
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    IEnumerator CheckTeethPossible()
    {
        string api_url = $"{ServerSettings.SERVER_URL}/api/brushing-teeth/check/{CountReport.selectedReportWeek}";

        UnityWebRequest response = UnityWebRequest.Get(api_url);

        string accessToken = PlayerPrefs.GetString("accessToken");
        string jwtToken = $"Bearer {accessToken}";

        response.SetRequestHeader("Authorization", jwtToken);

        yield return response.SendWebRequest();

        if (response.error == null)
        {
            Debug.Log(response.downloadHandler.text); // Debug Code

            TeethCheckResponseDtoClass teeth = JsonUtility.FromJson<APIResponse<TeethCheckResponseDtoClass>>(response.downloadHandler.text).data;
            brushTeethPossible = teeth.possibleYn;

            Debug.Log("BathScene CheckTeethPossible Success!"); // Debug Code
        }
        else
        {
            Debug.Log("BathScene CheckTeethPossible Error!"); // Debug Code

            if (response.responseCode == 403)
            {
                RefreshTokenManager.Instance.ReIssueRefreshToken();

                StartCoroutine(CheckTeethPossible());
            }
            else
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    IEnumerator DoBath()
    {
        string url = ServerSettings.SERVER_URL + "/api/bath/save"; // TEST CODE
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        string accessToken = PlayerPrefs.GetString("accessToken");
        string jwtToken = $"Bearer {accessToken}";
        request.SetRequestHeader("Authorization", jwtToken);

        yield return request.SendWebRequest();

        if (request.error == null)
        {
            Debug.Log("BathScene DoBath Success! " + request.error);
        }
        else
        {
            Debug.Log("BathScene DoBath Error!");

            if (request.responseCode == 403)
            {
                RefreshTokenManager.Instance.ReIssueRefreshToken();

                StartCoroutine(DoBath());
            }
            else
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    IEnumerator BrushTeeth()
    {
        string url = ServerSettings.SERVER_URL + "/api/brushing-teeth/save";
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        string accessToken = PlayerPrefs.GetString("accessToken");
        string jwtToken = $"Bearer {accessToken}";
        request.SetRequestHeader("Authorization", jwtToken);

        yield return request.SendWebRequest();

        if (request.error == null)
        {
            Debug.Log("BathScene BrushTeeth Success! " + request.error);
        }
        else
        {
            Debug.Log("BathScene BrushTeeth error!");

            if (request.responseCode == 403)
            {
                RefreshTokenManager.Instance.ReIssueRefreshToken();

                StartCoroutine(BrushTeeth());
            }
            else
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("BathScene - 종료!!!!!\n입장시간:" + PlayerPrefs.GetString("enterTime"));

            PlayerPrefs.SetString("exitTime", DateTime.Now.ToString("yyyy.MM.dd.HH:mm"));

            Debug.Log("퇴장시간:" + PlayerPrefs.GetString("exitTime"));

            DateTime enterTime = DateTime.Parse(PlayerPrefs.GetString("enterTime"));
            DateTime exitTime = DateTime.Parse(PlayerPrefs.GetString("exitTime"));
            TimeSpan timeDiff = exitTime - enterTime;

            int diffMinute = timeDiff.Days * 24 * 60 + timeDiff.Hours * 60 + timeDiff.Minutes;

            Debug.Log("OnApplicationPause - diffMinute: " + diffMinute);
            RefreshTokenManager.Instance.UpdateTotalTime(diffMinute);
        }
        else
        {
            Debug.Log("BathScene - OnApplicationPause");
            PlayerPrefs.SetString("enterTime", DateTime.Now.ToString("yyyy.MM.dd.HH:mm"));
            Debug.Log("새로운 입장 시간:" + PlayerPrefs.GetString("enterTime"));
        }
    }
}
