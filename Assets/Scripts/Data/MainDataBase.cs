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
using UnityEngine.SceneManagement;

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

    string savePath; // for Local
    string loginDataPath;

    #region forFirebasse
    public bool loadAdmin=false;
    public bool loadAll;
    public bool local;
    bool loadLand;
    bool loadPuzzle;


    //--------------nickName-------------- v
    bool notJunk = false; 
    #endregion


    unsafe void Awake()
    {
        local = false;
        savePath = Application.persistentDataPath + "/Save"; // for Local
        loginDataPath = Application.persistentDataPath + "/loginData"; // for Local
        //--------------------Set realtime database ----------------------------v
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://land-of-picross-8205260.firebaseio.com/");
        FirebaseApp.DefaultInstance.SetEditorP12FileName("land-of-picross-8205260-62de3e51bc93.p12");
        FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("land-of-picross-8205260@appspot.gserviceaccount.com");
        FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;        
    }
    #region FireBase

    
    #region BaseDB
    /// <summary>
    /// 0: ID, 1:Name, 2:Size, 3:UseSpriteNum_Min, 4: UseSpriteNum_Max, 5: Type(N or S)
    /// LoadPuzzles 보다 먼저 실행.
    /// </summary>
    public void LoadLands() {
        loadAll = false;
        StartCoroutine(LoadAllCheck());
        List<DataBase.Land> tempLandList = new List<DataBase.Land>();
        try
        {
            #region LoadLandDB_Firebase
            FirebaseDatabase.DefaultInstance.GetReference("Lands").GetValueAsync().ContinueWith
                (
                task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("Land, Handle the error");
                    // Handle the error...
                }
                    else if (task.IsCompleted)
                    {
                        Debug.Log("Land, TaskComplite");
                        DataSnapshot snapshot = task.Result;

                        for (int i = 1; i < snapshot.ChildrenCount; i++)
                        {
                            DataBase.Land tempLand = new DataBase.Land();
                            string i_ = i.ToString(); //'Lands'DB 용 ID.
                        tempLand.id = Convert.ToInt32(snapshot.Child(i_).Child("0").GetValue(true)); //snapShot에서 Land ID가져오기.
                        tempLand.name = snapshot.Child(i_).Child("1").GetValue(true).ToString();
                            tempLand.price = Convert.ToInt32(snapshot.Child(i_).Child("7").GetValue(true));
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
            #endregion
        }
        catch
        {
            if (LoadDB_Local() == true) {
                loadAll = true;
                Debug.Log("Load_LocalDB");
            }            
        }
    }

    public void LoadPuzzles()
    {
        loadAll = false;
        List<DataBase.Puzzle> tempPuzzleList = new List<DataBase.Puzzle>();
        try
        {
            FirebaseDatabase.DefaultInstance.GetReference("Puzzles").GetValueAsync().ContinueWith
              (
              task =>
              {
                  if (task.IsFaulted)
                  {
                      Debug.Log("Puzzle, Handle the error");
                  } // Handle the error...
                  else if (task.IsCompleted)
                  {
                      Debug.Log("Puzzle, TaskComplite");
                      DataSnapshot snapshot = task.Result;

                      #region tempPuzzleList
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
                      #endregion

                      List<DataBase.Puzzle[]> puzzleList = new List<DataBase.Puzzle[]>(); //tempPuzzleList의 것을 Land 별로 옮겨담을 곳.

                      #region puzzleList
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
                              if (targetPuzzleListStr == HarimTool.EditValue.EditText.Left(tempPuzzleList[i].id, 2))
                              {
                                  tempTempPuzzleList.Add(tempPuzzleList[i]);
                                  tempTempNum.Add(i);
                                  puzzleCount--;
                              }//Debug.Log(targetPuzzleListStr +", "+ puzzleCount);
                          }//Land가 같은 것 끼리 모음.1번 Land부터 시작.                      
                          DataBase.Puzzle[] tempPuzzleArr = new DataBase.Puzzle[tempTempPuzzleList.Count];
                          for (int i = 0; i < tempTempPuzzleList.Count; i++)
                          {
                              tempPuzzleArr[i] = tempTempPuzzleList[i];
                          }//PuzzleList에 순차적으로 Puzzle들을 담기 위해 List를 Array로 만든다.
                          puzzleList.Add(tempPuzzleArr);
                          tempTempPuzzleList.Clear();
                          tempTempNum.Clear();
                          targetPuzzleListNum++;
                      }
                      #endregion

                      #region PuzzleManager.puzzles
                      PuzzleManager.instance.puzzles = new DataBase.Puzzle[puzzleList.Count][];// [landCount][puzzleCount]                 
                      for (int i = 0; i < puzzleList.Count; i++)
                      {
                          for (int j = 0; j < puzzleList[i].Length; j++)
                          {
                                     
                              Array.Sort(puzzleList[i], delegate (DataBase.Puzzle a, DataBase.Puzzle b)
                              {
                                  return a.id.CompareTo(b.id);
                              });//퍼즐 ID를 순서대로 정렬
                          }
                          PuzzleManager.instance.puzzles[i] = puzzleList[i];
                      }
                      #endregion

                      loadPuzzle = true;
                      SaveDB_Local();
                      Debug.Log("Kind of Puzzle : Land Count : " + PuzzleManager.instance.puzzles.Length);                      
                      // Debug.Log(snapshot.Child("1").Child("0").GetValue(true));
                  }
              }
            );
        }
        catch
        {
            Debug.Log("Can't Load Puzzles");
        }
    }

    IEnumerator LoadAllCheck()
    {
        float time = 0;
        while (true)
        {
            if(loadLand == true && loadPuzzle == true)
            {
                loadAll = true;
                break;
            }
            else if(time>7){ SceneManager.LoadScene("TitleScene"); Debug.Log("Error_DB Load");  break; }
            //Debug.Log(string.Format( "loadLand : {0},  loadPuzzle : {1}",loadLand, loadPuzzle));
            yield return new WaitForSeconds(0.2f);
            time = time + Time.deltaTime;
        }
    }
    #endregion


    public bool OnLoadAdmin()
    {
        bool firstTime = true;
        Debug.Log("default ID : "+ UserManager.Instance.currentUser.id);
        FirebaseDatabase.DefaultInstance.GetReference("Users").Child(UserManager.Instance.currentUser.id).GetValueAsync().ContinueWith
       (
       task =>
       {
           if (!task.IsCompleted)
           {
               Debug.Log("OnLoadAdmin_Error");
           }
           else if (task.IsCompleted)
           {
               DataSnapshot snapshot = task.Result;
               if (snapshot.ChildrenCount > 0)
               {
                   #region Admin
                   UserManager.Instance.currentUser.name = snapshot.Child("username").GetValue(true).ToString(); //snapShot에서 name 가져오기.
                   EventManager.instance.NickNameCheckedFunc(true);

                   UserManager.Instance.currentUser.gem = Convert.ToInt32(snapshot.Child("gem").GetValue(true));
                   UserManager.Instance.currentUser.star = Convert.ToInt32(snapshot.Child("star").GetValue(true));
                   UserManager.Instance.currentUser.ClearLandCount = Convert.ToInt32(snapshot.Child("clrLndCnt").GetValue(true));
                   UserManager.Instance.currentUser.PlayTime = Convert.ToInt32(snapshot.Child("playTime").GetValue(true));
                   Debug.Log("playTime : "+UserManager.Instance.currentUser.PlayTime);
                   UserManager.Instance.currentUser.lastLand = Convert.ToInt32(snapshot.Child("lastLand").GetValue(true));
                   #endregion

                   #region Land
                   if (UserManager.Instance.currentUser.gotLandList.Count > 0) { UserManager.Instance.currentUser.gotLandList.Clear(); }
                   for (int i = 0; i < snapshot.Child("gotLands").ChildrenCount; i++)
                   {
                       string i_ = i.ToString();
                       SaveData.GotLand tempGotLand = new SaveData.GotLand();                       

                       tempGotLand.id = Convert.ToInt32(snapshot.Child("gotLands").Child(i_).Child("id").GetValue(true));
                       tempGotLand.weather = Convert.ToInt32(snapshot.Child("gotLands").Child(i_).Child("weather").GetValue(true));

                       for (int j = 0; j < snapshot.Child("gotLands").Child(i_).Child("clrPuzzles").ChildrenCount; j++)
                       {
                           string j_ = j.ToString();
                           tempGotLand.clearPuzzleList.Add(snapshot.Child("gotLands").Child(i_).Child("clrPuzzles").Child(j_).GetValue(true).ToString());
                       }
                       for (int j = 0; j < snapshot.Child("gotLands").Child(i_).Child("units").ChildrenCount; j++)
                       {
                           string j_ = j.ToString();
                           tempGotLand.unitList.Add(snapshot.Child("gotLands").Child(i_).Child("units").Child(j_).GetValue(true).ToString());
                       }

                       UserManager.Instance.currentUser.gotLandList.Add(tempGotLand);
                       #endregion
                   }
                   
               }

               if (UserManager.Instance.currentUser.PlayTime > 0) { firstTime = false; }

               Debug.Log(UserManager.Instance.currentUser.name + ", firstTime : " + firstTime);

               loadAdmin = true;
           }           
       });

        return firstTime;
    }

//    void OnLoadLand() { }

    public void NickNameChange(string nickName)
    {
        Debug.Log( nickName +"..." );
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

            DatabaseReference mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
            mDatabaseRef.Child("Users").Child(UserManager.Instance.currentUser.id).Child("username").SetValueAsync(newNickName).ContinueWith(
                    task =>
                    {
                        if (task.IsFaulted)
                        {
                            Debug.Log("NickName, Handle the error / currentUserID : "+ UserManager.Instance.currentUser.id);
                            // Handle the error...
                        }
                        else
                        {
                            UserManager.Instance.currentUser.name = nickName;
                            Debug.Log( "NickName : " + nickName );
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
        
        if (local == false)
        {
            #region SaveItem _Childs           
            string[] childs = new string[] { "gem", "star", "clrLndCnt", "playTime", "lastLand" }; // save item
            string[] childsValues = new string[]
            {
                UserManager.Instance.currentUser.gem.ToString(),
                UserManager.Instance.currentUser.star.ToString(),
                UserManager.Instance.currentUser.ClearLandCount.ToString(),
                UserManager.Instance.currentUser.PlayTime.ToString(),
                UserManager.Instance.currentUser.lastLand.ToString()
            }; // save itme value
            #endregion 
        
            DatabaseReference mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
            for (int i = 0; i < childs.Length; i++)
            {
                mDatabaseRef.Child("Users").Child(UserManager.Instance.currentUser.id).Child(childs[i]).SetValueAsync(childsValues[i]).ContinueWith(
                task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("Admin, Handle the error");
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
                        Debug.Log("Admin, Handle the error");
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
                    SaveThat(mDatabaseRef.Child("Users").Child(UserManager.Instance.currentUser.id).Child("gotLands").Child(i_).Child("units").Child(j_), UserManager.Instance.currentUser.gotLandList[i].unitList[j].ToString());
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
                    Debug.Log("Save error : " + currentUserDatabaseRef.ToString());
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
            FileInfo fi = new FileInfo(savePath + "/UserData.save");
            if (fi.Exists)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(savePath + "/UserData.save", FileMode.OpenOrCreate, FileAccess.Read);

                //var LoadFile = JsonUtility.FromJson<SaveData.User>((string)bf.Deserialize(fs));
                SaveData.User user = (SaveData.User)bf.Deserialize(fs);
                fs.Close();

                UserManager.Instance.currentUser = user;
                Debug.Log(user.name+", "+ user.PlayTime +", unitCount : " +user.gotLandList[0].unitList.Count);
            }
            else { Debug.Log("error_Local : Path");  }// UserManager.Instance.DefaultSetting(); }
        }
        catch (Exception e)
        {
            Debug.Log("error_Local Load : .save");
        }//load 필요없음.
    }

    void SaveDB_Local()
    {
        DataBase.LocalDB localDB = new DataBase.LocalDB();
        localDB.landList = LandManager.instance.landList;
        localDB.puzzles = PuzzleManager.instance.puzzles;

        DirectoryInfo di = new DirectoryInfo(savePath);
        if (di.Exists == false)
        {
            di.Create();
            Debug.Log("newFolder");
        }//폴더 없으면 만듦.

        BinaryFormatter bf = new BinaryFormatter();
        if (System.IO.File.Exists(savePath + "/LocalDB.dat")) { Debug.Log("double"); }//덮어쓸거야?}
        FileStream fs = new FileStream(savePath + "/LocalDB.dat", FileMode.Create);
        bf.Serialize(fs, localDB);
        fs.Close();
    }

    void SaveLocal_Game(string fileName)
    {

        DirectoryInfo di = new DirectoryInfo(savePath);
        if (di.Exists == false)
        {
            di.Create();
            Debug.Log("newFolder");
        }//폴더 없으면 만듦.

        BinaryFormatter bf = new BinaryFormatter();
        if (System.IO.File.Exists(savePath + "/" + fileName)) { Debug.Log("double"); }//덮어쓸거야?}
        FileStream fs = new FileStream(savePath + "/" + fileName, FileMode.Create);
        bf.Serialize(fs, UserManager.Instance.currentUser);
        fs.Close();

        Debug.Log(UserManager.Instance.currentUser);
    }

    public void SaveLocal_LoginData(string fileName)
    {
        DirectoryInfo di = new DirectoryInfo(loginDataPath);
        if (di.Exists == false)
        {
            di.Create();
            Debug.Log("newFolder");
        }//폴더 없으면 만듦.

        BinaryFormatter bf = new BinaryFormatter();
        if (System.IO.File.Exists(loginDataPath + "/" + fileName)) { Debug.Log("double"); }//덮어쓸거야?}
        FileStream fs = new FileStream(loginDataPath + "/" + fileName, FileMode.Create);
        bf.Serialize(fs, System.DateTime.Now);
        fs.Close();

        Debug.Log(UserManager.Instance.currentUser);
    }

    public void SaveSetting()
    {
        DirectoryInfo di = new DirectoryInfo(loginDataPath);
        if (di.Exists == false)
        {
            di.Create();
            Debug.Log("newFolder");
        }//폴더 없으면 만듦.

        BinaryFormatter bf = new BinaryFormatter();
        if (System.IO.File.Exists(loginDataPath + "/SettingData.txt")) { Debug.Log("double"); }//덮어쓸거야?}
        FileStream fs = new FileStream(loginDataPath + "/SettingData.txt", FileMode.Create);
        bf.Serialize(fs, UserManager.Instance.currentUser.settingVal);
        fs.Close();

    }

    bool LoadDB_Local()
    {
        FileInfo fi = new FileInfo(savePath + "/LocalDB.dat");
        if (fi.Exists == true)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(savePath + "/LocalDB.dat", FileMode.Open, FileAccess.Read);

                DataBase.LocalDB localDB = (DataBase.LocalDB)bf.Deserialize(fs);
                LandManager.instance.landList = localDB.landList;
                PuzzleManager.instance.puzzles = localDB.puzzles;

                fs.Close();
                return true;
            }
            catch
            {
                Debug.Log("Can't Accesse the Local Database");
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }

    public void LoadSetting() {
        //Debug.Log("aasdadsasdasdassdadsasdasdasdasdasdasdasdasdsdsdseeting");
        try
        {
            FileInfo fi = new FileInfo(loginDataPath + "/SettingData.txt");
            if (fi.Exists == true)
            {   
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(savePath + "/SettingData.txt", FileMode.Open, FileAccess.Read);

                //var LoadFile = JsonUtility.FromJson<SaveData.User>((string)bf.Deserialize(fs));
                bool[] settingValue = (bool[])bf.Deserialize(fs);
                fs.Close();

                UserManager.Instance.currentUser.settingVal = settingValue;
                //Debug.Log("settingValue : "+ settingValue);
            }
            else {
                UserManager.Instance.currentUser.settingVal = new bool[] { true, true, true };
                Debug.Log(UserManager.Instance.currentUser.settingVal);
            }// UserManager.Instance.DefaultSetting(); }
        }
        catch (Exception e)
        {
            UserManager.Instance.currentUser.settingVal = new bool[] { true, true, true };
            Debug.Log(UserManager.Instance.currentUser.settingVal);
            Debug.Log("error : LoadSettingValue");
        }//load 필요없음.
    }

    public bool LoginDataCheck()
    {
        bool success=false;
        try
        {
            FileInfo fi = new FileInfo(loginDataPath + "/LoginData.txt");
            if (fi.Exists)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(loginDataPath + "/LoginData.txt", FileMode.OpenOrCreate, FileAccess.Read);

                string loginData = (string)bf.Deserialize(fs);
                fs.Close();

                success = true;
                Debug.Log("loginData : "+ loginData);
            }
            else { Debug.Log("error_Local : Path"); }
        }
        catch (Exception e)
        {
            Debug.Log("error_Local Load : LoginData.txt");
        }
        return success;
    }
    #endregion






}