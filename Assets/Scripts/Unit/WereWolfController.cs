﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WereWolfController : MonoBehaviour {


    Vector3 firstPos = new Vector3(1.15f, 0.9f, -2.1f);//-20~20, -3f+y;

    void Start()
    {
        SetTransform();        
        EventManager.instance.WeatherChangedEvent += (SetTransform);
    }

    void SetTransform()
    {
        int sameCount = Unit.UnitBase.FindSameUnit(this.gameObject.name).Count;
        //Debug.Log("unitSameCount : " + sameCount);
        if (sameCount == 1)
        {
            transform.localPosition = firstPos;
        } //만들어지면 무조건 list에 추가되니까 최소값은 1.
        else
        {
            int loop = 10;

            transform.localPosition = Unit.UnitBase.RandomUnitPos(20, 20);//왠만하면 고정.
            float dir = transform.localPosition.y > 1.1f ? -0.1f : 0.1f;//y값 증감할 때 방향 정한 것.

            for (int i = 0; i < loop; i++)
            {
                if (SpawnRule.SpawnPossible.UnitSpawnPossibe(transform.name, transform.localPosition))//만약 위치해도 괜찮은 곳이면,
                {
                    i = loop;//for문 종료.
                }
                else
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + dir, transform.localPosition.z + 0.1f);   //y값 증감.                
                }
            }//10번만 시도.        
        }

        //  --------------  ETC  --------------  //
        IdleSelect();
        if(transform.GetChild(0).gameObject.activeSelf == true)
        {
            GetComponent<MoveupController>().twoDir = true;
            GetComponent<FightController>().AtkPoint = 1;
            GetComponent<FightController>().atkMode = false;
            GetComponent<FightController>().firstPos = firstPos;
            string[] targetIDs = { "0109" };
            GetComponent<FightController>().Search_U(new Vector3(0.5f, -0.3f, -0.3f), targetIDs, "M", 0.7f);
        }
    }

    public void IdleSelect()
    {
        if (UserManager.Instance.GetWeather(LandManager.instance.currentLand.id) == 2)//Midnight
        {
            transform.GetChild(0).gameObject.SetActive(true);
            Unit.UnitBase.UnitIdle(transform.GetChild(0).gameObject);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);            
        }
    }
}
