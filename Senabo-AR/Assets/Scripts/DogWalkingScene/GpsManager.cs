using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GpsManager : MonoBehaviour
{
    // ����, �浵(�̵� ��, �̵� ��)
    private double pastLat, pastLon, curLat, curLon;
    public Text positionText;

    // ������� �̵� �Ÿ�
    public Text userMovementDistanceText;
    private double userMovementDistance; 

    // ������� ����(�����ֱ�, �ȱ�, �޸���)
    public Text userStateText;
    private string userState;

    // ������Ʈ �ð�
    private double timeCounter;

    Animator welshAnim;


    IEnumerator Start()
    {
        positionText.text = "���� �ǳ�?";
        if (welshAnim == null)
            welshAnim = GameObject.Find("WelshCorgi(Clone)").GetComponentInChildren<Animator>();

            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
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

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            //Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

            // �� �ʱ�ȭ
            userMovementDistance = 0;
            userState = "��� ��";
            pastLat = Input.location.lastData.latitude;
            pastLon = Input.location.lastData.longitude;
      
            while (true)
            {
                // while ������ yeild return �ݵ�� �����!
                yield return null;
                positionText.text = "���� ��ǥ: " + pastLat + " / " + pastLon;
                userMovementDistanceText.text = "�̵� �Ÿ�: " + userMovementDistance + "m";
                userStateText.text = "����: " + userState;
            }
        }

        // Stops the location service if there is no need to query location updates continuously.
        // Input.location.Stop();
    }


    void Update()
    {
        timeCounter += Time.deltaTime;
        // 1�� ������ ����
        if (timeCounter > 1.0)
        {
            if (Input.location.status == LocationServiceStatus.Running)
            {
                curLat = Input.location.lastData.latitude;
                curLon = Input.location.lastData.longitude;

                // �̵� ��, �� ��ǥ�� �Ÿ� ���
                double dist = distance(pastLat, pastLon, curLat, curLon);

                // ���� Ƣ�� ��� ����, ����� 1�ʿ� �̵� ������ �Ÿ� �� ���
                if (dist < 10)
                {
                    userMovementDistance += dist;
                    pastLat = curLat;
                    pastLon = curLon;

                    if (dist < 0.5)
                    {
                        userState = "���� ����";
                        welshAnim.SetTrigger("WelshIdle");
                    }
                    else if (dist < 2.2)
                    {
                        userState = "�ȴ� ��";
                        welshAnim.SetTrigger("WelshWalk");
                    }
                    else
                    {
                        userState = "�ٴ� ��"; 
                        welshAnim.SetTrigger("WelshRun");
                    }
                }
            }
            timeCounter = 0;
        }
    }


    // �Ϲ����� ����(��ǥ�� �Ÿ���� ����)
    // ���� ��ġ�� ����, �浵 -> �������� ���� �浵
    private double distance(double lat1, double lon1, double lat2, double lon2)
    {
        double theta = lon1 - lon2;
        double dist = Math.Sin(Deg2Rad(lat1)) * Math.Sin(Deg2Rad(lat2))
        + Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2))
        * Math.Cos(Deg2Rad(theta));

        dist = Math.Acos(dist);
        dist = Rad2Deg(dist);
        dist = dist * 60 * 1.1515;
        dist = dist * 1609.344; // ���� ��ȯ

        return dist;
    }

    private double Deg2Rad(double deg)
    {
        return (deg * Math.PI / 180.0f);
    }

    private double Rad2Deg(double rad)
    {
        return (rad * 180.0f / Mathf.PI);
    }

}
