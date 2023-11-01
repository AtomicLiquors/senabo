using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    public GameObject actionModal;

    void Start()
    {
        
    }

    public void LoadBathScene()
    {
        Debug.Log("화장실 ㄱㄱ");
        SceneManager.LoadScene("BathScene");
    }

    public void LoadPoopScene()
    {
        Debug.Log("응가 ㄱㄱ");
        SceneManager.LoadScene("PoopScene");
    }

    public void LoadMoveHospitalScene()
    {
        SceneManager.LoadScene("MoveHospitalScene");
    }
    
    public void LoadMoveGroomingScene()
    {
        SceneManager.LoadScene("MoveGroomingScene");
    }
    
    public void LoadHeartScene()
    {
        SceneManager.LoadScene("HeartScene");
    }
    
    public void LoadOwnerDictScene()
    {
        SceneManager.LoadScene("OwnerDictScene");
    }
    
    public void LoadProfileScene()
    {
        SceneManager.LoadScene("ProfileScene");
    }
    
    public void LoadSettingScene()
    {
        SceneManager.LoadScene("SettingScene");
    }

}
