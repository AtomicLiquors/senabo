using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float cameraHeight = Camera.main.orthographicSize;

        // �̹����� Transform component ��������
        Transform transform = GetComponent<Transform>();

        // �̹��� ���̸� ī�޶� ���̿� �°� �����ϱ�
        transform.localScale = new Vector2(1, cameraHeight / transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
