using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonoBehaviour {

    Vector3 firstPos = new Vector3(1.5f, -0.7f, -3.7f);//-20~20, -3f+y;
    
    void Start()
    {
        Unit.UnitBase.UnitIdle(transform.GetChild(0).gameObject);
        SetTransform();
        EventManager.instance.WeatherChangedEvent += (SetTransform);
        GetComponent<MoveupController>().speed = 1.2f;
    }

    void SetTransform()
    {
        int sameCount = Unit.UnitBase.FindSameUnit(this.gameObject.name).Count;
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
        GetComponent<MoveupController>().twoDir = true;
        GetComponent<MoveupController>().havntGoal = true;
        GetComponent<MoveupController>().Patrol(transform.name);
    }
    

}
