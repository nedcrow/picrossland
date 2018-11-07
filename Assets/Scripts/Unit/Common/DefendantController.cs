using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendantController : MonoBehaviour {

    Vector3 firstPos = new Vector3(2.5f, -1.5f, -4.5f);//-20~20, -3f+y;    
    Vector3 secondPos = new Vector3(-0.1f, -1.5f, -4.5f);//-20~20, -3f+y;    

    void Start()
    {
        SetTransform();
        EventManager.instance.WeatherChangedEvent += (SetTransform);
    }

    void SetTransform() {
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


}
