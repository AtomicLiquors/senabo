using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowDeleteAccountSuccessAlertModal : MonoBehaviour
{
    public void SceneChange()
    {
        SceneManager.LoadScene("DeleteAccountSuccessAlertModal");
    }
}
