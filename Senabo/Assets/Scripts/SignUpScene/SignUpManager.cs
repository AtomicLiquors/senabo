using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignUpManager : MonoBehaviour
{
    static public SignUpRequestDtoClass signUpRequestDto;

    string email = "park@ssafy.com";
    string uid = "parkssafyuid";
    string deviceToken = "parkDeviceToken";

    private void Start()
    {
        
    }

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
}
