using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class ReceiptSceneClass
{
    public long id;
    public KeyValuePair<string, int>[] receipts;
}

public class ReceiptScene : MonoBehaviour
{
    public Text totalPriceText;
 
    private int totalPrice = 0; // private

    public GameObject receiptPrefab;
    public Transform receiptContent;

    void Start()
    {
        StartCoroutine(WebRequestGET());
    }

    IEnumerator WebRequestGET()
    {
        string email = "kim@ssafy.com";
        string url = ServerSettings.SERVER_URL + "/api/member/get/" + email;

        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            Debug.Log(www.downloadHandler.text); //
            string jsonString = www.downloadHandler.text;
            var response = JsonUtility.FromJson<APIResponse<ReceiptSceneClass>>(jsonString);

            Debug.Log("Received Object: " + response.data); //
            CreateReceiptPrefabs(response.data.receipts);
        }
        else
        {
            Debug.Log("WebRequest Error Occured");

            // Code For Test
            KeyValuePair<string, int>[] receipts = new KeyValuePair<string, int>[]
            { new("상품 1", 100), new("상품 2", 50), new("상품 3", 75), new("상품 4", 120) };
            CreateReceiptPrefabs(receipts);
        }
    }

    void CreateReceiptPrefabs(KeyValuePair<string, int>[] receipts)
    {
        for (int i = 0; i < receipts.Length; i++)
        {
            GameObject newReceipt = Instantiate(receiptPrefab, receiptContent);
            Text[] textElements = newReceipt.GetComponentsInChildren<Text>();
            if (textElements.Length == 2)
            {
                textElements[0].text = receipts[i].Key;
                textElements[1].text = receipts[i].Value.ToString();
                totalPrice += receipts[i].Value;
            }
            else
            {
                Debug.LogError("No TextElements in Prefab");
            }
        }
        
        totalPriceText.text = totalPrice.ToString() + "원";
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
