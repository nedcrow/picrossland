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
        puzzleM.heightKeyList.Clear();//초기화
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

            Goal(r);//Debug용
        }    
        //Debug.Log(puzzleM.currentPuzzle.id + ", " + r + ", " + puzzleM.currentSprites.Length);
        puzzleM.currentSprite = puzzleM.currentSprites[r];        
    }//Drawing을 위한 Key제작.

    /// <summary>
    ///  한 줄에서 알파 값이 1이상인 픽셀이 연속될 경우, 연속된 수량을 인자 변수 List<int>에 추가.  
    /// </summary>
    /// <param name="r">Target sprite 순서</param>
    /// <param name="i">r에서 탐색할 라인 순서</param>
    /// <param name="tempList">return값을 담을 List</param>
    /// <param name="dir">줄의 가로, 세로 방향</param>
    void StackKey(int r,  int i, List<int> tempList, string dir) {
        
        int foundPixel = 0;
        int t;

        for (int j = 0; j < currentPuzzleSize; j++) //currentPuzzle.x
        {
            t = (dir == "H") ? FindTrue(r, i, j) : FindTrue(r, j, i);
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
            }// 줄 마지막인 경우
            else
            {
                if (t==0 && foundPixel > 0)
                {
                    tempList.Add(foundPixel);
                    foundPixel = 0;
                }//지금은 픽셀을 못 찾았는데 이전까지 쌓아둔 픽셀이 있으면  tempList에 추가 
            }// 줄 마지막 아닌 경우

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

    /// <summary>
    /// 해당 sprite의 x,y위치 픽셀 alpha값이 1 보다 작으면 0, 이상이면 1 리턴.
    /// </summary>
    /// <param name="spriteNum"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    int FindTrue(int spriteNum, int x, int y)
    {        
        int b;
        Texture2D currentTexture = puzzleM.currentSprites[0].texture; // texture는 무조건 하나.
        int startPointX = spriteNum * currentPuzzleSize; //r*currentPuzzleSize 위치
        int startPointY = (currentTexture.GetPixels().Length / currentTexture.width) - currentPuzzleSize; //r*currentPuzzleSize 위치
        //Debug.Log(currentTexture.GetPixel(startPointX + x, startPointY + y).a);
        if (currentTexture.GetPixel(startPointX + x, startPointY + y).a < 1) { b = 0;  }
        else { b = 1; }
        return b;
    } //for stack key, for create key.

    [SerializeField]
    bool[] goal; //check tiles
    void Goal(int spriteNum)
    {
        Texture2D currentTexture = puzzleM.currentSprites[0].texture;
        int startPointX = spriteNum * currentPuzzleSize;
        int startPointY = (currentTexture.GetPixels().Length / currentTexture.width) - currentPuzzleSize;

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
