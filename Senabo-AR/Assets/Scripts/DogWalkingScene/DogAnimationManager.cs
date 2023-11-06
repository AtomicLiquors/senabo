using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimationManager : MonoBehaviour
{
    [SerializeField]
    private GameObject dogObject;

    Animator welshAnim;
    // Start is called before the first frame update
    void Start()
    {
        if (welshAnim == null)
            welshAnim = dogObject.GetComponentInChildren<Animator>();
    }

    public void handleDogMovement(string motion)
    {
        if(!EventStatusManager.GetDogEvent()) welshAnim.SetTrigger(motion);
    }

    public void handleDogSuddenEvent(string motion)
    {
        welshAnim.SetTrigger(motion);
    }
}
