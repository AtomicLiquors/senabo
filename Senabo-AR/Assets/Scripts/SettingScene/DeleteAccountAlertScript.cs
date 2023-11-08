using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteAccountAlertScript : MonoBehaviour
{
    public GameObject DeleteAccountAlertPanel;

    public void OnDeleteButtonClicked()
    {
        DeleteAccountAlertPanel.SetActive(true);
        Invoke("Delete", 2.0f);
        Debug.Log("잘 닫힘");
    }

    void Delete()
    {
        Debug.Log("호출도 된다");
        DeleteAccountAlertPanel.SetActive(false);
        SceneManager.LoadScene("SignUpScene");
    }
}
