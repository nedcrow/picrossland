using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleInfo
{
    public class FindPuzzlePos
    {
        static Vector3[][] normalPuzzlePos_A ={
                 new Vector3[]{ Vector3.zero, new Vector3(400, 900, 0), Vector3.zero, new Vector3(600, 780, 0), new Vector3(650, 530, 0), new Vector3(460, 850, 0), new Vector3(-100, -100, 0), Vector3.zero, new Vector3(320, 750, 0), new Vector3(-100, 0, 0), new Vector3(170, 880, 0) }, //11 (s : 1,3,8)
                 new Vector3[]{ Vector3.zero, new Vector3(150, 650, 0), new Vector3(360, 620, 0), new Vector3(570, 650, 0), Vector3.zero, new Vector3(460, 850, 0), new Vector3(-100, -100, 0), new Vector3(320, 750, 0), Vector3.zero,  new Vector3(-100, 0, 0), Vector3.zero}, //11 (s : 1,5,8,11)
                 new Vector3[]{ Vector3.zero, new Vector3(-100, 0, 0), new Vector3(-100, 0, 0), Vector3.zero, new Vector3(-100, 0, 0), new Vector3(-100, 0, 0), Vector3.zero, new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),},
                 new Vector3[]{ Vector3.zero, new Vector3(-100, 0, 0) }
            };//A[land][puzzle] day

        static Vector3[][] normalPuzzlePos_B ={
                 new Vector3[]{ Vector3.zero, new Vector3(400, 900, 0), Vector3.zero, new Vector3(-100, 0, 0) , new Vector3(650, 530, 0) , new Vector3(460, 850, 0) , new Vector3(550, 1100, 0) , Vector3.zero, new Vector3(320, 750, 0), new Vector3(-100, 0, 0), new Vector3(-100, 0, 0)   },
                 new Vector3[]{ Vector3.zero, new Vector3(-100, 0, 0), new Vector3(-100, 0, 0), Vector3.zero, new Vector3(-100, 0, 0), new Vector3(-100, 0, 0), Vector3.zero, new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),},
                 new Vector3[]{ Vector3.zero, new Vector3(-100, 0, 0) },
                 new Vector3[]{ Vector3.zero, new Vector3(-100, 0, 0) }
            };//B night

        static Vector3[][] normalPuzzlePos_C ={
                 new Vector3[]{ Vector3.zero, new Vector3(400, 900, 0), Vector3.zero, new Vector3(-100, 0, 0) , new Vector3(650, 530, 0) , new Vector3(460, 850, 0) , new Vector3(-100, 0, 0) , Vector3.zero, new Vector3(320, 750, 0), new Vector3(550, 1120, 0), new Vector3(-100, 0, 0)   },
                 new Vector3[]{ Vector3.zero, new Vector3(-100, 0, 0), new Vector3(-100, 0, 0), Vector3.zero, new Vector3(-100, 0, 0), new Vector3(-100, 0, 0), Vector3.zero, new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),},
                 new Vector3[]{ Vector3.zero, new Vector3(-100, 0, 0) },
                 new Vector3[]{ Vector3.zero, new Vector3(-100, 0, 0) }
            };//C

        static Vector3[][] normalPuzzlePos_D ={
                 new Vector3[]{ Vector3.zero, new Vector3(400, 900, 0), Vector3.zero, new Vector3(-100, 0, 0) , new Vector3(650, 530, 0) , new Vector3(460, 850, 0) , new Vector3(-100, 0, 0) , Vector3.zero, new Vector3(320, 750, 0), new Vector3(200, 1120, 0), new Vector3(-100, 0, 0)   },
                 new Vector3[]{ Vector3.zero, new Vector3(-100, 0, 0), new Vector3(-100, 0, 0), Vector3.zero, new Vector3(-100, 0, 0), new Vector3(-100, 0, 0), Vector3.zero, new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),new Vector3(-100, 0, 0),},
                 new Vector3[]{ Vector3.zero, new Vector3(-100, 0, 0) },
                 new Vector3[]{ Vector3.zero, new Vector3(-100, 0, 0) }
            };//D

        /// <summary>
        ///  normalPuzzlePos[Weather][Land][Puzzle]
        /// </summary>
        static Vector3[][][] normalPuzzlePos;        

        public static Vector3 puzzlePos(string puzzleID, int weather) {
            normalPuzzlePos = new Vector3[][][]{ normalPuzzlePos_A, normalPuzzlePos_B, normalPuzzlePos_C, normalPuzzlePos_D };//weather기준으로 변함. [Weather][Land][Puzzle]
            Vector3 tPos = new Vector3(-700, -1400, 0);           

            int _landID = System.Convert.ToInt32(HarimTool.EditValue.EditText.Left(puzzleID, 2));
            int _puzzleID = System.Convert.ToInt32(HarimTool.EditValue.EditText.Right(puzzleID, 2));

            Debug.Log("puzzleID : " +puzzleID + ", weather :"+weather);
            if(PuzzleManager.instance.puzzles[_landID-1][_puzzleID-1].type == "N")
            {
                if (_landID <= normalPuzzlePos[weather].Length)
                {
                    if (_puzzleID <= normalPuzzlePos[weather][_landID - 1].Length) { tPos = normalPuzzlePos[weather][_landID - 1][_puzzleID - 1]; }
                    else { Debug.Log("PuzzleID Bigger Then 'pos[LandIID][].length'."); }
                }
                else
                {
                    Debug.Log("LandID Bigger Then 'pos[].length'.");
                }
            }
            
            return tPos;
        }// weather에 따라 위치가 바뀔 수 있음.

    }   

    public class FindPuzzleEffect
    {
        static string[][] weatherEffects = {
            new string[] {"0101","0201" },
          new string[] {"0103","0204" },
          new string[] {"0108" },
          new string[] {"0309" }
        };

        public static int FindWeatherNum(string puzzleID)
        {
            bool findS = false;
            int weather=0;
            for (int i=0; i<4; i++)
            {
                foreach (string x in weatherEffects[i])
                {
                    if (puzzleID == x) { weather = i; }
                }
            }
            return weather;
        }// [x][y]일때, puzzleID가 들어있는 배열의 순서, 즉 x가 weather.(0~3)

        public static GameObject FindPrefab(string targetID)
        {            
            if(Resources.Load<GameObject>("Prefabs/Units/" + targetID)){
                GameObject target = Resources.Load<GameObject>("Prefabs/Units/" + targetID);                
                return target;
            }
            else
            {
                Debug.Log("not found targetID : "+ targetID);
                GameObject none = new GameObject();
                none.name = "noneTargetPrefab";
                return none;
            }
            
        }
    }


}

