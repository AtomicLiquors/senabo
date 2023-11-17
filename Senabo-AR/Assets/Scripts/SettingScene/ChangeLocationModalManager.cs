using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLocationModalManager : MonoBehaviour
{
    public GameObject ChangeLocationModalPanel;

    void Start() {
        ChangeLocationModalPanel.SetActive(false);
    }

    public void ShowChangeLocationModalPanel() {
        ChangeLocationModalPanel.SetActive(true);
    }

    public void CloseChangeLocationModalPanel() {
        ChangeLocationModalPanel.SetActive(false);
    }
}
