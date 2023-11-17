using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignUpManager : MonoBehaviour
{
    static public SignUpRequestDtoClass signUpRequestDto;

    string email = "oooooooo@ssafy.com";
    string uid = "duuuid";
    string deviceToken = "duuDeviceToken";

    public void StartSignUp()
    {
        signUpRequestDto = new SignUpRequestDtoClass();
        signUpRequestDto.email = email;
        signUpRequestDto.uid = uid;
        signUpRequestDto.deviceToken = deviceToken;
        signUpRequestDto.species = "CORGI";
        signUpRequestDto.sex = "F";

        SceneManager.LoadScene("LocationSettingScene");
    }

    public void OnLoginButtonClick()
    {
        SceneManager.LoadScene("LogInScene");
    }
}
