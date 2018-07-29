using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLoader : MonoBehaviour {

    PuzzleManager puzzleM;
    float tileUnit = 0.5f;
    public void SetPuzzle(string puzzleID)
    {
        puzzleM = GetComponent<PuzzleManager>();        

        int puzzlePosX = System.Convert.ToInt32(HarimTool.EditText.EditText.Left(puzzleID, 2)); 
        int puzzlePosY = System.Convert.ToInt32(HarimTool.EditText.EditText.Right(puzzleID, 2));

        #region currentPuzzle
        //DataBase.Puzzle target = puzzleM.puzzles[puzzlePosX][puzzlePosY];
        //puzzleM.currentPuzzle = target;
        #endregion

        DrawLine(puzzleM.currentPuzzleSize);
        StartCoroutine(DrawTiles(puzzleM.currentPuzzleSize));       //currentPuzzleSize
        
    }

    void DrawLine(int size)
    {
        float count = size * 0.2f - 1;//필요라인 수 /2
        float addPoint = 0.25f;
        for (int i=0; i<count; i++)
        {
            puzzleM.lines.transform.GetChild(i).transform.position = new Vector3(size * 0.25f+addPoint, ((i + 1) * 2.5f) + addPoint, -2);
            puzzleM.lines.transform.GetChild(i).transform.localScale = new Vector3(size, 0.16f, 1);
            puzzleM.lines.transform.GetChild(puzzleM.lines.transform.childCount - i-1).transform.position = new Vector3((i + 1) * 2.5f + addPoint, size * 0.25f + addPoint, -2);
            puzzleM.lines.transform.GetChild(puzzleM.lines.transform.childCount - i-1).transform.localScale = new Vector3(0.16f, size, 1);
        }

    }

    #region DrawTile
    IEnumerator DrawTiles(int x) {
        GameObject tiles = GetComponent<PuzzleManager>().tileGroup_Rest;
        float half;
        float bMark;
        int count;

        half = Half(x);
        Camera.main.transform.position = Camera.main.GetComponent<CameraController>().CameraPos_Puzzle(x,tileUnit);
        Camera.main.orthographicSize = x*0.53f;

        for (int i = 0; i < half; i++)
        {
            bMark = half - i; //5-i,10-i or 7-i 12-i

            count = Count(x, i);
            for (int j = 0; j < count; j++)
            {
                Vector3[] poss = new Vector3[]{
                        new Vector3(bMark * tileUnit, (bMark + j) * tileUnit, 0),//왼쪽
                        new Vector3((bMark+count-1) * tileUnit, (bMark + j) * tileUnit, 0),//오른쪽
                        new Vector3((bMark+j)*tileUnit,bMark * tileUnit, 0),//아랫줄,
                        new Vector3((bMark+j) * tileUnit, (bMark+count-1) * tileUnit, 0)//윗줄                        
                    };
                for (int k = 0; k < 4; k++)
                {
                    if (k == 0)
                    {
                        if (j == 0 || j == count - 1) { k += 2; }
                        if (i==0 && (x == 15 || x == 25)) { k += 1; }
                    }
                    tiles.transform.GetChild(tiles.transform.childCount - 1).GetComponent<TileController>().TileEffect();
                    tiles.transform.GetChild(tiles.transform.childCount - 1).position = poss[k];
                    tiles.transform.GetChild(tiles.transform.childCount - 1).SetParent(GetComponent<PuzzleManager>().tileGroup_Active.transform);
                }
            }
            yield return new WaitForSeconds(0.025f);
        }        
        yield return new WaitForSeconds(1f);
        ReDrawTiles();
        //GetComponent<AutoPuzzle>().AutoWrite();
        yield return null;
    }

    void ReDrawTiles()
    {
        GameObject tiles = GetComponent<PuzzleManager>().tileGroup_Active;

        for (int i = 0; i < puzzleM.currentPuzzleSize; i++)
        {
            for (int j = 0; j < puzzleM.currentPuzzleSize; j++)
            {
                tiles.transform.GetChild((i * puzzleM.currentPuzzleSize) + j).transform.position = new Vector3((i + 1) * tileUnit, (j + 1) * tileUnit, 0);
                tiles.transform.GetChild((i * puzzleM.currentPuzzleSize) + j).GetComponent<TileController>().pos = new Vector2(i, j);
            }
        }
        puzzleM.viewCon.puzzleView.transform.GetChild(1).GetComponent<PuzzleButton>().OnButtonChecker_Puzzle();
        puzzleM.DrawEnd = true;
    }
    #endregion

    float Half(int x) {
        float half=10;
        if (x == 10 || x == 20 || x == 30)
        {
           half = x * 0.5f;
        }//짝수
        else
        {
            half = x * 0.5f + 0.5f;
        }//홀수
        return half;
    }

    int Count(int x, int num)
    {
        int count;
        if (x == 10 || x == 20 || x == 30)
        {
            count = 2 * (num + 1); //2,4,6,8,10            
        }//짝수
        else
        {
            count = (2 * num) + 1; //1,3,5,7,9
        }//홀수
        return count;
    }

    // 101 102 201 202 
}
