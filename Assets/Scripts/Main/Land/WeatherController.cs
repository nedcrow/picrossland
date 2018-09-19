using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherController : MonoBehaviour {

    Dictionary<string, string> weatherDic = new Dictionary<string, string>()
    {
        {"0101","GoodMorning"},
        {"0103","GoodNight" },
        {"0108","GoodNight" },
        {"0201","RadioActive" }
    };

    public void OnWeather(string skillId)
    {
        Debug.Log("OnWeather : "+skillId);
        try
        {
            StartCoroutine(weatherDic[skillId]);
        }
        catch(Exception ex)
        {
            Debug.Log("no weatherDic in WeatherController : "+ skillId);
        }

    }

    public void OffWeather()
    {
        StopAllCoroutines();
        int childCnt = transform.childCount;
        if (childCnt > 0)
        {
            for(int i=0; i<childCnt; i++)
            {
                Destroy(transform.GetChild(childCnt - i - 1).gameObject);
            }
        }

    }

    IEnumerator GoodMorning()
    {
        GameObject tempSky = new GameObject();
        //Instantiate(new GameObject(), new Vector3(2.5f, 0, -5), Quaternion.identity);
        tempSky.name = "sky";
        tempSky.transform.position = new Vector3(2.5f, 0, -8);
        tempSky.transform.localScale = new Vector3(12, 12, 1);
        tempSky.transform.SetParent(transform);
        tempSky.AddComponent<SpriteRenderer>();
        tempSky.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/cursor");
        tempSky.GetComponent<SpriteRenderer>().color = new Vector4(0,0,0,1);  
        float r=0.2f;
        float g=0.1f;
        float b = 0.1f;
        float a=0;

        if (UserManager.Instance.GetCurrentInGotLandList(LandManager.instance.currentLand.id).clearPuzzleList.Count == 1)
        {
            LandManager.instance.views.popupView.GetComponent<PopupViewController>().guidePop.SetActive(true);
        }//clearPuzzle이 하나 뿐이면, 

        while (true)
        {
            if(MainDataBase.instance.loadAll == true)
            {
                r = r + 0.01f;
                g = g + 0.01f;
                b = b + 0.015f;
                a = a + 0.01f;
            }            
            tempSky.GetComponent<SpriteRenderer>().color = new Vector4(0+r, 0+g, 0+b, 1-a);
            if (a > 0.98f) { Destroy(tempSky.gameObject); break; }           
            //Debug.Log("GoodMorning");
            yield return new WaitForSeconds(0.08f);
        }

        #region etc_for_Units
        GameObject units = PuzzleManager.instance.currentLandObj.GetComponent<LandController>().units;
        for (int i = 0; i < units.transform.childCount; i++)
        {
            if (units.transform.GetChild(i).GetComponent<WolfController>()) { units.transform.GetChild(i).GetComponent<WolfController>().IdleSelect(); }
        }
        #endregion

    }//검은 렌더러의 알파값을 감소시킴.

    IEnumerator GoodNight()
    {
        GameObject tempSky = new GameObject();
        //Instantiate(new GameObject(), new Vector3(2.5f, 0, -5), Quaternion.identity);
        tempSky.name = "sky";
        tempSky.transform.position = new Vector3(2.5f, 0, -8);
        tempSky.transform.localScale = new Vector3(12, 12, 1);
        tempSky.transform.SetParent(transform);
        tempSky.AddComponent<SpriteRenderer>();
        tempSky.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/cursor");
        tempSky.GetComponent<SpriteRenderer>().color = new Vector4(1, 0, 0.5f, 0);
        float r = 1f;
        float g = 0.4f;
        float b = 0.4f;
        float a = 0;
        while (true)
        {
            if (MainDataBase.instance.loadAll == true)
            {
                r = r - 0.028f;
                g = g - 0.025f;                
                a = a + 0.015f;
            }
            tempSky.GetComponent<SpriteRenderer>().color = new Vector4(r,g,b,a);
            if (a > 0.4f) { break; }
            //Debug.Log("GoodNight");
            yield return new WaitForSeconds(0.08f);
        }
    }//검은 알파값을 증가시킴.

    IEnumerator RadioActive()
    {
        GameObject radio = transform.parent.GetComponent<LandController>().backgroundObj.transform.GetChild(0).GetChild(0).gameObject;
        radio.GetComponent<Animator>().Play("Radio_Active");
        yield return null;
    }
}
