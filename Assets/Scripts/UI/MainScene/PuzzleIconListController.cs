using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleIconListController : MonoBehaviour {
    #region ForPuzzleButtonSetting
    public List<GameObject> puzzleIconList_N_Active;
    public List<GameObject> puzzleIconList_N_Rest;
    public List<GameObject> puzzleIconList_S_Active;
    public List<GameObject> puzzleIconList_S_Rest;
    GameObject bg;
    GameObject restBtns_N;
    GameObject activeBtns_N;
    GameObject restBtns_S;    
    GameObject activeBtns_S;
    [SerializeField] float addScreenX; //screen사이즈 변화에 따른 비율변화값.
    [SerializeField] float addScreenY;
    #endregion

    #region ForSkillButtonDrag
    public Canvas mycanvas; // raycast가 될 캔버스 
    GraphicRaycaster gr;
    PointerEventData ped;

    GameObject touchedSkill;
    #endregion

    private void Awake()
    {
        BaseSetting(); //addScreen, childs, load Puzzle_Base, touchedSkill<GameObj>
    }

    void BaseSetting() {
        Screen.SetResolution(Screen.width, (Screen.width * 16) / 9, true);
        addScreenX = (float)System.Math.Round((Screen.width / 720.0f), 2);
        addScreenY = (float)System.Math.Round((Screen.height / 1280.0f), 2);
        
        bg = transform.GetChild(0).gameObject;
        activeBtns_N = transform.GetChild(1).gameObject;
        restBtns_N = transform.GetChild(2).gameObject;
        activeBtns_S = transform.GetChild(3).gameObject;
        restBtns_S = transform.GetChild(4).gameObject;

        LoadPuzzle_Base();

        ReadyTouchedSkill();
    }

    void LoadPuzzle_Base()
    {
        GameObject tempIcon = Resources.Load<GameObject>("Prefabs/Puzzles/Btn_Puzzle_S");
        for (int i=0; i<4; i++)
        {
            puzzleIconList_S_Rest.Add(Instantiate(tempIcon, restBtns_S.transform.position, Quaternion.identity));
            puzzleIconList_S_Rest[i].transform.SetParent(restBtns_S.transform);
        }
        tempIcon = Resources.Load<GameObject>("Prefabs/Puzzles/Btn_Puzzle_N");
        for (int i=0; i<20; i++)
        {
            puzzleIconList_N_Rest.Add(Instantiate(tempIcon, restBtns_N.transform.position, Quaternion.identity));
            puzzleIconList_N_Rest[i].transform.SetParent(restBtns_N.transform);
            puzzleIconList_N_Rest[i].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 64*addScreenY);
            puzzleIconList_N_Rest[i].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 72*addScreenY);
            puzzleIconList_N_Rest[i].transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 48*addScreenY);
            puzzleIconList_N_Rest[i].transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 48*addScreenY);
        }
    }
    
    public void PuzzleIconOnOff()
    {
        if (gameObject.activeSelf == true) { gameObject.SetActive(false); }
        else { gameObject.SetActive(true); }
    }

    public void SetPuzzleIconList(int currentLandID)
    {
        SetIconList_S(currentLandID);
        SetIconList_N(currentLandID);
        StartDragCheck();
    }

    void SetIconList_N(int currentLandID) {
        List<string> normalList = new List<string>();
        int btnCount;
        #region Btn Count Check       
        for (int i = 0; i < PuzzleManager.instance.puzzles[currentLandID - 1].Length; i++)
        {
            if (PuzzleManager.instance.puzzles[currentLandID - 1][i].type == "N")
            {
                normalList.Add(PuzzleManager.instance.puzzles[currentLandID - 1][i].id);
            }
        }//btn수량
        btnCount = normalList.Count; //추가 필요한 버튼 수
        #endregion

        #region Setting Btn GameObject to Parent 
//        Debug.Log("BtnCount : " + btnCount);
        if (puzzleIconList_N_Active.Count >= btnCount)
        {
            btnCount = puzzleIconList_N_Active.Count - btnCount;
            for (int i = 0; i < btnCount; i++)
            {
                puzzleIconList_N_Rest.Add(activeBtns_N.transform.GetChild(btnCount - i - 1).gameObject);
                puzzleIconList_N_Rest[puzzleIconList_N_Rest.Count - 1].transform.SetParent(restBtns_N.transform);
                puzzleIconList_N_Active.RemoveAt(btnCount - i - 1);
            }
        }
        else
        {
            btnCount = btnCount - puzzleIconList_N_Active.Count;            
            for (int i = 0; i < btnCount; i++)
            {
                puzzleIconList_N_Active.Add(restBtns_N.transform.GetChild(btnCount - i - 1).gameObject);
                puzzleIconList_N_Active[puzzleIconList_N_Active.Count - 1].transform.SetParent(activeBtns_N.transform);
                puzzleIconList_N_Rest.RemoveAt(btnCount - i - 1);
            }
        }
        #endregion

        #region Setting Btn in List
        int normalCount = LandManager.instance.currentLand.puzzleList_N.Count; //normal버튼 수
        for (int i = 0; i < normalCount; i++)
        {
            string puzzleID = LandManager.instance.currentLand.puzzleList_N[i]; //ID
            //Debug.Log("puzzleID : "+puzzleID);
            int weather = UserManager.Instance.GetWeather(currentLandID); //Weather

            Vector3 newPos = PuzzleInfo.FindPuzzlePos.puzzlePos(puzzleID, weather); //find Pos
            puzzleIconList_N_Active[i].GetComponent<RectTransform>().position = new Vector3(newPos.x * addScreenX, newPos.y * addScreenY, newPos.z); //Set Convert Pos

            puzzleIconList_N_Active[i].name = "btn_"+ puzzleID;
            if (i < normalCount)
            {                
                AddButton(puzzleIconList_N_Active[i], currentLandID, puzzleID);
            }
            else { Debug.Log(string.Format("btnCount = {0}, normalCount = {1}",btnCount, normalCount)); } //add Button
        }
        for(int i=0; i < restBtns_N.transform.childCount; i++) {
            restBtns_N.transform.GetChild(i).transform.position = restBtns_N.transform.position;
        }
        //Debug.Log(Screen.width+ "/.0 = "+ addScreenX + ", " + Screen.height +"/.0 = "+ addScreenY);
        #endregion

        #region Set Btn's Star in List
        Vector3[][] starPos = {
            new Vector3[]{new Vector3(0,-15,0) },
            new Vector3[]{new Vector3(-10,-18,0),new Vector3(10, -18, 0) },
            new Vector3[]{new Vector3(-18, -20, 0),new Vector3(0,-15,0),new Vector3(18, -20, 0) }
        };
                
        Sprite[] icons = Resources.LoadAll<Sprite>("Sprite/Icon");        
        for (int i=0; i< puzzleIconList_N_Active.Count; i++)
        {
            string puzzleID = HarimTool.EditValue.EditText.Right(puzzleIconList_N_Active[i].name, 4);
            int maxCnt = PuzzleManager.instance.GetPuzzleMaxCount(puzzleID); 
            int spawnCnt = PuzzleManager.instance.GetPuzzleSpawnCount(puzzleID); 
            if (maxCnt != 1) { maxCnt = System.Convert.ToInt32(maxCnt / spawnCnt); } //Stars Count

            for (int j=0; j<3; j++)
            {
                puzzleIconList_N_Active[i].transform.GetChild(2).GetChild(j).GetComponent<Image>().sprite = icons[9];
                puzzleIconList_N_Active[i].transform.GetChild(2).GetChild(j).gameObject.SetActive(false);                
            }//Set Stars
            for (int j = 0; j < maxCnt; j++)
            {
                puzzleIconList_N_Active[i].transform.GetChild(2).GetChild(j).gameObject.SetActive(true);
                puzzleIconList_N_Active[i].transform.GetChild(2).GetChild(j).GetComponent<RectTransform>().anchoredPosition3D = starPos[maxCnt - 1][j];
            }//Set Stars Position
            int unitCnt = LandManager.instance.GetComponent<UnitManager>().UnitCountCheck(puzzleID);
            int clearCnt = System.Convert.ToInt32(unitCnt / spawnCnt);
            for(int j=0; j< clearCnt; j++)
            {
                puzzleIconList_N_Active[i].transform.GetChild(2).GetChild(j).GetComponent<Image>().sprite = icons[10];
            }//Set Clear Stars           
        }
        Debug.Log("Stars Setting");
        #endregion
    }

    void SetIconList_S(int currentLandID)
    {
        List<string> skillList = new List<string>();

        #region Btn Count Check
        if (PuzzleManager.instance.puzzles.Length < currentLandID) { Debug.Log("puzzles.Length < land"); } ;
        for (int i = 0; i < PuzzleManager.instance.puzzles[currentLandID - 1].Length; i++)
        {
            if (PuzzleManager.instance.puzzles[currentLandID - 1][i].type == "S")
            {
                skillList.Add(PuzzleManager.instance.puzzles[currentLandID - 1][i].id);
            }
        }
        int btnCount = skillList.Count;        //CurrentLand의 specil puzzle 수. max=4;
        #endregion

        float buttonSizeBase = 100;
        float buttonSpaceBase = 10;
        float buttonSize = buttonSizeBase * addScreenX;
        float buttonSpace = buttonSpaceBase * addScreenX;
        activeBtns_S.GetComponent<GridLayoutGroup>().cellSize = new Vector2(buttonSizeBase, buttonSizeBase);
        activeBtns_S.GetComponent<GridLayoutGroup>().spacing = new Vector2(buttonSpaceBase, 0);

        float minusVal = (((btnCount - 1) * (buttonSpace) * 0.5f) + (((buttonSize) * (btnCount-1) * 0.5f)));

        #region Setting Btn Object to Parent        
        if (puzzleIconList_S_Active.Count >= btnCount)
        {
            btnCount = puzzleIconList_S_Active.Count - btnCount; Debug.Log("btnCount < : "+ skillList.Count);
            for (int i = 0; i < btnCount; i++)
            {
                puzzleIconList_S_Rest.Add(puzzleIconList_S_Active[puzzleIconList_S_Active.Count - 1].gameObject);
                puzzleIconList_S_Rest[puzzleIconList_S_Rest.Count - 1].transform.SetParent(restBtns_S.transform);
                puzzleIconList_S_Active.RemoveAt(puzzleIconList_S_Active.Count - 1);
            }
        }
        else
        {
            btnCount = btnCount - puzzleIconList_S_Active.Count; Debug.Log("btnCount > : " + skillList.Count);

            for (int i = 0; i < btnCount; i++)
            {
                puzzleIconList_S_Active.Add(puzzleIconList_S_Rest[puzzleIconList_S_Rest.Count - 1].gameObject);
                puzzleIconList_S_Active[puzzleIconList_S_Active.Count - 1].transform.SetParent(activeBtns_S.transform);
                puzzleIconList_S_Rest.RemoveAt(puzzleIconList_S_Rest.Count - 1);
            }
        }
        #endregion

        #region Setting Btn in List
        int skillCount = skillList.Count; Debug.Log("currentLand ID : "+LandManager.instance.currentLand.id);
        int imgSize=98;
        Debug.Log(skillCount+ ", vs " +puzzleIconList_S_Active.Count);

        if (skillCount == puzzleIconList_S_Active.Count)
        {
            for (int i = 0; i < skillCount; i++)
            {
                string puzzleID = LandManager.instance.currentLand.puzzleList_S[i]; 
                puzzleIconList_S_Active[i].transform.GetComponent<RectTransform>().localScale = Vector3.one;
                puzzleIconList_S_Active[i].transform.GetComponent<Image>().color = Vector4.one;
                puzzleIconList_S_Active[i].transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imgSize);// *addScreenX);
                puzzleIconList_S_Active[i].transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imgSize);// *addScreenX);
                puzzleIconList_S_Active[i].name = "Skill"+ puzzleID;
                puzzleIconList_S_Active[i].tag = "SkillBtn";
                AddButton(puzzleIconList_S_Active[i], currentLandID, puzzleID);
            }
        }//Btton Setting and Button.OnClick추가
        #endregion

        #region ActiveBtn_S BG Setting
        Vector3 pos = new Vector3(transform.parent.transform.position.x, activeBtns_S.transform.position.y, 0);// Debug.Log(activeBtns_S.transform.position);

        activeBtns_S.transform.position = new Vector3((pos.x - (minusVal)), (pos.y), pos.z); 

        float rectWidth = (buttonSizeBase * skillCount) + (buttonSpaceBase * (skillCount + 1));

        Vector2 rectPos = bg.GetComponent<RectTransform>().rect.position;
        Vector2 rectSize = bg.GetComponent<RectTransform>().rect.size;
        bg.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectWidth);//= new Rect().Set(rectPos.x, rectPos.y, rectWidth, rectSize.y)
        
        #endregion

        for(int i=0; i<restBtns_S.transform.childCount; i++)
        {
            restBtns_S.transform.GetChild(i).position = restBtns_S.transform.position;
        }
    }

    void AddButton(GameObject targetObj, int currentLandID, string puzzleID)
    {
        targetObj.SetActive(true);

        if (targetObj.GetComponent<ClickEffect>())
        {
            targetObj.GetComponent<ClickEffect>().StopAllCoroutines();
        }

        if (!targetObj.GetComponent<Button>())
        {
            targetObj.AddComponent<Button>();
        }

        #region add image
        if (UserManager.Instance.ClearPuzzleCheck(puzzleID)) {
            Sprite iconSprite = Resources.Load<Sprite>("Sprite/Puzzle/" + puzzleID);
            if (targetObj.transform.GetChild(0).GetComponent<Image>())
            {
                targetObj.transform.GetChild(0).GetComponent<Image>().sprite = iconSprite;
            }
            targetObj.transform.GetChild(1).GetComponent<Text>().text = "";
        }
        else
        {
            if (targetObj.transform.GetChild(0).GetComponent<Image>())
            {
                targetObj.transform.GetChild(0).GetComponent<Image>().sprite = null;
            }
            targetObj.transform.GetChild(1).GetComponent<Text>().text = "?";
        }
        #endregion

        #region add Btn
        targetObj.GetComponent<Button>().onClick.RemoveAllListeners();

        targetObj.GetComponent<Button>().onClick.AddListener(delegate {
            PuzzleManager.instance.viewCon.SceneOn(2);
        });
        targetObj.GetComponent<Button>().onClick.AddListener(delegate {
            PuzzleManager.instance.StartPuzzle(puzzleID);
        });
        //targetObj.GetComponent<Button>().onClick.AddListener(delegate {
        //    PuzzleManager.instance.viewCon.SceneOff(1);
        //});
        //Debug.Log("addBtn = " + puzzleID);
        #endregion
    }

    #region Drag SkillButton
    public void StartDragCheck() { StopAllCoroutines(); StartCoroutine(ButtonTouched()); }
    public void StopDragCheck() { StopCoroutine(ButtonTouched()); touchedSkill.transform.GetChild(0).GetComponent<Image>().enabled = false; }

    IEnumerator ButtonTouched()
    {
        gr = mycanvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);

        string skillID="";        
        Vector3 skillHome=Vector3.zero;
        while (true)
        {
            if (Input.touchCount > 0)
            {
                List<RaycastResult> results = TouchedObjs(0);
                if (results.Count>0 ) {                    
                    switch (Input.GetTouch(0).phase)
                    {
                        case TouchPhase.Began:
                            //Debug.Log("touched : " + results[0].gameObject.name);
                            if (results[0].gameObject.tag == "SkillBtn")
                            {
                                skillID = HarimTool.EditValue.EditText.Right(results[0].gameObject.name, 4);
                                if (UserManager.Instance.ClearPuzzleCheck(skillID))
                                {                                    
                                    touchedSkill.transform.position = new Vector3(-1000, -1000, touchedSkill.transform.position.z);
                                    touchedSkill.transform.GetChild(0).GetComponent<Image>().enabled = true;
                                    touchedSkill.transform.GetChild(0).GetComponent<Image>().sprite = results[0].gameObject.transform.GetChild(0).GetComponent<Image>().sprite;                                    
                                    skillHome = touchedSkill.transform.position;
                                    touchedSkill.tag = "SkillBtn";
                                    //touchedSkill.name = skillID;
                                }
                            }
                            else if ( results[0].gameObject.name == "HideBtn" ) {
                                Debug.Log("HideBtn");
                                LandManager.instance.views.againView.transform.GetChild(3).GetComponent<ViewCaseController>().CaseOnOff();
                            }
                            break;

                        case TouchPhase.Moved:
                            if (touchedSkill.tag == "SkillBtn")
                            {
                                DragSkillButton(touchedSkill, Input.GetTouch(0).position);
                            }
                            break;

                        case TouchPhase.Ended:
                            //Debug.Log("touched : end ");
                            if (touchedSkill.tag == "SkillBtn")
                            {
                                touchedSkill.transform.GetChild(0).GetComponent<Image>().enabled = false;
                                int newWeather = PuzzleInfo.FindPuzzleEffect.FindWeatherNum(skillID); //해당 skill ID 클리어 시 얻는 weather 체크.
                                UserManager.Instance.SetWeather(LandManager.instance.currentLand.id, newWeather); //현재 weather 수정.                            
                                PuzzleManager.instance.currentLandObj.GetComponent<LandController>().weather.GetComponent<WeatherController>().OffWeather();
                                PuzzleManager.instance.currentLandObj.GetComponent<LandController>().weather.GetComponent<WeatherController>().OnWeather(skillID);
                            }//currentWeather 변경 밑 OnWeather()
                            break;
                    }
                }
            }
            else { 
                touchedSkill.tag = "Untagged";
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    List<RaycastResult> TouchedObjs(int num)
    {
        Vector2 pos = Input.GetTouch(num).position;
        ped.position = new Vector3(pos.x, pos.y, 0.0f);//get world position.
        List<RaycastResult> results = new List<RaycastResult>(); // 여기에 히트 된 개체 저장 
        gr.Raycast(ped, results);
        return results;
    }

    void ReadyTouchedSkill()
    {
        if (!touchedSkill)
        {
            touchedSkill = Instantiate(Resources.Load<GameObject>("Prefabs/Puzzles/Btn_Puzzle_S"));
            touchedSkill.name = "touchedSkill";
            touchedSkill.transform.SetParent(mycanvas.transform);
            touchedSkill.transform.GetChild(0).GetComponent<Image>().enabled = false;
            Destroy(touchedSkill.GetComponent("Image"));
            Destroy(touchedSkill.transform.GetChild(1).gameObject);
        }
    }

    void DragSkillButton(GameObject target, Vector2 touchPos) {
        Debug.Log(target.name);
        //target.GetComponent<>
        target.transform.position = new Vector3(touchPos.x, touchPos.y, target.transform.position.z);
    }
    #endregion



}

// 반 - 0.5f    (x*i*0.5) - (x*0.5)