using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JurorDeskController : MonoBehaviour {

    Vector3 firstPos = new Vector3(0, -2.3f, -5.3f);//-20~20, -3f+y;
    Vector3[] seatPos = {
        new Vector3(-0.657f, -1.506f, -4.506f), new Vector3(0f, -1.506f, -4.506f), new Vector3(0.657f, -1.506f, -4.506f),
        new Vector3(-0.657f, -2.21f, -5.21f), new Vector3(0f, -2.21f, -5.21f), new Vector3(0.657f, -2.21f, -5.21f)
    };
    string[] targetIDs = { "0202", "0203" };
    List<GameObject> seatList = new List<GameObject>();

    void Start()
    {
        SetBase();
        EventManager.instance.WeatherChangedEvent += (SetBase);
    }

    void SetBase()
    {
        seatList = new List<GameObject>();
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
                StartCoroutine(TargetSeatCheck());
                #endregion
            }//weather case
            else
            {
                GetComponent<FightController>().StopAllCoroutines();
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    IEnumerator TargetSeatCheck()
    {
        List<bool> targetSeatList;

        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (GetComponent<FightController>().hitList != null)
            {                
                targetSeatList = new List<bool>();
                int i = 0;
                foreach(GameObject target in GetComponent<FightController>().hitList)
                {
                    if (target.GetComponent<MoveupController>().lastGoal == true) {

                        if (CheckSeatList(target) == false) {
                            Debug.Log("add target to seatList");
                            seatList.Add(target);
                            if (target.GetComponent<CitizenAController>() != null) { target.GetComponent<CitizenAController>().Seat(seatPos[i]); }//Target이 앉을 준비가 되었으면 앉으라고 명령.
                            else if (target.GetComponent<CitizenBController>() != null) { target.GetComponent<CitizenBController>().Seat(seatPos[i]); }
                        }                        
                        i++;
                    }
                }
                if (GetComponent<FightController>().hitList.Count >= 6) { break; }
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    bool CheckSeatList(GameObject target)
    {
        bool seatTarget = false;
        if(seatList != null)
        {
            foreach(GameObject seat in seatList)
            {
                if(seat == target) { seatTarget = true; }
            }
        }
        return seatTarget;
    }
}
