using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WalkResultInfo : MonoBehaviour
{
    private string email = "kim@ssafy.com";
    private string dogName = "두부부"; // PlayerPrefs에 저장돼 있다고 가정
    private int walkTime = 72;
    private double walkDistance = 2.3;

    public Text DogNameText;
    public Text TimeText;
    public Text DistanceText;

    public Text TodayWalkTimeText;
    public Text TodayWalkDistanceText;

    private void Awake()
    {
        DogNameText.text = dogName;
        TimeText.text = GetTimeFormat(walkTime);
        DistanceText.text = $"{walkDistance}km";

        StartCoroutine(GetTodayWalkInfo());
    }

    string GetTimeFormat(int time)
    {
        int hours = time / 60;
        int minutes = time % 60;

        if (hours > 0 && minutes > 0)
        {
            return $"{hours}시간 {minutes}분";
        }

        if (hours > 0)
        {
            return $"{hours}시간";
        }

        return $"{minutes}분";
    }

    IEnumerator GetTodayWalkInfo()
    {
        string api_url = ServerSettings.SERVER_URL + "/api/walk/today?email=" + email;

        UnityWebRequest response = UnityWebRequest.Get(api_url);

        string accessToken = "tokentoken"; // 추후 PlayerPrefs에서 추출할 예정
        string jwtToken = $"Bearer {accessToken}";

        response.SetRequestHeader("Authorization", jwtToken);

        yield return response.SendWebRequest();

        if (response.error == null)
        {
            Debug.Log(response.downloadHandler.text);

            TodayWalkInfoClass todayWalkInfo = JsonUtility.FromJson<APIResponse<TodayWalkInfoClass>>(response.downloadHandler.text).data;

            TodayWalkTimeText.text = GetTimeFormat(todayWalkInfo.todayTotalWalkTime);
            TodayWalkDistanceText.text = $"{todayWalkInfo.todayTotalWalkDistance}km";
        }
        else
        {
            Debug.Log("일일 산책 정보 불러오기 실패");
        }
    }
}
