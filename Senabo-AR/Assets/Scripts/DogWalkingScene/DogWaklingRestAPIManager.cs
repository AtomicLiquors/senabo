using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;

public class DogWalkingRestAPIManager : MonoBehaviour
{
    static public WalkEndRequestDtoClass walkEndRequestDto;
    void Start()
    {
        StartCoroutine(TestSenaboRequests());
       // StartCoroutine(SendWalkStartInfo());
       // StartCoroutine(SendWalkEndInfo());
    }

    public void handleWalkStart()
    {
        StartCoroutine(SendWalkStartInfo());
    }

    public void handleWalkEnd()
    {
        StartCoroutine(SendWalkEndInfo());
    }

    IEnumerator TestSenaboRequests()
    {
        // + PlayerPrefs.GetString("email");
        string url = $"{ServerSettings.SERVER_URL}/api/walk/list?email=ssafy@gmail.com";
        while (true)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(url);

            string accessToken = "tokentoken"; // 추후 PlayerPrefs에서 추출할 예정
            string jwtToken = $"Bearer {accessToken}";

            webRequest.SetRequestHeader("Authorization", jwtToken);

            yield return webRequest.SendWebRequest();

            // Request completed, check the result
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string jsonText = webRequest.downloadHandler.text;
                Debug.Log(jsonText);

                yield break;
            }
            else
            {
                // Request failed, handle the error
                Debug.LogError("Error: " + webRequest.error);
            }

            // Clean up the web request object
            webRequest.Dispose();

            // Wait for the specified delay before making the next request
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator SendWalkStartInfo()
    {
        string jsonFile = JsonUtility.ToJson(null);
        string url = $"{ServerSettings.SERVER_URL}/api/walk/start?email=ssafy@gmail.com";
        // Spring Security 적용 후에는 쿼리스트링 사용하지 않을 예정.

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, jsonFile))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonFile);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");


            yield return request.SendWebRequest();

            if (request.error == null)
            {
                Debug.Log(request.downloadHandler.text);
                yield break;
            }
            else
            {
                Debug.Log("산책 시작 정보 전송 실패");
                Debug.LogError(request.error.ToString());
            }
        }
    }

    IEnumerator SendWalkEndInfo()
    {
        walkEndRequestDto = new WalkEndRequestDtoClass(1.5);
        string jsonFile = JsonUtility.ToJson(walkEndRequestDto);
        string url = $"{ServerSettings.SERVER_URL}/api/walk/end?email=ssafy@gmail.com";

        
        UnityWebRequest request = UnityWebRequest.Put(url, jsonFile);
        
       // request.method = "PATCH";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonFile);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");


        yield return request.SendWebRequest();

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            yield break;
        }
        else
        {
            Debug.Log("산책 종료 정보 전송 실패");
            Debug.LogError(request.error.ToString());
        }
        
    }

}
