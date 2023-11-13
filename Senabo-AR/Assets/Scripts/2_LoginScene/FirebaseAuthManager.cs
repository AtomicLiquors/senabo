using Firebase;
using Firebase.Auth;
using Google;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class FirebaseAuthManager : MonoBehaviour
{
    public string webClientId;

    private GoogleSignInConfiguration configuration;

    static public SignUpRequestDtoClass signUpRequestDto;

    private void Awake()
    {
        Debug.Log("***************���̾�̽� � �Ŵ��� ����**********");
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestEmail = true,
            RequestIdToken = true
        };
    }

    public void SignInWithGoogle() 
    {
        Debug.Log("***************���̾�̽� � �Ŵ��� SignInWithGoogle**********");
        OnSignIn(); 
    }

    public void SignOutFromGoogle()
    {
        Debug.Log("�α׾ƿ� - SignOutFromGoogle");
        OnSignOut();
    }

    private void OnSignIn()
    {
        Debug.Log("***************���̾�̽� � �Ŵ��� OnSignIn**********");
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        Debug.Log("Calling SignIn");
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
        Debug.Log("End OnSignIn");
    }

    private void OnSignOut()
    {
        Debug.Log("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();

        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene("LoginScene");
    }

    public void OnDisconnect()
    {
        Debug.Log("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        Debug.Log("***************���̾�̽� � �Ŵ��� OnAuthenticationFinished**********");
        if (task.IsFaulted)
        {
            Debug.Log("isFaulted");
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.Log("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.Log("Got Unexpected Exception:" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.Log("Canceled");
        }
        else
        {
            Debug.Log("success");
            Debug.Log("Email = " + task.Result.Email);
            Debug.Log("Google ID Token = " + task.Result.IdToken);
            Debug.Log("AuthCode:" + task.Result.AuthCode);

            string email = task.Result.Email;

            StartCoroutine(PostFirebaseAuth(email));
        }
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        Debug.Log("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        Debug.Log("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    IEnumerator PostFirebaseAuth(string email)
    {
        SignInRequestDtoClass signInRequestDtoClass = new SignInRequestDtoClass();
        signInRequestDtoClass.email = email;

        string jsonFile = JsonUtility.ToJson(signInRequestDtoClass);
        string api_url = $"{ServerSettings.SERVER_URL}/api/member/sign-in";

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(api_url, jsonFile))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonFile);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            Debug.Log("���!!:\n" + request.downloadHandler.text);

            if (request.error == null)
            {
                Debug.Log(request.downloadHandler.text);

                SignInResponseDtoClass signInResponse = JsonUtility.FromJson<APIResponse<SignInResponseDtoClass>>(request.downloadHandler.text).data;

                if (signInResponse.isMember) // ȸ���� ���
                {
                    Debug.Log("ȸ���� ���");
                    PlayerPrefs.SetString("dogName", signInResponse.dogName);
                    PlayerPrefs.SetString("email", signInResponse.email);
                    PlayerPrefs.SetFloat("houseLatitude", (float)signInResponse.houseLatitude);
                    PlayerPrefs.SetFloat("houseLongitude", (float)signInResponse.houseLongitude);
                    PlayerPrefs.SetString("accessToken", signInResponse.token.accessToken);
                    PlayerPrefs.SetString("refreshToken", signInResponse.token.refreshToken);
                    PlayerPrefs.SetString("createTime", signInResponse.createTime);
                    PlayerPrefs.SetString("enterTime", DateTime.Now.ToString("yyyy.MM.dd.HH:mm"));

                    Debug.Log("����ð�: " + PlayerPrefs.GetString("enterTime"));
                    SceneManager.LoadScene("MainScene");
                }
                else // ��ȸ���� ���
                {
                    Debug.Log("��ȸ���� ���");

                    // ��ȸ�� alert

                    signUpRequestDto = new SignUpRequestDtoClass();
                    signUpRequestDto.email = email;

                    SceneManager.LoadScene("LocationSettingScene");
                }
            }
            else
            {
                Debug.Log("ȸ�� ���� Ȯ�� ����");
                SignOutFromGoogle();
            }
        }
    }
}
