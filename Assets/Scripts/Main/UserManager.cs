using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour {

    #region Singleton
    private static UserManager _instance;
    public static UserManager Instance
    {
        get
        {
            if (!_instance)
            {
                GameObject obj = GameObject.Find("LandOfPicross");
                if (obj == null)
                {
                    obj = new GameObject("LandOfPicross");
                    obj.AddComponent<UserManager>();
                }
                return obj.GetComponent<UserManager>();
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField]
    public SaveData.User currentUser = new SaveData.User();
    
    [SerializeField]
    bool firstGame;

    void Awake () {
        firstGame = false;
        DontDestroyOnLoad(transform.gameObject);
        DefaultSetting();
    }   

    public void DefaultSetting()
    {
        firstGame = true;
        currentUser.name = "first";
        currentUser.gem = 0;
        currentUser.ClearLandCount = 0;
        currentUser.gotLandList = new List<SaveData.GotLand>();
        currentUser.ingPuzzleList = new List<SaveData.IngPuzzle>().ToArray();
        currentUser.lastLand = 1;
    }

    /// <summary>
    /// Get Weather of CurrentLand In gotLandList. (0~3).
    /// </summary>
    /// <param name="currentLandID"></param>
    /// <returns></returns>
    public int GetWeather(int currentLandID)
    {
        int weather = 0;
        if (currentUser != null) {
            foreach (SaveData.GotLand x in currentUser.gotLandList)
            {
                if (x.id == currentLandID) { weather = x.weather; }
            }
        }
        return weather;
    }

    /// <summary>
    /// Set Weather of CurrentLand In gotLandList. (0~3).
    /// </summary>
    /// <param name="currentLandID"></param>
    /// <param name="weather"></param>
    public void SetWeather(int currentLandID, int weather)
    {
        if (currentUser != null)
        {
            foreach (SaveData.GotLand x in currentUser.gotLandList)
            {
                if (x.id == currentLandID) { x.weather = weather; }
            }
        }
    }

    public void SetStartTime()
    {
        currentUser.loginTime = System.DateTime.Now;
    }

    public bool ClearPuzzleCheck(string puzzleID) {
        bool check = false;
        
        for (int i = 0; i < currentUser.gotLandList.Count; i++)
        {
            SaveData.GotLand land = currentUser.gotLandList[i];
            check = HarimTool.EditValue.EditSomething.ContainAnB(puzzleID, land.clearPuzzleList);
            if(check == true) { return true; }
        }

        return check;
    }

    public SaveData.GotLand GetCurrentInGotLandList(int landID)
    {
//        Debug.Log(landID+", "+currentUser.gotLandList.Count);
        SaveData.GotLand current = currentUser.gotLandList[0];
        for (int i = 0; i < currentUser.gotLandList.Count; i++)
        {
            if (landID == currentUser.gotLandList[i].id) { current = currentUser.gotLandList[i]; }
            //else { Debug.Log("No Found This ID in GotLandList : " + landID + ", "+ currentUser.gotLandList[i].id); }
        }        
        return current;        
    }

    public void AddGotland(int landID)
    {       
        bool sameLand = false;
        for (int i = 0; i< currentUser.gotLandList.Count; i++)
        {
            if(currentUser.gotLandList[i].id == landID) {
                sameLand = true;
                if(currentUser.gotLandList[i].gotPuzzleList.Count != PuzzleManager.instance.puzzles[landID].Length)
                {
                    currentUser.gotLandList[i].gotPuzzleList = new List<string>();
                    currentUser.gotLandList[i].gotPuzzleList = AddGotPuzzles(landID);
                }
                break;
            }
        }
        if(sameLand == false) {
            #region tempLand_DefaultSetting
            SaveData.GotLand tempLand = new SaveData.GotLand();
            tempLand.id = landID;
            tempLand.weather = 0;
            tempLand.gotPuzzleList = AddGotPuzzles(landID);

            #endregion
            currentUser.gotLandList.Add(tempLand);
        }

    }

    /// <summary>
    /// 특정gotLand에 gotPuzzleList 추가 시 사용.
    /// </summary>
    /// <param name="landID"></param>
    /// <returns></returns>
    List<string> AddGotPuzzles(int landID)
    {
        List<string> gotPuzzleList=new List<string>();
        for (int i = 1; i < PuzzleManager.instance.puzzles[landID].Length+1; i++)
        {
            string tempPuzzleID_F = landID < 10 ? "0" + landID : landID.ToString();
            string tempPuzzleID_B = i < 10 ? "0" + i : i.ToString();

            gotPuzzleList.Add(tempPuzzleID_F + tempPuzzleID_B);
        }
        return gotPuzzleList;
    }
}
