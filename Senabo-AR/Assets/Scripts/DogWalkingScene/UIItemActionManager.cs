using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemSpawner;

public class UIItemActionManager : MonoBehaviour
{
    public GameObject goHomeButton;
    public GameObject itemPanel;
    public GameObject itemSpawner;

    ItemSpawner itemSpawnerScript;

    private bool isItemPanelOpen = false;

    void Start()
    {
        itemSpawnerScript = itemSpawner.GetComponent<ItemSpawner>();
        
    }
    public void SetIsItemPanelOpen(bool state)
    {
        itemPanel.SetActive(state);
        goHomeButton.SetActive(!state);
    }

    public void HandleSnackItemClick()
    {
        Debug.Log("Snack");
        if (true) //이벤트 진행 여부 조건문
        {
            itemSpawnerScript.HandleSpawnAction(ItemType.Snack);
        }
    }

    public void HandlePoopBagItemClick()
    {
        Debug.Log("PoopBag");
    }
}
