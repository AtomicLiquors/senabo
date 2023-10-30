using System;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private int[] randomTimes;
    private double countTimes;

    // Start is called before the first frame update
    void Start()
    {
        countTimes = 0;
        randomTimes = new int[4];

        // 1�к��� 30�б��� ������ �� 4���� randomTimes �迭�� �Է�
        for (int i = 0; i < randomTimes.Length; i++)
        {
            randomTimes[i] = UnityEngine.Random.Range(1, 61); // 1���� 60������ ������ �� ����
        }
    }

    // Update is called once per frame
    void Update()
    {
        countTimes += Time.deltaTime;

        // �ð��� �� ������ ��ȯ
        int currentMinutes = (int)(countTimes / 60);

        // ������ �ð��� ���� �ð��� ��ġ�ϴ��� Ȯ���Ͽ� �̺�Ʈ ����
        for (int i = 0; i < randomTimes.Length; i++)
        {
            if (currentMinutes == randomTimes[i])
            {
                Debug.Log("�̺�Ʈ �߻�! ���� �ð�: " + currentMinutes + "��, ���� �ð�: " + randomTimes[i] + "��");
            }
        }

    }


}
