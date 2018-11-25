using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CityzenA와 차이 : 0204(SNS) 클리어 시 사진찍음.
/// </summary>
public class CitizenBController : MonoBehaviour {

    public bool readySeat = false;

    #region Position_A_For_Weather
    Vector3[] firstPos = { new Vector3(-2f, -1f, -4f), new Vector3(10f, -2f, -5f), new Vector3(1f, -4f, -7f) };//-20~20, -3f+y;
    Vector3[] secondPos = { new Vector3(0, 0.2f, -2.8f), new Vector3(-0.4f, 0.2f, -2.8f), new Vector3(-0.2f, 0.2f, -2.8f) };//-20~20, -3f+y;
    Vector3[] thirdPos = { new Vector3(-0.2f, -4f, -7f), new Vector3(-0.3f, -4f, -7f), new Vector3(-0f, -4f, -7f) };//-20~20, -3f+y;
    Vector3[] fourthPos = { new Vector3(1.3f, -1.1f, -4.1f), new Vector3(0.9f, -1.3f, -4.3f), new Vector3(1f, -1.4f, -4.4f) };//-20~20, -3f+y;
    #endregion
    
    #region Position_B_For_Weather
    Vector3[] fifthPos = { new Vector3(-1.6f, -1.5f, -4.5f), new Vector3(-1.8f, -2f, -5.3f), new Vector3(-1.8f, -4.4f, -7.4f) };//-20~20, -3f+y;
    Vector3[] sixthPos = {
        new Vector3(-0.657f, -2.21f, -5.21f), new Vector3(0f, -2.21f, -5.21f), new Vector3(0.657f, -2.21f, -5.21f)
    };
    #endregion

    Color[] colors = { new Vector4(1, 1, 1, 1), new Vector4(0.9f, 1f, 0.8f, 1), new Vector4(1f, 0.8f, 0.8f, 1) };
    RuntimeAnimatorController[] animators;

    void Start()
    {
        EventManager.instance.WeatherChangedEvent += (SetBase);
        EventManager.instance.AttackedEvent += (Hit);

        animators = new RuntimeAnimatorController[] {
        Resources.Load<RuntimeAnimatorController>("Animations/0203/0203_A"),
        Resources.Load<RuntimeAnimatorController>("Animations/0203/0203_B"),
        Resources.Load<RuntimeAnimatorController>("Animations/0203/0203_C")
        };

        SetBase();
    }

    /// <summary>
    /// tranform.position, animator, idleAnimation setting.
    /// </summary>
    void SetBase()
    {
        transform.position = firstPos[GetComponent<UnitBase>().unitNum];
        IdleSelect();      
    }

    public void IdleSelect()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = colors[GetComponent<UnitBase>().unitNum];
        transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = animators[GetComponent<UnitBase>().unitNum];
        switch (UserManager.Instance.GetWeather(LandManager.instance.currentLand.id))
        {
            case 0:
                float waitTime = 3*(GetComponent<UnitBase>().unitNum * 2 + 1f);
                GetComponent<MoveupController>().MoveUp(secondPos[GetComponent<UnitBase>().unitNum], waitTime);
                break;
            case 1:
                transform.localPosition = sixthPos[GetComponent<UnitBase>().unitNum]; //Debug.Log(transform.position);
                GetComponent<MoveupController>().StopAllCoroutines();
                break;
            case 2:
                if(LandManager.instance.GetComponent<UnitManager>().SearchUnit("0209") != null)
                {
                    float waitTimeB = 3 * (GetComponent<UnitBase>().unitNum * 2f);
                    GetComponent<MoveupController>().MoveUp(secondPos[GetComponent<UnitBase>().unitNum], waitTimeB);
                }
                else
                {
                    GetComponent<MoveupController>().StopAllCoroutines();
                }
                break;
            case 3:
                break;
            default:
                break;
        }
    }

    public void Hit(GameObject attacker, GameObject target, int unitNum)
    {
        if (target.name == gameObject.name && GetComponent<UnitBase>().unitNum == unitNum)
        {
            int currentWeather = UserManager.Instance.GetWeather(LandManager.instance.currentLand.id);
            #region currentWeather_0
            if (currentWeather == 0)
            {
                if (attacker.name == "0201")
                {
                    Debug.Log("I'am Hit : " + name + ", num : " + unitNum);
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
                else if (attacker.name == "0204")
                {
                    for (int i = 0; i < transform.GetChildCount(); i++)
                    {
                        Unit.FighterMotion.Attack(transform.GetChild(i).gameObject, "1");
                    }
                    GetComponent<MoveupController>().MoveUp(thirdPos[GetComponent<UnitBase>().unitNum], 0.8f);
                }//unit을 때린 Gameobject가 LandObj2가 아니면, 사진찍고 바로 종점으로 이동.
            }
            #endregion

            #region currentWeather_1
            else if (currentWeather == 1)
            {
                if (attacker.name == "0207")
                {
                    Seat();
                }
            }
            #endregion

            #region currentWeather_2
            else if (currentWeather == 2)
            {
                if (attacker.name == "0209")
                {
                    #region Atk
                    for (int i = 0; i < transform.GetChildCount(); i++)
                    {
                        Unit.FighterMotion.Attack(transform.GetChild(i).gameObject);
                    }
                    attacker.GetComponent<RibbonController>().Hit(gameObject, attacker);
                    #endregion

                    #region Movement
                    GetComponent<MoveupController>().MoveUp(fourthPos[GetComponent<UnitBase>().unitNum], 0.4f);
                    #endregion
                }//ribbon에 도착했으면 ribbon을 atk하고 집으로 귀환.
            }
            #endregion
        }//Unit ID와 Num으로 피격대상이 본인인지 확인.
    }

    public void Seat()
    {
        for (int i = 0; i < transform.GetChildCount(); i++) { Unit.MoverMotion.Contact(transform.GetChild(i).gameObject, "0207"); }
        StartCoroutine(InJailCheck());
    }

    IEnumerator InJailCheck()
    {
        while (true)
        {
            if (UserManager.Instance.ClearPuzzleCheck("0206") == true)
            {
                GameObject target = LandManager.instance.GetComponent<UnitManager>().SearchUnit("0206");  //defendant
                if (target.GetComponent<DefendantController>().inJail == true)
                {
                    //yield return new WaitForSeconds(1.5f);
                    Vector3 lastPos = new Vector3(3f, transform.position.y, transform.position.z);
                    GetComponent<MoveupController>().MoveUp(lastPos, 0.1f);
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }// 피고가 감옥에 수감되면 집으로 Go Go.
    IEnumerator Hit_Co(GameObject attacker, GameObject target, int unitNum)
    {
        yield return new WaitForSeconds(0.01f);
        EventManager.instance.AttackedFunc(gameObject, PuzzleManager.instance.currentLandObj.GetComponent<LandController>().backgroundObj.transform.GetChild(0).gameObject);
        yield return null;
    }//Event 중첩 방지용 코루틴.
}
