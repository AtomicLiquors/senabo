using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float cameraHeight = Camera.main.orthographicSize;

        // 이미지의 Transform component 가져오기
        Transform transform = GetComponent<Transform>();

        // 이미지 높이를 카메라 높이에 맞게 설정하기
        transform.localScale = new Vector2(1, cameraHeight / transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
