using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendantController : MonoBehaviour {

    public GameObject pickUpBox;
    public GameObject jail;
    public bool inJail = false;   
        
    Vector3 firstPos = new Vector3(3f, -0.5f, -3.5f);//-20~20, -3f+y;    
    Vector3 secondPos = new Vector3(-0.1f, -0.5f, -3.5f);//-20~20, -3f+y;    
    Vector3 thirdPos = new Vector3(-3.5f, -0.5f, -3.5f);//-20~20, -3f+y;    

    void Start()
    {
        SetChild();
        SetTransform();
        EventManager.instance.WeatherChangedEvent += (SetTransform);
        EventManager.instance.AttackedEvent += (Hit);
    }

    void SetChild() {
        jail = transform.GetChild(1).gameObject;
        pickUpBox = transform.GetChild(2).gameObject;
    }

    void SetTransform() {        
        transform.localPosition = firstPos;
        jail.transform.SetParent(transform);
        jail.transform.position = transform.position;
        jail.SetActive(false);
        IdleSelect();
    }

    public void IdleSelect()
    {        
        if (UserManager.Instance.GetWeather(LandManager.instance.currentLand.id) == 1)//Day
        {
            inJail = false;
            transform.GetChild(0).gameObject.SetActive(true);
            jail.gameObject.SetActive(false);
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

    IEnumerator MinusMoney()
    {
        string[] citiyzen = { "0202", "0203" };
        int cnt = LandManager.instance.GetComponent<UnitManager>().SearchUnits(transform.position, citiyzen, false).Count;
        cnt = cnt > 1 ? cnt : 1;
        pickUpBox.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = UserManager.Instance.GetComponent<SpriteManager>().minusIcon;
        pickUpBox.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = UserManager.Instance.GetComponent<SpriteManager>().money;
        Vector3 pickUpPos = new Vector3(transform.position.x + 1, transform.position.y + 2, transform.position.z - 0.5f);

        for (int i = 0; i < cnt; i++)
        {
            EffectBasket.EffectBasket.instance.Pickup(pickUpBox, 0.2f, 0.02f, pickUpPos);
            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator DropJail(GameObject target)
    {
        while (true)
        {
            if (GetComponent<MoveupController>().goal == true)
            {
                target.GetComponent<LandSymbolControllerII>().Hit(gameObject, target); //판결 요청
                yield return new WaitForSeconds(5f);

                jail.SetActive(true);
                jail.GetComponent<Animator>().Play("Jail_Setting"); //감옥 시작
                yield return new WaitForSeconds(2f);

                inJail = true;
                yield return new WaitForSeconds(5.5f); //감옥 On

                StartCoroutine(MinusMoney());
                yield return new WaitForSeconds(0.5f);
                target.GetComponent<LandSymbolControllerII>().Hit(gameObject, target);
                yield return new WaitForSeconds(2f);//뇌물

                transform.GetChild(0).GetComponent<Animator>().Play("0206_StartRun");
                jail.GetComponent<Animator>().Play("Jail_Open_1"); //감옥 열고 튈 준비
                yield return new WaitForSeconds(2f);

                jail.transform.SetParent(transform.parent);
                GetComponent<MoveupController>().Run(thirdPos, 0.01f, 2.5f); //튐.
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
