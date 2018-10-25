using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;

namespace HarimTool
{    
    namespace Escape
    {
        public class Escape
        {
            public static void AppQuit()
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        Application.OpenURL("https://google.com");
#else
        Application.Quit();
#endif

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

    namespace EditValue
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

        public static class EditSomething
        {
            #region Swap
            /// <summary>
            /// (&변수A, &변수B) use in unsafe func.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public static unsafe void Swap(int* x, int* y)
            {
                int temp = *x;
                *x = *y;
                *y = temp;
            }

            public static unsafe void Swap(float* x, float* y)
            {
                float temp = *x;
                *x = *y;
                *y = temp;
            }

            public static unsafe void Swap(char* x, char* y)
            {
                char temp = *x;
                *x = *y;
                *y = temp;
            }
            #endregion

            #region ConvertToStrings
            /// <summary>
            /// List => String[]. 매개변수 <string> , <GameObject.Name>
            /// </summary>
            /// <param name="strList"></param>
            /// <returns></returns>
            public static string[] ConvertToStrings(List<string> strList)
            {
                if (strList.Count > 0)
                {
                    string[] strs = new string[strList.Count];
                    for (int i = 0; i < strList.Count; i++)
                    {
                        strs[i] = strList[i];
                    }
                    return strs;
                }
                else {
                    return null;
                }
            }

            public static string[] ConvertToStrings(List<GameObject> gameObjectList)
            {
                if (gameObjectList.Count > 0)
                {
                    string[] strs = new string[gameObjectList.Count];
                    for (int i = 0; i < gameObjectList.Count; i++)
                    {
                        strs[i] = gameObjectList[i].name;
                    }
                    return strs;
                }
                else
                {
                    return null;
                }
            }
            #endregion

            /// <summary>
            /// Colored By Color Scripter™
            /// </summary>
            /// <param name="source"></param>
            /// <param name="target"></param>
            /// <returns></returns>
            public static bool Equal(this object source, object target)
            {
                BinaryFormatter bf1 = new BinaryFormatter();
                MemoryStream ms1 = new MemoryStream();
                bf1.Serialize(ms1, source);

                BinaryFormatter bf2 = new BinaryFormatter();
                MemoryStream ms2 = new MemoryStream();
                bf1.Serialize(ms2, target);

                byte[] array1 = ms1.ToArray();
                byte[] array2 = ms2.ToArray();

                if (array1.Length != array2.Length)
                    return false;

                for (int i = 0; i < array1.Length; i++)
                {
                    if (array1[i] != array2[i])
                        return false;
                }
                return true;
            }

            /// <summary>
            /// <para>target안에 soruce와 같은 값의 객체가 포함되는지 확인 합니다.</para>
            /// <para>target들 안에 'id(field)'가 있으면, 두 매개변수의 타입이 다르더라도 탐색 가능합니다.</para>
            /// <para>Even if that two types are different, can search if that 'id(field)' in targets. </para>
            /// </summary>
            /// <param name="source"></param>
            /// <param name="target">source와 같거나 id field가 필요합니다.</param>
            /// <returns></returns>
            public static bool ContainAnB(object source, List<object> target)
            {
                return ContainAnB_(source, target);
            }

            public static bool ContainAnB(string source, List<string> target)
            {
                object s = source;
                List<object> tList = new List<object>();
                foreach (string t in target) { tList.Add(t); }
                //Debug.Log(source + " contain : " + tList.Count);
                return ContainAnB_(s, tList);
            }

            static bool ContainAnB_(object source, List<object> target)
            {
                List<string> tList = new List<string>();
                if (target.Count > 0)
                {
                    if (target[0].GetType() == source.GetType())
                    {
                        return target.Contains(source);
                    }// 두 타입이 같으면
                    else
                    {
                        FieldInfo[] fields = target[0].GetType().GetFields();
                        for (int i = 0; i < fields.Length; i++)
                        {
                            //Debug.Log(EditText.Right(fields[i].ToString(), 2));
                            if (EditText.Right(fields[i].ToString(), 2) == "id")
                            {
                                for (int j = 0; j < target.Count; j++)
                                {
                                    if (fields[i].GetValue(target[j]).ToString() == source.ToString())
                                    {
                                        return true;
                                    }
                                }
                            }
                        }// targtList Field 중에 id가 있으면, 

                    }// 두 타입이 다르면
                }
                return false;
            }
        }
    }
}
