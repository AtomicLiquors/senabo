using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BathScene : MonoBehaviour
{
    public int bathState = 0;
    public Image background;
    public Sprite wetImage;

    void Start()
    {
        Invoke("ChangeImage", 3.0f);
    }
    
    public void ChangeImage()
    {
        background.sprite = wetImage;
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
