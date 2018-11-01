using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCreator : MonoBehaviour {

    PuzzleManager puzzleM;
    int currentPuzzleSize;

    private void Start()
    {
        puzzleM = GetComponent<PuzzleManager>();
    }



    public void CreateKey()
    {
        puzzleM.heightKeyList.Clear();
        puzzleM.widthKeyList.Clear();

        int r = Random.Range(puzzleM.currentPuzzle.useSpriteNum1, puzzleM.currentPuzzle.useSpriteNum2+1); //currentPuzzle.useSpriteNum1,2+1 //사용할 스프라이트 선택.  
        currentPuzzleSize = puzzleM.currentPuzzleSize;
        
        for (int i = 0; i < currentPuzzleSize; i++) 
        {

            List<int> tempListH = new List<int>();
            List<int> tempListW = new List<int>();

                StackKey(r, i, tempListH,"H");
                StackKey(r, i, tempListW,"W");

            puzzleM.heightKeyList.Add(InputKey(tempListH));
            puzzleM.widthKeyList.Add(InputKey(tempListW));
            //Debug.Log(i + ", " + puzzleM.heightKeyList.Count + ", " + puzzleM.widthKeyList.Count);
            Goal(r);

        }
        //test(r);
        //Debug.Log(puzzleM.currentPuzzle.id + ", " + r + ", " + puzzleM.currentSprites.Length);
        Debug.Log("puzzleM.currentPuzzle.useSpriteNum1~2 : " + puzzleM.currentPuzzle.useSpriteNum1 +", "+ puzzleM.currentPuzzle.useSpriteNum2);
        puzzleM.currentSprite = puzzleM.currentSprites[r];        
    }

    void StackKey(int r,  int i, List<int> tempList, string dir) {
        
        int foundPixel = 0;
        int t;

        for (int j = 0; j < currentPuzzleSize; j++) //currentPuzzle.x
        {
            t = (dir == "H") ? FindTrue(r, i, j) : FindTrue(r, j, i);// if (dir == "H") { t = FindTrue(r, i, j); } else if (dir == "W") { FindTrue(r, j, i); }
            if ( t == 1)
            {
                foundPixel++;
            }//픽셀을 찾았으면 '찾은수량' 증가

            if (j == currentPuzzleSize - 1)
            {
                if (tempList.Count == 0 || (tempList.Count != 0 && foundPixel > 0)) {
                    tempList.Add(foundPixel);
                } //줄 마지막인데 tempList가 비었거나, 또는 그렇지 않아도 마지막에는 찾은것이 있다면 tempList에 추가
                foundPixel = 0;
            }// 줄 마지막
            else
            {
                if (t==0 && foundPixel > 0)
                {
                    tempList.Add(foundPixel);
                    foundPixel = 0;
                }//지금은 픽셀을 못 찾았는데 이전까지 쌓아둔 픽셀이 있으면  tempList에 추가 
            }// 줄 마지막 아님

        }
    }//for create key

    string[] InputKey(List<int> tempList)
    {
        string [] temps = new string[tempList.Count];
        for (int j = 0; j < temps.Length; j++)
        {
            temps[j] = tempList[j].ToString();
        }
        return temps;
    }//tempList의 key값을 Array로 변환 후 실제 사용할 KeyList에 담는다.


    int FindTrue(int spriteNum, int x, int y)
    {        
        int b;
        Texture2D currentTexture = puzzleM.currentSprites[0].texture; // texture는 무조건 하나.
        int startPointX = spriteNum * currentPuzzleSize; //r*currentPuzzleSize
        int startPointY = (currentTexture.GetPixels().Length / currentTexture.width) - currentPuzzleSize; //r*currentPuzzleSize   
        //Debug.Log(currentTexture.GetPixel(startPointX + x, startPointY + y).a);
        if (currentTexture.GetPixel(startPointX + x, startPointY + y).a < 1) { b = 0;  }
        else { b = 1; }
        return b;
    } //for stack key, for create key.

    [SerializeField]
    bool[] goal;
    void Goal(int spriteNum)
    {
        Texture2D currentTexture = puzzleM.currentSprites[0].texture;
        int startPointX = spriteNum * currentPuzzleSize; //r*currentPuzzleSize
        int startPointY = (currentTexture.GetPixels().Length / currentTexture.width) - currentPuzzleSize; //r*currentPuzzleSize   

        goal = new bool[currentPuzzleSize * currentPuzzleSize];
        for(int i = 0; i< currentPuzzleSize; i++)
        {
            for(int j=0; j<currentPuzzleSize; j++)
            {
                if( currentTexture.GetPixel(startPointX+i, startPointY+j).a < 1) { goal[i * currentPuzzleSize + j] = false; }
                else { goal[i * currentPuzzleSize + j] = true; }
            }
            
        }

        puzzleM.currentGoal = goal;
    }

}
