﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockController : MonoBehaviour {

    GameObject lockBG;
    GameObject lockButton;
    GameObject lockIcon;
    GameObject unLockButton;

    void Start () {
        lockBG = transform.GetChild(0).gameObject;
        lockButton = transform.GetChild(1).gameObject; 
        lockIcon = transform.GetChild(2).gameObject;
        unLockButton = transform.GetChild(3).gameObject;
    }

    public void OnLock()
    {
        bool gotIt = false; Debug.Log("GotLandListCount : "+UserManager.Instance.currentUser.gotLandList.Count);
        for(int i=0; i< UserManager.Instance.currentUser.gotLandList.Count; i++)
        {
            if (UserManager.Instance.currentUser.gotLandList[i].id == LandManager.instance.currentLand.id) {
                if (UserManager.Instance.currentUser.gotLandList[i].clearPuzzleList.Count > 0) { gotIt = true; break; }
            }
        }//currentLand에 clearpuzzle이 있으면 gotit = true.
        Debug.Log("gotIt : "+gotIt);
        if(gotIt == false)
        {
            lockBG.SetActive(true);

            #region LockButtonSetting
            lockButton.SetActive(true);            
            lockButton.GetComponent <Button> (). onClick = new Button.ButtonClickedEvent ();
            lockButton.GetComponent<Image>().color = Color.white;

            lockButton.GetComponent<Button>().onClick.AddListener(delegate {
                PuzzleManager.instance.viewCon.SceneOn(2);
            });
            Debug.Log("puzzleList_S : "+LandManager.instance.currentLand.puzzleList_S.Count);
            lockButton.GetComponent<Button>().onClick.AddListener(delegate {
                PuzzleManager.instance.StartPuzzle(LandManager.instance.currentLand.puzzleList_S[0]);
            });
            lockButton.GetComponent<Button>().onClick.AddListener(delegate {
                PuzzleManager.instance.viewCon.SceneOff(1);
            });
            #endregion

            #region LockIcon
            int starPrice = System.Convert.ToInt32(Mathf.Pow(LandManager.instance.currentLand.id,2)*2);
            if (LandManager.instance.currentLand.id > 2 && LandManager.instance.currentLand.price > 0 && starPrice > UserManager.Instance.currentUser.star) //lockIcon 조건 - 이전 Land의 clearPuzzle수량 check추가 가능성 있음.
            {
                lockIcon.SetActive(true);
                lockIcon.GetComponent<Animator>().Play("Lock_Idle");
                lockIcon.transform.GetChild(0).GetComponent<Text>().text = "( " +UserManager.Instance.currentUser.star + " / "+ starPrice + " )";
                unLockButton.SetActive(true);
            }//currentLand ID가 3일 때 부터 Lock
            else {
                lockIcon.SetActive(false);
                unLockButton.SetActive(false);
            }
            #endregion

            #region UnlockButton
            if(unLockButton.activeSelf == true)
            {
                unLockButton.transform.GetChild(0).gameObject.SetActive(true);
                unLockButton.transform.GetChild(0).GetComponent<Image>().color = new Vector4(210 * 0.004f, 0, 0, 1);
                //unLockButton.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Gem " + LandManager.instance.currentLand.price;
                unLockButton.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "업데이트\r\n-예정-";
                unLockButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate {
                    CheckGem(LandManager.instance.currentLand.price);
                });
            }
            #endregion
        }
        else
        {
            for(int i=0; i<transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }


    public void CheckGem(int price)
    {
        if( UserManager.Instance.currentUser.gem > price)
        {
            unLockButton.transform.GetChild(0).GetComponent<ClickEffect>().clicked(1);
        }
        else
        {
            LandManager.instance.views.popupView.GetComponent<PopupViewController>().warningPop.SetActive(true);
            LandManager.instance.views.popupView.GetComponent<PopupViewController>().warningPop.GetComponent<WarningController>().W_Gem();
        }
    }

    public void UnLock()
    {
        lockIcon.GetComponent<Animator>().Play("Lock_Open");
        StartCoroutine(UnLock_Co());
    }//구매팝업 이후로 바꿔야함.
	
    IEnumerator UnLock_Co()
    {
        float a=1.5f;
        float speed = 0.08f;
        while (true)
        {
            a = a - speed;
            yield return new WaitForSeconds(0.06f);
            lockIcon.GetComponent<Image>().color = new Vector4(1, 1, 1, a);
            if (a < 0.8f) { lockIcon.GetComponent<Image>().color = new Vector4(1, 1, 1, 0); break; }
        }
    }

}
