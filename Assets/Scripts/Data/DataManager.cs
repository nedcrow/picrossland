using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System;

//using Firebase;
//using Firebase.Database;
//using Firebase.Unity.Editor;

namespace DataManager
{

    /// <summary>
    /// 경로 지정 후 함수에 따른 데이터 불러오기.
    /// </summary>
    public class Load : MonoBehaviour
    {
        public static void LoadUserData(string path) // or int
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                if (di.Exists) {
                    string FileName = "UserData";
                    FileInfo fi = new FileInfo(path + FileName);
                    if (fi.Exists)
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        FileStream fs = new FileStream(path + FileName, FileMode.OpenOrCreate, FileAccess.Read);

                        var LoadFile = JsonUtility.FromJson<SaveData.User>((string)bf.Deserialize(fs));

                        fs.Close();
                    }
                    else { UserManager.Instance.DefaultSetting(); }//load 필요없음.
                }
                else{ UserManager.Instance.DefaultSetting(); }//load 필요없음.
            }
            catch (Exception e)
            {
                
            }
        }
    }

    /// <summary>
    /// 경로 지정 후 함수에 따른 데이터 저장. 경로 없으면 새로 만듦. 기존 파일이 있으면 덮어쓰기.
    /// </summary>
    public class Save : MonoBehaviour
    {

        public void FireBaseVerCheck()
        {
            //Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            //    var dependencyStatus = task.Result;
            //    if (dependencyStatus == Firebase.DependencyStatus.Available)
            //    {
            //        // Set a flag here indiciating that Firebase is ready to use by your
            //        // application.
            //    }
            //    else
            //    {
            //        UnityEngine.Debug.LogError(System.String.Format(
            //          "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            //        // Firebase Unity SDK is not safe to use here.
            //    }
            //});
        }
    }

    public class Puzzle : MonoBehaviour
    {
        public static void poolGameObjectInList(GameObject targetObj, int targetCount, List<GameObject> list)
        {
            for (int i = 0; i < targetCount; i++)
            {
                list.Add((GameObject)Instantiate(targetObj, new Vector3(0, 0, 0), Quaternion.identity) as GameObject);
                list[i].SetActive(false);
            }
        }
    }

    
}
