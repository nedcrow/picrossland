using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour {

    public string weaponID;
    public bool atkMode = false;
    public Vector3 firstPos;
    GameObject target;
    /// <summary>
    /// mode = M,D
    /// </summary>
    /// <param name="addPos"></param>
    /// <param name="targetID"></param>
    /// <param name="mode"></param>
    public void Search_U(Vector3 addPos, string targetID, string mode = "", float range = 0.0f) { StartCoroutine(Search_U_Co(addPos, targetID, mode, range)); }
    
    IEnumerator Search_U_Co(Vector3 addPos, string targetID, string mode = "", float range = 0.0f)
    {
        float sec = 0.01f;
        float time = 0;
        while (true)
        {
            time = time + sec;

            target = LandManager.instance.GetComponent<UnitManager>().SearchUnit(targetID);            
            if (target == null)
            {
                if (GetComponent<MoveupController>()) { GetComponent<MoveupController>().StopAllCoroutines(); }
            }
            else
            {
                if(mode == "M")
                {                    
                    if(atkMode == false) {
                        Vector3 tPos = (target.transform.localPosition) + addPos;
                        GetComponent<MoveupController>().MoveUp(tPos,1);
                        atkMode = true;
                    }
                    else
                    {
                        if (FrontTarget(range)) {
                            Debug.Log("stopMove");
                            StartCoroutine(EyeShoping_A(range));
                            break;
                        }
                    }
                }
                else if(mode == "D"){                                       
                    if (FrontTarget(range)) {
                        yield return new WaitForSeconds(0.5f);
                        Afraide_U();
                        break;
                    }                    
                }
            }
            yield return new WaitForSeconds(sec);
        }
    }

    bool FrontTarget(float range)
    {
        Vector3 tPos = new Vector3(target.transform.localPosition.x, target.transform.localPosition.y, 0);
        Vector3 mPos = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        float dist = Vector3.Distance(tPos, mPos);
        if (dist < range)
        {
            return true;
        }
        else { return false; }
    }

    void Afraide_U()
    {
        Unit.Fighter.Afraide(transform.GetChild(0).gameObject);
        StartCoroutine(Afraide_U_Co());
    }

    IEnumerator Afraide_U_Co()
    {
        float sec = 0.1f;
        float time = 0;
        float cutLine = 1;
        while (true)
        {
            time = time + sec;
            if (time > cutLine && FrontTarget(0.7f)) {
                List<string> clearPuzzleList = UserManager.Instance.GetCurrentInGotLandList(LandManager.instance.currentLand.id).clearPuzzleList;
                for(int i=0; i< clearPuzzleList.Count; i++)
                {
                    if (clearPuzzleList[i] == weaponID)
                    {
                        yield return new WaitForSeconds(1.2f);
                        Unit.Fighter.Attack(transform.GetChild(0).gameObject);
                        atkMode = true;
                        StartCoroutine(AtkMode_U_Co());
                        break;
                    }
                }
                break;
            }
            yield return new WaitForSeconds(sec);
        }        
    }

    IEnumerator AtkMode_U_Co()
    {
        float sec = 0.02f;     
        while (true)
        {
            if (!FrontTarget(1f))
            {
                StopAllCoroutines();
                Afraide_U();
                atkMode = false;
            }
            yield return new WaitForSeconds(sec);
        }
    }

    IEnumerator EyeShoping_A(float range)
    {
        float sec = 0.02f;
        while (true)
        {
            if (FrontTarget(range))//타겟이 눈앞이고,
            {
                if (target.GetComponent<FightController>())
                {
                    if (target.GetComponent<FightController>().atkMode == true) //타겟이 공격모드면
                    {
                        yield return new WaitForSeconds(2f); //잠시 상황보다가
                        Unit.Fighter.Afraide(transform.GetChild(0).gameObject); //놀라고
                        yield return new WaitForSeconds(0.35f);
                        GetComponent<MoveupController>().MoveUp(firstPos,0.1f); //튄다.
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(sec);
        }
    }
}

