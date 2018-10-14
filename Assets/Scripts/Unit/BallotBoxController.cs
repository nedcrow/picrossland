using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallotBoxController : MonoBehaviour {

    GameObject ballotBox;
	
	void Start () {
        ballotBox = transform.GetChild(0).gameObject;
        BaseSetting();

        EventManager.instance.AttackedEvent += (Hit);
    }
		
	void BaseSetting () {
        string[] targetIDs = { "0202", "0203" };
        GetComponent<FightController>().Search_U(Vector3.zero, targetIDs, "D", 0.5f);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    void Hit(GameObject target, int unitNum)
    {
        Debug.Log(target+", "+unitNum);
        if (target.name == gameObject.name && unitNum == 999)
        {
            EffectBasket.EffectBasket.instance.Pickup(transform.GetChild(1).gameObject, 0.2f, 0.02f);
            Unit.Fighter.Hit(transform.GetChild(0).gameObject);
        }
    }
}
