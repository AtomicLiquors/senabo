using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class BathScene : MonoBehaviour
{
    public int bathState = 0;
    public Image background;
    public Sprite wetImage;

    void Start()
    {
        Invoke("ChangeImage", 3.0f);
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

    public void ChangeImage()
    {
        background.sprite = wetImage;
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
