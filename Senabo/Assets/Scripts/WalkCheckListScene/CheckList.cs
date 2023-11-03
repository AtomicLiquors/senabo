using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckList : MonoBehaviour
{
    public GameObject uncheckedBox1;
    public GameObject uncheckedBox2;
    public GameObject uncheckedBox3;
    public GameObject uncheckedBox4;
    public GameObject uncheckedBox5;

    public GameObject checkedBox1;
    public GameObject checkedBox2;
    public GameObject checkedBox3;
    public GameObject checkedBox4;
    public GameObject checkedBox5;

    public bool isChecked1 = false;
    public bool isChecked2 = false;
    public bool isChecked3 = false;
    public bool isChecked4 = false;
    public bool isChecked5 = false;
    

    // Start is called before the first frame update
    void Start()
    {
        uncheckedBox1.SetActive(true);
        uncheckedBox2.SetActive(true);
        uncheckedBox3.SetActive(true);
        uncheckedBox4.SetActive(true);
        uncheckedBox5.SetActive(true);

        checkedBox1.SetActive(isChecked1);
        checkedBox2.SetActive(isChecked2);
        checkedBox3.SetActive(isChecked3);
        checkedBox4.SetActive(isChecked4);
        checkedBox5.SetActive(isChecked5);
    }


}
