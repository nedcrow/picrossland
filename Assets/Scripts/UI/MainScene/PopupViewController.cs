using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupViewController : MonoBehaviour {
    /// <summary>
    ///  0: clearPopup, 1: loadingPopup, 2: nickName
    /// </summary>
    [HideInInspector] public GameObject clearPop;
    [HideInInspector] public GameObject loadingPop;
    [HideInInspector] public GameObject nickNamePop;


    void Awake () {
        clearPop = transform.GetChild(0).gameObject;
        loadingPop = transform.GetChild(1).gameObject;
        nickNamePop = transform.GetChild(2).gameObject;

        loadingPop.SetActive(true);
    }	

    public void ClosePopup(int popupNum)
    {
        transform.GetChild(popupNum).gameObject.SetActive(false); //used Onclick() in clearPopup.
        
    }

}
