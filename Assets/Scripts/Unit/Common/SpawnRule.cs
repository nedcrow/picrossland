using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpawnRule
{
    public class SpawnPossible
    { 
        static Vector4[][] unitArea = new Vector4[][]{
            new Vector4[]{new Vector4(-0.34f, 0.74f, -0.8f, 0.2f), new Vector4(-1.5f, -0.5f, 0.4f, 1.4f), new Vector4(0.15f, 2.15f, -2f, 0f) }, //tree(0.5), wolf(0.5), house(1)
            new Vector4[]{new Vector4(-1, 1, -1, 1), new Vector4(-1, 1, -1, 1) }
        };//vector4 [landNums][unitAreas]


        public static bool UnitSpawnPossibe(string unitID, Vector3 unitPos)
        {
            bool possible=false;
            int okCount=0;

            int landNum = System.Convert.ToInt32(HarimTool.EditValue.EditText.Left(unitID, 2));
            float x = unitPos.x;
            float y = unitPos.y;

//            Debug.Log(unitPos);
            for (int i=0; i<unitArea[landNum].Length; i++)
            {
                if ((x < unitArea[landNum][i].x || x > unitArea[landNum][i].y) && (y < unitArea[landNum][i].z || x > unitArea[landNum][i].w)) {
                    okCount++;
                }// 비교순서 >Left, <Right, >Bottom, <Up
            }
            if (okCount == unitArea[landNum].Length) { possible = true; }
            return possible;
        } // unitArea 밖에 배치되면 True를 반환.



    }

}
