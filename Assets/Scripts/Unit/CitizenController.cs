using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenController : MonoBehaviour {

    Vector3[] firstPos = { new Vector3(-3f, -1f, -4f), new Vector3(11f, -2f, -5f), new Vector3(1f, -4f, -7f) };//-20~20, -3f+y;
    Vector3 secondPos = new Vector3(-0.4f, -0.3f, -3.3f);//-20~20, -3f+y;
    Vector3[] thirdPos = { new Vector3(-0.2f, -4f, -7f), new Vector3(-0.3f, -4f, -7f), new Vector3(-0f, -4f, -7f) };//-20~20, -3f+y;
    RuntimeAnimatorController[] animators;

    void Start()
    {
        animators = new RuntimeAnimatorController[] {
        Resources.Load<RuntimeAnimatorController>("Animations/0202_A"),
        Resources.Load<RuntimeAnimatorController>("Animations/0202_B"),
        Resources.Load<RuntimeAnimatorController>("Animations/0202_C")
        };
        SetBase();
        EventManager.instance.WeatherChangedEvent += (IdleSelect);
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
        GetComponent<MoveupController>().MoveUp(secondPos, GetComponent<MoveupController>().waitTimeForMove); 
    }

    void Hit(GameObject target, int unitNum) {
        if (target.name == gameObject.name && GetComponent<UnitBase>().unitNum == unitNum)
        {
            for (int i = 0; i < transform.GetChildCount(); i++)
            {
                Unit.Fighter.Attack(transform.GetChild(i).gameObject);
            }
            EventManager.instance.AttackedFunc(PuzzleManager.instance.currentLandObj.GetComponent<LandController>().backgroundObj.transform.GetChild(0).gameObject);
            GetComponent<MoveupController>().MoveUp(thirdPos[GetComponent<UnitBase>().unitNum], 0.8f);
        }
    }
}
