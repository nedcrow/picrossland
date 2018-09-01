using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour {

    public string weaponID;
    public bool atkMode = false;
    public float AtkDelay = 0.4f;
    public float HP = 0;
    public float AtkPoint = 0;
    public Vector3 firstPos;
    GameObject target;
    bool dead=false;

    #region search
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
    #endregion

    public void HPCheck(GameObject target)
    {
        if(target == gameObject && HP <= 0 && dead == false) {
            StopAllCoroutines();
            if (transform.GetChild(transform.GetChildCount() - 1)) {
                if(transform.GetChild(transform.GetChildCount() - 1).GetComponent<MarkNineTeen>())
                {
                    transform.GetChild(transform.GetChildCount() - 1).GetComponent<MarkNineTeen>().NineTeenMotion(0.5f);
                }                
            }
            Unit.UnitBase.Unit_Death(transform.GetChild(0).gameObject);
            dead = true;
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
    }//덜덜 떨고있다가, 무기가 있으면 공격모드로 바뀜.

    void Attack_U(GameObject moveTarget = null)
    {
        if (moveTarget != null)
        {
            transform.position = target.transform.position;
        }
        if (target.GetComponent<FightController>())
        {
            target.GetComponent<FightController>().HP = target.GetComponent<FightController>().HP - AtkPoint;
            if(target.GetComponent<FightController>().HP < 0) { target.GetComponent<FightController>().HP = 0; }
            EventManager.instance.AttackedFunc(target);
        }
    }//HP감소 시킬 때만 사용. moveTarget이 있으면 해당 위치로 순간이동.

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
    }//target이 벗어날때 까지 주시.

    IEnumerator EyeShoping_A(float range)
    {
        float sec = 0.02f;
        float time = 0;
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
                    else
                    {
                        if (time > 2 && target.GetComponent<FightController>().HP > 0) {
                            Unit.Fighter.Attack(transform.GetChild(0).gameObject);
                            yield return new WaitForSeconds(AtkDelay);
                            Attack_U(target);
                            yield return new WaitForSeconds(AtkDelay);                            
                        }
                        else if(target.GetComponent<FightController>().HP <= 0)
                        {
                            GetComponent<MoveupController>().MoveUp(firstPos, 0.1f); //귀가.
                            break;
                        }
                    }
                }
            }
            time = time + sec;
            yield return new WaitForSeconds(sec);
        }
    }//공격해도 되는 상대면 공격하고 HP를 0으로 만들면 집으로 귀가.


}

