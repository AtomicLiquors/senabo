using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReceiptScene : MonoBehaviour
{
    public int numberOfReceipt; // private

    public GameObject receiptPrefab;
    public Transform receiptContent;

    void Start()
    {
        CreateReceiptPrefabs();
    }

    void CreateReceiptPrefabs()
    {
        for (int i = 0; i < numberOfReceipt; i++)
        {
            Instantiate(receiptPrefab, receiptContent);
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
