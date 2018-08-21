using UnityEngine;
using System.Collections;


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

    public delegate void LandActivated();
    public event LandActivated LandActivatedEvent;


    //--------------------------------------------------------------------Event



    public void WeatherChangedFunc()//---------------메뉴명 확인
    {
        try { WeatherChangedEvent(); }
        catch { }
    }

    public void LandActivatedFunc()//---------------메뉴명 확인
    {
        try { LandActivatedEvent(); }
        catch { }

    }

}
