using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour {

    public string weaponID;
    public bool searchMode = false;
    public bool atkMode = false; 
    public bool afraideMode = false; 
    public bool oneHit = false; //true면 한 대만 침.
    public bool dead = false;
    public float AtkDelayA = 0.4f;
    public float AtkDelayB = 0.4f;
    public float HP = 0;
    public float AtkPoint = 0;
    public Vector3 firstPos;
    public Vector3 lastTargetPos; // 비교할일 없으면 삭제예정.
    public List<GameObject> targetList;
    public List<GameObject> hitList;
    GameObject target;
    

    private void Start()
    {        
        EventManager.instance.AttackedEvent += (HPCheck);
    }


    #region search
    /// <summary>
    ///  addPos : for speed, mode = M,D
    /// </summary>
    /// <param name="addPos"></param>
    /// <param name="targetID"></param>
    /// <param name="mode"> M : navigation , D : who is afraid. </param>
    public void Search_U(Vector3 addPos, string[] targetIDs, string mode = "", float range = 0.0f)
    {
        hitList = new List<GameObject>();
        StopAllCoroutines();
        StartCoroutine(Search_U_Co(addPos, targetIDs, mode, range));
    }
        /// <summary>
        /// 타겟'들'중, 가장 가까운 대상을 타겟으로 지정하고, 타입에 맞춰 찾는다.  
        /// </summary>
        /// <param name="addPos"></param>
        /// <param name="targetIDs"></param>
        /// <param name="mode">M : 이동 탐색, D : 재자리 탐색</param>
        /// <param name="range"></param>
        /// <returns></returns>
    IEnumerator Search_U_Co(Vector3 addPos, string[] targetIDs, string mode = "", float range = 0.0f)
    {
        float sec = 0.01f;
        float time = 0; //searching 확인 용.
        targetList = LandManager.instance.GetComponent<UnitManager>().SearchUnits(transform.position, targetIDs, false);
        searchMode = true;
        while (true)
        {
            time = time + sec;
            //Debug.Log(targetList.Count + ", " + atkMode + ", " + afraideMode);
            if (hitList.Count == targetList.Count || atkMode == true || afraideMode == true)
            {
                if(hitList.Count == targetList.Count) { Debug.Log(name +" End Searching _ All Target Hit"); break; } // Target이 Land에 없으면 search 종료.
                else if(TargetDeadAll(targetList) == true) { Debug.Log(name + " End Searching _ All Target Dead"); break; } // 모든 Target이 죽어있으면 search 종료.
                //조건별 타겟 리셋
            }
            else
            {
                SelectTarget(LandManager.instance.GetComponent<UnitManager>().SearchUnits(transform.position, targetIDs, true));
                if (mode == "M" && target != null)
                {
                    if (FrontTarget(range) == true && searchMode == false)
                    {
                        StartCoroutine(EyeShoping_A(range));
                        break;
                    }
                    else if (searchMode == true)
                    {
                        Vector3 tPos = (target.transform.localPosition) + addPos;
                        GetComponent<MoveupController>().MoveUp(tPos, 1);
                        searchMode = false;
                    }
                    else if (GetComponent<MoveupController>().goal == true) { searchMode = true; }
                }//이동식 1회용 탐색모드. 갔는데 없으면 다시 탐색.
                else if (mode == "D" )
                {
                    if (FrontTarget(range) == true && afraideMode == false)
                    {
                        yield return new WaitForSeconds(0.5f);
                        Afraide_U();
                        yield return new WaitForSeconds(AtkDelayB);
                    }
                }//재자리 지속 탐색모드.
            }
            //Debug.Log("Searching : " + time);
            yield return new WaitForSeconds(sec);
        }
    }
    #endregion   

    /// <summary>
    /// neer by target.
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    bool FrontTarget(float range)
    {
        bool success = false;
        for(int i=0; i< targetList.Count; i++)
        {
            Vector3 mPos = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
            Vector3 tPos = new Vector3(targetList[i].transform.localPosition.x, targetList[i].transform.localPosition.y, 0);
            lastTargetPos = tPos;
            float dist = Vector3.Distance(tPos, mPos);
            
            if (dist < range && target != null)
            {
                if(targetList[i].transform.position == target.transform.position) { success = true; }                
            }            
        }
        return success;
    }//Target이 Range 안에 들어오면 True.

    void SelectTarget(List<GameObject> tList)
    {
        target = null;
        List<GameObject> tempList = new List<GameObject>();

        #region minusHits
        bool OutOfHitList = true;        
        for (int i = 0; i < tList.Count; i++)
        {
            for (int j = 0; j < hitList.Count; j++)
            {
                //Debug.Log(tList[i].GetComponent<UnitBase>().unitNum + " vs " + hitList[j].GetComponent<UnitBase>().unitNum);
                if (tList[i] == hitList[j]) {
                    OutOfHitList = false;
                    if (tList[i].GetComponent<UnitBase>()) {
                        //Debug.Log(tList[i].GetComponent<UnitBase>().unitNum + " vs " + hitList[j].GetComponent<UnitBase>().unitNum);
                        if (tList[i].GetComponent<UnitBase>().unitNum != hitList[j].GetComponent<UnitBase>().unitNum) { OutOfHitList = true; Debug.Log("Add TempTargetList"); }
                    }
                }
                //Debug.Log("OutOfHitList : " + OutOfHitList);
            }
            if (OutOfHitList == true) { tempList.Add(tList[i]); } else {  }
        }//hitList GameObject 제외.
        #endregion

        //if (tempList.Count > 0) { target = tempList[0]; Debug.Log("Target : "+target.name + ", "+transform.position); }
        if(tempList != null)
        {
            if(tempList.Count > 0) {
                target = tempList[0];
                if (target.GetComponent<FightController>())
                {
                    if (target.GetComponent<FightController>().dead == true) { target = null; Debug.Log("Target Null"); }
                }
            }
        }
        
    }

    bool TargetDeadAll(List<GameObject> targetList) {
        bool someLive = false;
        foreach(GameObject t in targetList)
        {
            if (t.GetComponent<FightController>()) {
                if (t.GetComponent<FightController>().dead == false)
                {
                    someLive = true;
                }
            }
        }
        return !someLive;
    }

    public void HPCheck(GameObject attacker, GameObject target, int unitNum)
    {
        Debug.Log(name + " HPCheck, Attacker : " + attacker.name);
        if (target == gameObject && unitNum == GetComponent<UnitBase>().unitNum && HP <= 0 && dead == false)
        {
            if (transform.GetChild(transform.GetChildCount() - 1))
            {
                if (transform.GetChild(transform.GetChildCount() - 1).GetComponent<MarkNineTeen>())
                {
                    transform.GetChild(transform.GetChildCount() - 1).GetComponent<MarkNineTeen>().NineTeenMotion(1.5f);
                }
            }// childs에 19금 obj가 붙어 있으면, 19금 obj 모션 실행.
            Unit.UnitBase.Unit_Death(transform.GetChild(0).gameObject);
            if (hitList.Find(x => x == target) != null) { hitList.Remove(target); } // hitList에서 Target이 있으면 리무브.
            dead = true;
            atkMode = false;
            afraideMode = false;
            StopAllCoroutines();
        }//Target이 일치하고, HP가 0이하, dead가 false일 때
    }

    #region Afraide 
    void Afraide_U()
    {
        if (transform.GetChildCount()>0) { Unit.FighterMotion.Afraide(transform.GetChild(0).gameObject); }   //childs 가 1 이상이면, AfraideAni    
        if (afraideMode == false) { StartCoroutine(Afraide_U_Co(AtkDelayA)); }
    }

    IEnumerator Afraide_U_Co(float cutLine = 1)
    {
        afraideMode = true;
        float sec = 0.1f;
        float time = 0;//target이 근처에 있을 때부터 누적.
        while (true)
        {
            time = time + sec;
//            Debug.Log("Time + FrontTarget : " + time + ", " + FrontTarget(0.7f));
            if (time > cutLine && FrontTarget(0.7f)==true) {
                List<string> clearPuzzleList = UserManager.Instance.GetCurrentInGotLandList(LandManager.instance.currentLand.id).clearPuzzleList;
                for (int i=0; i< clearPuzzleList.Count; i++)
                {
                    if (clearPuzzleList[i] == weaponID)
                    {      
                        atkMode = true;
                        afraideMode = false;
                        StartCoroutine(AtkMode_U_Co());                        
                        break;
                    }
                }
                //afraideMode = false;
                //break;
            }
            yield return new WaitForSeconds(sec);
        }
    }
    #endregion 
    //일정기간 동안 적이 계속 근처에 있으면, 무기가 있으면, 공격모드로 바뀜.

    void Attack_U(bool hpAtk = false, GameObject potalTarget = null)
    {
        #region animation
        if (potalTarget != null)
        {
            transform.position = potalTarget.transform.position;
        }
        else
        {
            if (transform.GetChildCount() > 0) { Unit.FighterMotion.Attack(transform.GetChild(0).gameObject); }//childs 가 1 이상이면, AttackAni
        }
        #endregion

        #region HP
        if (hpAtk == true && target.GetComponent<FightController>())
        {
            target.GetComponent<FightController>().HP = target.GetComponent<FightController>().HP - AtkPoint;
            if (target.GetComponent<FightController>().HP < 0) { target.GetComponent<FightController>().HP = 0; }  
        }
        #endregion

        EventManager.instance.AttackedFunc(gameObject, target, target.GetComponent<UnitBase>().unitNum);
    }//hpAtk이 트루면 HP감소( 기본 false). potalTarget이 있으면 해당 위치로 순간이동.

    IEnumerator AtkMode_U_Co()
    {
        float sec = 0.02f;     
        while (true)
        {
            if (FrontTarget(0.7f)==true && atkMode==true)
            {
                if (oneHit == true)
                {
                    if (hitList.Count == targetList.Count) { break; } // target 다 때렸으면 끝.
                    else
                    {
                        bool success = false;
                        
                        if (hitList.Find(x => x == target) == null) {
                            success = true;
                        }
                        else if (hitList.Find(x => x == target).GetComponent<UnitBase>() != null)
                        {
                            int unitNum = hitList.Find(x => x == target).GetComponent<UnitBase>().unitNum; //Debug.Log(hitList.Find(x => x == target)+" : "+unitNum);
                            if (unitNum != target.GetComponent<UnitBase>().unitNum) { success = true; }
                        }
                        
                        if(success == true) {
                            hitList.Add(target);
//                            targetList.Remove(target);
                            Attack_U();
                            atkMode = false; Debug.Log("LastAttack");
                            break;
                        }//hitList에 target이 없으면 추가.
                    }
                }
                else
                {
                    Attack_U(); Debug.Log("ContinueAttack");
                    yield return new WaitForSeconds(AtkDelayB);
                }//oneHit면 한 번만 때리고 공격 끝.  
            }
            else
            {
                //Afraide_U();
                atkMode = false;
                break;
            }
            yield return new WaitForSeconds(sec);
        }
    }//target이 벗어날때 까지 주시. 일정기간 안에 안 나가면 공격.

    IEnumerator EyeShoping_A(float range)
    {
        float sec = 0.02f;
        float time = 0;
        while (true)
        {
//            Debug.Log(target.name + " : " + target.GetComponent<FightController>().atkMode);
            if (FrontTarget(range)==true)//타겟이 눈앞이고,
            {
                if (target.GetComponent<FightController>())
                {
                    //Debug.Log(target.name + " : " + target.GetComponent<FightController>().atkMode);
                    if (target.GetComponent<FightController>().atkMode == true) //타겟이 공격모드면
                    {
                        yield return new WaitForSeconds(2f); //잠시 상황보다가
                        Unit.FighterMotion.Afraide(transform.GetChild(0).gameObject); //놀라고
                        yield return new WaitForSeconds(0.35f);
                        atkMode = false;
                        GetComponent<MoveupController>().MoveUp(firstPos,0.1f); //튄다.     
                        atkMode = false;
                        break;
                    }
                    else
                    {
                        if (time > 3 && target.GetComponent<FightController>().HP > 0) {
                            Unit.FighterMotion.Attack(transform.GetChild(0).gameObject);
                            yield return new WaitForSeconds(AtkDelayB);
                            Attack_U(true, target);
                            yield return new WaitForSeconds(AtkDelayB);                            
                        }
                        else if(target.GetComponent<FightController>().HP <= 0)
                        {
                            yield return new WaitForSeconds(1.2f);
                            atkMode = false;
                            GetComponent<MoveupController>().MoveUp(firstPos, 0.1f); //귀가.    
                            Debug.Log(firstPos);
                            break;
                        }
                    }
                }
            }
            time = time + sec;
            yield return new WaitForSeconds(sec);
        }
    }//공격해도 되는 상대면 공격하고 HP를 0으로 만들면 집으로 귀가(Afraid 켬).

   

}

