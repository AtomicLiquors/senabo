using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WeeklyReportInfo : MonoBehaviour
{
    public Text weeklyReportDateText;
    public Text weeklyReportTimeDynamicText;

    public Text affectionIndexDiffText;
    public Text stressIndexDiffText;

    public Text affectionIndexText;
    public Text stressIndexText;

    public GameObject CommunicationGage;
    public GameObject FeedGage;
    public GameObject WalkGage;
    public GameObject PoopGage;
    public GameObject HealthGage;

    private void Awake()
    {
        CommunicationGage.SetActive(false);
        FeedGage.SetActive(false);
        WalkGage.SetActive(false);
        PoopGage.SetActive(false);
        HealthGage.SetActive(false);

        StartCoroutine(GetDetailReport());
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

    IEnumerator GetDetailReport()
    {
        Debug.Log(CountReport.selectedReportWeek);

        string api_url = $"{ServerSettings.SERVER_URL}/api/report/list/{CountReport.selectedReportWeek}?email={PlayerPrefs.GetString("email")}";

        UnityWebRequest response = UnityWebRequest.Get(api_url);

        string accessToken = "tokentoken"; // 추후 PlayerPrefs에서 추출할 예정
        string jwtToken = $"Bearer {accessToken}";

        response.SetRequestHeader("Authorization", jwtToken);

        yield return response.SendWebRequest();

        if (response.error == null)
        {
            Debug.Log(response.downloadHandler.text);

            DetailReportClass report = JsonUtility.FromJson<APIResponse<DetailReportClass>>(response.downloadHandler.text).data;

            Debug.Log(report.createTime);

            weeklyReportDateText.text = $"{DateTime.Parse(report.createTime):yyyy.MM.dd} - {DateTime.Parse(report.createTime).AddDays(6):yyyy.MM.dd}";

            weeklyReportTimeDynamicText.text = $"{report.dogName}{GetVerb(report.dogName)} {report.totalTime}시간";

            int affectionIndexDiff = report.endAffectionScore - report.startAffectionScore;
            affectionIndexDiffText.text = $"{(affectionIndexDiff < 0 ? "-" : "+")}{Math.Abs(affectionIndexDiff)}";
            if (affectionIndexDiff < 0)
            {
                affectionIndexDiffText.color = Color.red;
            }
            else
            {
                affectionIndexDiffText.color = Color.blue;
            }

            int stressIndexDiff = report.endStressScore - report.startStressScore;
            stressIndexDiffText.text = $"{(stressIndexDiff < 0 ? "-" : "+")}{Math.Abs(stressIndexDiff)}";
            if (stressIndexDiff < 0)
            {
                stressIndexDiffText.color = Color.red;
            }
            else
            {
                stressIndexDiffText.color = Color.blue;
            }

            affectionIndexText.text = report.endAffectionScore.ToString();
            stressIndexText.text = report.endStressScore.ToString();

            CommunicationGage.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 50);
            CommunicationGage.SetActive(true);
            FeedGage.GetComponent<Image>().rectTransform.sizeDelta = new Vector2((float)6.3 * report.feedScore, 50);
            FeedGage.SetActive(true);
            WalkGage.GetComponent<Image>().rectTransform.sizeDelta = new Vector2((float)6.3 * report.walkScore, 50);
            WalkGage.SetActive(true);
            PoopGage.GetComponent<Image>().rectTransform.sizeDelta = new Vector2((float)6.3 * report.poopScore, 50);
            PoopGage.SetActive(true);
            HealthGage.GetComponent<Image>().rectTransform.sizeDelta = new Vector2((float)6.3 * report.diseaseScore, 50);
            HealthGage.SetActive(true);
        }
        else
        {
            Debug.Log("주간 상세 리포트 불러오기 실패");
        }
    }
}