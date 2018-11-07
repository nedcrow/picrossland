using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenBController : MonoBehaviour {
    #region Position_A_For_Weather
    Vector3[] firstPos = { new Vector3(-2f, -1f, -4f), new Vector3(10f, -2f, -5f), new Vector3(1f, -4f, -7f) };//-20~20, -3f+y;
    Vector3[] secondPos = { new Vector3(0, 0.2f, -2.8f), new Vector3(-0.4f, 0.2f, -2.8f), new Vector3(-0.2f, 0.2f, -2.8f) };//-20~20, -3f+y;
    Vector3[] thirdPos = { new Vector3(-0.2f, -4f, -7f), new Vector3(-0.3f, -4f, -7f), new Vector3(-0f, -4f, -7f) };//-20~20, -3f+y;
    Vector3[] fourthPos = { new Vector3(1.3f, -1.1f, -4.1f), new Vector3(0.9f, -1.3f, -4.3f), new Vector3(1f, -1.4f, -4.4f) };//-20~20, -3f+y;
    #endregion
    #region Position_B_For_Weather
    Vector3[] fifthPos = { new Vector3(-1.6f, -1.5f, -4.5f), new Vector3(1.3f, -2f, -5.3f), new Vector3(1.5f, -4.4f, -7.4f) };//-20~20, -3f+y;
    #endregion
    RuntimeAnimatorController[] animators;

    void Start()
    {
        animators = new RuntimeAnimatorController[] {
        Resources.Load<RuntimeAnimatorController>("Animations/0203/0203_A"),
        Resources.Load<RuntimeAnimatorController>("Animations/0203/0203_B"),
        Resources.Load<RuntimeAnimatorController>("Animations/0203/0203_C")
        };

        SetBase();
        EventManager.instance.WeatherChangedEvent += (SetBase);
        //EventManager.instance.WeatherChangedEvent += (IdleSelect);
        EventManager.instance.AttackedEvent += (Hit);
    }

    /// <summary>
    /// tranform.position, animator, idleAnimation setting.
    /// </summary>
    void SetBase()
    {        
        IdleSelect();        
        transform.position = firstPos[GetComponent<UnitBase>().unitNum];
    }

    public void IdleSelect()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = animators[GetComponent<UnitBase>().unitNum];
        switch (UserManager.Instance.GetWeather(LandManager.instance.currentLand.id))
        {
            case 0:
                float waitTime = GetComponent<MoveupController>().waitTimeForMove * GetComponent<UnitBase>().unitNum + 0.1f;
                GetComponent<MoveupController>().MoveUp(secondPos[GetComponent<UnitBase>().unitNum], waitTime);
                break;
            case 1:
                GetComponent<MoveupController>().MoveUp(fifthPos[GetComponent<UnitBase>().unitNum], 1);
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }

    void Hit(GameObject attacker, GameObject target, int unitNum) {        
        if (target.name == gameObject.name && GetComponent<UnitBase>().unitNum == unitNum)
        {
            int currentWeather = UserManager.Instance.GetWeather(LandManager.instance.currentLand.id);
            if (attacker.name == "0201" && currentWeather == 0)
            {
                #region Atk
                for (int i = 0; i < transform.GetChildCount(); i++)
                {
                    Unit.FighterMotion.Attack(transform.GetChild(i).gameObject);
                }
                attacker.GetComponent<LandSymbolControllerII>().Hit(gameObject, attacker);
                #endregion

                #region Movement
                if (UserManager.Instance.ClearPuzzleCheck("0204"))
                {
                    GetComponent<MoveupController>().MoveUp(fourthPos[GetComponent<UnitBase>().unitNum], 0.4f);
                }//0204퍼즐을 클리어 했으면,4번 위치로 이동.
                else
                {
                    GetComponent<MoveupController>().MoveUp(thirdPos[GetComponent<UnitBase>().unitNum], 0.4f);
                }//클리어 안 했으면 3번 위치로 이동.
                #endregion

            }//unit을 때린 Gameobject가 LandObj2이면, 투표 후 3,4번 목적지 중 하나로 이동.
            else
            {
                for (int i = 0; i < transform.GetChildCount(); i++)
                {
                    Unit.FighterMotion.Attack(transform.GetChild(i).gameObject,"1");
                }
                GetComponent<MoveupController>().MoveUp(thirdPos[GetComponent<UnitBase>().unitNum], 0.8f);
            }//unit을 때린 Gameobject가 LandObj2가 아니면, 사진찍고 바로 종점으로 이동.
        }//Unit ID와 Num으로 피격대상이 본인인지 확인.
    }

    IEnumerator Hit_Co(GameObject attacker, GameObject target, int unitNum)
    {
        yield return new WaitForSeconds(0.01f);
        EventManager.instance.AttackedFunc(gameObject, PuzzleManager.instance.currentLandObj.GetComponent<LandController>().backgroundObj.transform.GetChild(0).gameObject);
        yield return null;
    }//Event 중첩 방지용 코루틴.
}
