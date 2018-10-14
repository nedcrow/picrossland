using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminViewController : MonoBehaviour {

    GameObject NickName;
    GameObject Star;
    GameObject Gem;
    GameObject SettingButton;

    private void Start()
    {
        NickName = transform.GetChild(1).gameObject;
        Star = transform.GetChild(2).gameObject;
        Gem = transform.GetChild(3).gameObject;
        SettingButton = transform.GetChild(4).gameObject;
    }

    public void SetAdminView()
    {
        NickName.GetComponent<Text>().text = UserManager.Instance.currentUser.name;

        int clearPuzzleCount = 0;
        for (int i = 0; i < UserManager.Instance.currentUser.gotLandList.Count; i++)
        {
            clearPuzzleCount += UserManager.Instance.currentUser.gotLandList[i].clearPuzzleList.Count;
        }
        //Debug.Log("clearPuzzleCount : " + clearPuzzleCount);
        Star.transform.GetChild(0).GetComponent<Text>().text = clearPuzzleCount.ToString();

        Gem.transform.GetChild(0).GetComponent<Text>().text = UserManager.Instance.currentUser.gem.ToString();
        
    }
}
