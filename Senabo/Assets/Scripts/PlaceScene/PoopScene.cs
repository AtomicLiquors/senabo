using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoopScene : MonoBehaviour
{
    void Start()
    {
        
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
