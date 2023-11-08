using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MoveToSettingScene : MonoBehaviour
{
    public void SceneChange()
    {
        SceneManager.LoadScene("SettingScene");
    }
}
