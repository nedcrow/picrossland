using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginController : MonoBehaviour {

    public GameObject notice_Login;
    public GameObject notice_Hi;
    public GameObject notice_Fail;
    public GameObject Btn_Login;
    public GameObject Btn_Local;
    public GameObject Btn_Start;   

    private void Start()
    {
        TryLoginGP(false);
    }

    public void TryLoginGP(bool clicked)
    {
        LoginManager.instance.OnClickGoogleLogin(clicked);
        StartCoroutine(LoginGPGS(clicked));
    }//log in googlePlay admin. Save on GooglePlay

    public void LoginLocal()
    {
        notice_Hi.SetActive(true);
        notice_Fail.SetActive(false);
        notice_Login.SetActive(false);
        Btn_Login.SetActive(false);
        Btn_Local.SetActive(false);
        Btn_Start.SetActive(true);
        string path = Application.persistentDataPath;
        MainDataBase.instance.local=true;
        MainDataBase.instance.LoadLocal(); // 경로나 파일 없으면 UserManager.Instance.DefaultSetting();

    }//Load from Local if exists Data.

    IEnumerator LoginGPGS(bool clicked)
    {
        float tryTime = 0;
        float waitTime = 5;
        if(clicked == false) { waitTime = 3f; }

        notice_Login.SetActive(true);
        notice_Fail.SetActive(false);
        Btn_Login.SetActive(false);
        Btn_Local.SetActive(false);
        Btn_Start.SetActive(false);
        while (true)
        {
            if(LoginManager.instance.googleLoginSuccess == true)
            {
//                UserManager.Instance.currentUser.loginTime = System.DateTime.Now;
                DebugViewer.Instance.debugTextObjectList[2].GetComponent<Text>().text = "google Login Success";
                notice_Hi.SetActive(true);
                notice_Hi.GetComponent<Text>().text = "Hi " + UserManager.Instance.currentUser.name;
                notice_Login.SetActive(false);
                Btn_Login.SetActive(false);
                Btn_Local.SetActive(false);
                Btn_Start.SetActive(true);      
                break;
            }

            if (tryTime > waitTime)
            { // 초 조정하기
                notice_Login.SetActive(false);
                notice_Fail.SetActive(true);
                Btn_Login.SetActive(true);
                Btn_Local.SetActive(true);
                break;
            }

            tryTime += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        
    }

 
}
