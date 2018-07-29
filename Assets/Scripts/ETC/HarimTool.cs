using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace HarimTool
{
    namespace PoolObj
    {
        public class PoolObj : MonoBehaviour
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
    namespace DataBase
    {
        public class DataBase : MonoBehaviour
        {
            public static IEnumerator Timer(int min, int sec)
            {

                int sum = (min * 60) + sec;
                yield return new WaitForSeconds(sum);

                yield return sum;
            }

            /// <summary>
            /// fileName.xxx
            /// </summary>
            /// <param name="fileName"></param>
            /// <returns></returns>
            public static void LoadLocalBinary(object data, string fileName)
            {
                string path = Application.persistentDataPath + "/Save"; // for Local
                try
                {                    
                    FileInfo fi = new FileInfo(path + "/"+ fileName);
                    if (fi.Exists)
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        FileStream fs = new FileStream(path + "/UserData.save", FileMode.OpenOrCreate, FileAccess.Read);
                        data = (object)bf.Deserialize(fs);
                        fs.Close();
                    }
                    else { Debug.Log("error_Local : Path"); }// UserManager.Instance.DefaultSetting(); }
                }
                catch (Exception e)
                {
                    Debug.Log("error_Local Load : .save");
                }//load 필요없음.
            }

            /// <summary>
            /// fileName.xxx
            /// </summary>
            /// <param name="fileName"></param>
            /// <returns></returns>
            public static void SaveLocalBinary(object data, string fileName)
            {
                string path = Application.persistentDataPath + "/Save"; // for Local

                DirectoryInfo di = new DirectoryInfo(path);
                if (di.Exists == false)
                {
                    di.Create();
                    Debug.Log("newFolder");
                }//폴더 없으면 만듦.
                BinaryFormatter bf = new BinaryFormatter();
                if (System.IO.File.Exists(path + "/" + fileName)) { Debug.Log("double"); }//덮어쓸거야?}
                FileStream fs = new FileStream(path + "/" + fileName, FileMode.Create);
                bf.Serialize(fs,data);
                fs.Close();

                Debug.Log(UserManager.Instance.currentUser);
            }

        }
    }

    namespace EditText
    {
        public class EditText
        {

            public static string Left(string text, int textLength)
            {       //-----Left
                string convertText;
                if (text.Length < textLength)
                {
                    textLength = text.Length;
                }
                convertText = text.Substring(0, textLength);
                return convertText;
            }


            public static string Right(string text, int textLength)
            {    //-----Right
                string convertText;
                if (text.Length < textLength)
                {
                    textLength = text.Length;
                }
                convertText = text.Substring(text.Length - textLength, textLength);
                return convertText;
            }


            public static string Mid(string text, int startPoint, int nLength)
            { //-----Mid text의 startpoint자에서 nLength까지의 문자를 구한다.
                string sReturn;
                //--startPoint;

                if (startPoint <= text.Length)
                {
                    if ((startPoint + nLength) <= text.Length)
                    {
                        if (nLength == 0)
                        {                         //-------nLangth가 0이면 끝까지 출력
                            int sLength = text.Length - startPoint; // sametext.Length
                            sReturn = text.Substring(startPoint, sLength);
                            //Debug.Log(sReturn);
                        }
                        else
                        {
                            sReturn = text.Substring(startPoint, nLength);
                            //Debug.Log(sReturn);
                        }
                    }
                    else
                    {
                        sReturn = text.Substring(startPoint);
                    }
                }
                else
                {
                    sReturn = string.Empty;
                }
                return sReturn;
            }
        }

    }
}
