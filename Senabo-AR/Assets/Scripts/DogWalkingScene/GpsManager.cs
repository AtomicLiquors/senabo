using System;
using System.Collections;
using UnityEngine;

using UnityEngine.Android;
using UnityEngine.UI;

public class GpsManager : MonoBehaviour
{
    // 위도, 경도(이동 전, 이동 후)
    private double pastLat, pastLon, curLat, curLon;

    // 사용자의 이동 거리
    public Text userMovementDistanceText;
    private double userMovementDistance;

    IEnumerator Start()
    {

        // 스마트폰에서 Location 정보를 Permission을 요청하는 코드
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
            // 값 초기화
            userMovementDistance = 0;
            pastLat = Input.location.lastData.latitude;
            pastLon = Input.location.lastData.longitude;

            while (true)
            {
                // 3초 대기
                yield return new WaitForSeconds(3);
                Debug.Log("Gps 반복 중");
                // GPS 값을 가져오는 함수 호출
                ContinuousGPSUpdates();
            }
        }
    }


    // 게임 오브젝트가 사라질때 실행
    private void OnDestroy()
    {
        StopAllCoroutines();
        // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();
    }

    // 1초마다 GPS 데이터를 받아서 사용자 이동 거리, 속도 반환
    void ContinuousGPSUpdates()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            curLat = Input.location.lastData.latitude;
            curLon = Input.location.lastData.longitude;

            // 이동 전, 후 좌표로 거리 계산
            double dist = distance(pastLat, pastLon, curLat, curLon);
            if (dist < 0) return;

            // 값이 튀는 경우 방지, 사람이 1초에 이동 가능한 거리 일 경우
            if (dist < 10)
            {
                userMovementDistance += dist;
                pastLat = curLat;
                pastLon = curLon;
            }
        }
        userMovementDistanceText.text = String.Join("", userMovementDistance.ToString("F2"), "km");
    }


    // 하버싸인 공식(지표면 거리계산 공식)
    // 현재 위치의 위도, 경도 -> 목적지의 위도 경도
    private double distance(double lat1, double lon1, double lat2, double lon2)
    {
        try
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
        catch (Exception e)
        {
            // 분모가 0일 경우 방지
            Debug.Log("GPS 예외 발생");
            return -1; // 에러가 발생한 경우 -1 또는 다른 적절한 값 반환
        }

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
