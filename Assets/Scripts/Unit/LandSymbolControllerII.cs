using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandSymbolControllerII : MonoBehaviour {

    public int visitCount = 0;
    public GameObject currentSymbol = null;

    GameObject[] LandSymbols;
    GameObject ballotBox;
    GameObject balanceScale;
    GameObject deceasedPortrait;
    GameObject pickUpObject;

    Vector3 pickUpBasePos;

    float[] ranges = {0.5f, 2.1f, 1f, 0f };
    string[] weaponIds = {"0201", "0201", "0201", "0201" };
    string[][] targetIDs = {
           new string[] { "0202", "0203" },
           new string[] { "0206" },
        };


    void Start () {        
        transform.name = "0201";
        LandSymbolSetting();
        BaseSetting();

        EventManager.instance.WeatherChangedEvent += (BaseSetting);
//        EventManager.instance.AttackedEvent += (Hit);
    }
		
	void BaseSetting ()
    {
        transform.position = new Vector3(transform.position.x, 0.5f, -2.5f);
        int weatherID = UserManager.Instance.GetWeather(LandManager.instance.currentLand.id);

        #region ChildObjects
        LandSymbolChange();
        pickUpObject = transform.GetChild(transform.GetChildCount() - 1).gameObject;        
        pickUpObject.SetActive(false);        
        #endregion

        #region Targets        
        string[] targetID = targetIDs[weatherID];
        #endregion

        #region FightController        
        visitCount = 0;        
        GetComponent<FightController>().oneHit = true;
        GetComponent<FightController>().weaponID = weaponIds[weatherID];
        GetComponent<FightController>().Search_U(Vector3.zero, targetID, "D", ranges[weatherID]);
        #endregion
    }

    public void Hit(GameObject attacker, GameObject target)
    {
        int currentWeather = UserManager.Instance.GetWeather(LandManager.instance.currentLand.id);
        //Debug.Log(target.name+", "+unitNum);
        if (target.name == gameObject.name)
        {
            switch (currentWeather)
            {
                case 0:
                    pickUpObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = UserManager.Instance.GetComponent<SpriteManager>().plusIcon;
                    pickUpObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = UserManager.Instance.GetComponent<SpriteManager>().ballot;
                    pickUpBasePos = pickUpObject.transform.position;
                    EffectBasket.EffectBasket.instance.Pickup(pickUpObject, 0.2f, 0.02f, pickUpBasePos);
                    Unit.FighterMotion.Hit(LandSymbols[currentWeather]);
                    visitCount++;
                    if (visitCount >= GetComponent<FightController>().targetList.Count)
                    {
                        Unit.FighterMotion.Hit(LandSymbols[currentWeather], "1");
                    }
                    break;
                case 1:
                    if (attacker.GetComponent<DefendantController>())
                    {
                        if (attacker.GetComponent<DefendantController>().inJail == false) {
                            balanceScale.GetComponent<Animator>().Play("0201_Attack"); Debug.Log("weather_1_Hit");
                        }
                        else {
                            pickUpObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = UserManager.Instance.GetComponent<SpriteManager>().plusIcon;
                            pickUpObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = UserManager.Instance.GetComponent<SpriteManager>().money;
                            pickUpBasePos = new Vector3(pickUpObject.transform.position.x + 0.8f, pickUpObject.transform.position.y + 0.4f, pickUpObject.transform.position.z);
                            EffectBasket.EffectBasket.instance.Pickup(pickUpObject, 0.2f, 0.02f, pickUpBasePos);
                            Unit.FighterMotion.Hit(LandSymbols[currentWeather]);
                        }
                    }
                    break;

                default :
                    break;
                    
            }
        }
    }

    void LandSymbolSetting()
    {
        ballotBox = transform.GetChild(0).gameObject;
        balanceScale = transform.GetChild(1).gameObject;
        deceasedPortrait = transform.GetChild(2).gameObject;
        LandSymbols = new GameObject[]{ ballotBox, balanceScale, deceasedPortrait, null };
        currentSymbol = LandSymbols[UserManager.Instance.GetWeather(LandManager.instance.currentLand.id)]; Debug.Log("currentSymbol : " + currentSymbol);
    }
    void LandSymbolChange()
    {
        for(int i=0; i< LandSymbols.Length; i++) {
            if (LandSymbols[i]) { LandSymbols[i].SetActive(false); }
        }
        currentSymbol = LandSymbols[UserManager.Instance.GetWeather(LandManager.instance.currentLand.id)];
        //Debug.Log("currentSymbol : " + currentSymbol);
        currentSymbol.SetActive(true);
    }
}
