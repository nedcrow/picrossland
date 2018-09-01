using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveupController : MonoBehaviour {
    public float speed=1;
    public float cutLine = 4;
    int coroutineCnt = 0;


    private void Start()
    {
        //EventManager.instance.LandActivatedEvent += (MoveUp);
    }

    public void MoveUp(Vector3 targetPos, float waitTime = 0)
    {        
        if (coroutineCnt > 0) { StopAllCoroutines(); coroutineCnt = 0; }
        if(gameObject.activeSelf == true) { StartCoroutine(MoveUp_U(targetPos, waitTime)); }
    }

    IEnumerator MoveUp_U(Vector3 targetPos = new Vector3(), float waitTime = 0)
    {
        float sec = 0.01f;
        float time = 0;        
        float scaleX = transform.localScale.x;
        coroutineCnt += 1;      // Debug.Log("coroutineCnt : "+coroutineCnt);
        Vector3 firstTargetPos = targetPos;
        while (true)
        {
            time = time + sec;
            if (waitTime == 0) { waitTime = cutLine; }
            if (time > waitTime)
            {
                Unit.Mover.Move_U(transform.GetChild(0).gameObject);

                #region targetPos                
                int loop = 20;
                float dir = transform.localPosition.y > 1.1f ? -0.1f : 0.1f;//y값 증감할 때 방향 정한 것.
                if (firstTargetPos == Vector3.zero)
                {
                    for (int i = 0; i < loop; i++)
                    {

                        targetPos = Unit.UnitBase.RandomUnitPos(20, 20);//왠만하면 고정.
                        if (SpawnRule.SpawnPossible.UnitSpawnPossibe(transform.name, targetPos))//만약 위치해도 괜찮은 곳이면,
                        {
                            i = loop;  //for문 종료.
                        }
                        else
                        {
                            if (i == loop - 1) { targetPos = transform.localPosition; }
                            else
                            {
                                if (i / 2 == 0) { targetPos = new Vector3(targetPos.x, targetPos.y + dir, targetPos.z + dir); }//y값 증감.   
                                else { targetPos = new Vector3(targetPos.x + dir, targetPos.y, targetPos.z); }//x값 증감.      
                            }
                        }

                    }//10번만 시도. 
                }
                else { Debug.Log("tPos : "+ targetPos); }
                #endregion

                while (true)
                {
                    #region unitDirection
                    Vector3 _dir = (targetPos - transform.localPosition).normalized;
                    float unitDir = scaleX;
                    if (targetPos.x > transform.localPosition.x) {//우측이동
                        if (scaleX > 0) { unitDir = -1 * scaleX; }//오른쪽을 보아라!
                    }
                    else
                    {
                        if (scaleX < 0) { unitDir = -1 * scaleX; }//왼쪽을 보아라!
                    }//기본적으로 왼쪽을 보고 있음.                    
                    transform.localScale = new Vector3(unitDir, transform.localScale.y, transform.localScale.z);//방향전환
                    #endregion
                                        
                    transform.Translate(_dir * Time.deltaTime * speed);
                    Vector3 tempPos = new Vector3(transform.localPosition.x, transform.localPosition.y, targetPos.z);
                    float dist = Vector3.Distance(targetPos, tempPos);
//                    Debug.Log(targetPos+ ", "+ tempPos + ", " + dist);
                    if (dist < 0.2f) {
                        time = 0;
                        Unit.UnitBase.Idle_U(transform.GetChild(0).gameObject);
                        Debug.Log("moveEnd");
                        coroutineCnt -= 1;
                        if (firstTargetPos == Vector3.zero)
                        {
                            break;
                        }
                        else
                        {
                            StopAllCoroutines();
                        }
                    }
                    yield return new WaitForSeconds(sec);
                }
            }
            yield return new WaitForSeconds(sec);
        }

    }
}
