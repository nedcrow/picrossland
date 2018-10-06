using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallotBoxController : MonoBehaviour {

    GameObject ballotBox;
	
	void Start () {
        ballotBox = transform.GetChild(0).gameObject;
        BaseSetting();

    }
		
	void BaseSetting () {
        //Unit.UnitBase.UnitIdle(ballotBox);
        string[] targetIDs = { "0202", "0203" };
        GetComponent<FightController>().Search_U(Vector3.zero, targetIDs, "D", 0.7f);
	}
}
