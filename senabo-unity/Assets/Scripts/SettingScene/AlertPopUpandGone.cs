using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlertPopUpandGone : MonoBehaviour
{
    public GameObject LogoutAlertPanel;

    public void OnLogoutButtonClicked()
    {
        LogoutAlertPanel.SetActive(true);
        Invoke("logoutAAA", 2.0f);
        Debug.Log("잘 닫힘");
    }

    void logoutAAA()
    {
        Debug.Log("호출도 된다");
        LogoutAlertPanel.SetActive(false);
        SceneManager.LoadScene("LogInScene");
    }
}
