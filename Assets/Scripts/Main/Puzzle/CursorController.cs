using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

    public AudioClip tick;
    GameObject[] lineObjs = new GameObject[4]; //Up,Right,Down,Left
    Sprite[] tiles;

    public bool ready;
    int posForChild;
    Vector2 cursorPos;

    private void Awake()
    {        
        ready = false;
        for (int i=0; i< lineObjs.Length; i++)
        {
            lineObjs[i] = transform.GetChild(i).gameObject;
            lineObjs[i].GetComponent<SpriteRenderer>().color = new Vector4(1,0.8f,0.6f,0.5f); //line color
        }
        Sprite[] tileSprites = Resources.LoadAll<Sprite>("Sprite/Tile");//currentPuzzleID
        tiles = new Sprite[4]{ tileSprites[3], tileSprites[0], tileSprites[2], tileSprites[1] };
        targetList = new List<GameObject>();

        tick = Resources.Load<AudioClip>("SFX/cursor_tick");
    }

    public void ReadyPosition()
    {
        MovePosition(new Vector3(0.5f, PuzzleManager.instance.currentPuzzleSize * 0.5f, -1));
        ready = true;
        Debug.Log("ready");        
    }

    private AudioSource source;
    public void MovePosition(Vector3 pos)
    {
        transform.position = pos;
        posForChild = CheckPosForChild();
        OnLine();
                
        source = GetComponent<AudioSource>();
        source.PlayOneShot(tick, 1);
    }

    int firstCheck = 0;
    List<GameObject> targetList; 
    public void CheckOn(int num, float checkTime) {
        posForChild = CheckPosForChild();
        GameObject target = PuzzleManager.instance.tileGroup_Active.transform.GetChild(posForChild).gameObject;
        //Debug.Log(target.transform.position+", "+  target.GetComponent<TileController>().check+ " / targetList Count : " + targetList.Count);
        int sametile = 0;        
        bool possible=false;
        if (targetList.Count > 0)
        {
            for (int i = 0; i < targetList.Count; i++)
            {
                if (target.transform.position == targetList[i].transform.position)
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
            targetList.Add(target);
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
        PuzzleManager.instance.puzzleKeyBrush.Bingo(cursorPos);
        PuzzleManager.instance.gameObject.GetComponent<ClearChecker>().clearCheck();
    }//방문한 타일들을 기록하고 첫 방문 타일에만 체크.

    public void CheckOut()
    {
        targetList = new List<GameObject>();
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

    int CheckPosForChild()
    {
        int size = PuzzleManager.instance.currentPuzzleSize;
        float unUnit = 2;// = 1/0.5f
        int x = System.Convert.ToInt32( transform.position.x*unUnit-1 );
        int y = System.Convert.ToInt32( transform.position.y*unUnit-1 );
        cursorPos = new Vector2(x,y);        
        return x * size + y; ;
    }

    // ------ sound Test ------ //

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            source = GetComponent<AudioSource>();
            source.PlayOneShot(tick, 1);
        }
    }
}
