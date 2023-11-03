using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PoopScene : MonoBehaviour
{
    public Image poopPadImage;

    public Sprite[] poopPadSprites;

    private int clickedCount = 0, spriteIndex = 0, changeLimit = 3;

    void Start()
    {
        Button button = poopPadImage.GetComponent<Button>();
        button.onClick.AddListener(OnClickPoopPad);
    }

    void OnClickPoopPad() {
        clickedCount++;
        Debug.Log("click 횟수: " + clickedCount + ", 사진 순번: " + spriteIndex);
        if(clickedCount > changeLimit && spriteIndex < poopPadSprites.Length) {
            poopPadImage.sprite = poopPadSprites[spriteIndex++];
            clickedCount = 0;
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
