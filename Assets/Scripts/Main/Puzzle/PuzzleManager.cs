using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour {

    #region Singleton
    private static PuzzleManager _instance;
    public static PuzzleManager instance
    {
        get
        {
            if (!_instance)
            {
                GameObject obj = GameObject.Find("Puzzle");
                if (obj == null)
                {
                    obj = new GameObject("Puzzle");
                    obj.AddComponent<UserManager>();
                }
                return obj.GetComponent<PuzzleManager>();
            }
            return _instance;
        }
    }
    #endregion

    public DataBase.Land[] lands;
   // public DataBase.Land currentLand;
   [HideInInspector] public GameObject currentLandObj;

    public DataBase.Puzzle[][] puzzles;
    public DataBase.Puzzle currentPuzzle;
    public int currentPuzzleSize;

    #region HierarchyObjects
    public GameObject tileGroup_Rest;
    public GameObject tileGroup_Active;
    public GameObject lines;
    public GameObject cursor; // 여유있으면 '비어있을 때 하이어락키에서 찾으라는 것'도 추가함.
    public GameObject puzzleBG;
    public ViewController viewCon;
    public PuzzleKeyBrush puzzleKeyBrush;
    #endregion

    public List<string[]> heightKeyList = new List<string[]>();
    public List<string[]> widthKeyList = new List<string[]>();

    public Sprite[] currentSprites;
    public Sprite currentSprite;
    public Color[,] currentPixels;
    public bool[] currentGoal;
    public bool DrawEnd = false;

    [HideInInspector] public GameObject land_base;
    [HideInInspector] public GameObject tile_base;
    [HideInInspector] public GameObject line_base;


    public void StartPuzzle(string puzzleID)
    {
        DrawEnd = false;
        
        CheckCurrentPuzzle(puzzleID);
        if(Resources.LoadAll<Sprite>("Sprite/Puzzle/" + puzzleID).Length==0) {
            Debug.Log("StopPuzzle");
            StopPuzzle(true);
            viewCon.popupView.GetComponent<PopupViewController>().warningPop.SetActive(true);
            viewCon.popupView.GetComponent<PopupViewController>().warningPop.GetComponent<WarningController>().W_Resource();
        }
        else
        {
            #region Key
            currentSprites = Resources.LoadAll<Sprite>("Sprite/Puzzle/" + puzzleID);//currentPuzzleID
            currentPixels = new Color[currentPuzzleSize, currentPuzzleSize]; //for test
            GetComponent<KeyCreator>().CreateKey();// Puzzle ID
            #endregion

            currentLandObj.transform.GetChild(2).GetComponent<WeatherController>().OffWeather();
            currentLandObj.SetActive(false);

            #region DrawPuzzle
            LoadBaseTiles();
            LoadBaseLine();
            puzzleBG.SetActive(true);
            StartCoroutine(DelayForStartPuzzle(puzzleID, 0.8f));
            puzzleKeyBrush.DrawPuzzleKey();
            #endregion

            #region UI
            viewCon.SceneOff(1);

            if (UserManager.Instance.currentUser.settingVal[2] == true)
            {
                viewCon.puzzleView.transform.GetChild(1).gameObject.SetActive(true);
                viewCon.puzzleView.transform.GetChild(2).gameObject.SetActive(false);
                viewCon.puzzleView.transform.GetChild(1).GetChild(0).GetChild(4).gameObject.SetActive(true); //ControllerA - btn_cover
            }
            else
            {
                viewCon.puzzleView.transform.GetChild(2).gameObject.SetActive(false);
                viewCon.puzzleView.transform.GetChild(1).gameObject.SetActive(true);
            }
            
            viewCon.againView.transform.GetChild(2).GetComponent<PuzzleIconListController>().StopDragCheck();

            
            AdMobManager.instance.ShowBannerAd();
            #endregion
        }
    }

    public int GetPuzzleMaxCount(string puzzleID)
    {
        int first = System.Convert.ToInt32(HarimTool.EditValue.EditText.Left(puzzleID, 2));
        int cnt = 0;
        for (int i = 0; i < puzzles[first - 1].Length; i++)
        {
            if (puzzles[first - 1][i].id == puzzleID) { cnt = puzzles[first - 1][i].maxCount; }
        }
        return cnt;
    }

    public void StopPuzzle(bool error) {
        if (DrawEnd == true || error == true) { GetComponent<ClearChecker>().ClosePuzzle(false); viewCon.puzzleView.transform.GetChild(1).GetComponent<PuzzleButton>().EndButtonChecker_Puzzle(); } //controllerA
    }//Used Onclick() in Back_Button

    void CheckCurrentPuzzle(string puzzleID) {
        int find = 0;
        for (int i = 0; i < puzzles.Length; i++)
        {
            if (HarimTool.EditValue.EditText.Left(puzzles[i][0].id, 2) == HarimTool.EditValue.EditText.Left(puzzleID, 2))
            {
                for (int j = 0; j < puzzles[i].Length; j++)
                {
                    if (puzzles[i][j].id == puzzleID) { currentPuzzle = puzzles[i][j]; find++; }
                }
            }
        }
        if(find > 0)
        {
            
            currentPuzzleSize = currentPuzzle.size;
        }
        else
        {
            Debug.Log("Not found puzzle of this ID : "+puzzleID);
        }

    }//DB에 해당 퍼즐이 있는지 확인.

    void LoadBaseTiles()
    {
        tile_base = Resources.Load<GameObject>("Prefabs/Tile");
        for (int i=0; i<900; i++)
        {
            GameObject tile = Instantiate(tile_base,new Vector3(-100,-100,0), Quaternion.identity);
            tile.transform.SetParent(tileGroup_Rest.transform);
        }
        //Test(2);
    }

    void LoadBaseLine()
    {
        line_base = Resources.Load<GameObject>("Prefabs/Line");
        for (int i = 0; i < 10; i++)
        {
            GameObject line = Instantiate(line_base, new Vector3(-100, -100, -2), Quaternion.identity);
            line.transform.SetParent(lines.transform);
        }
        //Test(2);
    }

   
    

    void Test(int r) {
        Texture2D currentTexture = currentSprites[0].texture;
        int startPointX = r * currentPuzzleSize;
        int startPointY = (int)(currentTexture.GetPixels().Length / currentTexture.width) - currentPuzzleSize; //r*currentPuzzleSize
        for (int i = 0; i < currentPuzzleSize; i++) //currentPuzzle.x
        {
            for (int j = 0; j < currentPuzzleSize; j++) //currentPuzzle.x
            {
                currentPixels[i, j] = currentTexture.GetPixel(startPointX + i, startPointY + j);
                GameObject testTile = Instantiate(tile_base, new Vector3(i * 0.5f+0.5f, j * 0.5f+0.5f, 0), Quaternion.identity);
                testTile.SetActive(true);
                testTile.GetComponent<SpriteRenderer>().color = currentPixels[i, j];
                //Debug.Log(testTile.GetComponent<SpriteRenderer>().color);
                //Debug.Log(currentPixels[i, j]);
                testTile.name = i + "_" + j;
            }
        }
    }


    IEnumerator DelayForStartPuzzle(string puzzleID, float time) {

        yield return new WaitForSeconds(time);
        GetComponent<PuzzleLoader>().SetPuzzle(puzzleID.ToString());
    }
}
