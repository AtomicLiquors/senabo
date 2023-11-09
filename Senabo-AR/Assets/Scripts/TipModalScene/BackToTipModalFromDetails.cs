using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTipModalFromDetails : MonoBehaviour
{

    public void LeashDetailButton()
    {
        SceneManager.LoadScene("LeashTipDetail");
    }

    public void NameTagDetailButton()
    {
        SceneManager.LoadScene("NameTagTipDetail");
    }

    public void WaterBottleDetailButton()
    {
        SceneManager.LoadScene("WaterAndBottleTipDetail");
    }

    public void PaperBagDetailButton()
    {
        SceneManager.LoadScene("PaperBagForShitTipDetail");
    }

    public void GagDetailButton()
    {
        SceneManager.LoadScene("GagForDogTipDetail");
    }

    public void BackButton()
    {
        SceneManager.LoadScene("TipModalScene");
    }
    
}
