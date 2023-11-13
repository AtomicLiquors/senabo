using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CountReport : MonoBehaviour
{
    static public long selectedReportWeek;

    public GameObject prefab;
    public GridLayoutGroup reportListGroup;
    public GameObject reportEmptyGroup;

    public Vector3 scale = new Vector3(1f, 1f, 1f);

    private void Awake()
    {
        StartCoroutine(GetTotalReport());
    }

    private void MoveToWeeklyReportScene(long reportWeek)
    {
        selectedReportWeek = reportWeek;
        SceneManager.LoadScene("WeeklyReportScene");
    }

    IEnumerator GetTotalReport()
    {
        string api_url = $"{ServerSettings.SERVER_URL}/api/report/list";

        UnityWebRequest response = UnityWebRequest.Get(api_url);

        string accessToken = PlayerPrefs.GetString("accessToken");
        string jwtToken = $"Bearer {accessToken}";

        response.SetRequestHeader("Authorization", jwtToken);

        yield return response.SendWebRequest();

        if (response.error == null)
        {
            Debug.Log(response.downloadHandler.text);

            List<SimpleReportClass> reports = JsonUtility.FromJson<APIResponse<List<SimpleReportClass>>>(response.downloadHandler.text).data;

            if (reports.Count == 0)
            {
                reportListGroup.gameObject.SetActive(false);
                reportEmptyGroup.SetActive(true);
            }
            else
            {
                reportListGroup.gameObject.SetActive(true);
                reportEmptyGroup.SetActive(false);

                for (int i = reports.Count - 1; i >= 0; i--)
                {
                    SimpleReportClass report = reports[i];

                    // Prefab 복제
                    GameObject reportElement = Instantiate(prefab);

                    reportElement.name = $"{report.week}";

                    // 복제된 Prefab을 GridLayoutGroup에 연결
                    reportElement.transform.SetParent(reportListGroup.transform);

                    // 복제된 Prefab의 local scale 설정
                    reportElement.transform.localScale = scale;

                    // AddListener로 버튼에 moveWeeklyReportScene 함수 연결
                    reportElement.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(
                        () => MoveToWeeklyReportScene(report.week));

                    if (report.week == 9)
                    {
                        reportElement.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text = $"최종";
                    }
                    else
                    {
                        reportElement.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text = $"{report.week}주차";
                    }

                    if (report.endStressScore > 20)
                    {
                        reportElement.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                        reportElement.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        reportElement.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                        reportElement.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            Debug.Log("전체 리포트 불러오기 실패");

            if (response.responseCode == 403)
            {
                RefreshTokenManager.Instance.ReIssueRefreshToken();

                StartCoroutine(GetTotalReport());
            }
            else
            {
                SceneManager.LoadScene("ProfileScene");
            }
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("ReportScene - 종료!!!!!\n입장시간:" + PlayerPrefs.GetString("enterTime"));

            PlayerPrefs.SetString("exitTime", DateTime.Now.ToString("yyyy.MM.dd.HH:mm"));

            Debug.Log("퇴장시간:" + PlayerPrefs.GetString("exitTime"));

            DateTime enterTime = DateTime.Parse(PlayerPrefs.GetString("enterTime"));
            DateTime exitTime = DateTime.Parse(PlayerPrefs.GetString("exitTime"));
            TimeSpan timeDiff = exitTime - enterTime;

            int diffMinute = timeDiff.Days * 24 * 60 + timeDiff.Hours * 60 + timeDiff.Minutes;

            Debug.Log("OnApplicationPause - diffMinute: " + diffMinute);
            RefreshTokenManager.Instance.UpdateTotalTime(diffMinute);
        }
        else
        {
            Debug.Log("ReportScene - OnApplicationPause");
            PlayerPrefs.SetString("enterTime", DateTime.Now.ToString("yyyy.MM.dd.HH:mm"));
            Debug.Log("새로운 입장 시간:" + PlayerPrefs.GetString("enterTime"));
        }
    }
}
