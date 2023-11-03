using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PoopScene : MonoBehaviour
{
    public Image poopPadImage;

    public Sprite[] poopPadSprites;

    private int clickedCount = 0, spriteIndex = 0, changeLimit = 3;

    void Start()
    {
        StartCoroutine(Upload());

        Button button = poopPadImage.GetComponent<Button>();
        button.onClick.AddListener(OnClickPoopPad);
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
    
    void OnClickPoopPad() {
        clickedCount++;
        Debug.Log("click 횟수: " + clickedCount + ", 사진 순번: " + spriteIndex);
        if(clickedCount > changeLimit && spriteIndex < poopPadSprites.Length) {
            poopPadImage.sprite = poopPadSprites[spriteIndex++];
            clickedCount = 0;
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
