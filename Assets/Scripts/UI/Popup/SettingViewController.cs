using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SettingViewController : MonoBehaviour {

    public GameObject buttons_BGM;
    public GameObject buttons_SFX;
    public GameObject buttons_Con_Button;
    public GameObject buttons_Con_Touch;

    bool[] settingVal;

    private void Start()
    {
        buttons_BGM = transform.GetChild(2).GetChild(0).gameObject;
        buttons_SFX = transform.GetChild(2).GetChild(1).gameObject;
        buttons_Con_Button = transform.GetChild(2).GetChild(2).gameObject;
        buttons_Con_Touch = transform.GetChild(2).GetChild(3).gameObject;
    }

    public void SetView()
    {
        settingVal = UserManager.Instance.currentUser.settingVal;
        
        buttons_BGM.transform.GetChild(0).GetComponent<Text>().text = settingVal[0] == true ? "ON" : "OFF";
        buttons_SFX.transform.GetChild(0).GetComponent<Text>().text = settingVal[1] == true ? "ON" : "OFF";
        buttons_Con_Button.transform.GetChild(0).GetComponent<Image>().color = settingVal[2] == true ? Color.white : Color.gray;
        buttons_Con_Touch.transform.GetChild(0).GetComponent<Image>().color = settingVal[2] == true ? Color.white : Color.gray;
    }

    public void BGMOnf()
    {
        settingVal[0] = settingVal[0] == true ? false : true;
        //if (settingVal[0] == true)
        //{
        //    settingVal[0] = false;
        //}
        //else
        //{
        //    settingVal[0] = true;
        //}
        ReSetView();
    }

    public void SFXOnf()
    {
        settingVal[1] = settingVal[1] == true ? false : true;
        ReSetView();
    }

    public void ChangePuzzleController(int conNum)
    {
        settingVal[2] = conNum == 0 ? true : false;
        ReSetView();
    }

    void ReSetView()
    {
        UserManager.Instance.currentUser.settingVal = settingVal;
        SetView();
    }
}
