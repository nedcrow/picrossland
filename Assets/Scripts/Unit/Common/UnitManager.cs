using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    [SerializeField]
    public List<GameObject> unitList;

    public void UnitSpawn(string puzzleID, bool afterClear, int count = 0 )
    {        
        int sameCount=0;
        for(int i=0; i< unitList.Count; i++)
        {
            if( unitList[i].name == puzzleID) { sameCount++; } // 이미 생성된 유닛들 중 지금 것과 같은 Unit 수량 확인.
        }
        
        if (sameCount < PuzzleManager.instance.GetPuzzleMaxCount(puzzleID)) //일정 갯수 밑으로만 가능. 
        {
            GameObject target = PuzzleInfo.FindPuzzleEffect.FindPrefab(puzzleID); //unitObj찾기.

            if (target.name == "noneTargetPrefab" || target.name == "noneTargetPrefab(Clone)") { Destroy(target.gameObject); } //찾는 obj가 없었으면 임시 gameObj를 지운다.
            else
            {
                for (int i = 0; i < count; i++)
                {
                    target = Instantiate(target);
                    target.transform.SetParent(PuzzleManager.instance.currentLandObj.GetComponent<LandController>().units.transform);
                    target.name = puzzleID;
                    unitList.Add(target);
                    if (afterClear)
                    {
                        UserManager.Instance.GetCurrentInGotLandList(LandManager.instance.currentLand.id).unitList.Add(puzzleID);//저장정보 unitList in gotLandList
                    }
                    //Debug.Log(UserManager.Instance.GetCurrentInGotLandList(LandManager.instance.currentLand.id).unitList.Count);
                }
            }
        }
        
        
    }

    public void SetUnitsEvents(int landNum)
    {
        for (int i = 0; i < unitList.Count; i++)
        {
            int unitNum = System.Convert.ToInt32(HarimTool.EditValue.EditText.Left(unitList[i].name, 2));
            //Debug.Log("landNum : " + landNum + ", " + "" + unitNum);
            if (unitNum != landNum)
            {
                if (unitList[i].GetComponent<MoveupController>())
                {
                    EventManager.instance.LandActivatedEvent -= (unitList[i].GetComponent<MoveupController>().MoveUp);
                }
            }
            else
            {
                if (unitList[i].GetComponent<MoveupController>())
                {
                    EventManager.instance.LandActivatedEvent += (unitList[i].GetComponent<MoveupController>().MoveUp);
                }
            }
        }
    }

    public GameObject SearchUnit(string unitID)
    {
        if(unitList.Count > 0)
        {
            for (int i = 0; i < unitList.Count; i++)
            {
                if (unitList[i].name == unitID) { return unitList[i]; }
            }
            return null;
        }
        else
        {
            return null;
        }
    }

}
