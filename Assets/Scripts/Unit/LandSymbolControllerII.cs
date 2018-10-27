using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandSymbolControllerII : MonoBehaviour {

    GameObject[] LandSymbols;
    GameObject ballotBox;
    GameObject balanceScale;
    GameObject deceasedPortrait;
    GameObject pickUpObject;
    public GameObject currentSymbol=null;
    public int visitCount = 0;


    void Start () {        
        transform.name = "0201";
        LandSymbolSetting();
        BaseSetting();

        EventManager.instance.WeatherChangedEvent += (BaseSetting);
//        EventManager.instance.AttackedEvent += (Hit);
    }
		
	void BaseSetting ()
    {
        #region ChildObjects
        LandSymbolChange();
        pickUpObject = transform.GetChild(transform.GetChildCount() - 1).gameObject;
        pickUpObject.SetActive(false);
        #endregion

        #region FightController
        visitCount = 0;
        string[] targetIDs = { "0202", "0203" };
        GetComponent<FightController>().oneHit = true;
        GetComponent<FightController>().Search_U(Vector3.zero, targetIDs, "D", 0.5f);
        #endregion
    }

    public void Hit(GameObject attacker, GameObject target)
    {
        int currentWeather = UserManager.Instance.GetWeather(LandManager.instance.currentLand.id);
        //Debug.Log(target.name+", "+unitNum);
        if (target.name == gameObject.name)
        {
            EffectBasket.EffectBasket.instance.Pickup(pickUpObject, 0.2f, 0.02f);
            Unit.FighterMotion.Hit(LandSymbols[currentWeather]);
            visitCount++;
            if (visitCount >= GetComponent<FightController>().targetList.Count) {
                Unit.FighterMotion.Hit(LandSymbols[currentWeather],"1");
            }
        }
    }

    void LandSymbolSetting()
    {
        ballotBox = transform.GetChild(0).gameObject;
        balanceScale = transform.GetChild(1).gameObject;
        deceasedPortrait = transform.GetChild(2).gameObject;
        LandSymbols = new GameObject[]{ ballotBox, balanceScale, deceasedPortrait };        
    }
    void LandSymbolChange()
    {
        for(int i=0; i< LandSymbols.Length; i++) {
            if (LandSymbols[i]) { LandSymbols[i].SetActive(false); }
        }
        currentSymbol = LandSymbols[UserManager.Instance.GetWeather(LandManager.instance.currentLand.id)];
        currentSymbol.SetActive(true);
    }
}
