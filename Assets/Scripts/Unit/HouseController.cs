using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour {


    Vector3 firstPos = new Vector3(-1.2f, -1.0f, -4.0f);//-20~20, -3f+y;

    void Start()
    {
        SetTransform();
        EventManager.instance.WeatherChangedEvent += (IdleSelect);
    }

    void SetTransform()
    {
        IdleSelect();
        int sameCount = Unit.UnitBase.FindSameUnit(this.gameObject.name).Count;        
        if (sameCount == 1)
        {
            transform.localPosition = firstPos;
        } //만들어지면 무조건 list에 추가되니까 최소값은 1.
        else
        {
            Debug.Log("To Much Same Count : Please Check DB.");      
        }
    }

    public void IdleSelect()
    {
        if (UserManager.Instance.GetWeather(LandManager.instance.currentLand.id) == 0)//Day
        {
            Unit.UnitBase.UnitIdle(transform.GetChild(0).gameObject);
        }
        else
        {
            Unit.UnitBase.UnitIdle(transform.GetChild(0).gameObject, "_B");
        }
    }

}
