using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

    [HideInInspector] public AudioClip tick;
    GameObject[] lineObjs = new GameObject[4]; //Up,Right,Down,Left
    Sprite[] tiles;

    private AudioSource source;

    //상태
    public bool ready;
    int firstCheck = 0;

    //위치
    List<int> childNumList = new List<int>(); // for Hint
    int posForChild;
    Vector2 cursorPos;
    List<GameObject> targetTileList;


    private void Awake()
    {        
        ready = false;
        for (int i=0; i< lineObjs.Length; i++)
        {
            lineObjs[i] = transform.GetChild(i).gameObject;
            lineObjs[i].GetComponent<SpriteRenderer>().color = new Vector4(1,0.8f,0.6f,0.5f); //line color
        }
        Sprite[] tileSprites = Resources.LoadAll<Sprite>("Sprite/Tile");//currentPuzzleID
        tiles = new Sprite[4]{ tileSprites[3], tileSprites[0], tileSprites[2], tileSprites[1] }; //check, question, x, empty
        targetTileList = new List<GameObject>();

        tick = Resources.Load<AudioClip>("SFX/cursor_tick");
    }

    public void ReadyPosition()
    {
        MovePosition(new Vector3(0.5f, PuzzleManager.instance.currentPuzzleSize * 0.5f, -1));
        ready = true;
        Debug.Log("ready");        
    }

    
    public void MovePosition(Vector3 pos)
    {
        transform.position = pos;
        posForChild = CheckPosForChild();
        OnLine();
                
        source = GetComponent<AudioSource>();
        source.PlayOneShot(tick, 1);
    }

    public void CheckOn(int num, float checkTime) {
        posForChild = CheckPosForChild();Debug.Log(posForChild);
        GameObject target = PuzzleManager.instance.tileGroup_Active.transform.GetChild(posForChild).gameObject;        //Debug.Log(target.transform.position+", "+  target.GetComponent<TileController>().check+ " / targetList Count : " + targetList.Count);

        int sametile = 0;        
        bool possible=false;
        if (targetTileList.Count > 0)
        {
            for (int i = 0; i < targetTileList.Count; i++)
            {
                if (target.transform.position == targetTileList[i].transform.position)
                {
                    sametile++;
                }
            }
            if(sametile == 0) { possible = true; }
        }
        else
        {
            possible = true;
            firstCheck = target.GetComponent<TileController>().check;
        }

        if(possible == true)
        {
            targetTileList.Add(target);
            bool firstSame = firstCheck == num ? true : false;
            bool targetSame = target.GetComponent<TileController>().check == num ? true : false;
            //Debug.Log("targetList Count : "+ targetList.Count +" / targetCheck : "+ target.GetComponent<TileController>().check + " " + firstSame +", "+ targetSame +" / " + target.transform.position);
            if ((targetSame == true && checkTime<0.01f) || (firstSame == true))
            {
                target.GetComponent<SpriteRenderer>().sprite = tiles[0];
                target.GetComponent<TileController>().check = 0;
            }
            else if(target.GetComponent<TileController>().check == 0)
            {
                target.GetComponent<SpriteRenderer>().sprite = tiles[num];
                target.GetComponent<TileController>().check = num;
            }
        }

        #region Hint
        
        if (PuzzleManager.instance.hintMode == true) {
            bool newHint = true;
            foreach (int childNum in childNumList) { if (childNum == posForChild) { newHint = false; break; } }
            
            if (newHint == true)
            {
                childNumList.Add(posForChild);
                int goal = PuzzleManager.instance.currentGoal[posForChild] == true ? 1 : 0;
                target.GetComponent<TileController>().check = goal;
                if (goal == 1) { target.GetComponent<SpriteRenderer>().sprite = tiles[1]; }
                else { target.GetComponent<SpriteRenderer>().sprite = tiles[2]; }
                PuzzleManager.instance.viewCon.puzzleView.transform.GetChild(1).GetComponent<PuzzleButton>().HintTileCheck();
            }

        }//만약 HintMode면 무조건 정답으로 체크.
        #endregion

        PuzzleManager.instance.puzzleKeyBrush.Bingo(cursorPos);
        PuzzleManager.instance.gameObject.GetComponent<ClearChecker>().clearCheck();
    }//방문한 타일들을 기록하고 첫 방문 타일에만 체크.

    public void CheckOut()
    {
        targetTileList = new List<GameObject>();
    }

    void OnLine()
    {
        int size = PuzzleManager.instance.currentPuzzleSize;

        float x = transform.position.x;
        float y = transform.position.y;
        lineObjs[0].transform.position = new Vector3(x, (size*0.25f)+ (y*2*0.25f)+0.25f, -1); //5.25~0.75 success
        lineObjs[1].transform.position = new Vector3((size * 0.25f) + (x*2*0.25f)+0.25f, y, -1);
        lineObjs[2].transform.position = new Vector3(x, y*0.5f, -1);
        lineObjs[3].transform.position = new Vector3(x * 0.5f, y, -1);

        lineObjs[0].transform.localScale = new Vector3(1,(size)-(y*2f),1);
        lineObjs[1].transform.localScale = new Vector3((size)-(x*2f),1,1);
        lineObjs[2].transform.localScale = new Vector3(1, y*2 - (1), -1);
        lineObjs[3].transform.localScale = new Vector3(x*2-(1),1,1);

    }

    /// <summary>
    /// 커서 위치(x,v)의 타일이 그룹 내 몇번째 타일인지 확인.
    /// </summary>
    /// <returns></returns>
    int CheckPosForChild()
    {//world 기준 0.5f씩 이동.
        int size = PuzzleManager.instance.currentPuzzleSize;
        float unUnit = 2;// = 1/0.5f
        int x = System.Convert.ToInt32( transform.position.x*unUnit-1 );
        int y = System.Convert.ToInt32( transform.position.y*unUnit-1 );
        cursorPos = new Vector2(x,y);        
        return x * size + y; ;
    }

    // ------ sound Test ------ //

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        source = GetComponent<AudioSource>();
    //        source.PlayOneShot(tick, 1);
    //    }
    //}
}
