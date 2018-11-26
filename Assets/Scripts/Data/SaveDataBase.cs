using UnityEngine;
using System;
using System.Collections.Generic;

namespace SaveData
{

    [Serializable]
    public class User
    {
        #region Admin
        public string id;
        public string name;
        public int gem;
        public int star;
        public int ClearLandCount;
        public System.DateTime loginTime;//Hour
        public int PlayTime;//Hour
        public bool[] settingVal;
        #endregion

        #region Lands
        public int lastLand;
        public List<GotLand> gotLandList; //(ID, Weather, gotPuzzleList)
        #endregion

        #region Puzzles
        public string lastpuzzle;
        public IngPuzzle[] ingPuzzleList; // ID, Clear, hint, marks
        #endregion
    }


    [Serializable]
    public class IngPuzzle
    {
        public int id; // 리소스 이름도 퍼즐에 맞춤.

        public int clear; // 0:시작x, 1:미완성, 2:성공

        public bool hint;

        public int[,] marks; //[X*Y] 0:빈칸, 1:체크, :2:?표시
    }

    [Serializable]
    public class GotLand
    {
        public int id;
        public int weather;

        public List<string> gotPuzzleList = new List<string>(); // ID
        public List<string> clearPuzzleList = new List<string>(); // ID
        public List<string> unitList = new List<string>(); // ID
    }

    //public class PlayData
    //{
    //    public bool localConnected;
    //}
}


namespace DataBase {
    [Serializable]
    public class Lands
    {
        public Land[] lands;//ID
    }

    [Serializable]
    public class Land
    {
        public int id;
        public string name;        
        public List<string> puzzleList_S = new List<string>(); 
        public List<string> puzzleList_N = new List<string>();
        public int price;
    }

    [Serializable]
    public class Puzzles
    {
        public string[] puzzles;
    }

    [Serializable]
    public class Puzzle
    {
        public string id; //resource도 동일.
        public string name;
        public int size;
        public int useSpriteNum1;
        public int useSpriteNum2;
        public string type;
        public int spawnCount;
        public int maxCount; //1,3,6,9
    }

    [Serializable]
    public class LocalDB
    {
        public List<Land> landList;
        public Puzzle[][] puzzles;
    }
}






