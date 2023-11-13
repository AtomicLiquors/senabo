using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class LocationSettingSceneUserLocation : MonoBehaviour
{
    private double latitude, longitude;

    [SerializeField]
    private MapManager mapManager;
    
    public double GetLatitude()
    {
        return latitude;
    }

    public double GetLongitude()
    {
        return longitude;
    }

    public void OnEnable()
    {
        StartCoroutine(GetLocation());
    }

    IEnumerator GetLocation()
    {
        Debug.Log("���� �����̼� ���� 1");
        // Location Permission ��û
        while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }

        Debug.Log("���� �����̼� ���� 2");
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location not enabled on device or app does not have permission to access location");
            yield break;
        }

        // Starts the location service.
        Input.location.Start(1, 1); // ��Ȯ��, ������Ʈ �Ÿ�

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Check for service initialization timeout
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // Check for service connection failure
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        else
        {
            // If the connection succeeded, retrieves the device's current location and displays it in the Console window.
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;

            // ó���� �� ������ get�� ȣ���ϵ��� ������ �ڵ� �߰�
            mapManager.UpdateLocation(latitude, longitude);
            
        }

        // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();
    }

    private void OnDisable()
    {
        // �����ϰų� �����ϴ� �۾��� �ʿ��� ��� ���⿡ �߰��� �� �ֽ��ϴ�.
        Input.location.Stop();
    }
}
