using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
    public Text EmailText;
    public GameObject FailSignUpAlertModalPanel;
    public InputField inputField;

    void Awake()
    {
        FailSignUpAlertModalPanel.SetActive(false);
        EmailText.text = SignUpManager.signUpRequestDto.email;
    }

    public void OnAdoptButtonClick()
    {
        SignUpManager.signUpRequestDto.dogName = inputField.text;
        StartCoroutine(PostMember());
    }

    IEnumerator PostMember()
    {
        string jsonFile = JsonUtility.ToJson(SignUpManager.signUpRequestDto);

        string api_url = $"{ServerSettings.SERVER_URL}/api/member/sign-up";

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(api_url, jsonFile))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonFile);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.error == null)
            {
                Debug.Log(request.downloadHandler.text);

                /*
                    PlayerPrefs�� ����� ���� ����
                */

                SceneManager.LoadScene("MainScene");
            }
            else
            {
                Debug.Log("ȸ������ ����");
            }
        }
    }
}
