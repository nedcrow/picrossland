using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class MainDataBase : MonoBehaviour
{
    #region Singleton
    private static MainDataBase _instance;
    public static MainDataBase instance
    {
        get
        {
            if (!_instance)
            {
                GameObject obj = GameObject.Find("LandOfPicross");
                if (obj == null)
                {
                    obj = new GameObject("LandOfPicross");
                    obj.AddComponent<MainDataBase>();
                }
                return obj.GetComponent<MainDataBase>();
            }
            return _instance;
        }        
    }
    #endregion

    string path; // for Local

    #region forFirebasse
    public bool loadAll;
    public bool local;
    bool loadLand;
    bool loadPuzzle;
    bool loadAdmin;

    //--------------nickName-------------- v
    bool notJunk = false; 
    #endregion


    void Awake()
    {
        local = false;
        path = Application.persistentDataPath + "/Save"; // for Local
        //--------------------Set realtime database ----------------------------v
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://land-of-picross-8205260.firebaseio.com/");
        FirebaseApp.DefaultInstance.SetEditorP12FileName("land-of-picross-8205260-62de3e51bc93.p12");
        FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("land-of-picross-8205260@appspot.gserviceaccount.com");
        FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

    }
    #region FireBase


    //public void OnClickTransactionSave()
    //{
    //    const int MaxScoreRecordCount = 5;
    //    int score = Random.Range(0, 100);
    //    string email = "testEmail";

    //    databaseReference.Child("users").RunTransaction(mutableData => {
    //        List<object> leaders = mutableData.Value as List<object>;

    //        if (leaders == null)
    //        {
    //            leaders = new List<object>();
    //        }

    //        // 랭킹에 등록된 점수를 비교합니다.
    //        else if (mutableData.ChildrenCount >= MaxScoreRecordCount)
    //        {
    //            long minScore = long.MaxValue;
    //            object minVal = null;
    //            foreach (var child in leaders)
    //            {
    //                if (!(child is Dictionary<string, object>))
    //                    continue;
    //                long childScore = (long)((Dictionary<string, object>)child)["score"];
    //                if (childScore < minScore)
    //                {
    //                    minScore = childScore;
    //                    minVal = child;
    //                }
    //            }
    //            if (minScore > score)
    //            {
    //                // 현재 점수가 최하위 점수보다 낮으면 중단합니다.(랭킹에 못오르니깐)
    //                return TransactionResult.Abort();
    //            }

    //            // 기존 최하위 데이터를 제거합니다.(랭킹 변경)
    //            leaders.Remove(minVal);
    //        }

    //        Dictionary<string, object> newScoreMap = new Dictionary<string, object>();

    //        newScoreMap["score"] = score;
    //        newScoreMap["email"] = email;

    //        leaders.Add(newScoreMap);
    //        mutableData.Value = leaders;
    //        return TransactionResult.Success(mutableData);
    //    });
    //}

    //public void OnClickRemove()
    //{
    //    databaseReference.Child("users").Child("노드이름").Child("노드이름")
    //                   .RemoveValueAsync();
    //}
    

    #region BaseDB
    /// <summary>
    /// 0: ID, 1:Name, 2:Size, 3:UseSpriteNum_Min, 4: UseSpriteNum_Max, 5: Type(N or S)
    /// </summary>
    public void LoadLands() {
        loadAll = false;
        StartCoroutine(LoadAllCheck());
        List<DataBase.Land> tempLandList = new List<DataBase.Land>();
        FirebaseDatabase.DefaultInstance.GetReference("Lands").GetValueAsync().ContinueWith
            (
            task =>
            {
                if (task.IsFaulted)
                {
                    DebugViewer.Instance.debugTextObjectList[2].GetComponent<Text>().text = "Land, Handle the error";
                    // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    DebugViewer.Instance.debugTextObjectList[2].GetComponent<Text>().text = "Land, TaskComplite";
                    DataSnapshot snapshot = task.Result;

                    for (int i = 1; i < snapshot.ChildrenCount; i++)
                    {
                        DataBase.Land tempLand = new DataBase.Land();
                        string i_ = i.ToString(); //'Lands'DB 용 ID.
                        tempLand.id = System.Convert.ToInt32(snapshot.Child(i_).Child("0").GetValue(true)); //snapShot에서 Land ID가져오기.
                        tempLand.name = snapshot.Child(i_).Child("1").GetValue(true).ToString();

                        string puzzleIDFront = tempLand.id < 10 ? "0" + tempLand.id : tempLand.id.ToString();

                        #region puzzleList_S
                        List<string> skillNum = new List<string>();
                        for (int j = 0; j < 4; j++)
                        {
                            int tempj = 3 + j;

                            string spNum = snapshot.Child(i_).Child(tempj.ToString()).GetValue(true).ToString();
                            if (spNum != "0")
                            {
                                spNum = System.Convert.ToInt32(spNum) < 10 ? "0" + spNum : spNum;
                                skillNum.Add(spNum);//skillPuzzleID_Back
                            }
                        }
                        tempLand.puzzleList_S = new List<string>();
                        for (int j = 0; j < skillNum.Count; j++)
                        {
                            tempLand.puzzleList_S.Add(puzzleIDFront + skillNum[j]);//skillPuzzleID_All
                        }
                        #endregion//skill Puzzle 만 모아놓기

                        #region puzzleList_N
                        List<string> normalIDList = new List<string>();
                        //tempLand.puzzleList_N = new string[System.Convert.ToInt32(snapshot.Child(i_).Child("2").GetValue(true))];
                        for (int j = 0; j < System.Convert.ToInt32(snapshot.Child(i_).Child("2").GetValue(true)); j++)
                        {
                            int sameCnt = 0;
                            int max = j + 1;
                            string puzzleIDBack = max < 10 ? "0" + max : max.ToString();
                            foreach (string x in skillNum)
                            {
                                if (x == puzzleIDBack) { sameCnt++; }
                            }
                            if (sameCnt == 0)
                            {
                                string normalID = puzzleIDFront + puzzleIDBack;
                                normalIDList.Add(normalID);
                            }
                        }
                        tempLand.puzzleList_N = new List<string>();
                        for (int j = 0; j < normalIDList.Count; j++)
                        {
                            tempLand.puzzleList_N.Add(normalIDList[j]);
                        }
                        #endregion // 모아놓은 skill puzzle을 제외한 normal puzzle만 모아놓기.


                        tempLandList.Add(tempLand);
                    }
                    LandManager.instance.landList = tempLandList;
                    loadLand = true;
                }

            }
     );

    }

    public void LoadPuzzles()
    {
        loadAll = false;
        List<DataBase.Puzzle> tempPuzzleList = new List<DataBase.Puzzle>();

        FirebaseDatabase.DefaultInstance.GetReference("Puzzles").GetValueAsync().ContinueWith
          (
          task =>
          {
              if (task.IsFaulted)
              {
                  DebugViewer.Instance.debugTextObjectList[3].GetComponent<Text>().text = "Puzzle, Handle the error";
                  // Handle the error...
              }
              else if (task.IsCompleted)
              {
                  DebugViewer.Instance.debugTextObjectList[3].GetComponent<Text>().text = "Puzzle, TaskComplite";
                  DataSnapshot snapshot = task.Result;
                  // Do something with snapshot...

                  for (int i = 1; i < snapshot.ChildrenCount; i++)
                  {
                      DataBase.Puzzle tempPuzzle = new DataBase.Puzzle();
                      string i_ = i.ToString();

                      tempPuzzle.id = snapshot.Child(i_).Child("0").GetValue(true).ToString();
                      tempPuzzle.name = snapshot.Child(i_).Child("1").GetValue(true).ToString();
                      tempPuzzle.size = System.Convert.ToInt32(snapshot.Child(i_).Child("2").GetValue(true));
                      tempPuzzle.useSpriteNum1 = System.Convert.ToInt32(snapshot.Child(i_).Child("3").GetValue(true));
                      tempPuzzle.useSpriteNum2 = System.Convert.ToInt32(snapshot.Child(i_).Child("4").GetValue(true));
                      tempPuzzle.type = snapshot.Child(i_).Child("5").GetValue(true).ToString();
                      tempPuzzle.spawnCount = System.Convert.ToInt32(snapshot.Child(i_).Child("6").GetValue(true));
                      tempPuzzle.maxCount = System.Convert.ToInt32(snapshot.Child(i_).Child("7").GetValue(true));

                      tempPuzzleList.Add(tempPuzzle);
                  }//칼럼 제외 이유로 i = 1 부터 시작.

                  List<DataBase.Puzzle[]> PuzzleList = new List<DataBase.Puzzle[]>(); //tempPuzzleList의 것을 옮겨담을 곳.


                  int targetPuzzleListNum = 1;
                  int puzzleCount = tempPuzzleList.Count;

                  while (puzzleCount > 0)
                  {
                      #region tempBaseSetting
                      List<DataBase.Puzzle> tempTempPuzzleList = new List<DataBase.Puzzle>(); //tempPuzzleList의 Puzzle을 Land순서별로 담는다.
                      List<int> tempTempNum = new List<int>(); //tempTempPuzzleList의 퍼즐을 가져올때 사용할 int
                      #endregion
                      for (int i = 0; i < tempPuzzleList.Count; i++)
                      {
                          string targetPuzzleListStr = targetPuzzleListNum < 10 ? "0" + targetPuzzleListNum : targetPuzzleListNum.ToString();
                          if (targetPuzzleListStr == HarimTool.EditText.EditText.Left(tempPuzzleList[i].id, 2))
                          {
                              tempTempPuzzleList.Add(tempPuzzleList[i]);
                              tempTempNum.Add(i);
                              puzzleCount--;
                          }
                          //Debug.Log(targetPuzzleListStr +", "+ puzzleCount);
                      }//Land가 같은 것 끼리 모음.1번 Land부터 시작.                      
                      DataBase.Puzzle[] tempPuzzleArr = new DataBase.Puzzle[tempTempPuzzleList.Count];
                      for (int i = 0; i < tempTempPuzzleList.Count; i++)
                      {
                          tempPuzzleArr[i] = tempTempPuzzleList[i];
                      }//PuzzleList에 순차적으로 Puzzle들을 담기 위해 List를 Array로 만든다.
                      PuzzleList.Add(tempPuzzleArr);
                      tempTempPuzzleList.Clear();
                      tempTempNum.Clear();
                      targetPuzzleListNum++;
                  }

                  PuzzleManager.instance.puzzles = new DataBase.Puzzle[PuzzleList.Count][];// [landCount][puzzleCount]                 
                  for (int i = 0; i < PuzzleList.Count; i++)
                  {
                      for (int j = 0; j < PuzzleList[i].Length; j++)
                      {
                          //int id = System.Convert.ToInt32(HarimTool.EditText.EditText.Right(PuzzleList[i][j].id, 2));
                          Array.Sort(PuzzleList[i], delegate (DataBase.Puzzle a, DataBase.Puzzle b)
                          {
                              return a.id.CompareTo(b.id);
                          });
                      }
                      PuzzleManager.instance.puzzles[i] = PuzzleList[i];

                      //foreach (DataBase.Puzzle x in PuzzleList[i])  { Debug.Log(x.id); }
                  }
                  loadPuzzle = true;
                  Debug.Log(PuzzleManager.instance.puzzles.Length);
                  // Debug.Log(snapshot.Child("1").Child("0").GetValue(true));
              }
          }
   );

    }


    IEnumerator LoadAllCheck()
    {
        while (true)
        {
            if(loadLand == true && loadPuzzle == true)
            {
                loadAll = true;
                break;
            }
            DebugViewer.Instance.debugTextObjectList[0].GetComponent<Text>().text = "LoadLand : " + loadLand.ToString();
            DebugViewer.Instance.debugTextObjectList[1].GetComponent<Text>().text = "loadPuzzle : " + loadPuzzle.ToString();
            Debug.Log(string.Format( "loadLand : {0},  loadPuzzle : {1}",loadLand, loadPuzzle));
            yield return new WaitForSeconds(0.2f);
        }
    }
    #endregion


    public bool OnLoadAdmin()
    {
        bool success = false;

        Debug.Log(UserManager.Instance.currentUser.id);
        FirebaseDatabase.DefaultInstance.GetReference("Users").Child(UserManager.Instance.currentUser.id).GetValueAsync().ContinueWith
       (
       task =>
       {
           if (task.IsFaulted || task.IsCanceled)
           {
               EventManager.instance.NickNameCheckedFunc(false);
               Debug.Log("OnLoadAdmin_Error");
           }
           else if (task.IsCompleted)
           {
               DataSnapshot snapshot = task.Result;
               if (snapshot.ChildrenCount > 0)
               {
                   UserManager.Instance.currentUser.name = snapshot.Child("username").GetValue(true).ToString(); //snapShot에서 name 가져오기.
               }
               success = true;
               EventManager.instance.NickNameCheckedFunc(true);
               Debug.Log(UserManager.Instance.currentUser.name + ", success : "+success);
               DebugViewer.Instance.debugTextObjectList[3].GetComponent<Text>().text = "Admin Load :"+ UserManager.Instance.currentUser.name;
           }
       });
        if (local == true) { success = true; }
        return success;
    }

//    void OnLoadLand() { }

    public void NickNameChange(string nickName)
    {
        DebugViewer.Instance.debugTextObjectList[3].GetComponent<Text>().text = nickName+"...";
        string newNickName = nickName;


        if (nickName.Length > 12 || CussWords.AType.WordCheck(nickName) == false)
        {
            LandManager.instance.views.popupView.GetComponent<PopupViewController>().nickNamePop.GetComponent<NickNameController>().inputF.text = "";
            LandManager.instance.views.popupView.GetComponent<PopupViewController>().nickNamePop.GetComponent<NickNameController>().notice.text = "Can't this Nick. Please try again.";
            LandManager.instance.views.popupView.GetComponent<PopupViewController>().nickNamePop.GetComponent<NickNameController>().btn_Complte.SetActive(false);

            Debug.Log('"' + nickName + '"' + "!! Retry your nickname.");
        }
        else
        {
            if (UserManager.Instance.currentUser.id == "" || UserManager.Instance.currentUser.id == null) { newNickName = "empty"; }
            DebugViewer.Instance.debugTextObjectList[3].GetComponent<Text>().text = nickName + ".....";


            DatabaseReference mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
            mDatabaseRef.Child("Users").Child(UserManager.Instance.currentUser.id).Child("username").SetValueAsync(newNickName).ContinueWith(
                    task =>
                    {
                        if (task.IsFaulted)
                        {
                            Debug.Log(UserManager.Instance.currentUser.id);
                            DebugViewer.Instance.debugTextObjectList[3].GetComponent<Text>().text = "NickName, Handle the error";
                            // Handle the error...
                        }
                        else
                        {
                            UserManager.Instance.currentUser.name = nickName;
                            DebugViewer.Instance.debugTextObjectList[3].GetComponent<Text>().text = "NickName : " + nickName;
                            LandManager.instance.views.popupView.GetComponent<PopupViewController>().nickNamePop.SetActive(false); //close nickNamePopup                            
                        }
                    }
                );
        }



    }

    public void OnSaveAdmin()
    {
        Debug.Log(UserManager.Instance.currentUser.PlayTime + ", " + (System.DateTime.Now + ", " + UserManager.Instance.currentUser.loginTime) + ", " + (System.DateTime.Now - UserManager.Instance.currentUser.loginTime).Seconds); 
        UserManager.Instance.currentUser.PlayTime = UserManager.Instance.currentUser.PlayTime + (System.DateTime.Now - UserManager.Instance.currentUser.loginTime).Seconds;
        DebugViewer.Instance.debugTextObjectList[2].GetComponent<Text>().text = UserManager.Instance.currentUser.PlayTime.ToString();

        if (local == false)
        {
            #region SaveItem _Childs           
            string[] childs = new string[] { "gem", "ClrLndCnt", "playtime", "lastLand" }; // save item
            string[] childsValues = new string[]
            {
                UserManager.Instance.currentUser.gem.ToString(),
                UserManager.Instance.currentUser.ClearLandCount.ToString(),
                UserManager.Instance.currentUser.PlayTime.ToString(),
                UserManager.Instance.currentUser.lastLand.ToString()
            }; // save itme value
            #endregion 
            DebugViewer.Instance.debugTextObjectList[2].GetComponent<Text>().text = "save...";

            DatabaseReference mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
            for (int i = 0; i < childs.Length; i++)
            {
                mDatabaseRef.Child("Users").Child(UserManager.Instance.currentUser.id).Child(childs[i]).SetValueAsync(childsValues[i]).ContinueWith(
                task =>
                {
                    if (task.IsFaulted)
                    {
                        DebugViewer.Instance.debugTextObjectList[2].GetComponent<Text>().text = "Admin, Handle the error";
                        // Handle the error...
                    }
                });
            } // "Users" 항목에 순차적으로 저장.
        }
        else
        {
            SaveLocal_Game("UserData.save");
        }//LocalDB  "lastLand", "gotLandList", "lastpuzzle", "ingPuzzleList"

    }//FireBaseDB

    public void OnSaveLand()
    {
        if (local == false)
        {
            DatabaseReference mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference; 
            for (int i = 0; i < UserManager.Instance.currentUser.gotLandList.Count; i++)
            {
                string i_ = i.ToString();

                mDatabaseRef.Child("Users").Child(UserManager.Instance.currentUser.id).Child("gotLands").Child(i_).Child("id").SetValueAsync(UserManager.Instance.currentUser.gotLandList[i].id.ToString()).ContinueWith(
                task =>
                {
                    if (task.IsFaulted)
                    {
                        DebugViewer.Instance.debugTextObjectList[2].GetComponent<Text>().text = "Admin, Handle the error";
                        // Handle the error...
                    }
                    if (task.IsCompleted)
                    {
                    }
                });
                SaveThat(mDatabaseRef.Child("Users").Child(UserManager.Instance.currentUser.id).Child("gotLands").Child(i_).Child("weather"), UserManager.Instance.currentUser.gotLandList[i].weather.ToString());
                for (int j = 0; j < UserManager.Instance.currentUser.gotLandList[i].clearPuzzleList.Count; j++)
                {
                    string j_ = j.ToString();
                    SaveThat(mDatabaseRef.Child("Users").Child(UserManager.Instance.currentUser.id).Child("gotLands").Child(i_).Child("clrPuzzles").Child(j_), UserManager.Instance.currentUser.gotLandList[i].clearPuzzleList[j].ToString());
                }
                for (int j = 0; j < UserManager.Instance.currentUser.gotLandList[i].unitList.Count; j++)
                {
                    string j_ = j.ToString();
                    SaveThat(mDatabaseRef.Child("Users").Child(UserManager.Instance.currentUser.id).Child("gotLands").Child(i_).Child("units").Child(j_), UserManager.Instance.currentUser.gotLandList[i].clearPuzzleList[j].ToString());
                }

            }
        }
        else
        {
            SaveLocal_Game("UserData.save");
        }
    }


    void SaveThat(DatabaseReference currentUserDatabaseRef, string value)
    {
        currentUserDatabaseRef.SetValueAsync(value).ContinueWith(
            task =>
            {
                if (!task.IsCompleted)
                {
                    DebugViewer.Instance.debugTextObjectList[2].GetComponent<Text>().text = "Save error : " + currentUserDatabaseRef.ToString();
                    // Handle the error...
                }
            });
    }
    #endregion

    #region Local
    public void LoadLocal()
    {
        try
        {
            FileInfo fi = new FileInfo(path + "/UserData.save");
            if (fi.Exists)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(path + "/UserData.save", FileMode.OpenOrCreate, FileAccess.Read);

                //var LoadFile = JsonUtility.FromJson<SaveData.User>((string)bf.Deserialize(fs));
                SaveData.User user = (SaveData.User)bf.Deserialize(fs);
                fs.Close();

                UserManager.Instance.currentUser = user;
                Debug.Log(user.name+", "+ user.PlayTime +", unitCount" +user.gotLandList[0].unitList.Count);
            }
            else { Debug.Log("error_Local : Path");  }// UserManager.Instance.DefaultSetting(); }
        }
        catch (Exception e)
        {
            Debug.Log("error_Local Load : .save");
        }//load 필요없음.
    }

    void SaveLocal_Game(string fileName)
    {

        DirectoryInfo di = new DirectoryInfo(path);
        if (di.Exists == false)
        {
            di.Create();
            Debug.Log("newFolder");
        }//폴더 없으면 만듦.

        BinaryFormatter bf = new BinaryFormatter();
        if (System.IO.File.Exists(path + "/" + fileName)) { Debug.Log("double"); }//덮어쓸거야?}
        FileStream fs = new FileStream(path + "/" + fileName, FileMode.Create);
        bf.Serialize(fs, UserManager.Instance.currentUser);
        fs.Close();

        Debug.Log(UserManager.Instance.currentUser);
    }    
    #endregion






}