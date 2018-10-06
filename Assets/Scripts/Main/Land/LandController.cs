using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandController : MonoBehaviour {

    public GameObject landTiles;
    public GameObject weather;
    public GameObject units;
    public GameObject backgroundObj;
    List<string> weatherList;
    string currentWeather; // skill Puzzle ID

    private void Awake()
    {
        landTiles = transform.GetChild(1).gameObject;
        weather = transform.GetChild(2).gameObject;
        units = transform.GetChild(3).gameObject;
        backgroundObj = transform.GetChild(4).gameObject;
    }

    public void LandSetting(int landID)
    {
        name = "land" + landID;

        //int clearPuzzleList = UserManager.Instance.currentUser.gotLandList[landID - 1].ClearPuzzleList.Length;
        //int gotPuzzleList = UserManager.Instance.currentUser.gotLandList[landID - 1].gotPuzzleList.Length;
        //int unitList = UserManager.Instance.currentUser.gotLandList[landID - 1].unitList.Length;

        //Minimum land ID is 1 
        #region BG
        Sprite[] tempSprites;
        tempSprites = Resources.LoadAll<Sprite>("Sprite/Land/LBG" + landID);//currentPuzzleID
        for (int i = 0; i < landTiles.transform.childCount; i++)
        {
            landTiles.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = tempSprites[Random.Range(0, 7)];
        }
        //transform.position = new Vector3(0,0,0);
        #endregion //BGController만들어서 붙일것.

        #region BG_Obj
        if (backgroundObj.transform.GetChildCount() == 0)
        {
            try
            {
                GameObject obj = Resources.Load<GameObject>("Prefabs/Lands/LandObj" + landID);
                obj = Instantiate(obj, Vector3.zero, Quaternion.identity);
                obj.transform.localPosition = new Vector3(2.5f, 0, -3);
                obj.transform.SetParent(backgroundObj.transform);
                //Destroy(obj.gameObject);
            }
            catch { }
        }

        #endregion

        #region Weather    
        weatherList = new List<string>();

        for(int i=0; i< LandManager.instance.landList.Count; i++)
        {
            if(LandManager.instance.landList[i].id == landID)
            {
                weatherList = new List<string>();
                for(int j=0; j< LandManager.instance.landList[i].puzzleList_S.Count; j++)
                {
                    //Debug.Log(LandManager.instance.landList[i].puzzleList_S[j]);
                    weatherList.Add(LandManager.instance.landList[i].puzzleList_S[j]);
                }
            }
        }
        int weatherNum;
        if (landID <= UserManager.Instance.currentUser.gotLandList.Count)
        {
            weatherNum = UserManager.Instance.currentUser.gotLandList[landID - 1].weather;
        }
        else
        {
            weatherNum = 0;
        }
        
        
        currentWeather = weatherList[weatherNum];
        //        Debug.Log("currentWeather : "+ currentWeather);
        if (UserManager.Instance.currentUser.gotLandList.Count > 0) { weather.GetComponent<WeatherController>().OnWeather(currentWeather); }        
        #endregion


        
    }

}
