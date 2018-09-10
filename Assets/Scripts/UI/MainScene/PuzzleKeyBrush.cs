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

        ReSetColors(x,y);

        FindBingo(x, "H", "A"); //x면 세로, A,B는 진행 방향  Bottom > Up
        FindBingo(x, "H", "B");  // Up > Bottom
        FindBingo(y, "W", "A");  // L > R
        FindBingo(y, "W", "B");  // R > L
        //Debug.Log("bingo");
    }//한줄씩 4번

    void ReSetColors(int x, int y) {
        int[] keyLengths = {
            PuzzleManager.instance.heightKeyList[x].Length,
            PuzzleManager.instance.widthKeyList[y].Length
        };
        //Debug.Log(string.Format("PuzzleManager.instance.heightKeyList[x].Length : {0}, PuzzleManager.instance.widthKeyList[x].Length : {1}", keyLengths[0], keyLengths[1]));
        int[] loops = { x, y }; // H : bottom>up , W : right > left     
        int[] addSome = { };
        GameObject[] targetObjs = { Keys_ActiveH, Keys_ActiveW };

        for(int i=0; i<2; i++)
        {
            int num = 0;
            for (int j = 0; j < loops[i]; j++)
            {
                if (i == 0)
                {
                    num = (num + PuzzleManager.instance.heightKeyList[j].Length) ;
                }
                else
                {
                    num = (num + PuzzleManager.instance.widthKeyList[j].Length) ;
                }
            }
          
            for (int j=num; j<num+keyLengths[i]; j++)
            {
                Transform target = targetObjs[i].transform.GetChild(j);
                //Debug.Log(targetObjs[i].transform.GetChild(j).GetChild(0).GetComponent<Text>().text );
                KeyColorChange(target.GetChild(0).GetComponent<Text>().text, target.gameObject);
            }
            //KeyColorChange(keyLengths[i], Keys_ActiveH)
        }
    }//커서 위치를 참조하는 라인 key값을 초기화 

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

        #region num_TargetGroup 
        int num_TargetGroup = 0;
        int zero_H; // H, W로 loop 반전에 사용.
        int loop;
        zero_H = dir == "H" ? 0 : 1;
        loop = type == "A" ? point+zero_H : point+1-zero_H ; //역방향이 반복횟수 +1(한 그룹의 시작과 끝을 찾아야하기 때문에)
        for (int j = 0; j < loop; j++)
        {
            num_TargetGroup = num_TargetGroup + targetList[j].Length;
        }
        if(dir == "H") {
            num_TargetGroup = type == "A" ? num_TargetGroup : num_TargetGroup - 1; //역방향이 -1
        }
        else
        {
            num_TargetGroup = type == "A" ? num_TargetGroup - 1 : num_TargetGroup; //정방향이 -1
        }

        #endregion //hierarchy에서 (각 방향에서 처음만나는) key의 순서

        #region num2
        int num2 = 0; 
        for (int i = 0; i < point; i++)
        {
            num2 = num2 + targetList[i].Length;
        }
        #endregion // hierarchy에서 target key의 각 행열 순서

        #region case1       

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

            int checkNum = PuzzleManager.instance.tileGroup_Active.transform.GetChild(childNum).GetComponent<TileController>().check;
            if (checkNum == 2)
            {
                xCount++;
            }//check타일이 나올 때 까지 x타일 갯수를 쌓는다.
            if (checkNum == 1)
            {
                //Debug.Log("i : " + i+", xCount : "+xCount);
                if (i == 0 || xCount == i)
                {
                    checkStart = true;
                    
                }//첫 칸, 또는 xCount제외하고 첫 칸에 시작.
                if (checkStart == true)
                {
                    count++;
                }                    
                else { i = size; }
            }//첫 칸, 또는 xCount를 제하고 첫 칸일 때, check타일이 연속된 만큼 기록.
            else
            {
                checkStart = false;
            }

            if (count > 0 && checkStart == false)
            {
                #region TestDebug
                if (dir == "H")
                {
                    if (type == "A") { Debug.Log("체크 H : " + count.ToString() + ", 시작 : " + keyNum); }
                    else { Debug.Log("체크 H : " + count.ToString() + ", 끝 : " + keyNum); }
                }
                else
                {
                    if (type == "A") { Debug.Log("체크 W : " + count.ToString() + ", 시작 : " + keyNum); }
                    else { Debug.Log("체크 W : " + count.ToString() + ", 끝 : " + keyNum); }
                }
                #endregion
                Debug.Log("num_TargetGroup = " + num_TargetGroup + "/ Count : " + count + ", keyNum : " +keyNum);
                if (count.ToString() == keyNum)
                {
                    targetGroup.transform.GetChild(num_TargetGroup).GetChild(0).GetComponent<Text>().color = Color.gray;
                    count = 0;
                    break;
                }
            }//keynum이 0이면

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

    }//특정 상황의 Key를 회색으로 교체.

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
