using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class TestSave : MonoBehaviour {


    // Use this for initialization
    void Start() {



        /*
        //string save = JsonUtility.ToJson(saveData, prettyPrint: true);

        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/TestSave");
        if (di.Exists == false)
        {
            di.Create();
        }//폴더 없으면 만듦.

        BinaryFormatter bf = new BinaryFormatter();
        if (System.IO.File.Exists(Application.persistentDataPath + "/TestSave/" + "Test" + ".save")) { Debug.Log("double"); }//덮어쓸거야?}
        FileStream fs = new FileStream(Application.persistentDataPath + "/TestSave/" + "Test" + ".save", FileMode.Create);
        bf.Serialize(fs, puzzleList);
        fs.Close();
        */
    }

    public void SaveTest()
    {
        DataBase.Lands testData = new DataBase.Lands();
        List<DataBase.Land> landList = new List<DataBase.Land>();

        for(int i=0; i<5; i++)
        {
            DataBase.Land aa = new DataBase.Land();
            aa.id = 0100+i;
            aa.name = "asd"+i;
           // aa.puzzleList_N = new string[6] { "0101", "0102", "0103", "0104", "0105", "0106" };
           // aa.puzzleList_S = new string[3] { "0101", "0103", "0108" };

            landList.Add(aa);
           
        }
        Debug.Log(landList[0].id);
        testData.lands = landList.ToArray();

        string td = JsonUtility.ToJson(testData);
        Debug.Log(td);

        DataBase.Lands testData2 = JsonUtility.FromJson<DataBase.Lands>(td);
        for (int i=0; i< testData2.lands.Length; i++)
        {
            Debug.Log(testData2.lands[i].name);
        }


        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/TestSave");
        if (di.Exists == false)
        {
            di.Create();
        }//폴더 없으면 만듦.

        BinaryFormatter bf = new BinaryFormatter();
        if (System.IO.File.Exists(Application.persistentDataPath + "/TestSave/" + "Test" + ".save")) { Debug.Log("double"); }//덮어쓸거야?}
        FileStream fs = new FileStream(Application.persistentDataPath + "/TestSave/" + "Test" + ".json", FileMode.Create);
        bf.Serialize(fs, td);
        fs.Close();

        Debug.Log(td);
        
    }

    public int[] mark1;
    public int[] mark2;
    public	void LoadTest()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(Application.persistentDataPath + "/TestSave/" + "Test" + ".save", FileMode.OpenOrCreate, FileAccess.Read);

        //puzzleList2 = (List<puzzle>)bf.Deserialize(fs);
        //var LoadFile = JsonUtility.FromJson<RoomData>((string)bf.Deserialize(fs));
        fs.Close();        
    }
    
}

[Serializable]
class TestObjs
{
    public TestObj[] tos;
}
[Serializable]
class TestObj
{
    public int age;
    public string name;
}
