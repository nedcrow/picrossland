﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupViewController : MonoBehaviour {
    /// <summary>
    ///  0: clearPopup, 1: loadingPopup, 2: nickName, 3:guide, 4. setting, 5. warning
    /// </summary>
    [HideInInspector] public GameObject clearPop;
    [HideInInspector] public GameObject loadingPop;
    [HideInInspector] public GameObject nickNamePop;
    [HideInInspector] public GameObject guidePop;
    [HideInInspector] public GameObject settingPop;
    [HideInInspector] public GameObject shopPop; 
    [HideInInspector] public GameObject warningPop;
    [HideInInspector] public GameObject escapePop;
    [HideInInspector] public GameObject tutorialPop;

    void Awake () {
        clearPop = transform.GetChild(0).gameObject;
        loadingPop = transform.GetChild(1).gameObject;
        nickNamePop = transform.GetChild(2).gameObject;
        guidePop = transform.GetChild(3).gameObject;
        settingPop = transform.GetChild(4).gameObject;
        shopPop = transform.GetChild(5).gameObject;
        warningPop = transform.GetChild(6).gameObject;
        escapePop = transform.GetChild(7).gameObject;
        tutorialPop = transform.GetChild(8).gameObject;

        loadingPop.SetActive(true);
        warningPop.SetActive(true);
        warningPop.GetComponent<WarningController>().SetWarning();
    }	

    public void OpenPopup(int popupNum)
    {
        transform.GetChild(popupNum).gameObject.SetActive(true);
    }

    public void ClosePopup(int popupNum)
    {
        transform.GetChild(popupNum).gameObject.SetActive(false); //used Onclick() in clearPopup.
        
    }

}
