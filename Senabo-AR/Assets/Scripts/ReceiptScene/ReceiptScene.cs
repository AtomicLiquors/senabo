using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class ReceiptHistory
{
    public string item;
    public string detail;
    public int amount;

    public ReceiptHistory(string item, string detail, int amount)
    {
        this.item = item;
        this.detail = detail;
        this.amount = amount;
    }
}

public class ReceiptType
{
    public const int InitialCost = 100;
    public const int EssentialCost = 200;
    public const int MonthlyCost = 300;
    public const int HospitalCost1 = 410;
    public const int HospitalCost2 = 420;
    public const int HospitalCost3 = 430;
    public const int HospitalCost4 = 440;
    public const int GroomingCost = 500;
    public const int DamageCost = 600;

}

public class ReceiptScene : MonoBehaviour
{
    public static int type;
    public Text totalPriceText;
    public GameObject receiptPrefab;
    public Transform receiptContent;
    private List<ReceiptHistory> receiptHistories = new List<ReceiptHistory>();

    void Start()
    {
        SelectReceiptPrefabs();
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", PlayerPrefs.GetString("email"));

        // string url = ServerSettings.SERVER_URL + "/api/expense/save";
        string url = ServerSettings.SERVER_URL + "/api/expense/save?email=" + PlayerPrefs.GetString("email"); // TEST CODE
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        string postJsonData = JsonUtility.ToJson(new { receiptHistories });
        byte[] jsonBytes = new System.Text.UTF8Encoding().GetBytes(postJsonData);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("ReceiptScene Error! " + request.error);
        }
        else
        {
            Debug.Log("Success!");
        }
    }

    void SelectReceiptPrefabs()
    {
        switch (type)
        {
            case ReceiptType.InitialCost: // 초기 비용 13만 원
                receiptHistories.Add(new ReceiptHistory("사료 그릇", "", 38000));
                receiptHistories.Add(new ReceiptHistory("리드줄", "", 29000));
                receiptHistories.Add(new ReceiptHistory("입마개", "", 8000));
                receiptHistories.Add(new ReceiptHistory("방석", "", 25000));
                receiptHistories.Add(new ReceiptHistory("장난감", "", 5000));
                receiptHistories.Add(new ReceiptHistory("발톱깎이", "", 10000));
                receiptHistories.Add(new ReceiptHistory("이동장", "", 15000));
                break;
            case ReceiptType.EssentialCost: // 필수 병원 비용 59만 원
                receiptHistories.Add(new ReceiptHistory("DHPPL 종합 백신 1차 접종", "", 25000));
                receiptHistories.Add(new ReceiptHistory("코로나 장염 백신 1차 접종", "", 15000));
                receiptHistories.Add(new ReceiptHistory("DHPPL 종합 백신 2차 접종", "", 25000));
                receiptHistories.Add(new ReceiptHistory("코로나 장염 백신 2차 접종", "", 15000));
                receiptHistories.Add(new ReceiptHistory("DHPPL 종합 백신 3차 접종", "", 25000));
                receiptHistories.Add(new ReceiptHistory("컨넬 코프 백신 1차 접종", "", 15000));
                receiptHistories.Add(new ReceiptHistory("DHPPL 종합 백신 4차 접종", "", 25000));
                receiptHistories.Add(new ReceiptHistory("컨넬 코프 백신 2차 접종", "", 15000));
                receiptHistories.Add(new ReceiptHistory("DHPPL 종합 백신 5차 접종", "", 25000));
                receiptHistories.Add(new ReceiptHistory("신종 플루 백신 1차 접종", "", 30000));
                receiptHistories.Add(new ReceiptHistory("신종 플루 백신 2차 접종", "", 30000));
                receiptHistories.Add(new ReceiptHistory("광견병 예방 접종", "", 20000));
                receiptHistories.Add(new ReceiptHistory("중성화 수술 비용", "", 300000));
                receiptHistories.Add(new ReceiptHistory("심장사상충 약", "", 25000));
                break;
            case ReceiptType.MonthlyCost: // 정기 비용 19만 8천 원
                receiptHistories.Add(new ReceiptHistory("사료", "", 100000));
                receiptHistories.Add(new ReceiptHistory("간식", "", 30000));
                receiptHistories.Add(new ReceiptHistory("배변 패드", "", 12000));
                receiptHistories.Add(new ReceiptHistory("배변 봉투", "", 5000));
                receiptHistories.Add(new ReceiptHistory("샴푸", "", 20000));
                receiptHistories.Add(new ReceiptHistory("귀 세정제", "", 13000));
                receiptHistories.Add(new ReceiptHistory("치약 및 칫솔", "", 18000));
                break;
            case ReceiptType.HospitalCost1: // 1. 단순 검진 비용
                receiptHistories.Add(new ReceiptHistory("진료 비용", "", 80000));
                break;
            case ReceiptType.HospitalCost2: // 2. 정기 검진 비용
                receiptHistories.Add(new ReceiptHistory("검진 비용", "", 250000));
                break;
            case ReceiptType.HospitalCost3: // 3. 알러지성 질환 치료 비용
                receiptHistories.Add(new ReceiptHistory("알레르기 완화제", "", 60000));
                break;
            case ReceiptType.HospitalCost4: // 4. 슬개골 탈구 치료 비용
                receiptHistories.Add(new ReceiptHistory("수술 전 혈액검사", "", 33000));
                receiptHistories.Add(new ReceiptHistory("방사선 검사", "", 40000));
                receiptHistories.Add(new ReceiptHistory("슬개골 탈구 수술 비용", "", 600000));
                receiptHistories.Add(new ReceiptHistory("입원비", "", 50000));
                break;
            case ReceiptType.GroomingCost: // 미용 비용
                receiptHistories.Add(new ReceiptHistory("미용비", "", 50000));
                break;
            case ReceiptType.DamageCost: // 훼손된 물건 복구 비용
                receiptHistories.Add(new ReceiptHistory("무선 이어폰", "", 200000));
                break;
        }

        StartCoroutine(Upload());
        CreateReceiptPrefabs();
    }

    string intToStringWon(int amount)
    {
        return amount.ToString("#,0") + "원";
    }

    void CreateReceiptPrefabs()
    {
        int totalPrice = 0;
        foreach (ReceiptHistory history in receiptHistories)
        {
            GameObject newReceipt = Instantiate(receiptPrefab, receiptContent);
            Text[] textElements = newReceipt.GetComponentsInChildren<Text>();
            textElements[0].text = history.item;
            textElements[1].text = intToStringWon(history.amount);
            totalPrice += history.amount;
        }

        totalPriceText.text = intToStringWon(totalPrice);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
