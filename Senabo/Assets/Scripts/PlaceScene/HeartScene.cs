using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class HeartScene : MonoBehaviour
{
    void Start()
    {
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
    
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    
}
