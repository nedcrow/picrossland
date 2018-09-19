using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;

namespace HarimTool
{
    namespace ObjectCheck
    {
        public class ClickedObjectCheck : MonoBehaviour
        {
            private static ClickedObjectCheck _instance;
            public static ClickedObjectCheck instance
            {
                get
                {
                    if (!_instance)
                    {
                        GameObject obj = GameObject.Find("ClickedObjectCheck");
                        if (obj == null)
                        {
                            obj = new GameObject("ClickedObjectCheck");
                            obj.AddComponent<ClickedObjectCheck>();
                        }
                        return obj.GetComponent<ClickedObjectCheck>();
                    }
                    return _instance;
                }
            }

            /// <summary>
            /// item : name or tag
            /// </summary>
            public void CheckGameObjectOnUI(string item, int limitCount = 1 )
            {
                StartCoroutine(ObjectCheckUI(item, limitCount));                
            }

            IEnumerator ObjectCheckUI(string item, int limit)
            {
                int cnt = 0;
                bool loop = true;
                while (loop)
                {
                    string btnName;                    
                    if (Input.touchCount > 1)
                    {
                        Debug.Log("CheckForUI 1");
                        for (int i = 0; i < Input.touchCount; i++)
                        {
                            List<RaycastResult> results = TouchedObjs(i);
                            if (results.Count > 0) { PrintObject(item, results); }
                        }
                        cnt++;
                    }
                    else if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("CheckForUI 2");
                        List<RaycastResult> results = TouchedObjs(100); //10 = mouse
                        if (results.Count > 0) { PrintObject(item, results); }
                        cnt++;
                    }
                    loop = cnt < limit || cnt == 0 ? true : false;
                    yield return new WaitForSeconds(0.01f);
                    
                }
            }

            void PrintObject(string item, List<RaycastResult> targetList)
            {
                int tNum = 0;
                foreach(RaycastResult target in targetList)
                {
                    if (item == "name") { Debug.Log("result[" + tNum + "] :" + target.gameObject.name); }
                    else if (item == "tag") { Debug.Log("result[" + tNum + "] :" + target.gameObject.tag); }
                    else { Debug.Log("result[" + tNum + "] :" + target.gameObject); }
                    tNum++;
                }
            }

            /// <summary>
            /// 100 = mouse
            /// </summary>
            /// <param name="num"></param>
            /// <returns></returns>
            List<RaycastResult> TouchedObjs(int num = 100)
            {
                PointerEventData ped = new PointerEventData(null);

                if (num == 100)
                {
                    ped.position = Input.mousePosition;
                } // mouse
                else
                {
                    Touch tempTouch = Input.GetTouch(num);
                    Vector2 pos = tempTouch.position;
                    ped.position = new Vector3(pos.x, pos.y, 0.0f);//get world position.
                } //touch

                List<RaycastResult> results = new List<RaycastResult>(); // 여기에 히트 된 개체 저장 
                GraphicRaycaster gr = GameObject.Find("Canvas").GetComponent<Canvas>().GetComponent<GraphicRaycaster>();
                gr.Raycast(ped, results);

                return results;
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

        public class EditSomething
        {
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
        }
    }
}
