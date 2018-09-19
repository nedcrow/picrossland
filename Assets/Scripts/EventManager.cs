using UnityEngine;
using System.Collections.Generic;


public class EventManager : MonoBehaviour
{
    #region instance
    private static EventManager _instance;
    public static EventManager instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = GameObject.Find("EventManager");
                if (obj == null)
                {
                    obj = new GameObject("EventManager");
                    obj.AddComponent<EventManager>();
                }
                return obj.GetComponent<EventManager>();
            }
            else
            {
                return _instance;
            }
        }
    }
    #endregion

    public delegate void WeatherChanged();
    public event WeatherChanged WeatherChangedEvent;
    public event WeatherChanged TempWeatherEvent;

    public delegate void LandActivated(Vector3 target, float waitTime);
    public event LandActivated LandActivatedEvent;

    public delegate void NickNameChecked(bool success);
    public event NickNameChecked NickNameCheckedEvent;
    
    public delegate void Attacked(GameObject target);
    public event Attacked AttackedEvent;
    //--------------------------------------------------------------------Event



    public void WeatherChangedFunc()//---------------메뉴명 확인
    {        
        try
        {            
            List<GameObject> unitList = LandManager.instance.GetComponent<UnitManager>().unitList;
            foreach (WeatherChanged d in WeatherChangedEvent.GetInvocationList())
            {
                //Debug.Log("ActivatedFunc :" + d.Target);
                string unitID = HarimTool.EditValue.EditText.Left(d.Target.ToString(), 4);
                //Debug.Log(LandManager.instance.GetComponent<UnitManager>().SearchUnit(unitID).transform.parent.parent.name);
                if (LandManager.instance.GetComponent<UnitManager>().SearchUnit(unitID).transform.parent.parent.gameObject.activeSelf == false) {                    
                    WeatherChangedEvent -= d;
                    TempWeatherEvent += d;
                    Debug.Log("TempWeatherEvent.GetInvocationList().Length : " + TempWeatherEvent.GetInvocationList().Length);
                }
                
            }
            WeatherChangedEvent();            
        }         
        catch { }
        if (TempWeatherEvent != null)
        {
            foreach (WeatherChanged d in TempWeatherEvent.GetInvocationList())
            {
                //Debug.Log(d.Target);
                TempWeatherEvent -= d;
                WeatherChangedEvent += d;
            }
        }
    }

    public void LandActivatedFunc(Vector3 pos = new Vector3(), float waitTime=0)//---------------메뉴명 확인
    {
        try {  LandActivatedEvent(pos,waitTime); }
        catch { }
    }

    public void NickNameCheckedFunc(bool success)
    {
        try { NickNameCheckedEvent(success); }
        catch { }
    }

    public void AttackedFunc(GameObject target=null)
    {
        try { AttackedEvent(target); }
        catch { }
    }

}
