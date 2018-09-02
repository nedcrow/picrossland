using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public void LoadScene(int sceneNum)
    {
        //DebugViewer.Instance.debugTextObjectList[0].GetComponent<Text>().text = "Local Login try";
        Debug.Log("login");
        SceneManager.LoadScene("MainScene");
        //DebugViewer.Instance.debugTextObjectList[0].GetComponent<Text>().text = "Login Login ok";

    }
}
