using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveupController : MonoBehaviour {
    public float speed=1;
    public float runSpeed=1;
    public float waitTimeForMove = 4;
    public bool twoDir = false;
    public bool havntGoal = false;
    public bool goal = false;
    public bool lastGoal = false;
    int moveCoroutineCnt = 0;
    int moveLoopCoroutineCnt = 0;
    Coroutine moveCoroutine;
    Coroutine moveLoopCoroutine;

    public void Run(Vector3 targetPoss, float waitTime, float speed = 1)
    {
        runSpeed = speed;
        MoveUp(targetPoss, waitTime, true);
    }

    public void Patrol(string unitID)
    {   
        MoveUp(RandomTargetPos(unitID),0,false,unitID);
    }

    public void MoveUp(Vector3[] targetPoss, float waitTime = 0, bool run = false)
    {
        lastGoal = false;
        if (moveLoopCoroutineCnt > 0) { Debug.Log(name + " CoroutineCount : " + moveLoopCoroutineCnt); StopCoroutine(moveLoopCoroutine); moveLoopCoroutineCnt = 0; }
        if (gameObject.transform.parent.parent.gameObject.activeSelf == true)
        {
            if (gameObject.activeSelf == true) { moveLoopCoroutine = StartCoroutine(MoveUp_U_Loop(targetPoss, waitTime)); }
        }
    }//For Many Target

    public void MoveUp(Vector3 targetPos, float waitTime = 0, bool run = false, string unitID="")
    {
        //Debug.Log(name + " CoroutineCount : " + moveCoroutineCnt);
        if (moveCoroutineCnt > 0) { StopCoroutine(moveCoroutine); moveCoroutineCnt = 0; }
        if (gameObject.transform.parent.parent.gameObject.activeSelf == true)
        {
            if (gameObject.activeSelf == true) { moveCoroutine = StartCoroutine(MoveUp_U(targetPos, waitTime, run, unitID)); }
        }

    }//For One Target

    IEnumerator MoveUp_U_Loop(Vector3[] targetPoss, float waitTime = 0)
    {
        int clearPos = 0;
        goal = true;
        moveLoopCoroutineCnt++;
        while (true)
        {           
            if (clearPos < targetPoss.Length)
            {
                if (goal == true) //unit이 정지 걷지 않을 때만 실행.
                {
                    //Debug.Log("MoveName : " + name);
                    if (gameObject.activeSelf == true) { MoveUp(targetPoss[clearPos], waitTime); clearPos++; }
                }
            }
            else
            {
                if (goal == true)
                {
                    Debug.Log("LastGoal : " + name);
                    lastGoal = true;
                    StopAllCoroutines();
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator MoveUp_U(Vector3 targetPos = new Vector3(), float waitTime = 0, bool run = false, string unitID = "")
    {
        float sec = 0.01f;
        float time = 0;
        moveCoroutineCnt += 1;      // Debug.Log("coroutineCnt : "+coroutineCnt);
        goal = false;
        while (true)
        {
            time = time + sec;
            if (waitTime == 0) { waitTime = waitTimeForMove; }
            if (time > waitTime)
            {                
                #region  Direction
                string checkedDir = DirectionCheck(targetPos);
                for (int i = 0; i < transform.GetChildCount(); i++)
                {
                    switch (checkedDir)
                    {
                        case "u":
                            if (run == true) { Unit.MoverMotion.UnitRun(transform.GetChild(i).gameObject, "Up"); }
                            else { Unit.MoverMotion.UnitMove(transform.GetChild(i).gameObject, "Up"); }                            
                            break;
                        case "d":
                            if (run == true) { Unit.MoverMotion.UnitRun(transform.GetChild(i).gameObject, "Down"); }
                            else { Unit.MoverMotion.UnitMove(transform.GetChild(i).gameObject, "Down"); }                            
                            break;
                        case "r":
                        case "l":
                            if (run == true) { Unit.MoverMotion.UnitRun(transform.GetChild(i).gameObject); }
                            else { Unit.MoverMotion.UnitMove(transform.GetChild(i).gameObject); }                            
                            break;
                        default: break;
                    }
                }
                #endregion

                #region Translate   
                Vector3 firstPos = transform.localPosition; //Debug.Log(targetPos);
                float tempSpeed;
                if (run == true) { tempSpeed = runSpeed; yield return new WaitForSeconds(0.2f); } else { tempSpeed = speed; }
                while (true)
                {                    
                    Vector3 _dir = (targetPos - firstPos).normalized;
                    //Debug.Log(name+", tp : "+targetPos+", dir : "+_dir+", lp : "+ transform.localPosition);
                    transform.Translate(_dir * Time.deltaTime * tempSpeed);
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -3 + transform.localPosition.y);// z값 보정.
                    Vector3 tempPos = new Vector3(transform.localPosition.x, transform.localPosition.y, targetPos.z);
                    float dist = Vector3.Distance(targetPos, tempPos); //Debug.Log(name+", dist : "+ dist);
                    
                    if (dist < 0.1f 
                        || ((firstPos.x > targetPos.x) && (targetPos.x > transform.localPosition.x)) 
                        || ((firstPos.x < targetPos.x) && (targetPos.x < transform.localPosition.x))
                        || ((firstPos.y < targetPos.y) && (targetPos.y < transform.localPosition.y))
                        || ((firstPos.y < targetPos.y) && (targetPos.y < transform.localPosition.y))
                        )
                    {
                        time = 0;
                        for (int i=0; i< transform.GetChildCount(); i++) { Unit.UnitBase.UnitIdle(transform.GetChild(i).gameObject); }                        
                        //Debug.Log("moveEnd : "+name + ", "+GetComponent<UnitBase>().unitNum);                       
                        goal = true;

                        if (unitID != "") { targetPos = RandomTargetPos(unitID); }
                        break;                        
                    }
                    yield return new WaitForSeconds(sec);
                }                
                #endregion
            }
            if(goal == true && havntGoal == false) { break; }
            yield return new WaitForSeconds(sec);
        }
    }



    /// <summary>
    /// return one of [u(p),d(own),r(ight),l(eft)]
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    string DirectionCheck(Vector3 targetPos = new Vector3())
    {
        string[] dirs = { "u", "d", "r", "l" };
        string dir = "";
        float scaleX = transform.localScale.x;
        float scaleY = transform.localScale.y;
        #region distX
        float distX = Vector3.Distance(
            new Vector3(transform.localPosition.x,0,0),
            new Vector3(targetPos.x,0,0)
        );
        #endregion
        #region distY
        float distY = Vector3.Distance(
            new Vector3(transform.localPosition.y, 0, 0),
            new Vector3(targetPos.y, 0, 0)
        );
        #endregion
        // Debug.Log("distX : "+distX + ", distY : " + distY);        
        #region unitDirection
        if (twoDir == false && distX < distY)
        {
            if (targetPos.y > transform.localPosition.y)
            {
                dir = "u";
            }
            else
            {
                dir = "d";
            }
        }//위,아래 구분 
        else
        {            
            float unitDir = scaleX; // 왼쪽이 기본
            if (targetPos.x > transform.localPosition.x)
            {
                if (scaleX > 0)
                {
                    unitDir = -1 * scaleX;
                }//왼쪽을 보고 있다면, 오른쪽을 보아라!
                dir = "r";
            }
            else
            {
                if (scaleX < 0)
                {
                    unitDir = -1 * scaleX;
                }//오른쪽을 보고 있다면, 왼쪽을 보아라!
                dir = "l";
            }
            transform.localScale = new Vector3(unitDir, transform.localScale.y, transform.localScale.z);//방향전환            
        }//좌, 우구분. 기본적으로 왼쪽을 보고 있음.
        #endregion

        return dir;  // 위, 아래, 오른쪽, 왼쪽 기준.
    }

    Vector3 RandomTargetPos(string unitID) {
        Vector3 tPos = Unit.UnitBase.RandomUnitPos(20, 20);
        float dir = tPos.y > 1.1f ? -0.1f : 0.1f;
        for (int i = 0; i < 10; i++)
        {
            if (SpawnRule.SpawnPossible.UnitSpawnPossibe(unitID, tPos))//만약 위치해도 괜찮은 곳이면,
            {
                i = 10;//for문 종료.
            }
            else
            {
                tPos = new Vector3(tPos.x, tPos.y + dir, tPos.z + 0.1f);   //y값 증감.                
            }
        }
        return tPos;
    }
}
