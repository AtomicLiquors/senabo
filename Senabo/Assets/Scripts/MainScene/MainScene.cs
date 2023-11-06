using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class MainSceneClass
{
    public long id;
    public string name;
    public int days;
}

public class MainScene : MonoBehaviour
{
    public Text MainTitleText;

    public GameObject actionModal;

    void Start()
    {
        StartCoroutine(WebRequestGET());
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("파라메타", "데이터");

        UnityWebRequest www = UnityWebRequest.Post("http://www.my-server.com/myform", form);
        www.SetRequestHeader("헤더", "헤더 값");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("성공!");
        }
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
            var response = JsonUtility.FromJson<APIResponse<MainSceneClass>>(jsonString);

            // 현재 날짜와 비교하여 며칠 째인지 확인해야 함
            Debug.Log("Received Object: " + response.data); //
        }
        else
        {
            MainSceneClass receivedData = new MainSceneClass { id = 123456, name = "만두", days = 30 };

            MainTitleText.text = receivedData.name + "와(과) 함께한 지 " + receivedData.days + "일째";

            Debug.Log("WebRequest Error Occured");
        }
    }

    public void LoadBathScene()
    {
        SceneManager.LoadScene("BathScene");
    }

    public void LoadMealScene()
    {
        SceneManager.LoadScene("MealScene");
    }

    public void LoadPoopScene()
    {
        SceneManager.LoadScene("PoopScene");
    }

    public void LoadDogWalking2DScene()
    {
        SceneManager.LoadScene("DogWalking2DScene");
    }

    public void LoadMoveHospitalScene()
    {
        SceneManager.LoadScene("MoveHospitalScene");
    }

    public void LoadMoveGroomingScene()
    {
        SceneManager.LoadScene("MoveGroomingScene");
    }

    public void LoadHeartScene()
    {
        SceneManager.LoadScene("HeartScene");
    }

    public void LoadOwnerDictScene()
    {
        SceneManager.LoadScene("OwnerDictScene");
    }

    public void LoadProfileScene()
    {
        SceneManager.LoadScene("ProfileScene");
    }

    public void LoadSettingScene()
    {
        SceneManager.LoadScene("SettingScene");
    }

}
