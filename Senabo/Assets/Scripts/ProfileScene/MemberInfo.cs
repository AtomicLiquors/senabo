using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MemberInfo : MonoBehaviour
{
    public Text dogNameText;
    public Text emailText;

    public Text affectionIndexText;
    public Text stressIndexText;

    public Text totalExpenditureAmountText;

    public Image googleIcon;

    public string email;
    public long id;

    private void Awake()
    {
        id = 3;
        email = "kim@ssafy.com";
        
        StartCoroutine(GetMemberInfo());
        StartCoroutine(GetTotalExpenditureAmount());
    }

    IEnumerator GetMemberInfo()
    {
        string api_url = ServerSettings.SERVER_URL + "/api/member/get/" + email;

        UnityWebRequest response = UnityWebRequest.Get(api_url);

        yield return response.SendWebRequest();

        if (response.error == null)
        {
            Debug.Log(response.downloadHandler.text);

            MemberClass member = JsonUtility.FromJson<APIResponse<MemberClass>>(response.downloadHandler.text).data;

            dogNameText.text = member.dogName;
            emailText.text = member.email;
            affectionIndexText.text = $"{member.affection}";
            stressIndexText.text = $"{member.stressLevel}";
        }
        else
        {
            Debug.Log("사용자 정보 불러오기 실패");
        }
    }

    IEnumerator GetTotalExpenditureAmount()
    {
        string api_url = ServerSettings.SERVER_URL + "/api/activity/expense/total?id=" + id;

        UnityWebRequest response = UnityWebRequest.Get(api_url);

        string accessToken = "tokentoken"; // 추후 PlayerPrefs에서 추출할 예정
        string jwtToken = $"Bearer {accessToken}";

        response.SetRequestHeader("Authorization", jwtToken);

        yield return response.SendWebRequest();

        if (response.error == null)
        {
            Debug.Log(response.downloadHandler.text);

            TotalExpenditureAmountClass totalExpenditureAmount = JsonUtility.FromJson<APIResponse<TotalExpenditureAmountClass>>(response.downloadHandler.text).data;

            if (totalExpenditureAmount.totalAmount == 0)
            {
                totalExpenditureAmountText.text = "0원";
            } else
            {
                totalExpenditureAmountText.text = string.Format("{0:#,###}원", totalExpenditureAmount.totalAmount);
            }
        }
        else
        {
            Debug.Log("총 금액 불러오기 실패");
        }
    }
}
