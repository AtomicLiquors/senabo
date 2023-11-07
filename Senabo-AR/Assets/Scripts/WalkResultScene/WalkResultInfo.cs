using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WalkResultInfo : MonoBehaviour
{
    private int walkTime = 72;
    private double walkDistance = 2.3;

    public Text DogNameText;
    public Text WithText;
    public Text TimeText;
    public Text DistanceText;

    public Text TodayWalkTimeText;
    public Text TodayWalkDistanceText;

    private void Awake()
    {
        DogNameText.text = PlayerPrefs.GetString("dogName");
        WithText.text = GetVerb(PlayerPrefs.GetString("dogName"));
        TimeText.text = GetTimeFormat(walkTime);
        DistanceText.text = $"{walkDistance}km";

        StartCoroutine(GetTodayWalkInfo());
    }
    string GetVerb(string dogName)
    {
        char lastLetter = dogName.ElementAt(dogName.Length - 1);

        // 한글의 제일 처음과 끝의 범위 밖일 경우 
        if (lastLetter < 0xAC00 || lastLetter > 0xD7A3)
        {
            return "와";
        }

        return (lastLetter - 0xAC00) % 28 > 0 ? "이와" : "와";
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
        string api_url = $"{ServerSettings.SERVER_URL}/api/walk/today?email={PlayerPrefs.GetString("email")}";

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
