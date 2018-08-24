using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


using GooglePlayGames;
using GooglePlayGames.BasicApi;

/// <summary>
/// GooglePlayGame Login With FireBase
/// </summary>
public class LoginManager : MonoBehaviour {

    #region Singleton
    private static LoginManager _instance;
    public static LoginManager instance
    {
        get
        {
            if (!_instance)
            {
                GameObject obj = GameObject.Find("LoginManager");
                if (obj == null)
                {
                    obj = new GameObject("LoginManager");
                    obj.AddComponent<LoginManager>();
                }
                return obj.GetComponent<LoginManager>();
            }
            return _instance;
        }
    }
    #endregion

    public string result;
    public bool googleLoginSuccess;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    string displayName;
    string emailAddress;
    string photoUrl;

    void Awake () {
        googleLoginSuccess = false;
        GooglePlayServiceInitialize();
        FirebaseInitialize();
        //게임 로그인 되어 있냐?
    }

    void GooglePlayServiceInitialize()
    {
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
            .RequestServerAuthCode(false)//option
            .EnableSavedGames()
            .RequestIdToken()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
#endif
    }

    void FirebaseInitialize()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        DebugViewer.Instance.debugTextObjectList[0].GetComponent<Text>().text = "Setting up Firebase Auth";
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user) //현재 접속 유저가 
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            //googleLoginSuccess = signedIn; //true
            if (!signedIn && user != null)
            {
                DebugViewer.Instance.debugTextObjectList[0].GetComponent<Text>().text = string.Format("Signed out {0}", user.UserId);
                Debug.Log("빵꾸");
            }//접속 사용자가 없을 경우. 접속 버튼을 보게 만들자.
            user = auth.CurrentUser;
            if (signedIn)
            {
                DebugViewer.Instance.debugTextObjectList[0].GetComponent<Text>().text = (string.Format("Signed in {0}", user.UserId));
                displayName = user.DisplayName ?? "";
                emailAddress = user.Email ?? "";
                DebugViewer.Instance.debugTextObjectList[0].GetComponent<Text>().text = (string.Format("Signed in {0} _ {1}", displayName, emailAddress));
            }// 접속 사용자가 있을 경우. 
        }
    }//인증상태 변화    

    #region GoogleLogin
    public void OnClickGoogleLogin(bool clicked)
    {
        Debug.Log("Start_Login");
       // GooglePlayServiceInitialize();
       // Debug.Log("GooglePlayServiceInitialize");

        Social.localUser.Authenticate((bool success) => // error position
        {
            Debug.Log("Try Login");
            result = string.Format("succes : {0}, userName : {1}", success, Social.localUser.userName);
            DebugViewer.Instance.debugTextObjectList[0].GetComponent<Text>().text = result;

            if (success)
            {
                StartCoroutine(coLogin(clicked));
            }
            else
            {
                Debug.Log(result);
            }

        });
        //Social.localUser.Authenticate(success => {
        //    if (success)
        //    {
        //        Debug.Log("Authentication successful");
        //        string userInfo = "Username: " + Social.localUser.userName +
        //            "\nUser ID: " + Social.localUser.id +
        //            "\nIsUnderage: " + Social.localUser.underage;
        //        Debug.Log(userInfo);
        //    }
        //    else
        //        Debug.Log("Authentication failed");
        //});
    }


    IEnumerator coLogin(bool clicked)
    {
        result = "try login....";
        DebugViewer.Instance.debugTextObjectList[1].GetComponent<Text>().text = result;
        while (System.String.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
        {
            yield return null;
        }

       
        if (clicked == false)
        {
            SignUp();
            //if (MainDataBase.instance.OnLoadAdmin()) { SignUp(); Debug.Log("자동 접속 시도"); } //과거 접속 기록이 있으면 자동접속.
        }//시작 시 자동 접속은 기존에 접속기록이 있을 경우만 실행. -> Token 저장해서 구별하던지 하고, 일단 인증 가능한 유저는 자동 접속.
        else
        {
            Debug.Log("접속 시도");
            SignUp();
        } 

    }
    #endregion


    void SignUp()
    {
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken(); //PlayGamesPlatform.Instance.GetIdToken();

        DebugViewer.Instance.debugTextObjectList[1].GetComponent<Text>().text = "getToken";



        Firebase.Auth.Credential credential =
           Firebase.Auth.GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
            {
                user = task.Result;

                //result = string.Format("Email : {0}, name : {1}", user.Email, user.DisplayName);

                UserManager.Instance.currentUser.id = user.UserId.ToString();
                UserManager.Instance.currentUser.name = Social.localUser.userName;
                DebugViewer.Instance.debugTextObjectList[0].GetComponent<Text>().text = user.DisplayName.ToString() + " / " + Social.localUser.userName;

                googleLoginSuccess = true;
                MainDataBase.instance.SaveLocal_LoginData("LoginData.txt");
            }
        });

    }
}
