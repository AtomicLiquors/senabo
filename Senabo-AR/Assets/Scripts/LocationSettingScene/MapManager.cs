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
    public double latitude = 35.0;
    public double longitude = 35.0;
    public int zoom = 20;
    public int mapWidth = 400;
    public int mapHeight = 400;
    public string APIKey = "";


    private void Start()
    {
        mapRawImage = GetComponent<RawImage>();
        StartCoroutine(LoadMap());
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
}
