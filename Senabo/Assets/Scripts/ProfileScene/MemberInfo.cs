using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberInfo : MonoBehaviour
{
    public string snsType = "KAKAO"; // KAKAO, GOOGLE
    public int affectionIndex = 30;
    public int stressIndex = 50;
    public int totalExpenditureAmount = 819000;

    public Text dogNameText;
    public Text emailText;

    public Text affectionIndexText;
    public Text stressIndexText;

    public Text totalExpenditureAmountText;

    public Image kakaoIcon;
    public Image googleIcon;

    // Start is called before the first frame update
    void Start()
    {
        dogNameText.text = "´©·îÀÌ";
        emailText.text = "kimssafy@gmail.com";

        if (snsType == "KAKAO") {
            kakaoIcon.gameObject.SetActive(true);
            googleIcon.gameObject.SetActive(false);
        } else if (snsType == "GOOGLE") {
            kakaoIcon.gameObject.SetActive(false);
            googleIcon.gameObject.SetActive(true);
        }

        affectionIndexText.text = affectionIndex.ToString();
        stressIndexText.text = stressIndex.ToString();

        totalExpenditureAmountText.text = string.Format("{0:#,###}¿ø", totalExpenditureAmount);
    }
}
