using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;

public class PuzzleKeyBrush : MonoBehaviour {

    public GameObject Keys_Rest;
    public GameObject Keys_ActiveH;
    public GameObject Keys_ActiveW;

    GameObject keyTile;

    private void Awake()
    {
        LoadKeySample(300);
    }
    

    public void DrawPuzzleKey(){

        TakeOutKey(PuzzleManager.instance.heightKeyList,"H");
        TakeOutKey(PuzzleManager.instance.widthKeyList,"W");
    }


    public void Bingo(Vector2 tilePos)
    {
        int x = System.Convert.ToInt32(tilePos.x);
        int y = System.Convert.ToInt32(tilePos.y);     

        FindBingo(x, "H", "A"); //x면 세로
        FindBingo(x, "H", "B");
        FindBingo(y, "W", "A");
        FindBingo(y, "W", "B");
        //Debug.Log("bingo");
    }//한줄씩 4번

    /// <summary>
    /// case1.가장 가까이 연속으로 check된 경우 해당 key색 회색. case2.전부 check되어도 회색. 
    /// </summary>
    /// <param name="point">x or y poistion</param>
    /// <param name="target">Keys_ActiveH or Keys_ActiveW</param>
    /// <param name="type">A or B</param>
    void FindBingo(int point, string dir, string type)
    {        
        int size = PuzzleManager.instance.currentPuzzleSize;
        int count = 0;
        int xCount = 0;
        int childNum=0;
        string keyNum;
        List<string[]> targetList;
        GameObject targetGroup;
        bool checkStart = false;

        targetList = dir == "H" ? PuzzleManager.instance.heightKeyList : PuzzleManager.instance.widthKeyList;
        targetGroup = dir == "H" ? Keys_ActiveH : Keys_ActiveW;        
        keyNum = type == "A" ? targetList[point][0] : targetList[point][targetList[point].Length - 1]; //각 행열의 시작과 끝 key 
       
        #region num1 
        int num1 = 0; 
        int loop = type == "A" ? point : point + 1; //역방향이 +1
        for (int j = 0; j < loop; j++)
        {
            num1 = num1 + targetList[j].Length;
        }
        num1 = type == "A" ? num1 : num1 - 1; //역방향이 -1
        #endregion //hierarchy에서 (각 방향에서 처음만나는) key의 순서

        #region num2
        int num2 = 0; 
        for (int i = 0; i < point; i++)
        {
            num2 = num2 + targetList[i].Length;
        }
        #endregion // hierarchy에서 target key의 각 행열 순서

        #region case1       

        for (int i=0; i<targetList[point].Length; i++)
        {
            KeyColorChange(keyNum, targetGroup.transform.GetChild(num2+i).gameObject); //key 색상 기본값 복원(초기화)
        }
        
        for (int i = 0; i < size; i++)
        {
            if(dir == "H")
            {
                childNum = type == "A" ? point * size + i : point * size + (size - i - 1); //BottomStart or LeftStart : UpStart or RightStart
            }
            else
            {
                childNum = type == "A" ? i * size + point : (size - i - 1) * size + point;
            }

            if (PuzzleManager.instance.tileGroup_Active.transform.GetChild(childNum).GetComponent<TileController>().check == 2)
            {
                xCount++;
            }//check타일이 나올 때 까지 x타일 갯수를 쌓는다.
            if (PuzzleManager.instance.tileGroup_Active.transform.GetChild(childNum).GetComponent<TileController>().check == 1)
            {
                if (i == 0 || xCount == i)
                {
                    checkStart = true;
                }
                if (checkStart == true)
                {
                    count++;
                }                    
                else { i = size; }
            }//check타일이 연속된 만큼 기록.
            else
            {
                checkStart = false;
            }

            if (checkStart == true && (i == size - 1 || PuzzleManager.instance.tileGroup_Active.transform.GetChild(childNum).GetComponent<TileController>().check != 1))
            {
                #region TestDebug
                if (dir == "H")
                {
                    if (type == "A") { Debug.Log("체크 : " +count.ToString() + ", 시작 : " + keyNum);  }
                    else { Debug.Log("체크 : " + count.ToString() + ", 끝 : " + keyNum); }
                }
                else
                {
                    if (type == "A") { Debug.Log("체크 : " + count.ToString() + ", 시작 : " + keyNum); }
                    else { Debug.Log("체크 : " + count.ToString() + ", 끝 : " + keyNum); }
                }
                #endregion
                
                if (count.ToString() == keyNum)
                {
                    targetGroup.transform.GetChild(num1).GetChild(0).GetComponent<Text>().color = Color.gray;                    
                }
                i = size; count = 0; checkStart = false;
            }
        }
        #endregion

        #region case2
        count = 0;
        List<int> tempCountList = new List<int>();
        for (int i = 0; i < size; i++)
        {
            if(dir == "H")
            {
                childNum = type == "A" ? point * size + i : point * size + (size - i - 1);
            }
            else
            {
                childNum = type == "A" ? i*size + point : (size - i - 1)*size + point;
            }


            if (PuzzleManager.instance.tileGroup_Active.transform.GetChild(childNum).GetComponent<TileController>().check == 1)
            {
                checkStart = true;
                if (checkStart == true)
                {
                    count++;
                }
            }            
            if (checkStart == true )
            {
                if (i == size - 1 || PuzzleManager.instance.tileGroup_Active.transform.GetChild(childNum).GetComponent<TileController>().check != 1)
                {
                    tempCountList.Add(count);
                    count = 0;
                    checkStart = false;
                }
            }
        }        

        if (tempCountList.Count == targetList[point].Length)
        {            
            int tempGoal=0;
            for (int j = 0; j < targetList[point].Length; j++) {                
                if (dir == "H") {
                    if (tempCountList[j].ToString() == targetList[point][j]) { tempGoal++; }
                }
                else {
                    if (tempCountList[j].ToString() == targetList[point][targetList[point].Length-1-j]) { tempGoal++; }
                }
            }
            if(tempGoal == targetList[point].Length) {
                for (int j = 0; j < targetList[point].Length; j++)
                {                    
                    targetGroup.transform.GetChild(num2 + j).GetChild(0).GetComponent<Text>().color = Color.gray;                    
                }                    
            }
        }
        #endregion

    }

    void TakeOutKey(List<string[]> keyList, string dir) {
        int size = PuzzleManager.instance.currentPuzzleSize;
        float lineSpace =  600 / (size);//Line Space
        float fontSize = 18; // 7+(22 - (size * 0.8f));
        float fontSpace = fontSize * 1.05f;
        for (int i = 0; i < PuzzleManager.instance.currentPuzzleSize; i++)//PuzzleManager.Instance.currentPuzzle.x
        {
            for (int j = 0; j < keyList[i].Length; j++)
            {
                string key;
                key = keyList[i][keyList[i].Length - 1 - j];                
                GameObject temp = Keys_Rest.transform.GetChild(Keys_Rest.transform.childCount - 1).gameObject;
                temp.SetActive(true);               

                if (!temp.transform.GetChild(0).GetComponent<Text>()) { temp.transform.GetChild(0).gameObject.AddComponent<Text>(); }
                
                temp.transform.GetChild(0).GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                temp.transform.GetChild(0).GetComponent<Text>().fontSize = Mathf.FloorToInt(fontSize);

                if (dir == "H") {
                    key = keyList[i][j];
                    temp.transform.SetParent(Keys_ActiveH.transform);
                    temp.GetComponent<RectTransform>().localPosition = new Vector3((i * lineSpace)+(lineSpace*0.5f), (j * fontSpace), 0);                    
                }
                else {
                    temp.transform.SetParent(Keys_ActiveW.transform);
                    temp.GetComponent<RectTransform>().localPosition = new Vector3((-j * fontSpace), (i * lineSpace)+(lineSpace*0.5f), 0);
                }
                KeyColorChange(key, temp);
                temp.transform.GetChild(0).GetComponent<Text>().text = key;
            }            
        }
    }

    void KeyColorChange(string key, GameObject target)
    {
        if (System.Convert.ToInt32(key) == 0) { target.transform.GetChild(0).GetComponent<Text>().color = Color.gray; }
        else if (System.Convert.ToInt32(key) < 10) { target.transform.GetChild(0).GetComponent<Text>().color = new Vector4(0.6f,1.0f,0.7f,1.0f); }
        else if (System.Convert.ToInt32(key) < 20) { target.transform.GetChild(0).GetComponent<Text>().color = Color.yellow; }
        else if (System.Convert.ToInt32(key) < 30) { target.transform.GetChild(0).GetComponent<Text>().color = new Vector4(1.0f,1.0f,0.5f,1.0f); }
        else if (System.Convert.ToInt32(key) == 30) { target.transform.GetChild(0).GetComponent<Text>().color = Color.red; }
       
    }

    void LoadKeySample(int count)
    {
        keyTile = Resources.Load<GameObject>("Prefabs/KeySample");

        for(int i=0; i<count; i++)
        {
            GameObject temp = Instantiate(keyTile, Vector3.zero, Quaternion.identity);
            temp.transform.SetParent(Keys_Rest.transform);
            temp.SetActive(false);
        }
    }
}
