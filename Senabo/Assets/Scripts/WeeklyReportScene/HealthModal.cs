using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HealthModal : MonoBehaviour
{
    private string email = "kim@ssafy.com";
    private string dogName = "�κκ�";

    public GameObject DayHealthElement;
    public GameObject HealthRecordElemnt;
    public GameObject HealthContent;

    private Vector3 scale = new Vector3(1f, 1f, 1f);

    private void Awake()
    {
        StartCoroutine(GetWeeklyHealthList());
    }

    IEnumerator GetWeeklyHealthList()
    {
        string api_url = $"{ServerSettings.SERVER_URL}/api/disease/list/{CountReport.selectedReportWeek}?email={email}";

        UnityWebRequest response = UnityWebRequest.Get(api_url);

        string accessToken = "tokentoken"; // ���� PlayerPrefs���� ������ ����
        string jwtToken = $"Bearer {accessToken}";

        response.SetRequestHeader("Authorization", jwtToken);

        yield return response.SendWebRequest();

        if (response.error == null)
        {
            Debug.Log(response.downloadHandler.text);

            List<HealthClass> healths = JsonUtility.FromJson<APIResponse<List<HealthClass>>>(response.downloadHandler.text).data;

            List<List<HealthClass>> healthsByCreateTime = new List<List<HealthClass>>();
            DateTime date = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy.MM.dd"));

            int index = -1;
            for (int i = 0; i < healths.Count; i++)
            {
                HealthClass health = healths[i];

                DateTime healthDate = Convert.ToDateTime(DateTime.Parse(health.createTime).ToString("yyyy.MM.dd"));

                if (DateTime.Compare(date, healthDate) != 0)
                {
                    date = healthDate;
                    healthsByCreateTime.Add(new List<HealthClass>());
                    index++;
                }

                healthsByCreateTime[index].Add(health);
            }

            for (int i = 0; i < healthsByCreateTime.Count; i++)
            {
                GameObject dayHealthElement = Instantiate(DayHealthElement);
                dayHealthElement.name = $"{DateTime.Parse(healthsByCreateTime[i][0].createTime):yyyy.MM.dd}";
                dayHealthElement.transform.SetParent(HealthContent.transform);
                dayHealthElement.transform.localScale = scale;

                dayHealthElement.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = $"{DateTime.Parse(healthsByCreateTime[i][0].createTime):yyyy.MM.dd}"; // ��¥ ����

                for (int j = 0; j < healthsByCreateTime[i].Count; j++)
                {
                    HealthClass health = healthsByCreateTime[i][j];

                    GameObject healthRecordElemnt = Instantiate(HealthRecordElemnt);
                    healthRecordElemnt.transform.SetParent(dayHealthElement.transform.GetChild(1).transform);
                    healthRecordElemnt.transform.localScale = scale;

                    healthRecordElemnt.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = $"{DateTime.Parse(health.createTime):HH:mm}";
                    healthRecordElemnt.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = $"{dogName}{GetVerb(dogName)} {health.diseaseName}�� �ɷ� ���;��.";
                }
            }
        }
        else
        {
            Debug.Log("�ְ� �ǰ� ���� �ҷ����� ����");
        }
    }

    string GetVerb(string dogName)
    {
        char lastLetter = dogName.ElementAt(dogName.Length - 1);

        // �ѱ��� ���� ó���� ���� ���� ���� ��� 
        if (lastLetter < 0xAC00 || lastLetter > 0xD7A3)
        {
            return "��";
        }

        return (lastLetter - 0xAC00) % 28 > 0 ? "�̰�" : "��";
    }

}
