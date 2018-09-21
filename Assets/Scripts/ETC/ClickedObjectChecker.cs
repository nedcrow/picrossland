using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickedObjectChecker : MonoBehaviour
{
    //private static ClickedObjectCheck _instance;
    //public static ClickedObjectCheck instance
    //{
    //    get
    //    {
    //        if (!_instance)
    //        {
    //            GameObject obj = GameObject.Find("ClickedObjectCheck");
    //            if (obj == null)
    //            {
    //                obj = new GameObject("ClickedObjectCheck");
    //                obj.AddComponent<ClickedObjectCheck>();
    //            }
    //            return obj.GetComponent<ClickedObjectCheck>();
    //        }
    //        return _instance;
    //    }
    //}

    [Range(0, 1)]
    public int itemNum = 0;
    [Range(1, 10)]
    public int limitCount = 0;
    public string[] item = { "name", "tag" };

    private void Start()
    {
        CheckGameObjectOnUI(item[itemNum], limitCount);

    }

    /// <summary>
    /// item : name or tag
    /// </summary>
    public void CheckGameObjectOnUI(string item, int limitCount = 1)
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
                List<RaycastResult> results = TouchedObjs(100); //100 = mouse
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
        foreach (RaycastResult target in targetList)
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
