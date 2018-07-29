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
                GameObject obj = GameObject.Find("PicrossLand");
                if (obj == null)
                {
                    obj = new GameObject("PicrossLand");
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
        for(int i=0; i< currentUser.gotLandList.Count; i++)
        {
            SaveData.GotLand land = currentUser.gotLandList[i];

            for (int j = 0; j < land.clearPuzzleList.Count; j++)
            {
                if (land.clearPuzzleList[j] == puzzleID)
                {
                    check = true; //Debug.Log(puzzleID);
                }
            }
        }
        return check;
    }

    public SaveData.GotLand GetCurrentInGotLandList(int landID)
    {
        //Debug.Log(landID+", "+currentUser.gotLandList[0].id);
        SaveData.GotLand current = currentUser.gotLandList[0];
        for (int i = 0; i < currentUser.gotLandList.Count; i++)
        {
            if (landID == currentUser.gotLandList[i].id) { current = currentUser.gotLandList[i]; }
            else { Debug.Log("No Found This ID in GotLandList : " + landID + ", "+ currentUser.gotLandList[i].id); }
        }        
        return current;        
    }
}
