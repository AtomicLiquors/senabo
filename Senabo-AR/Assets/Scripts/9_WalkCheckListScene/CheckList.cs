using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckList : MonoBehaviour
{
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

    public GameObject WalkCheckListTipModalPanel;

    // Start is called before the first frame update
    void Start()
    {
        walkCheckListAlertModal.SetActive(false);
        WalkCheckListTipModalPanel.SetActive(false);

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

    public void ShowTipModal()
    {
        initTipModal();
        WalkCheckListTipModalPanel.SetActive(true);
    }

    public void CloseTipModal()
    {
        WalkCheckListTipModalPanel.SetActive(false);
    }

    public void ChangeIsChecked1()
    {
        isChecked1 = !isChecked1;
        uncheckedBox1.SetActive(!isChecked1);
        checkedBox1.SetActive(isChecked1);
    }

    public void ChangeIsChecked2()
    {
        isChecked2 = !isChecked2;
        uncheckedBox2.SetActive(!isChecked2);
        checkedBox2.SetActive(isChecked2);
    }

    public void ChangeIsChecked3()
    {
        isChecked3 = !isChecked3;
        uncheckedBox3.SetActive(!isChecked3);
        checkedBox3.SetActive(isChecked3);
    }

    public void ChangeIsChecked4()
    {
        isChecked4 = !isChecked4;
        uncheckedBox4.SetActive(!isChecked4);
        checkedBox4.SetActive(isChecked4);
    }

    public void ChangeIsChecked5()
    {
        isChecked5 = !isChecked5;
        uncheckedBox5.SetActive(!isChecked5);
        checkedBox5.SetActive(isChecked5);
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
        string api_url = $"{ServerSettings.SERVER_URL}/api/walk/start";

        WWWForm form = new WWWForm();

        UnityWebRequest response = UnityWebRequest.Post(api_url, form);

        string accessToken = PlayerPrefs.GetString("accessToken");
        string jwtToken = $"Bearer {accessToken}";

        response.SetRequestHeader("Authorization", jwtToken);

        yield return response.SendWebRequest();

        if (response.error == null)
        {
            Debug.Log(response.downloadHandler.text);
            SceneManager.LoadScene("DogWalkingScene");
        }
        else
        {
            Debug.Log("산책 시작하기 실패");

            if (response.responseCode == 403)
            {
                RefreshTokenManager.Instance.ReIssueRefreshToken();

                StartCoroutine(StartWalk());
            }
            else
            {
                SceneManager.LoadScene("MainScene");
            }
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

    public GameObject WalkTip1ArrowIconRight;
    public GameObject WalkTip1ArrowIconDown;
    public GameObject WalkTip1BodyGroup;

    public GameObject WalkTip2ArrowIconRight;
    public GameObject WalkTip2ArrowIconDown;
    public GameObject WalkTip2BodyGroup;

    public GameObject WalkTip3ArrowIconRight;
    public GameObject WalkTip3ArrowIconDown;
    public GameObject WalkTip3BodyGroup;

    public GameObject WalkTip4ArrowIconRight;
    public GameObject WalkTip4ArrowIconDown;
    public GameObject WalkTip4BodyGroup;

    public GameObject WalkTip5ArrowIconRight;
    public GameObject WalkTip5ArrowIconDown;
    public GameObject WalkTip5BodyGroup;

    private bool walkTip1Flag;
    private bool walkTip2Flag;
    private bool walkTip3Flag;
    private bool walkTip4Flag;
    private bool walkTip5Flag;

    private void initTipModal()
    {
        walkTip1Flag = false;
        WalkTip1ArrowIconRight.SetActive(true);
        WalkTip1ArrowIconDown.SetActive(false);
        WalkTip1BodyGroup.SetActive(false);

        walkTip2Flag = false;
        WalkTip2ArrowIconRight.SetActive(true);
        WalkTip2ArrowIconDown.SetActive(false);
        WalkTip2BodyGroup.SetActive(false);

        walkTip3Flag = false;
        WalkTip3ArrowIconRight.SetActive(true);
        WalkTip3ArrowIconDown.SetActive(false);
        WalkTip3BodyGroup.SetActive(false);

        walkTip4Flag = false;
        WalkTip4ArrowIconRight.SetActive(true);
        WalkTip4ArrowIconDown.SetActive(false);
        WalkTip4BodyGroup.SetActive(false);

        walkTip5Flag = false;
        WalkTip5ArrowIconRight.SetActive(true);
        WalkTip5ArrowIconDown.SetActive(false);
        WalkTip5BodyGroup.SetActive(false);
    }

    public void OnClickWalkTip1()
    {
        walkTip1Flag = !walkTip1Flag;
        WalkTip1ArrowIconRight.SetActive(!walkTip1Flag);
        WalkTip1ArrowIconDown.SetActive(walkTip1Flag);
        WalkTip1BodyGroup.SetActive(walkTip1Flag);
    }

    public void OnClickWalkTip2()
    {
        walkTip2Flag = !walkTip2Flag;
        WalkTip2ArrowIconRight.SetActive(!walkTip2Flag);
        WalkTip2ArrowIconDown.SetActive(walkTip2Flag);
        WalkTip2BodyGroup.SetActive(walkTip2Flag);
    }

    public void OnClickWalkTip3()
    {
        walkTip3Flag = !walkTip3Flag;
        WalkTip3ArrowIconRight.SetActive(!walkTip3Flag);
        WalkTip3ArrowIconDown.SetActive(walkTip3Flag);
        WalkTip3BodyGroup.SetActive(walkTip3Flag);
    }

    public void OnClickWalkTip4()
    {
        walkTip4Flag = !walkTip4Flag;
        WalkTip4ArrowIconRight.SetActive(!walkTip4Flag);
        WalkTip4ArrowIconDown.SetActive(walkTip4Flag);
        WalkTip4BodyGroup.SetActive(walkTip4Flag);
    }

    public void OnClickWalkTip5()
    {
        walkTip5Flag = !walkTip5Flag;
        WalkTip5ArrowIconRight.SetActive(!walkTip5Flag);
        WalkTip5ArrowIconDown.SetActive(walkTip5Flag);
        WalkTip5BodyGroup.SetActive(walkTip5Flag);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("WalkCheckListScene - 종료!!!!!\n입장시간:" + PlayerPrefs.GetString("enterTime"));

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
            Debug.Log("WalkCheckListScene - OnApplicationPause");
            PlayerPrefs.SetString("enterTime", DateTime.Now.ToString("yyyy.MM.dd.HH:mm"));
            Debug.Log("새로운 입장 시간:" + PlayerPrefs.GetString("enterTime"));
        }
    }
}
