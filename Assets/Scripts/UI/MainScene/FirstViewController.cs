using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstViewController : MonoBehaviour {


    private void Start()
    {
        SetFirsttVuewButton();
        NickNameCheck();
    }

    void SetFirsttVuewButton()
    {
        transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
        {
            LandManager.instance.views.GetComponent<ViewController>().SceneOn(2);
            PuzzleManager.instance.StartPuzzle("0101");
            LandManager.instance.views.GetComponent<ViewController>().SceneOff(0);
        });
    }

    void NickNameCheck()
    {
        bool firstTime = LandManager.instance.firstGame;
        Debug.Log("success :"+ firstTime);
        if (firstTime == true && MainDataBase.instance.local == false)
        {
            LandManager.instance.views.popupView.GetComponent<PopupViewController>().nickNamePop.SetActive(true);
        }
        else
        {
            LandManager.instance.views.popupView.GetComponent<PopupViewController>().nickNamePop.SetActive(false);
        }
    }//NicknameCheck 성공 시 Popup 안 띄움.

}
