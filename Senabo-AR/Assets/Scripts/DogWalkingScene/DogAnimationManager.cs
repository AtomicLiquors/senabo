using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimationManager : MonoBehaviour
{
    [SerializeField]
    private GameObject dogObject;
    
    [SerializeField]
    private GameObject strapObject;


    Animator welshAnim;
    Animator strapAnim;
    // Start is called before the first frame update
    void Start()
    {
        if (welshAnim == null)
            welshAnim = dogObject.GetComponentInChildren<Animator>();
        if (strapAnim == null && strapObject.activeInHierarchy)
            strapAnim = strapObject.GetComponentInChildren<Animator>();
    }

    public void handleDogMovement(string motion)
    {
        if (!EventStatusManager.GetDogEvent())
        {
            welshAnim.SetTrigger(motion);
            if (strapObject.activeInHierarchy)
            {
                strapAnim.SetTrigger(motion);
            }
        }
    }

    public void handleDogSuddenEvent(string motion)
    {
        welshAnim.SetTrigger(motion);
        if (strapObject.activeInHierarchy)
        {
            strapAnim.SetTrigger(motion);
        }
    }
}
