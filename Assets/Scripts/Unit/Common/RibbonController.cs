using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbonController : MonoBehaviour {

    List<GameObject> flowerList = new List<GameObject>();
    List<GameObject> flowerList_active = new List<GameObject>();

    Vector3[] firstPos = { new Vector3(0f, 0f, -3f), new Vector3(0f, 0f, -3f) };//-20~20, -3f+y;

    int cityzenCount;
    string[] weaponIDs = { "0202", "0203" };
    string[][] targetIDs = { new string[] { "0202" }, new string[] { "0203" } };


    void Start () {
        EventManager.instance.WeatherChangedEvent += (SetBase);
        SetBase();
    }
	
	void SetBase () {
        transform.localPosition = firstPos[GetComponent<UnitBase>().unitNum];

        cityzenCount = LandManager.instance.GetComponent<UnitManager>().SearchUnits(transform.position, targetIDs[GetComponent<UnitBase>().unitNum], false).Count; //Debug.Log(cityzenCount);
        ReadyFlower(cityzenCount);

        GetComponent<FightController>().oneHit = true;
        GetComponent<FightController>().weaponID = weaponIDs[GetComponent<UnitBase>().unitNum];
        GetComponent<FightController>().Search_U(Vector3.zero, targetIDs[GetComponent<UnitBase>().unitNum], "D", 0.5f);
    }

    void ReadyFlower(int count)
    {
        if(flowerList_active.Count > 0)
        {
            int cnt = flowerList_active.Count;
            for (int i=0; i< cnt; i++)
            {
                flowerList.Add(flowerList_active[0]);
                flowerList_active.RemoveAt(0);
            }
        }

        if(flowerList.Count < count)
        {
            int cnt = count - flowerList.Count;
            for (int i = 0; i < cnt; i++)
            {
                flowerList.Add(new GameObject());
            }
        }

        Sprite[] flower_s = Resources.LoadAll<Sprite>("Sprite/Puzzle/0209_Flower");
        for (int i=0; i< flowerList.Count; i++)
        {            
            flowerList[i].name = "flower";
            flowerList[i].transform.SetParent(transform);
            flowerList[i].AddComponent<SpriteRenderer>();
            flowerList[i].GetComponent<SpriteRenderer>().sprite = flower_s[GetComponent<UnitBase>().unitNum];
            flowerList[i].transform.localPosition = new Vector3(-10,-10,0);
            flowerList[i].transform.localScale = new Vector3(0.25f, 0.25f, 1);
            //flowerList[k].SetActive(false);
        }
    }

    public void Hit(GameObject attacker, GameObject target)
    {
        if(target == gameObject)
        {
            int plus = attacker.name == "0202" ? 1 : -1;
            float x = plus * -0.24f * (attacker.GetComponent<UnitBase>().unitNum + 1);
            flowerList[0].transform.localPosition = new Vector3(x, 1.3f, 0.45f);
            flowerList_active.Add(flowerList[0]);
            flowerList.RemoveAt(0);
            //Debug.Log(string.Format("x :{0}",x));
            Debug.Log("Hit_0209 : End");
        }
    }
}
