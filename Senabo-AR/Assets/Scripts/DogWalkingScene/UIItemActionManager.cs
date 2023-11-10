using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemActionManager : MonoBehaviour
{
    public GameObject goHomeButton;
    public GameObject itemPanel;

    private bool isItemPanelOpen = false;
    public void SetIsItemPanelOpen(bool state)
    {
        itemPanel.SetActive(state);
        goHomeButton.SetActive(!state);
    }

    public void HandleSnackItemClick()
    {
        Debug.Log("Snack");
    }

    public void HandlePoopBagItemClick()
    {
        Debug.Log("PoopBag");
    }
}
