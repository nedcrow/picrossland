﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour {

    public string weaponID;
    public bool gotWeapon = false;
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
    float range;
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
        this.range = range;
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
        atkMode = false;
        afraideMode = false;
        while (true)
        {
            time = time + sec;
            //if(hitList.Count != null) { Debug.Log(hitList.Count + ", " + atkMode + ", " + afraideMode); }
            if (atkMode == true || afraideMode == true)
            {
                //Debug.Log("Target : " + target.name);
                if (hitList != null && targetList != null)
                {
                    if (hitList.Count == targetList.Count) { Debug.Log(name + " End Searching _ All Target Hit"); break; } // Target이 Land에 없으면 search 종료.
                    else if (TargetDeadAll(targetList) == true) { Debug.Log(name + " End Searching _ All Target Dead"); break; } // 모든 Target이 죽어있으면 search 종료.
                }
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
        if(targetList != null)
        {
            //Debug.Log(target.name+", "+ target.GetComponent<UnitBase>().unitNum);
            for (int i = 0; i < targetList.Count; i++)
            {
                Vector3 mPos = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
                Vector3 tPos = new Vector3(targetList[i].transform.localPosition.x, targetList[i].transform.localPosition.y, 0);
                lastTargetPos = tPos;
                float dist = Vector3.Distance(tPos, mPos); //Debug.Log(name +", dist :"+ dist);   if (name == "0207") { Debug.Log(dist); }
                if (dist < range && target != null)
                {
                    if (targetList[i].transform.position == target.transform.position) { success = true; }
                }
            }
        }
        return success;
    }//Target이 Range 안에 들어오면 True.

    void SelectTarget(List<GameObject> tList)
    {
        target = null;
        List<GameObject> tempList = new List<GameObject>();        
        #region minusHits        
        if (tList != null) {
            for (int i = 0; i < tList.Count; i++)
            {
                bool OutOfHitList = true;
                for (int j = 0; j < hitList.Count; j++)
                {                    
                    if (tList[i] == hitList[j])
                    {
                        OutOfHitList = false;
                        if (tList[i].GetComponent<UnitBase>())
                        {
                           // Debug.Log( tList[i].name + " : " + tList[i].GetComponent<UnitBase>().unitNum + " vs " + hitList[j].name + " : " + hitList[j].GetComponent<UnitBase>().unitNum);
                            if (tList[i].GetComponent<UnitBase>().unitNum != hitList[j].GetComponent<UnitBase>().unitNum) { OutOfHitList = true; }
                        }
                    }                   
                    //타입이 같아도 unitNum이 다르면 tempList에 추가.
                }
                //Debug.Log(tList[i].name + "'s OutOfHitList : " + OutOfHitList);
                if (OutOfHitList == true) {
                    bool dead = false;
                    if (tList[i].GetComponent<FightController>())
                    {
                        if (tList[i].GetComponent<FightController>().dead == true) { dead = true; }
                    }
                    if(dead == false) { tempList.Add(tList[i]); }                   
                } //HitList 밖이고 Dead상태가 아니면 TempList에 추가.
            }//targetList에서 hitList GameObject들을 제외.
        }
        #endregion
        if (tempList != null)
        {
            if(tempList.Count > 0) {
                target = tempList[0]; // Debug.Log("Target : " + target.name + ", " +target.GetComponent<UnitBase>().unitNum); 
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
            else {
                someLive = true;
            }
        }
        return !someLive;
    }//Target에 FightController를 참고하여 죽었는지 확인. 

    public void HPCheck(GameObject attacker, GameObject target, int unitNum)
    {
        Debug.Log(name + " HPCheck, Attacker : " + attacker.name);
        if (GetComponent<UnitBase>()) {
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
    }

    #region Afraide 
    Coroutine Afraide;
    void Afraide_U()
    {
        if (transform.GetChildCount()>0) { Unit.FighterMotion.Afraide(transform.GetChild(0).gameObject); }   //childs 가 1 이상이면, AfraideAni    
        if (afraideMode == false) { Afraide = StartCoroutine(Afraide_U_Co(AtkDelayA)); }
    }

    IEnumerator Afraide_U_Co(float cutLine = 1)
    {
        afraideMode = true;
        float sec = 0.1f;
        float time = 0;//target이 근처에 있을 때부터 누적.
        while (true)
        {
            time = time + sec;
            //Debug.Log(name + " : FrontTarget(0.7) : " + FrontTarget(0.7f));
            if (time > cutLine && FrontTarget(range)==true) {
                List<string> clearPuzzleList = UserManager.Instance.GetCurrentInGotLandList(LandManager.instance.currentLand.id).clearPuzzleList;
                for (int i=0; i< clearPuzzleList.Count; i++)
                {
                    if (clearPuzzleList[i] == weaponID)//무기가 있으면
                    {
                        gotWeapon = true;
                        atkMode = true;
                        afraideMode = false;
                        StartCoroutine(AtkMode_U_Co());
                        break;
                    }
                    else {

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
    
    IEnumerator AtkMode_U_Co()
    {
        float sec = 0.02f;     
        while (true)
        {
            if (FrontTarget(range)==true && atkMode==true)
            {
                if (oneHit == true)
                {
                    //Debug.Log(target.name + " in TargetList ( "+targetList.Count+" )");
                    if (hitList.Count == targetList.Count) { Debug.Log("HitAll"); break; } // target 다 때렸으면 끝.
                    else
                    {
                        
                        bool HitListAddable = false;
                        
                        if (hitList.Find(x => x == target) == null) {
                            HitListAddable = true;
                        }//hitList에 타겟이 없으면
                        else if (hitList.Find(x => x == target).GetComponent<UnitBase>() != null)
                        {
                            int unitNum = hitList.Find(x => x == target).GetComponent<UnitBase>().unitNum; Debug.Log(target.name+": " + target.GetComponent<UnitBase>().unitNum + " vs "+ hitList.Find(x => x == target)+" : "+unitNum);
                            if (unitNum != target.GetComponent<UnitBase>().unitNum) { HitListAddable = true; }
                        }//hitList에 타겟이 있는데, unitNum이 다르면

                        if (HitListAddable == true) {
                            hitList.Add(target);
                            Attack_U();
                            atkMode = false; //Debug.Log(name+", LastAttack");
                            StopCoroutine(Afraide);
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
    }//hpAtk이 true면 HP감소( 기본 false). potalTarget이 있으면 해당 위치로 순간이동.


    /// <summary>
    /// 상대가 무기 있으면 도망(귀가). 없으면 공격. 상대 HP가 0이면 집으로 귀가(Afraid 켬). 
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
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
                    if (target.GetComponent<FightController>().gotWeapon == true) //타겟이 무기를 가졌으면,
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
    }

   

}

