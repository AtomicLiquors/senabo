using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CountReport : MonoBehaviour
{
    private string email = "kim@ssafy.com";

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
        string api_url = ServerSettings.SERVER_URL + "/api/report/list?email=" + email;

        UnityWebRequest response = UnityWebRequest.Get(api_url);

        string accessToken = "tokentoken"; // 추후 PlayerPrefs에서 추출할 예정
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
    }
}
