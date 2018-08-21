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
        bool gotNickname = MainDataBase.instance.OnLoadAdmin();
        Debug.Log(gotNickname);
        if (gotNickname == false)
        {
            LandManager.instance.views.GetComponent<PopupViewController>().nickNamePop.SetActive(true);
        }
        else
        {
            LandManager.instance.views.popupView.GetComponent<PopupViewController>().nickNamePop.SetActive(false);
        }
    }//Nickname Popup

}
