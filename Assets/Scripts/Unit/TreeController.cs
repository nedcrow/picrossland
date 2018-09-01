﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour {

    Vector3 firstPos = new Vector3(0.24f, -0.3f, -3.3f);//-20~20, -3f+y;
    public bool firstTree=false;

    void Start () {
        EventManager.instance.WeatherChangedEvent += (IdleSelect);
        SetTransform();
        StartCoroutine(GrowUp());
    }
    


    void SetTransform()
    {
        int firstTreeCount = 0;
//        Debug.Log(LandManager.instance.GetComponent<UnitManager>().unitList.Count);
        if(LandManager.instance.GetComponent<UnitManager>().unitList.Count > 0)
        {
            for(int i=0; i< LandManager.instance.GetComponent<UnitManager>().unitList.Count; i++)
            {
                if(this.gameObject.name == LandManager.instance.GetComponent<UnitManager>().unitList[i].name)
                {                    
                    if (LandManager.instance.GetComponent<UnitManager>().unitList[i].GetComponent<TreeController>().firstTree == true) { firstTreeCount++; }                   
                }
            }
        }
        //Debug.Log(sameCount);
        if  (firstTreeCount == 0) {
            transform.localPosition = firstPos;
            firstTree = true;
        } //만들어지면 무조건 list에 추가되니까 최소값은 1.
        else {
            int loop = 10;
            
            transform.localPosition = Unit.UnitBase.RandomUnitPos(20, 20);//왠만하면 고정.
            float dir = transform.localPosition.y > 1.1f ? -0.1f : 0.1f;//y값 증감할 때 방향 정한 것.
            
            for (int i=0; i<loop; i++)
            {
                if (SpawnRule.SpawnPossible.UnitSpawnPossibe(transform.name, transform.localPosition))//만약 위치해도 괜찮은 곳이면,
                {
                    i = loop;//for문 종료.
                }
                else
                {
                    if (i / 2 == 0) { transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + dir, transform.localPosition.z + dir); }//y값 증감.   
                    else { transform.localPosition = new Vector3(transform.localPosition.x + dir, transform.localPosition.y, transform.localPosition.z); }//x값 증감.           
                }
            }//10번만 시도.        
        }
    }

    public void IdleSelect()
    {
        if (UserManager.Instance.GetWeather(LandManager.instance.currentLand.id) == 2)//fullMoon
        {
            Unit.UnitBase.Unit_Death(transform.GetChild(0).gameObject);
        }
        else
        {
            Unit.UnitBase.UnitIdle(transform.GetChild(0).gameObject);
        }
    }

    IEnumerator GrowUp() 
    {        
        yield return new WaitForSeconds(0.5f); // about 12f/s.
        float a = 0.6f; //max = 2 = 반원;
        float maxSize = 2f;
        float minSize = 1f;
        float speed = 0.25f;
        float yPos = transform.localPosition.y;
        while (a < 2)
        {
            yield return new WaitForSeconds(0.083f); // about 12f/s.
            float x = (maxSize - minSize) * (float)Math.Sin(a * Math.PI * 0.5f); //0.6~1~0.
            transform.localScale = new Vector3(2, 2 + x, 1);
            transform.localPosition = new Vector3(transform.localPosition.x, yPos + (x*0.5f), transform.localPosition.z);
            a = a + speed;
        }
        transform.localScale = new Vector3(2, 2, 1);//localScale reset.
        transform.localPosition = new Vector3(transform.localPosition.x, yPos, transform.localPosition.z);//localScale reset.
        yield return null;
    }// 한 번 점프 하면서 컸다 작아짐.

}
