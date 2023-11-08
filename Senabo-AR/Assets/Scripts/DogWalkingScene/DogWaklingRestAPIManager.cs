using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;

public class DogWalkingRestAPIManager : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(SendSenaboRequests());
    }


    IEnumerator SendSenaboRequests()
    {
        // + PlayerPrefs.GetString("email");
        string url = $"{ServerSettings.SERVER_URL}/api/member/check?email=gyqls234@gmail.com";

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
    /*
    IEnumerator SendWalkStartInfo()
    {
        string url = ServerSettings.SERVER_URL + "/api/member/check?email=gyqls234@gmail.com";
        string jsonFile = JsonUtility.ToJson(SignUpManager.signUpRequestDto);
        string url = $"{ServerSettings.SERVER_URL}/api/member/sign-up";

         using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, jsonFile))
        {
             byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonFile);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

    
            yield return request.SendWebRequest();
        }
        while (true)
        {
            UnityWebRequest webRequest = UnityWebRequest.Post(url, json);

            // Optionally, set headers or add other configuration
            Debug.Log(webRequest.result);

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
    }*/


}
