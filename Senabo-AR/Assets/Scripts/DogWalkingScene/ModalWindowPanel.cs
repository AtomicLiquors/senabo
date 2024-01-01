using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalWindowPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Action onConfirmAction;
    private Action onCancelAction;
    private Action onAlternateAction;

    public void Confirm()
    {
        onConfirmAction?.Invoke();
        //Close();
    }
    public void Cancel()
    {
        onCancelAction?.Invoke();
        //Close();
    }

    public void Alternate()
    {
        onAlternateAction?.Invoke();
    }
         
}
