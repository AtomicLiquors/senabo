using Firebase.Messaging;
using Firebase;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class FCMManager : MonoBehaviour
{
    FirebaseApp _app;

    void Start()
    {
        Debug.Log("세나보 시작");

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            Debug.Log("파베 앱 초기화");

            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("파베 앱 초기화2");

                _app = FirebaseApp.DefaultInstance;
                FirebaseMessaging.TokenReceived += OnTokenReceived;
                FirebaseMessaging.MessageReceived += OnMessageReceived;

                Debug.Log("파베 앱 초기화3");

                MoveScene();
            }
        });
    }

    void MoveScene()
    {
        Debug.Log("파베 앱 초기화4");

        Debug.Log("email: " + PlayerPrefs.GetString("email"));
        Debug.Log("dogName: " + PlayerPrefs.GetString("dogName"));
        Debug.Log("houseLatitude: " + PlayerPrefs.GetFloat("houseLatitude"));
        Debug.Log("houseLongitude: " + PlayerPrefs.GetFloat("houseLongitude"));
        Debug.Log("accessToken: " + PlayerPrefs.GetString("accessToken"));
        Debug.Log("refreshToken: " + PlayerPrefs.GetString("refreshToken"));
        Debug.Log("enterTime: " + PlayerPrefs.GetString("enterTime"));
        Debug.Log("exitTime: " + PlayerPrefs.GetString("exitTime"));
        Debug.Log("createTime: " + PlayerPrefs.GetString("createTime"));

        if (PlayerPrefs.HasKey("email"))
        {
            Debug.Log("파베 앱 초기화5");

            PlayerPrefs.SetString("enterTime", DateTime.Now.ToString("yyyy.MM.dd.HH:mm"));

            Debug.Log("입장시간: " + PlayerPrefs.GetString("enterTime"));
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            Debug.Log("파베 앱 초기화6");

            SceneManager.LoadScene("LoginScene");
        }
    }

    void OnTokenReceived(object sender, TokenReceivedEventArgs e)
    {
        if (e != null)
        {
            Debug.Log($"\n[FIREBASE] Token: {e.Token}");
        }
    }

    void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        if (e != null && e.Message != null && e.Message.Notification != null)
        {
            Debug.Log($"\n[FIREBASE] From: {e.Message.From}");
            Debug.Log($"\n[FIREBASE] Title: {e.Message.Notification.Title}");
            Debug.Log($"\n[FIREBASE] Text: {e.Message.Notification.Body}");
        }
    }
}
