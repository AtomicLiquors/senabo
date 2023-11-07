using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckList : MonoBehaviour
{
    private string email = "kim@ssafy.com";

    public GameObject uncheckedBox1;
    public GameObject uncheckedBox2;
    public GameObject uncheckedBox3;
    public GameObject uncheckedBox4;
    public GameObject uncheckedBox5;

    public GameObject checkedBox1;
    public GameObject checkedBox2;
    public GameObject checkedBox3;
    public GameObject checkedBox4;
    public GameObject checkedBox5;

    public GameObject walkCheckListAlertModal;

    public bool isChecked1 = false;
    public bool isChecked2 = false;
    public bool isChecked3 = false;
    public bool isChecked4 = false;
    public bool isChecked5 = false;
    
    // Start is called before the first frame update
    void Start()
    {
        walkCheckListAlertModal.SetActive(false);

        uncheckedBox1.SetActive(!isChecked1);
        uncheckedBox2.SetActive(!isChecked2);
        uncheckedBox3.SetActive(!isChecked3);
        uncheckedBox4.SetActive(!isChecked4);
        uncheckedBox5.SetActive(!isChecked5);

        checkedBox1.SetActive(isChecked1);
        checkedBox2.SetActive(isChecked2);
        checkedBox3.SetActive(isChecked3);
        checkedBox4.SetActive(isChecked4);
        checkedBox5.SetActive(isChecked5);
    }

    private void Update()
    {
        uncheckedBox1.SetActive(!isChecked1);
        uncheckedBox2.SetActive(!isChecked2);
        uncheckedBox3.SetActive(!isChecked3);
        uncheckedBox4.SetActive(!isChecked4);
        uncheckedBox5.SetActive(!isChecked5);

        checkedBox1.SetActive(isChecked1);
        checkedBox2.SetActive(isChecked2);
        checkedBox3.SetActive(isChecked3);
        checkedBox4.SetActive(isChecked4);
        checkedBox5.SetActive(isChecked5);
    }

    public void ChangeIsChecked1()
    {
        isChecked1 = !isChecked1;
    }

    public void ChangeIsChecked2()
    {
        isChecked2 = !isChecked2;
    }

    public void ChangeIsChecked3()
    {
        isChecked3 = !isChecked3;
    }

    public void ChangeIsChecked4()
    {
        isChecked4 = !isChecked4;
    }

    public void ChangeIsChecked5()
    {
        isChecked5 = !isChecked5;
    }

    void ShowAlertModal()
    {
        walkCheckListAlertModal.SetActive(true);
    }

    void CloseAlertModal()
    {
        walkCheckListAlertModal.SetActive(false);
    }

    IEnumerator StartWalk()
    {
        string api_url = $"{ServerSettings.SERVER_URL}/api/walk/start?email={email}";

        WWWForm form = new WWWForm();

        UnityWebRequest response = UnityWebRequest.Post(api_url, form);

        string accessToken = "tokentoken"; // 추후 PlayerPrefs에서 추출할 예정
        string jwtToken = $"Bearer {accessToken}";

        response.SetRequestHeader("Authorization", jwtToken);

        yield return response.SendWebRequest();

        if (response.error == null)
        {
            Debug.Log(response.downloadHandler.text);
            //SceneManager.LoadScene("WalkScene");
        }
        else
        {
            Debug.Log("산책 시작하기 실패");
        }
    }

    public void Check()
    {
        if (isChecked1 && isChecked2 && isChecked3 && isChecked4 && isChecked5)
        {
            StartCoroutine(StartWalk());
        } else
        {
            ShowAlertModal();
            Invoke("CloseAlertModal", 2.0f);
        }
    }
}
