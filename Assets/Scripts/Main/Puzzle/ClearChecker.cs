using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearChecker : MonoBehaviour {

 	public void clearCheck () {
        int currentSize = PuzzleManager.instance.currentPuzzleSize;
        int ok=0;
        for(int i=0; i< currentSize* currentSize; i++)
        {
            bool x = PuzzleManager.instance.tileGroup_Active.transform.GetChild(i).GetComponent<TileController>().check == 1 ? true : false;
            if (PuzzleManager.instance.currentGoal[i] ==  x) { ok++; }
        }

        if (ok == currentSize * currentSize) { Debug.Log("goal"); DebugViewer.Instance.debugTextObjectList[2].GetComponent<Text>().text = "Goal!"; OnGoal(); }
        else { }
//        DebugViewer.Instance.debugTextObjectList[2].GetComponent<Text>().text = "checkTileCount : "+ok+" (ing)"); }
	}

    #region ClosePuzzle
    public void ClosePuzzle(bool clear)
    {
        #region RemovePuzzle
        int tempCount = PuzzleManager.instance.tileGroup_Active.transform.childCount;
        for (int i=0; i < tempCount; i++)
        {
            GameObject tempTile = PuzzleManager.instance.tileGroup_Active.transform.GetChild(tempCount-1-i).gameObject;
            tempTile.transform.position = new Vector3(-100, -100, 0);
            tempTile.transform.SetParent(PuzzleManager.instance.tileGroup_Rest.transform);
        }//remove tile
        for(int i=0; i< PuzzleManager.instance.lines.transform.childCount; i++)
        {
            PuzzleManager.instance.lines.transform.GetChild(i).transform.position = new Vector3(-100, -100, -2);
        }//remove line 
        PuzzleManager.instance.cursor.transform.position = new Vector3(-20,-20,-1);//remove cursor
        PuzzleManager.instance.puzzleBG.SetActive(false);//remove puzzle background
        #endregion

        #region RemovePuzzleKey
        MovePuzzleKey(PuzzleManager.instance.puzzleKeyBrush.Keys_ActiveH.transform, PuzzleManager.instance.puzzleKeyBrush.Keys_Rest.transform);
        MovePuzzleKey(PuzzleManager.instance.puzzleKeyBrush.Keys_ActiveW.transform, PuzzleManager.instance.puzzleKeyBrush.Keys_Rest.transform);
        #endregion

        PuzzleManager.instance.viewCon.againView.SetActive(true);
        if(clear == true)
        {
            int cnt = PuzzleManager.instance.currentPuzzle.spawnCount;
            LandManager.instance.GetComponent<UnitManager>().UnitSpawn(PuzzleManager.instance.currentPuzzle.id, true, cnt); // 유닛생성 명령. 

            int weather = PuzzleInfo.FindPuzzleEffect.FindWeatherNum(PuzzleManager.instance.currentPuzzle.id); 
            EventManager.instance.WeatherChangedFunc(); // Weather 바뀜 선언.
        }
        else
        {
            //puzzle save
        }
        LandManager.instance.OnLand(true);
        MainDataBase.instance.OnSaveLand();
       // AdMobManager.instance.ShowInterstitialAd();
    }



    void MovePuzzleKey(Transform startObj, Transform targetObj)
    {
        int loopCount = startObj.childCount;
        for (int i = 0; i < loopCount; i++)
        {
            startObj.GetChild(0).SetParent(targetObj);
        }
    }
    #endregion

    public void OnGoal() {
        PuzzleManager.instance.viewCon.puzzleView.transform.GetChild(1).GetComponent<PuzzleButton>().EndButtonChecker_Puzzle(); //버튼 체크 코루틴 종료
        PuzzleManager.instance.viewCon.views[3].GetComponent<PopupViewController>().clearPop.SetActive(true); //클리어 팝업 뜸.

        #region CurrentWeather
        if ( PuzzleManager.instance.currentPuzzle.type == "S") {
            int weather = PuzzleInfo.FindPuzzleEffect.FindWeatherNum(PuzzleManager.instance.currentPuzzle.id);
            UserManager.Instance.SetWeather(LandManager.instance.currentLand.id,weather); //현재 weather 수정.
        }
        else { }//Normal Btn은 MethodList에서 찾기.
        #endregion

        #region CurrentPuzzle
        GameObject puzzleIcon = PuzzleManager.instance.viewCon.views[3].transform.GetChild(0).GetChild(0).GetChild(0).gameObject;       
        GameObject puzzleName = PuzzleManager.instance.viewCon.views[3].transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        puzzleIcon.GetComponent<Image>().sprite = PuzzleManager.instance.currentSprite; //현재 퍼즐 스프라이트
        puzzleName.transform.GetChild(0).GetComponent<Text>().text = PuzzleManager.instance.currentPuzzle.name; //현재 퍼즐 이름
        #endregion

        #region ClearPuzzle In GotLandList 
        if (!UserManager.Instance.ClearPuzzleCheck(PuzzleManager.instance.currentPuzzle.name))
        {
            SaveData.GotLand land = UserManager.Instance.GetCurrentInGotLandList(LandManager.instance.currentLand.id);
            land.clearPuzzleList.Add(PuzzleManager.instance.currentPuzzle.id);
            //Debug.Log(PuzzleManager.instance.currentPuzzle.id);
        }//현재 Land에서 ClearPuzzle이 없다면 해당 ClearList에 퍼즐이름 추가.
        #endregion

        UserManager.Instance.currentUser.lastLand = LandManager.instance.currentLand.id;

        MainDataBase.instance.OnSaveAdmin();
    }

    public void OnGoal_For_Editor()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) { OnGoal(); }
    }

}
