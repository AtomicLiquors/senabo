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

        string accessToken = "tokentoken"; // ���� PlayerPrefs���� ������ ����
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

                    // Prefab ����
                    GameObject reportElement = Instantiate(prefab);

                    reportElement.name = $"{report.week}";

                    // ������ Prefab�� GridLayoutGroup�� ����
                    reportElement.transform.SetParent(reportListGroup.transform);

                    // ������ Prefab�� local scale ����
                    reportElement.transform.localScale = scale;

                    // AddListener�� ��ư�� moveWeeklyReportScene �Լ� ����
                    reportElement.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(
                        () => MoveToWeeklyReportScene(report.week));

                    if (report.week == 9)
                    {
                        reportElement.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text = $"����";
                    }
                    else
                    {
                        reportElement.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text = $"{report.week}����";
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
