using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Search_U());
	}

    IEnumerator Search_U()
    {
        float sec = 0.01f;
        float time = 0;
        while (true)
        {
            time = time + sec;

            GameObject target = LandManager.instance.GetComponent<UnitManager>().SearchUnit("0109");
            if(target != null)
            {
                GetComponent<MoveupController>().StopAllCoroutines();
            }
            else
            {
                GetComponent<MoveupController>().MoveUp(target);
            }
            yield return new WaitForSeconds(sec);
        }

    }


}
