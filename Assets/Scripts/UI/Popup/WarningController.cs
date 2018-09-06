using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WarningController : MonoBehaviour {

    Text notice;
    GameObject buttons;

    public void SetWarning()
    {
        notice = transform.GetChild(2).GetComponent<Text>();
        buttons = transform.GetChild(3).gameObject;

        gameObject.SetActive(false);
    }

    public void W_Gem() {
        notice.text = "Gem이 부족합니다.";
        ButtonActive(0);
    }

    public void W_Resource() {
        notice.text = "업데이트 예정입니다.";
        ButtonActive(1);
    }

    void ButtonActive(int btnNum)
    {
        for(int i=0; i< buttons.transform.GetChildCount(); i++)
        {
            buttons.transform.GetChild(i).gameObject.SetActive(false);
        }
        buttons.transform.GetChild(btnNum).gameObject.SetActive(true);
    }
}
