using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WeeklyReportInfo : MonoBehaviour
{
    private string email = "kim@ssafy.com";

    public Text weeklyReportDateText;
    public Text weeklyReportTimeDynamicText;

    public Text affectionIndexDiffText;
    public Text stressIndexDiffText;

    public Text affectionIndexText;
    public Text stressIndexText;

    private void Awake()
    {
        StartCoroutine(GetDetailReport());
    }

    string GetVerb(string dogName)
    {
        char lastLetter = dogName.ElementAt(dogName.Length - 1);

        // �ѱ��� ���� ó���� ���� ���� ���� ��� 
        if (lastLetter < 0xAC00 || lastLetter > 0xD7A3)
        {
            return "��";
        }

        return (lastLetter - 0xAC00) % 28 > 0 ? "�̿�" : "��";
    }

    IEnumerator GetDetailReport()
    {
        Debug.Log(CountReport.selectedReportWeek);

        string api_url = $"{ServerSettings.SERVER_URL}/api/report/list/{CountReport.selectedReportWeek}?email={email}";

        UnityWebRequest response = UnityWebRequest.Get(api_url);

        string accessToken = "tokentoken"; // ���� PlayerPrefs���� ������ ����
        string jwtToken = $"Bearer {accessToken}";

        response.SetRequestHeader("Authorization", jwtToken);

        yield return response.SendWebRequest();

        if (response.error == null)
        {
            Debug.Log(response.downloadHandler.text);

            DetailReportClass report = JsonUtility.FromJson<APIResponse<DetailReportClass>>(response.downloadHandler.text).data;

            Debug.Log(report.createTime);

            weeklyReportDateText.text = $"{DateTime.Parse(report.createTime):yyyy.MM.dd} - {DateTime.Parse(report.createTime).AddDays(6):yyyy.MM.dd}";

            weeklyReportTimeDynamicText.text = $"{report.dogName}{GetVerb(report.dogName)} {report.totalTime}�ð�";

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
        }
        else
        {
            Debug.Log("�ְ� �� ����Ʈ �ҷ����� ����");
        }
    }
}