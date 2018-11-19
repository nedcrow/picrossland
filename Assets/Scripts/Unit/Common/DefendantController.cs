using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendantController : MonoBehaviour {

    public bool inJail = false;

    GameObject pickUpBox;
    Vector3 firstPos = new Vector3(3f, -0.5f, -3.5f);//-20~20, -3f+y;    
    Vector3 secondPos = new Vector3(-0.1f, -0.5f, -3.5f);//-20~20, -3f+y;    
    Vector3 thirdPos = new Vector3(-3.5f, -0.5f, -3.5f);//-20~20, -3f+y;    

    void Start()
    {
        SetTransform();
        EventManager.instance.WeatherChangedEvent += (SetTransform);
        EventManager.instance.AttackedEvent += (Hit);
    }

    void SetTransform() {
        pickUpBox = transform.GetChild(2).gameObject;
        transform.localPosition = firstPos;
        IdleSelect();
    }


    public void IdleSelect()
    {
        if (UserManager.Instance.GetWeather(LandManager.instance.currentLand.id) == 1)//Day
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
            GetComponent<MoveupController>().MoveUp(secondPos,0.5f);
        }
        else
        {
            for (int i = 0; i < transform.GetChildCount(); i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

        }
    }

    public void Hit(GameObject attacker, GameObject target, int unitNum)
    {
        if (target.name == gameObject.name)
        {
            int currentWeather = UserManager.Instance.GetWeather(LandManager.instance.currentLand.id);
            #region currentWeather_0
            if (currentWeather == 1)
            {
                Debug.Log("attacker Name : " + attacker.name);
                if (attacker.name == "0201")
                {
                    if (UserManager.Instance.ClearPuzzleCheck("0207") == false)
                    {
                        Debug.Log("Hit Name : "+name);
                        MinusMoney();
                        attacker.GetComponent<LandSymbolControllerII>().Hit(gameObject, attacker);
                    }
                    else
                    {
                        StartCoroutine(DropJail(attacker));
                    }
                }               
            }
            #endregion

        }//Unit ID와 Num으로 피격대상이 본인인지 확인.
    }

    void MinusMoney() {
        Vector3 pickUpPos = new Vector3(transform.position.x+1, transform.position.y+ 2,transform.position.z -0.5f);
        pickUpBox.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = UserManager.Instance.GetComponent<SpriteManager>().minusIcon;
        pickUpBox.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = UserManager.Instance.GetComponent<SpriteManager>().money;
        EffectBasket.EffectBasket.instance.Pickup(pickUpBox, 0.2f, 0.02f, pickUpPos);
    }

    IEnumerator DropJail(GameObject target)
    {
        while (true)
        {
            if (GetComponent<MoveupController>().goal == true)
            {
                yield return new WaitForSeconds(4f);
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(1).GetComponent<Animator>().Play("Jail_Setting"); //감옥 시작
                yield return new WaitForSeconds(2f);

                inJail = true;
                yield return new WaitForSeconds(5f); //감옥 On

                MinusMoney();
                EventManager.instance.AttackedFunc(gameObject,target); // 뇌물 주기
                yield return new WaitForSeconds(2f);

                transform.GetChild(0).GetComponent<Animator>().Play("0206_StartRun");
                transform.GetChild(1).GetComponent<Animator>().Play("Jail_Open_1"); //감옥 열고 튈 준비
                yield return new WaitForSeconds(2f);

                transform.GetChild(1).SetParent(transform.parent);
                GetComponent<MoveupController>().Run(thirdPos, 0.01f, 2.5f); //튐.
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
