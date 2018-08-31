﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherController : MonoBehaviour {

    Dictionary<string, string> weatherDic = new Dictionary<string, string>()
    {
        {"0101","GoodMorning"},
        {"0103","GoodNight" },
        {"0108","GoodNight" }
    };

    public void OnWeather(string skillId)
    {
        try
        {
            StartCoroutine(weatherDic[skillId]);
        }
        catch(Exception ex)
        {
            Debug.Log("no weatherDic in WeatherController");
            DebugViewer.Instance.debugTextObjectList[3].GetComponent<Text>().text = ex.ToString();
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
        while (true)
        {
            if(MainDataBase.instance.loadAll == true)
            {
                r = r + 0.01f;
                g = g + 0.01f;
                b = b + 0.015f;
                a = a + 0.025f;
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

    }

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
            if (a > 0.35f) { break; }
            //Debug.Log("GoodNight");
            yield return new WaitForSeconds(0.08f);
        }
    }
}
