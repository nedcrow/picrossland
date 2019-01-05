using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleButton : MonoBehaviour {

    public Canvas mycanvas; // raycast가 될 캔버스 
    public GameObject hintButton;
    GraphicRaycaster gr;
    PointerEventData ped;

    Touch tempTouch;
    Vector2 touchedPos;

    bool touchOn;
    int markTouch = 0;

    int[] xDist;
    int[] yDist;

    private void Awake()
    {    
        xDist = new int[]{0,1,0,-1}; //U,R,D,L
        yDist = new int[]{1,0,-1,0}; //U,R,D,L
    }

    List<Coroutine> playCoList = new List<Coroutine>();
    Coroutine playedCoroutine;
    public void OnButtonChecker_Puzzle()
    {
        if (playCoList.Count < 1)
        {
            playedCoroutine = StartCoroutine(ButtonTouched()); // mobile
            playCoList.Add(playedCoroutine);
            Debug.Log("playCoList.Count(add) : " + playCoList.Count);
        }
    }

    public void EndButtonChecker_Puzzle()
    {
        if (playedCoroutine!=null) {
            StopCoroutine(playedCoroutine);
            if (playCoList.Count>0) playCoList.RemoveAt(playCoList.Count - 1);
            Debug.Log("playCoList.Count(remove) : " + playCoList.Count);
        }        
    }

    void ReStartButtonChecker_Puzzle()
    {
        if (playedCoroutine != null) { StopCoroutine(playedCoroutine); }
        playedCoroutine = StartCoroutine(ButtonTouched());
    }


    #region touch
    int currentBtn;
    float checkTime;
    float mouseFirstTime=0.1f;
    float mouseTime; //이동 touch 시 계속 상승하며, 첫 이동이 아니면 mouseFirstTime보다 높을 때에만 이동을 실행시킨다. 이동 시 waitTime만큼 감소.
    float mouseTimeGrade;
    float waitTime = 0.025f;
    int mouseTimeGradeCount = 10; // mouseTimeGrade가 GradeCount를 넘어서면 이동이 빨라짐.
    
    IEnumerator ButtonTouched()
    {
        bool touched = false; // Touch 시작 여부 체크.
        gr = mycanvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);
        while (true)
        {
            if (Input.touchCount > 0)
            {
                touched = true;
                //Debug.Log(Input.touchCount);
                for (int i = 0; i < Input.touchCount; i++)
                {
                    List<RaycastResult> results = TouchedObjs(i);
                    switch (System.Convert.ToInt32 (tempTouch.phase)) 
                    {
                        case 0:
                            MoveButtonCheck(results);
                            if (PuzzleManager.instance.cursor.GetComponent<CursorController>().ready == true)
                            {
                                MarkButtonCheck(results);
                            }
                            break;

                        case 1:
                        case 2 :
                            if (mouseTime > mouseFirstTime) { MoveButtonCheck(results); } else { mouseTime+=0.01f; }                            
                            if (PuzzleManager.instance.cursor.GetComponent<CursorController>().ready == true)
                            {
                                MarkButtonCheck(results);
                            }
                            break;

                        case 3 ://TouchPhase.End
                            //Debug.Log(results[0].gameObject.name);
                            if (results[0].gameObject.tag == "MoveBtn") { mouseTimeGrade = 0; mouseTime += 0.01f; }
                            else if (results[0].gameObject.name == "Btn_Hint") { HintButtonClicked(); }
                            else { MarkButtonOutCheck(results); }
                            break;                            
                    }
                } 
                if (Input.touchCount == 1)
                {
                    List<RaycastResult> results = TouchedObjs(0);
                    if (results[0].gameObject.tag == "CheckBtn") { mouseTimeGrade = 0; mouseTime = 0; } else { MarkButtonOutCheck(results); }
                }
            }
            else {
                mouseTimeGrade = 0;
                mouseTime = 0;
                checkTime = 0;
                PuzzleManager.instance.cursor.GetComponent<CursorController>().CheckOut();
                if (touched == true) { touched = false; }
            }
            yield return null;
        }
    }

    List<RaycastResult> TouchedObjs(int num) {
        tempTouch = Input.GetTouch(num);
        Vector2 pos = tempTouch.position;
        ped.position = new Vector3(pos.x, pos.y, 0.0f);//get world position.
        List<RaycastResult> results = new List<RaycastResult>(); // 여기에 히트 된 개체 저장 
        gr.Raycast(ped, results);
        return results;
    }
    #endregion


    /// <summary>
    /// dir = 0:Up, 1:Right, 2:Down, 3:Left
    /// </summary>
    /// <param name="dir"></param>
    void MoveCursor(int dir)
    {
        int size = PuzzleManager.instance.currentPuzzleSize;
        float distUnit = 0.5f;//size * (1 / size) * 0.5f;
        Vector3 oldPos = PuzzleManager.instance.cursor.transform.position;
        float newX = oldPos.x + (xDist[dir] * distUnit);
        float newY = oldPos.y + (yDist[dir] * distUnit);
        if ((newX <= size * 0.5f && newX >= distUnit) && (newY <= size * 0.5f && newY >= distUnit))
        {
            PuzzleManager.instance.cursor.GetComponent<CursorController>().MovePosition(new Vector3(newX, newY, oldPos.z));
        }
        else
        {
            if (newX > size * 0.5f) { newX = distUnit; }
            else if (newX < distUnit) { newX = size * 0.5f; }
            else if (newY > size * 0.5f) { newY = distUnit; }
            else if (newY < distUnit) { newY = size * 0.5f; }
            PuzzleManager.instance.cursor.GetComponent<CursorController>().MovePosition(new Vector3(newX, newY, oldPos.z));
        }
    }

    void MoveButtonCheck(List<RaycastResult> results)
    {
        GameObject obj = results[0].gameObject;
        if (obj.name == "Btn_Cover")
        {
            obj.SetActive(false);
            PuzzleManager.instance.cursor.GetComponent<CursorController>().ReadyPosition();
        }
        else
        {
            if(obj.tag == "MoveBtn")
            {
                if (obj.name == "Btn_Up") { MoveCursor(0); }
                else if (obj.name == "Btn_Right") { MoveCursor(1); }
                else if (obj.name == "Btn_Down") { MoveCursor(2); }
                else if (obj.name == "Btn_Left") { MoveCursor(3); }               
            }            
        }
        mouseTime += 0.01f;
        Timer_Move();
    }
    
    void MarkButtonCheck(List<RaycastResult> results) {       
        GameObject obj = results[0].gameObject;
        bool longCheck = checkTime > 0.2f ? true : false;
        if (obj.tag == "CheckBtn")
        {            
            if (obj.name == "Btn_Check")
            {
                 currentBtn = 1;
            }
            else if (obj.name == "Btn_X")
            {
                currentBtn = 2;
            }
            else if (obj.name == "Btn_QMark")
            {
                currentBtn = 3;
            }
            PuzzleManager.instance.cursor.GetComponent<CursorController>().CheckOn(currentBtn,checkTime); 
            
        }        
        Timer_Mark(0.01f);
    }

    void MarkButtonOutCheck(List<RaycastResult> results)
    {
        GameObject obj = results[0].gameObject;
        if (obj.tag == "CheckBtn")
        {         
            PuzzleManager.instance.cursor.GetComponent<CursorController>().CheckOut();
        }        
    }

    public void HintButtonClicked()
    {
        if (PuzzleManager.instance.DrawEnd == true)
        {
            //Debug.Log(UserManager.Instance.currentUser.name);
            if (UserManager.Instance.currentUser.name == "nedcrow")// || UserManager.Instance.currentUser.name == "test")
            {
                PuzzleManager.instance.GetComponent<ClearChecker>().OnGoal();
            }
            else {
                if(hintButton.transform.GetChild(2).GetComponent<ButtonEffect>().coolDownMode == false)
                {
                    PuzzleManager.instance.hintMode = true;
                    PuzzleManager.instance.hintCount = 10;
                    hintButton.transform.GetChild(1).GetComponent<Text>().text = "10";
                    hintButton.GetComponent<Image>().color = new Vector4(0.8f, 0.7f, 0.2f, 1);
                    hintButton.transform.GetChild(0).gameObject.SetActive(true);
                    hintButton.transform.GetChild(0).GetComponent<ButtonEffect>().SideLight(36, 9);

                    AdMobManager.instance.ShowInterstitialAd();
                }
            }                       
        }
    }

    public void HintTileCheck() {
        PuzzleManager.instance.hintCount--;
        hintButton.transform.GetChild(1).GetComponent<Text>().text = PuzzleManager.instance.hintCount.ToString();
        if (PuzzleManager.instance.hintCount <= 0)
        {
            PuzzleManager.instance.hintMode = false;
            HintModeOut(false);
        }
    }

    public void HintModeOut(bool first)
    {
        hintButton.GetComponent<Image>().color = new Vector4(0.5f, 0.5f, 0.5f, 1);
        hintButton.transform.GetChild(1).GetComponent<Text>().text = "!";
        hintButton.transform.GetChild(0).gameObject.SetActive(false);

        if (first == false)
        {
            hintButton.transform.GetChild(2).gameObject.SetActive(true);
            hintButton.transform.GetChild(2).GetComponent<ButtonEffect>().CoolDownButton(60);
        }       
    }

    void Timer_Move() {
        if (mouseTimeGrade == 0)
        {
            mouseTime = 0;
            mouseTimeGrade++;
        }
        else
        {
            if(mouseTimeGrade >= mouseTimeGradeCount)
            {
                mouseTime = mouseFirstTime - 0.01f; //0
            }
            else
            {
                mouseTime = mouseFirstTime - waitTime;
                mouseTimeGrade++;
            }
            if(mouseTime <= 0) { Debug.Log("Error : too much big minus time"); }
        }
    }

    void Timer_Mark(float time) {
        checkTime = checkTime + time;
    }

    void Timer_Close(List<RaycastResult> results)
    {
        if (results[0].gameObject.tag == "MoveBtn")
        {
            checkTime = 0;
        }
        else
        {
            mouseTime = 0;
        }
    }

}
