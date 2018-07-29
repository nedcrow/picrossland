using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : MonoBehaviour {

    private void Update()
    {
        GetComponent<Text>().text = "LogIn : " + LoginManager.instance.googleLoginSuccess.ToString();
    }
}
