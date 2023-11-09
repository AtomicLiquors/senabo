using System;
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
    public GameObject TextLengthZeroModalPanel;
    public GameObject TextLengthOverModalPanel;
    public InputField inputField;

    void Awake()
    {
        FailSignUpAlertModalPanel.SetActive(false);
        TextLengthZeroModalPanel.SetActive(false);
        TextLengthOverModalPanel.SetActive(false);

        EmailText.text = SignUpManager.signUpRequestDto.email;
        // EmailText.text = "test@ssafy.com"; // TEST CODE

        inputField.onValueChanged.AddListener(delegate { CheckInput(); });
    }

    void InvokeCloseModal()
    {
        FailSignUpAlertModalPanel.SetActive(false);
        TextLengthZeroModalPanel.SetActive(false);
        TextLengthOverModalPanel.SetActive(false);
    }

    void CheckInput()
    {
        if (inputField.text.Length >= 6)
        {
            TextLengthOverModalPanel.SetActive(true);
            Invoke(nameof(InvokeCloseModal), 2.0f);
            inputField.text = inputField.text[..5];
        }
    }

    public void OnAdoptButtonClick()
    {
        if (inputField.text.Length == 0)
        {
            TextLengthZeroModalPanel.SetActive(true);
            Invoke(nameof(InvokeCloseModal), 1.0f);
        }
        else
        {
            SignUpManager.signUpRequestDto.dogName = inputField.text;
            StartCoroutine(PostMember());
        }
    }

    void CloseFailSignUpAlertModalPanel()
    {
        FailSignUpAlertModalPanel.SetActive(false);
        SceneManager.LoadScene("SignUpScene");
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
                Debug.Log(request.downloadHandler.text); // Debug Code
                SignUpResponseDtoClass signUpResponseDto = JsonUtility.FromJson<APIResponse<SignUpResponseDtoClass>>(request.downloadHandler.text).data;
                PlayerPrefs.SetString("email", signUpResponseDto.email);
                PlayerPrefs.SetString("dogName", signUpResponseDto.dogName);
                SceneManager.LoadScene("MainScene");
            }
            else
            {
                Debug.Log("회원가입 실패"); // Debug Code
                FailSignUpAlertModalPanel.SetActive(true);
                Invoke("CloseFailSignUpAlertModalPanel", 2.0f);
            }
        }
    }
}
