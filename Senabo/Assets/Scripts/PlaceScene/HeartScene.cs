using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartScene : MonoBehaviour
{
    void Start()
    {
        
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    
}
