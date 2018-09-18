using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class WarningController : MonoBehaviour
{

    Text notice;
    GameObject buttons;

    public void SetWarning()
    {
        notice = transform.GetChild(2).GetComponent<Text>();
        buttons = transform.GetChild(3).gameObject;

        gameObject.SetActive(false);
    }

    public void W_Gem()
    {
        notice.text = "Gem이 부족합니다.";
        ButtonActive(0);
    }

    public void W_Resource()
    {
        notice.text = "업데이트 예정입니다.";
        ButtonActive(1);
    }

    void ButtonActive(int btnNum)
    {
        for (int i = 0; i < buttons.transform.GetChildCount(); i++)
        {
            buttons.transform.GetChild(i).gameObject.SetActive(false);
        }
        buttons.transform.GetChild(btnNum).gameObject.SetActive(true);

        StartCoroutine(CheckButton());
    }

    IEnumerator CheckButton()
    {        
        while (true)
        {
            string btnName;
            if (Input.GetMouseButtonDown(0))
            {
                List<RaycastResult> results = TouchedObjs(100); //10 = mouse
                if (EndCheck(results) == true) { break; }
            }
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    List<RaycastResult> results = TouchedObjs(i);             
                    if (EndCheck(results) == true) { break; }
                }               
            }
            yield return new WaitForSeconds(0.01f);
        }
        gameObject.SetActive(false);
    }

    bool EndCheck(List<RaycastResult> results)
    {
        string btnName;
        for (int i = 0; i < results.Count; i++)
        {
            btnName = HarimTool.EditText.EditText.Mid(results[i].gameObject.name, 4, results[i].gameObject.name.Length - 4);
            //Debug.Log(btnName);

            if (btnName == "Shop")
            {
                LandManager.instance.views.popupView.GetComponent<PopupViewController>().shopPop.SetActive(true);
                return true;                
            }
            else if (btnName == "Complite")
            {
                return true;
            }            
        }
        return false;
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
        GraphicRaycaster gr = PuzzleManager.instance.viewCon.GetComponent<Canvas>().GetComponent<GraphicRaycaster>();
        gr.Raycast(ped, results);
        
        return results;
    }
}
