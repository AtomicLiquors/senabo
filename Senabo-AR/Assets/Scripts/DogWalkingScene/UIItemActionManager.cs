using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemSpawner;

public class UIItemActionManager : MonoBehaviour
{
    public GameObject goHomeButton;
    public GameObject itemPanel;
    public GameObject itemSpawner;

    [SerializeField]
    GameObject dogAnimationManager;

    DogAnimationManager dogAnimator;

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
        if (!EventStatusManager.GetDogStopResolved()) //이벤트 진행 여부 조건문
        {
            if(dogAnimator == null)
            {
                dogAnimator = dogAnimationManager.GetComponent<DogAnimationManager>();
            }
            itemSpawnerScript.HandleSpawnAction(ItemType.Snack);
            EventStatusManager.SwitchDogStopResolved(true);
            dogAnimator.handleDogSuddenEvent("welshEat");
        }
    }

    public void HandlePoopBagItemClick()
    {
        if (!EventStatusManager.GetDogPoopResolved())
        {
            EventStatusManager.SwitchDogPoopResolved(true);
        }
    }
}
