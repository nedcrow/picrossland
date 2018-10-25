using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoJoneController : MonoBehaviour {

    Vector3 firstPos = new Vector3(1, -1.3f, -4.3f);//-20~20, -3f+y;

    void Start()
    {
        BaseSetting();

        EventManager.instance.WeatherChangedEvent += (BaseSetting);
    }

    void BaseSetting()
    {
        transform.localPosition = firstPos;
        string[] targetIDs = { "0202", "0203" };
        GetComponent<FightController>().oneHit = true;
        GetComponent<FightController>().Search_U(Vector3.zero, targetIDs, "D", 0.5f);
    }
}
