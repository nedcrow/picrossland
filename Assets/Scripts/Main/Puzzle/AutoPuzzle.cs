using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPuzzle : MonoBehaviour {

    List<KeyList> tempKeyListHA = new List<KeyList>();// HA,HB로 나눠야함.
    List<KeyList> tempKeyListHB = new List<KeyList>();
    List<KeyList> tempKeyListWA = new List<KeyList>();
    List<KeyList> tempKeyListWB = new List<KeyList>();


    int currentSize;
    public void AutoWrite()
    {
        CheckA(PuzzleManager.instance.heightKeyList, "H");
        DrawA(tempKeyListHA, tempKeyListHB, "H");

        CheckA(PuzzleManager.instance.widthKeyList, "W");
        DrawA(tempKeyListWA, tempKeyListWB, "W");

        CheckB(tempKeyListHA, "A", "H");
        CheckB(tempKeyListHB, "B", "H");
        CheckB(tempKeyListWA, "A", "W");
        CheckB(tempKeyListWB, "B", "W");
    }

    void CheckA( List<string[]> targetList, string dir ){
        currentSize = PuzzleManager.instance.currentPuzzleSize;    
        for (int i = 0; i < targetList.Count; i++)
        {
            KeyList A = new KeyList();
            KeyList B = new KeyList();
            A.midKeyList = new List<string[]>();
            B.midKeyList = new List<string[]>();

            int beforCountA = 0;
            int beforCountB = 0;
            for (int j = 0; j < targetList[i].Length; j++)//string[]
            {
                int countA;
                int countB = System.Convert.ToInt32(targetList[i][targetList[i].Length-1-j]);
                string[] tempKeyA = new string[currentSize];               
                string[] tempKeyB = new string[currentSize];
                tempKeyA = Reset(tempKeyA);//false로 도배
                tempKeyB = Reset(tempKeyB);//false로 도배

                int xA = 0;
                int xB = 0;
                xA = beforCountA;
                xB = beforCountB;

                #region tempKeys
                countA = System.Convert.ToInt32(targetList[i][j]);
                for (int k = 0; k < countA; k++) { tempKeyA[k + xA] = "T"; }
                A.midKeyList.Add(tempKeyA);
                //Debug.Log(tempListA[tempListA.Count-1][0]+", "+ tempListA[tempListA.Count - 1][1] + ", " + tempListA[tempListA.Count - 1][2] + ", " + tempListA[tempListA.Count - 1][3] + ", " + tempListA[tempListA.Count - 1][4] + ", " + tempListA[tempListA.Count - 1][5] + ", " + tempListA[tempListA.Count - 1][6] + ", " + tempListA[tempListA.Count - 1][7] + ", " + tempListA[tempListA.Count - 1][8] + ", " + tempListA[tempListA.Count - 1][9]);
                for (int k = 0; k < countB; k++) { tempKeyB[currentSize - 1 - (k + xB)] = "T"; }
                B.midKeyList.Add(tempKeyB);
                #endregion

                beforCountA = beforCountA + countA + 1;
                beforCountB = beforCountB + countB + 1;               
            }            
            //Debug.Log(tempListA[tempListA.Count-1][0]+", "+ tempListA[tempListA.Count - 1][1] + ", " + tempListA[tempListA.Count - 1][2] + ", " + tempListA[tempListA.Count - 1][3] + ", " + tempListA[tempListA.Count - 1][4] + ", " + tempListA[tempListA.Count - 1][5] + ", " + tempListA[tempListA.Count - 1][6] + ", " + tempListA[tempListA.Count - 1][7] + ", " + tempListA[tempListA.Count - 1][8] + ", " + tempListA[tempListA.Count - 1][9]);
            if(dir == "H")
            {
                tempKeyListHA.Add(A);
                tempKeyListHB.Add(B);
            }
            else
            {
                tempKeyListWA.Add(A);
                tempKeyListWB.Add(B);
            }            
        }        
    }

    /// <summary>
    /// H, W로 한 세트씩 나눔.
    /// </summary>
    /// <param name="targetList1"></param>
    /// <param name="targetList2"></param>
    void DrawA(List<KeyList> targetList1, List<KeyList> targetList2, string dir)
    {
        for (int i = 0; i < targetList1.Count; i++)// func로 뺄까?
        {
            for (int j = 0; j < targetList1[i].midKeyList.Count; j++)
            {
                //Debug.Log(targetList1[i].midKeyList[j][0] + ", " + targetList1[i].midKeyList[j][1] + ", " + targetList1[i].midKeyList[j][2] + ", " + targetList1[i].midKeyList[j][3] + ", " + targetList1[i].midKeyList[j][4] + ", " + targetList1[i].midKeyList[j][5] + ", " + targetList1[i].midKeyList[j][6] + ", " + targetList1[i].midKeyList[j][7] + ", " + targetList1[i].midKeyList[j][8] + ", " + targetList1[i].midKeyList[j][9]);
                for (int k = 0; k < currentSize; k++)
                {

                    if (targetList1[i].midKeyList[j][k] == targetList2[i].midKeyList[targetList1[i].midKeyList.Count - 1 - j][k] && targetList1[i].midKeyList[j][k] == "T")
                    {
                        if (dir == "H")
                        {
                            PuzzleManager.instance.tileGroup_Active.transform.GetChild(i * currentSize + k).GetComponent<SpriteRenderer>().color = Color.black;
                            PuzzleManager.instance.tileGroup_Active.transform.GetChild(i * currentSize + k).GetComponent<TileController>().check = 1;
                            PuzzleManager.instance.tileGroup_Active.transform.GetChild(i * currentSize + k).GetComponent<TileController>().answer = true;
                        }
                        else
                        {
                            PuzzleManager.instance.tileGroup_Active.transform.GetChild(i + currentSize * k).GetComponent<SpriteRenderer>().color = Color.black;
                            PuzzleManager.instance.tileGroup_Active.transform.GetChild(i + currentSize * k).GetComponent<TileController>().check = 1;
                            PuzzleManager.instance.tileGroup_Active.transform.GetChild(i + currentSize * k).GetComponent<TileController>().answer = true;
                        }
                    }
                    #region confirm
                    //if (dir == "H")
                    //{
                    //    if (tempKeyListB[i].midKeyList[j][k] == "T")
                    //    {
                    //        PuzzleManager.Instance.tileGroup_Active.transform.GetChild(i * currentSize + k).GetComponent<SpriteRenderer>().color = Color.black;
                    //    }

                    //}
                    //else
                    //{
                    //    if (tempKeyListA[i].midKeyList[j][k] == "T")
                    //    {
                    //        PuzzleManager.Instance.tileGroup_Active.transform.GetChild(i + currentSize * k).GetComponent<SpriteRenderer>().color = Color.black;
                    //    }
                    //}
                    #endregion
                }
            }
        }
    }

    /// <summary>
    /// targetList, A~B, H~R
    /// </summary>
    /// <param name="targetList"></param>
    /// <param name="dir"></param>
    void CheckB(List<KeyList> targetList, string type, string dir)
    {
        int fromJ;
        for (int i = 0; i < targetList.Count; i++)
        {            
            bool foundSP = false;
            for (int j = 0; j < targetList[i].midKeyList[0].Length; j++)
            {
                if(type == "A")
                {
                    fromJ = j;
                }
                else
                {
                    fromJ = targetList[i].midKeyList[0].Length - j - 1;
                }
                if (targetList[i].midKeyList[0][fromJ] == "T")
                {
                    if (dir == "H")
                    {
                        if (foundSP == false)
                        {
                            if (PuzzleManager.instance.tileGroup_Active.transform.GetChild(i * currentSize + fromJ).GetComponent<TileController>().answer == true) { foundSP = true; Debug.Log(i+", "+ j); }
                        }
                        else
                        {
                            PuzzleManager.instance.tileGroup_Active.transform.GetChild(i * currentSize + fromJ).GetComponent<TileController>().answer = true;
                            PuzzleManager.instance.tileGroup_Active.transform.GetChild(i * currentSize + fromJ).GetComponent<TileController>().check = 1;
                            PuzzleManager.instance.tileGroup_Active.transform.GetChild(i * currentSize + fromJ).GetComponent<SpriteRenderer>().color = Color.black;
                        }
                    }
                    else
                    {
                        if (foundSP == false)
                        {
                            if (PuzzleManager.instance.tileGroup_Active.transform.GetChild(i + currentSize * fromJ).GetComponent<TileController>().answer == true) { foundSP = true; }
                        }
                        else
                        {
                            PuzzleManager.instance.tileGroup_Active.transform.GetChild(i + currentSize * fromJ).GetComponent<TileController>().answer = true;
                            PuzzleManager.instance.tileGroup_Active.transform.GetChild(i + currentSize * fromJ).GetComponent<TileController>().check = 1;
                            PuzzleManager.instance.tileGroup_Active.transform.GetChild(i + currentSize * fromJ).GetComponent<SpriteRenderer>().color = Color.black;
                        }
                    }
                }

            }            
        }
    }

    string[] Reset(string[] target) {
        for (int k = 0; k < target.Length; k++) { target[k] = "F"; }
        return target;
    }
}

class KeyList
{
    public List<string[]> midKeyList;
}
