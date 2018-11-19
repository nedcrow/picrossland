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

    public delegate void LandActivated(Vector3 target, float waitTime, bool run);
    public event LandActivated LandActivatedEvent;

    public delegate void NickNameChecked(bool success);
    public event NickNameChecked NickNameCheckedEvent;
    
    public delegate void Attacked(GameObject attacker, GameObject target, int unitNum);
    public event Attacked AttackedEvent;
    public event Attacked TempAttackedEvent;
    //--------------------------------------------------------------------Event



    public void WeatherChangedFunc()//---------------메뉴명 확인
    {
        int EventCount = 0;
        try
        {
//            EventCount = WeatherChangedEvent.GetInvocationList().Length;
            List<GameObject> unitList = LandManager.instance.GetComponent<UnitManager>().unitList;
            Debug.Log("weatherChange0");
            if (WeatherChangedEvent != null)
            {
                //TempWeatherEvent = new WeatherChanged();
                foreach (WeatherChanged d in WeatherChangedEvent.GetInvocationList())
                {
                    string unitID = HarimTool.EditValue.EditText.Left(d.Target.ToString(), 4);
                    //Debug.Log(unitID);
                    bool sleepCheck = true;
                    if (LandManager.instance.GetComponent<UnitManager>().SearchUnit(unitID) != null)
                    {
                        if (LandManager.instance.GetComponent<UnitManager>().SearchUnit(unitID).transform.parent.parent.gameObject.activeSelf == true)
                        {
                            sleepCheck = false; //찾는 GameObject가 Active상태면 비활성화시키지 않음.
                        }
                    }
                    if (sleepCheck == true)
                    {
                        WeatherChangedEvent -= d;
                        TempWeatherEvent += d;
                        //Debug.Log("TempWeatherEvent.GetInvocationList().Length : " + TempWeatherEvent.GetInvocationList().Length);
                    } //active fales 또는 존재하지 않는 GameObject의 Event를 임시보관함.     

                }
            }
            Debug.Log("weatherChange1");
            if (WeatherChangedEvent != null) { if (WeatherChangedEvent.GetInvocationList() != null) { WeatherChangedEvent(); } }
            Debug.Log("weatherChange2");
        }
        catch { Debug.Log("Error_weatherChange"); } 
        if (TempWeatherEvent != null)
        {
            foreach (WeatherChanged d in TempWeatherEvent.GetInvocationList())
            {
                //Debug.Log(d.Target);
                TempWeatherEvent -= d;
                WeatherChangedEvent += d;
            }
        }//Event 실행 후 임시보관 중 Event가 있다면 원래 위치로 복구.      
        //Debug.Log("weatherChange3");
    }

    public void LandActivatedFunc(Vector3 pos = new Vector3(), float waitTime=0, bool run=false)//---------------메뉴명 확인
    {
        try {            
            if (LandActivatedEvent != null) {
                //Debug.Log("LandActiveatedFunc0 : "+ LandActivatedEvent.GetInvocationList().Length);
                //foreach (LandActivated d in LandActivatedEvent.GetInvocationList())
                //{
                //    string unitID = HarimTool.EditValue.EditText.Left(d.Target.ToString(), 4);
                //    Debug.Log(unitID);
                //}
                LandActivatedEvent(pos, waitTime, run);
            }            
        }
        catch { Debug.Log("Error_LandActiveatedFunc"); }
    }//For unit's MoveEvent

    public void NickNameCheckedFunc(bool success)
    {
        try { NickNameCheckedEvent(success); }
        catch { }
    }

    public void AttackedFunc(GameObject attacker, GameObject target, int unitNum=999)
    {
        List<GameObject> unitList = LandManager.instance.GetComponent<UnitManager>().unitList;
        if (AttackedEvent != null)
        {
            foreach (Attacked d in AttackedEvent.GetInvocationList())
            {
                string unitID = HarimTool.EditValue.EditText.Left(d.Target.ToString(), 4);
                //Debug.Log(attacker.name+", "+target.name+", "+ unitID);
                if (LandManager.instance.GetComponent<UnitManager>().SearchUnit(unitID) != null)
                {
                    if (LandManager.instance.GetComponent<UnitManager>().SearchUnit(unitID).transform.parent.parent.gameObject.activeSelf == true)
                    {
                        if (unitID == target.name && attacker.name != target.name)
                        {
                            TempAttackedEvent += d;
                        }//UnitID로 맞는놈이랑 확인했는데, 때리는놈이랑 맞는놈이 다르면,  Event를 임시보관한다.
                    }//찾은 게임오브젝트가 active true이고,
                }//찾는 게임오브젝트가 있고,
            }
        }
        try
        {
            if (TempAttackedEvent != null)
            {
//                Debug.Log("AttackEvent0");
                TempAttackedEvent(attacker, target, unitNum);
                foreach (Attacked d in TempAttackedEvent.GetInvocationList()) { TempAttackedEvent -= d; }//임시보관 초기화.
            }
        }
        catch { Debug.Log("Error_AttackEvent"); }
    }
}
