using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SettingViewController : MonoBehaviour {

    [HideInInspector] public GameObject buttons_BGM;
    [HideInInspector] public GameObject buttons_SFX;
    [HideInInspector] public GameObject buttons_Con_Button;
    [HideInInspector] public GameObject buttons_Con_Touch;

    bool[] settingVal;

    private void Start()
    {
        signGameOBjs();
    }

    public void SetView()
    {
        settingVal = UserManager.Instance.currentUser.settingVal;
        
        if(buttons_BGM == null)
        {
            signGameOBjs();
        }
        buttons_BGM.transform.GetChild(0).GetComponent<Text>().text = settingVal[0] == true ? "ON" : "OFF";
        buttons_SFX.transform.GetChild(0).GetComponent<Text>().text = settingVal[1] == true ? "ON" : "OFF";
        buttons_Con_Button.GetComponent<Image>().color = settingVal[2] == true ? Color.white : Color.gray;
        buttons_Con_Touch.GetComponent<Image>().color = settingVal[2] == true ? Color.gray : Color.white;
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

    void signGameOBjs()
    {
        buttons_BGM = transform.GetChild(2).GetChild(0).gameObject;
        buttons_SFX = transform.GetChild(2).GetChild(1).gameObject;
        buttons_Con_Button = transform.GetChild(2).GetChild(2).gameObject;
        buttons_Con_Touch = transform.GetChild(2).GetChild(3).gameObject;
    }

    void ReSetView()
    {
        UserManager.Instance.currentUser.settingVal = settingVal;
        MainDataBase.instance.SaveSetting();
        SetView();
    }
}
