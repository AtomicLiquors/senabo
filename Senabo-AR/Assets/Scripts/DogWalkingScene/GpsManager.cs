using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GpsManager : MonoBehaviour
{
    // 위도, 경도(이동 전, 이동 후)
    private double pastLat, pastLon, curLat, curLon;
    public Text positionText;

    // 사용자의 이동 거리
    public Text userMovementDistanceText;
    private double userMovementDistance; 

    // 사용자의 상태(멈춰있기, 걷기, 달리기)
    public Text userStateText;
    private string userState;

    // 업데이트 시간
    private double timeCounter;

    Animator welshAnim;


    IEnumerator Start()
    {
        positionText.text = "실행 되나?";
        if (welshAnim == null)
            welshAnim = GameObject.Find("WelshCorgi(Clone)").GetComponentInChildren<Animator>();

            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }

        // Starts the location service.
        Input.location.Start(1, 1); // 정확도, 업데이트 거리

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

            // 값 초기화
            userMovementDistance = 0;
            userState = "출발 전";
            pastLat = Input.location.lastData.latitude;
            pastLon = Input.location.lastData.longitude;
      
            while (true)
            {
                // while 돌릴때 yeild return 반드시 써야함!
                yield return null;
                positionText.text = "현재 좌표: " + pastLat + " / " + pastLon;
                userMovementDistanceText.text = "이동 거리: " + userMovementDistance + "m";
                userStateText.text = "상태: " + userState;
            }
        }

        // Stops the location service if there is no need to query location updates continuously.
        // Input.location.Stop();
    }


    void Update()
    {
        timeCounter += Time.deltaTime;
        // 1초 단위로 실행
        if (timeCounter > 1.0)
        {
            if (Input.location.status == LocationServiceStatus.Running)
            {
                curLat = Input.location.lastData.latitude;
                curLon = Input.location.lastData.longitude;

                // 이동 전, 후 좌표로 거리 계산
                double dist = distance(pastLat, pastLon, curLat, curLon);

                // 값이 튀는 경우 방지, 사람이 1초에 이동 가능한 거리 일 경우
                if (dist < 10)
                {
                    userMovementDistance += dist;
                    pastLat = curLat;
                    pastLon = curLon;

                    if (dist < 0.5)
                    {
                        userState = "멈춰 있음";
                        welshAnim.SetTrigger("WelshIdle");
                    }
                    else if (dist < 2.2)
                    {
                        userState = "걷는 중";
                        welshAnim.SetTrigger("WelshWalk");
                    }
                    else
                    {
                        userState = "뛰는 중"; 
                        welshAnim.SetTrigger("WelshRun");
                    }
                }
            }
            timeCounter = 0;
        }
    }


    // 하버싸인 공식(지표면 거리계산 공식)
    // 현재 위치의 위도, 경도 -> 목적지의 위도 경도
    private double distance(double lat1, double lon1, double lat2, double lon2)
    {
        double theta = lon1 - lon2;
        double dist = Math.Sin(Deg2Rad(lat1)) * Math.Sin(Deg2Rad(lat2))
        + Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2))
        * Math.Cos(Deg2Rad(theta));

        dist = Math.Acos(dist);
        dist = Rad2Deg(dist);
        dist = dist * 60 * 1.1515;
        dist = dist * 1609.344; // 미터 변환

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
