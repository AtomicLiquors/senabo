using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveReceiptScene : MonoBehaviour
{
    public void SceneChange()
    {
        SceneManager.LoadScene("ReceiptScene");
    }
}
