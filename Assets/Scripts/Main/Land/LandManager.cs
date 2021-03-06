﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandManager : MonoBehaviour {

    #region Singleton
    private static LandManager _instance;
    public static LandManager instance
    {
        get
        {
            if (!_instance)
            {
                GameObject obj = GameObject.Find("Land");
                if (obj == null)
                {
                    obj = new GameObject("Land");
                    obj.AddComponent<LandManager>();
                }
                return obj.GetComponent<LandManager>();
            }
            return _instance;
        }
    }
    #endregion

    public ViewController views;

    public List<DataBase.Land> landList = new List<DataBase.Land>();
    public List<GameObject> landObjList = new List<GameObject>();
    public List<GameObject> gotLandObjList = new List<GameObject>();
    public DataBase.Land currentLand;

    public bool firstGame=false;

    void Awake () {
        DontDestroyOnLoad(transform.gameObject);
    }

   

    public void BaseSetting() {
        views.popupView.GetComponent<PopupViewController>().loadingPop.SetActive(true);
        views.popupView.GetComponent<PopupViewController>().loadingPop.GetComponent<LoadingController>().WaitLoading();

        MainDataBase.instance.LoadBaseDB();
        MainDataBase.instance.OnLoadAdmin();
        MainDataBase.instance.LoadSetting();
        OnLand(false);
    }

   

    /// <summary>
    ///  실행 시기 : 게임 실행 후, Puzzle Clear 후.
    ///  Land
    /// </summary>
    public void OnLand(bool afterClear)
    {        
        if (afterClear == true)
        {
            SelectLandOnly(currentLand.id);
            CurrentLand_Unit(true); //after clear
        }
        else
        {
            StartCoroutine(_OnLand());
        }             
    }

    IEnumerator _OnLand()
    {
        float waitTime = 0;
        while (true)
        {
            bool passAddWait=false;
            //bool logCheck = MainDataBase.instance.LoginDataCheck(); 
            if (MainDataBase.instance.local == true) { passAddWait = true; }// Debug.Log("passAddWait : true"); }
            else { passAddWait = MainDataBase.instance.loadAdmin; } // local 아니면, admin 불러올때까지 더 기다려야함.
            //Debug.Log("LoadAll : " + MainDataBase.instance.loadAll + ", passAddWait : " + passAddWait);
            if (MainDataBase.instance.loadAll == true && passAddWait == true)
            {               
                Debug.Log("coroutine : _OnLand / "+"PlayTime : "+ UserManager.Instance.currentUser.PlayTime);

                if (UserManager.Instance.currentUser.gotLandList.Count == 0) { firstGame = true; }
                else if (UserManager.Instance.currentUser.gotLandList[0].clearPuzzleList.Count == 0) { firstGame = true; }
                else { firstGame = false; }

                if(firstGame == true) {
                    views.firstView.SetActive(true);
                    views.firstView.GetComponent<FirstViewController>().NickNameCheck(true);
                    views.againView.SetActive(false); Debug.Log("firstGame");
                }
                else
                {
                    views.firstView.SetActive(false);
                    views.againView.SetActive(true);
                    //views.againView.transform.GetChild(1).GetComponent<PuzzleIconListController>().StartDragCheck();
                }

                LandObjectSetting();
                StopCoroutine(_OnLand());
                break;                
            }
            yield return new WaitForSeconds(0.02f);
            waitTime += 0.02f;
            if (waitTime > 5f) { Debug.Log("Need_DB_Check"); break; }
        }

    }

    /// <summary>
    /// BaseSetting ( BG, ETC )
    /// </summary>
    public void LandObjectSetting()
    {        
        for(int i=0; i< landList.Count; i++)
        {
            GameObject tempLand = Resources.Load<GameObject>("Prefabs/Lands/Land_Base");
            
            landObjList.Add(Instantiate(tempLand, Vector3.zero, Quaternion.identity));
            landObjList[i].gameObject.SetActive(true);
            landObjList[i].gameObject.name = (i + 1).ToString();
            landObjList[i].transform.SetParent(transform);
            landObjList[i].GetComponent<LandController>().LandSettingBG(i+1); // LandSetting( Land ID );
        } // LandObjBase Setting

        if (firstGame == true)
        {
            gotLandObjList.Add(landObjList[0]);
            gotLandObjList.Add(landObjList[1]);

            UserManager.Instance.AddGotland(1);
            UserManager.Instance.AddGotland(2);

            UserManager.Instance.currentUser.name = "test";//local ID 만들어서 넣어야함.

            currentLand = landList[0];
            PuzzleManager.instance.currentLandObj = gotLandObjList[0]; //currentLandObj등록            
        } // first면 처음 만드는거.
        else
        {
            Debug.Log("non first");
            gotLandObjList.Clear();
            for (int i=0; i< UserManager.Instance.currentUser.gotLandList.Count; i++)
            {
                gotLandObjList.Add(landObjList[i]);
            }// gotLandObjList갱신.      

            currentLand = landList[UserManager.Instance.currentUser.lastLand-1];
            PuzzleManager.instance.currentLandObj = landObjList[currentLand.id - 1];        

            //CurrentLandSetting(false);  //after clear

        } // first가 아니면 currentLand 불러오기. + Land Setting.
        //Destroy(tempLand);
        SelectLandOnly(currentLand.id);
        //Debug.Log("gotLandCount : "+UserManager.Instance.currentUser.gotLandList.Count);
    }

    #region Active One Land Only
    void SelectLandOnly(int landNum)
    {
        for (int i = 0; i < landObjList.Count; i++)
        {
            if (i + 1 != landNum)
            {
                landObjList[i].GetComponent<LandController>().weather.GetComponent<WeatherController>().OffWeather(); //먼저 날씨 obj가 있으면 지운다.                
                landObjList[i].SetActive(false);
            }
            else
            {
                landObjList[i].SetActive(true);
                landObjList[i].GetComponent<LandController>().LandSettingBG(landNum); //Weather, BG                
                PuzzleManager.instance.currentLandObj = landObjList[i];
                
                CurrentLand_Unit(false);  //Unit Spawn.
                CurrentLand_UI();  //UI

                if (firstGame == true) { PuzzleManager.instance.viewCon.againView.SetActive(false); firstGame = false; }
            }//입력반은 landNum의 Land만 활성화.
        }//gotLandObjList는 1부터 순서대로 쌓인다.

        GetComponent<UnitManager>().SetUnitsEvents(landNum);//Unit의 필요 Event만 추리기.

        views.againView.transform.GetChild(0).GetComponent<AdminViewController>().SetAdminView();
        views.againView.transform.GetChild(4).GetComponent<LockController>().OnLock();

        EventManager.instance.LandActivatedFunc();
        EventManager.instance.WeatherChangedFunc();//날씨 바뀜 선언. unit motion 초기화.
    }//현재 Land만 활성화

    void CurrentLand_UI()
    {
        PuzzleManager.instance.viewCon.puzzleView.SetActive(false);
        PuzzleManager.instance.viewCon.againView.SetActive(true); //Canvas
        PuzzleManager.instance.viewCon.againView.transform.GetChild(2).GetComponent<PuzzleIconListController>().SetPuzzleIconList(currentLand.id);//PuzzleIcon
        PuzzleManager.instance.viewCon.againView.transform.GetChild(1).GetComponent<LandViewController>().SetLandName(currentLand.id);

        Camera.main.transform.position = Camera.main.GetComponent<CameraController>().CameraPos_Land();//Camera
        Camera.main.orthographicSize = 4f;
    }

    void CurrentLand_Unit(bool afterClear)
    {
         Debug.Log("afterClear : "+afterClear + " / CurrentLand ID : "+currentLand.id);

        #region Units
        if (afterClear == false) {
            List<string> unitList = UserManager.Instance.GetCurrentInGotLandList(currentLand.id).unitList;
            List<GameObject> unitObjectList = GetComponent<UnitManager>().unitList;
            List<GameObject> CurrentLandUnitList = new List<GameObject>();
            if (unitObjectList.Count > 0)
            {
                foreach (GameObject unitObj in unitObjectList) {
                    int unitObjLand = System.Convert.ToInt32(HarimTool.EditValue.EditText.Left(unitObj.name, 2));
                    if (unitObjLand == currentLand.id)
                    {
                        CurrentLandUnitList.Add(unitObj);
                    }
                }
            }//생성해야할 UnitList확보.

            if (unitList.Count > CurrentLandUnitList.Count)
            {
                for (int i = 0; i < unitList.Count; i++)
                {
                    int unitLand = System.Convert.ToInt32(HarimTool.EditValue.EditText.Left(unitList[i], 2));
                    if (unitLand == currentLand.id)
                    {
                        GetComponent<UnitManager>().UnitSpawn(unitList[i], false, 1); // 퍼즐ID, 생성수량     
                    }// CurrentLand Unit만 생성.                    
                }
            }//생성 명령.

        }//afterClear가 true면 Unit추가 생성은 ClearCheck에서 실행.
        #endregion

        
    }//Canvas + Camera + PuzzleIcon + unit
    #endregion

    public void LandChange(string dir)
    {
        //Debug.Log("Try_LandChange");
        int landNum = currentLand.id;

        if (dir == "R" || dir == "r") { landNum++; }
        else if (dir == "L" || dir == "l") { landNum--; }
        if (landNum <= landObjList.Count && landNum > 0) {
            currentLand = landList[landNum-1];
            SelectLandOnly(landNum);            
        }
    }
}
