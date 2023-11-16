using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationSettingManager : MonoBehaviour
{
    public double latitude = 35.33;
    public double longitude = 35.33;

    public void SetHouseLocation()
    {
        SignUpManager.signUpRequestDto.houseLatitude = latitude;
        SignUpManager.signUpRequestDto.houseLongitude = longitude;

        SceneManager.LoadScene("SignUpProfileScene");
    }
}
