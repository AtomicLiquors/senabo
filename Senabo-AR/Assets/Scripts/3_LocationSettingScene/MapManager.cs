using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public RawImage mapRawImage;

    public string baseURL = "https://maps.googleapis.com/maps/api/staticmap?";
    public int zoom = 20;
   
    public int mapWidth = 400;
    public int mapHeight = 400;
    public string APIKey = "";

    public GameObject UserLocation;

    private double latitude, longitude;

    public void UpdateLocation(double latitude, double longitude)
    {
        this.latitude = latitude;
        this.longitude = longitude;

        Debug.Log("위도:" + latitude + "\n경도:" + longitude);

        UserLocation.SetActive(false);
        StartCoroutine(LoadMap());
    }

    private void Start()
    {
        mapRawImage = GetComponent<RawImage>();

        UserLocation.SetActive(true);
    }

    IEnumerator LoadMap()
    {
        string url = baseURL 
            + "center=" + latitude + "," + longitude 
            + "&zoom=" + zoom.ToString()
            + "&size=" + mapWidth.ToString() + "x" + mapHeight.ToString()
            + "&markers=color:blue%7Clabel:S%7C" + latitude + "," + longitude
            + "&key=" + APIKey;

        Debug.Log("URL=" + url);

        url = UnityWebRequest.UnEscapeURL(url);
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        mapRawImage.texture = DownloadHandlerTexture.GetContent(request);
    }

    public void SetHouseLocation()
    {
        FirebaseAuthManager.signUpRequestDto.houseLatitude = latitude;
        FirebaseAuthManager.signUpRequestDto.houseLongitude = longitude;

        SceneManager.LoadScene("ProfileSettingScene");
    }
}
