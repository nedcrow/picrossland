using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendantController : MonoBehaviour {

    GameObject pickUpBox;
    Vector3 firstPos = new Vector3(2.5f, -0.5f, -3.5f);//-20~20, -3f+y;    
    Vector3 secondPos = new Vector3(-0.1f, -0.5f, -3.5f);//-20~20, -3f+y;    

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
        if (target.name == gameObject.name && GetComponent<UnitBase>().unitNum == unitNum)
        {
            int currentWeather = UserManager.Instance.GetWeather(LandManager.instance.currentLand.id);
            #region currentWeather_0
            if (currentWeather == 1)
            {                
                if (attacker.name == "0201")
                {
                    if (UserManager.Instance.ClearPuzzleCheck("0207") == false)
                    {
                        MinusMoney();
                        attacker.GetComponent<LandSymbolControllerII>().Hit(gameObject, attacker);
                    }

                    
  
                }               
            }
            #endregion

            #region currentWeather_1
            else if (currentWeather == 1)
            {
                if (attacker.name == "0207")
                {
                 //   GetComponent<MoveupController>().MoveUp(sixthPos[GetComponent<UnitBase>().unitNum], 0.1f);
                }
            }
            #endregion
        }//Unit ID와 Num으로 피격대상이 본인인지 확인.
    }

    void MinusMoney() {
        Vector3 pickUpPos = new Vector3(transform.position.x -0.13f, transform.position.y+ 1,transform.position.z -0.5f);
        pickUpBox.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = UserManager.Instance.GetComponent<SpriteManager>().minusIcon;
        EffectBasket.EffectBasket.instance.Pickup(pickUpBox, 0.2f, 0.02f, pickUpPos);
    }
}
