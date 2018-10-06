using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenController : MonoBehaviour {

    Vector3 firstPos = new Vector3(-3f, -1f, -4f);//-20~20, -3f+y;
    Vector3 secondPos = new Vector3(-0.2f, -0.3f, -3.3f);//-20~20, -3f+y;
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
    }

    /// <summary>
    /// tranform.position, animator, idleAnimation setting.
    /// </summary>
    void SetBase()
    {
        IdleSelect();
        int sameCount = Unit.UnitBase.FindSameUnit(this.gameObject.name);
        if (sameCount == 1)
        {
            transform.localPosition = firstPos;
        } //만들어지면 무조건 list에 추가되니까 최소값은 1.
        else
        {

            int loop = 10;

            transform.localPosition = Unit.UnitBase.RandomUnitPos(20, 20);//왠만하면 고정.
            float dir = transform.localPosition.y > 1.1f ? -0.1f : 0.1f;//y값 증감할 때 방향 정한 것.

            for (int i = 0; i < loop; i++)
            {
                if (SpawnRule.SpawnPossible.UnitSpawnPossibe(transform.name, transform.localPosition))//만약 위치해도 괜찮은 곳이면,
                {
                    i = loop;//for문 종료.
                }
                else
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + dir, transform.localPosition.z + 0.1f);   //y값 증감.                
                }
            }//10번만 시도.        
        }
    }

    public void IdleSelect()
    {
        transform.GetChild(0).gameObject.SetActive(true);

        int sameCount = Unit.UnitBase.FindSameUnit(this.gameObject.name);

        switch (sameCount)  //만들어지면 무조건 list에 추가되니까 최소값은 1.}
        {
            case 1: transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = animators[0]; break;
            case 2: transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = animators[1]; break;
            case 3: transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = animators[2]; break;
        }

//        Unit.UnitBase.UnitIdle(transform.GetChild(0).gameObject);
        GetComponent<MoveupController>().MoveUp(secondPos, GetComponent<MoveupController>().waitTimeForMove); 
    }

}
