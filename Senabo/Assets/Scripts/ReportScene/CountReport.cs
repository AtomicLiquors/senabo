using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CountReport : MonoBehaviour
{
    public int reportCount = 5;
    public int[] stressArr = {30, 10, 40, 50, 0};

    public GameObject prefab;
    public GridLayoutGroup reportListGroup;
    public GameObject reportEmptyGroup;

    public Vector3 scale = new Vector3(1f, 1f, 1f);

    void Start()
    {
        if (reportCount == 0)
        {
            reportListGroup.gameObject.SetActive(false);
            reportEmptyGroup.SetActive(true);
        }
        else
        {
            reportListGroup.gameObject.SetActive(true);
            reportEmptyGroup.SetActive(false);

            for (int i = reportCount - 1; i >= 0; i--)
            {
                // Prefab ����
                GameObject reportElement = Instantiate(prefab);

                reportElement.name = $"{i + 1}"; 

                // ������ Prefab�� GridLayoutGroup�� ����
                reportElement.transform.SetParent(reportListGroup.transform);

                // ������ Prefab�� local scale ����
                reportElement.transform.localScale = scale;

                // AddListener�� ��ư�� moveWeeklyReportScene �Լ� ����
                reportElement.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => moveWeeklyReportScene(reportElement.name));

                if (i == 8)
                {
                    reportElement.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text = $"����";
                }
                else
                {
                    reportElement.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text = $"{i + 1}����";
                }

                if (stressArr[i] > 20)
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

    void moveWeeklyReportScene(string name) 
    {
        Debug.Log($"{name}");
    }
}
