using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbonController : MonoBehaviour {

    List<GameObject> flowerList = new List<GameObject>();

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

        cityzenCount = LandManager.instance.GetComponent<UnitManager>().SearchUnits(transform.position, targetIDs[GetComponent<UnitBase>().unitNum], false).Count; Debug.Log(cityzenCount);
        ReadyFlower(cityzenCount);

        GetComponent<FightController>().oneHit = true;
        GetComponent<FightController>().weaponID = weaponIDs[GetComponent<UnitBase>().unitNum];
        GetComponent<FightController>().Search_U(Vector3.zero, targetIDs[GetComponent<UnitBase>().unitNum], "D", 0.5f);
    }

    void ReadyFlower(int count)
    {
        Sprite flower_s = Resources.Load<Sprite>("Sprite/Puzzle/0209_Flower");
        for(int i=0; i< count; i++)
        {
            flowerList.Add(new GameObject());
            int k = flowerList.Count - 1;
            flowerList[k].name = "flower";
            flowerList[k].transform.SetParent(transform);
            flowerList[k].AddComponent<SpriteRenderer>();
            flowerList[k].GetComponent<SpriteRenderer>().sprite = flower_s;           
            //flowerList[k].SetActive(false);
        }
    }

    public void Hit(GameObject attacker, GameObject target)
    {
        int x = GetComponent<UnitBase>().unitNum == 0 ? 1 : -1;       
        flowerList[0].transform.position = new Vector3(x * -0.24f* (GetComponent<UnitBase>().unitNum+1), 1.3f, 0.45f);
    }
}
