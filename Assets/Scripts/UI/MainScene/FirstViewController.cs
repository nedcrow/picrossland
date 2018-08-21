using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstViewController : MonoBehaviour {

    private void Start()
    {
        EventManager.instance.NickNameCheckedEvent += (NickNameCheck);

        SetFirsttVuewButton();
        MainDataBase.instance.OnLoadAdmin();        
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

    void NickNameCheck(bool success)
    {
        Debug.Log("success :"+ success);
        if (success == false || MainDataBase.instance.local == true)
        {
            LandManager.instance.views.popupView.GetComponent<PopupViewController>().nickNamePop.SetActive(true);
        }
        else
        {
            LandManager.instance.views.popupView.GetComponent<PopupViewController>().nickNamePop.SetActive(false);
        }
    }//NicknameCheck 성공 시 Popup 안 띄움.

}
