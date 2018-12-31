using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JurorDeskController : MonoBehaviour {

    Vector3 firstPos = new Vector3(0, -2.3f, -5.3f);//-20~20, -3f+y;
    string[] targetIDs = { "0202", "0203" };

    void Start()
    {
        SetBase();
        EventManager.instance.WeatherChangedEvent += (SetBase);
    }

    void SetBase()
    {
        transform.localPosition = firstPos;
        IdleSelect();
    }

    public void IdleSelect()
    {
        for (int i = 0; i < transform.GetChildCount(); i++) {
            if (UserManager.Instance.GetWeather(LandManager.instance.currentLand.id) == 1)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                #region FightController   
                GetComponent<FightController>().oneHit = true;
                GetComponent<FightController>().weaponID = "0202";
                GetComponent<FightController>().Search_U(Vector3.zero, targetIDs, "D", 3);                
                #endregion
            }//weather case
            else
            {
                GetComponent<FightController>().StopAllCoroutines();
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
