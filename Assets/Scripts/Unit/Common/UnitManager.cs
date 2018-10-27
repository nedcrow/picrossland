using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                    target.AddComponent<UnitBase>().unitNum = sameCount;
                    target.transform.SetParent(PuzzleManager.instance.currentLandObj.GetComponent<LandController>().units.transform);
                    target.name = puzzleID;
                    unitList.Add(target);
                    if (afterClear == true)
                    {
                        UserManager.Instance.GetCurrentInGotLandList(LandManager.instance.currentLand.id).unitList.Add(puzzleID);//저장정보 unitList in gotLandList
                    }
                    //Debug.Log(UserManager.Instance.GetCurrentInGotLandList(LandManager.instance.currentLand.id).unitList.Count);
                }
            }
        }
        
        
    }

    /// <summary>
    /// Active Land의 units만 특정 이벤트(MoveUp)를 활성화
    /// </summary>
    /// <param name="landNum"></param>
    public void SetUnitsEvents(int landNum)
    {
        for (int i = 0; i < unitList.Count; i++)
        {
            int unitNum = System.Convert.ToInt32(HarimTool.EditValue.EditText.Left(unitList[i].name, 2));            
            if (unitNum != landNum)
            {
                if (unitList[i].GetComponent<MoveupController>())
                {
                    EventManager.instance.LandActivatedEvent -= (unitList[i].GetComponent<MoveupController>().MoveUp);
                    Debug.Log("MinusEvent_MoveUp" + unitList[i].name);
                }
            }
            else
            {
                if (unitList[i].GetComponent<MoveupController>())
                {
                    Debug.Log("PlusEvent_MoveUp : "+unitList[i].name);
                    EventManager.instance.LandActivatedEvent += (unitList[i].GetComponent<MoveupController>().MoveUp);
                }
            }
        }
    }//해당 Land 안에 있는 유닛들만 Event를 활성화한다.

    #region SearchUnit
    /// <summary>
    /// this is search GameObject the Same unitID or Nearest in UnitList. or CurrentLand's Symbol.
    /// </summary>
    /// <param name="mPosition"></param>
    /// <param name="unitIDs"></param>
    /// <returns></returns>
    public GameObject SearchUnit(string unitID)
    {
        if(unitList.Count > 0)
        {
            for (int i = 0; i < unitList.Count; i++)
            {
                if (unitList[i].name == unitID) { return unitList[i]; }
            }//먼저 UnitList를 뒤진다. 있으면 Return.

            GameObject symbol = PuzzleManager.instance.currentLandObj.GetComponent<LandController>().backgroundObj.gameObject;
            if (symbol.transform.GetChildCount() > 0)
            {
                symbol = symbol.transform.GetChild(0).gameObject;
                if (unitID == symbol.name)
                {
                    return symbol;
                }
            }//Land Background와 비교해보고      
            return null;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mPosition"></param>
    /// <param name="unitIDs"></param>
    /// <param name="nearby"> True면 가까운 순서대로 정렬. </param>
    /// <returns></returns>
    public List<GameObject> SearchUnits(Vector3 mPosition, string[] unitIDs, bool nearby) 
    {
        if (unitList.Count > 0)
        {
            List<GameObject> targetObjectList = new List<GameObject>();
            List<Target> targetList = new List<Target>();
            for (int i = 0; i < unitList.Count; i++)
            {
                for(int j=0; j< unitIDs.Length; j++)
                {
                    if (unitList[i].name == unitIDs[j])
                    {                        
                        Vector3 mPos = new Vector3(mPosition.x, mPosition.y, 0);
                        Vector3 tPos = new Vector3(unitList[i].transform.position.x, unitList[i].transform.position.y, 0);
                        float dist = Vector3.Distance(mPos, tPos);
                        Target t = new Target(unitList[i], dist);                        
                        targetList.Add(t);
                        targetObjectList.Add(unitList[i]);
                    }
                }
            }
            if(targetList.Count == 0) { return null; }
            else {
                if(nearby == false) { return targetObjectList; }
                else
                {
                    targetList.Sort(delegate (Target a, Target b) {
                        return a.dist.CompareTo(b.dist);
                    }); //First is minimom value in dists.  
                    targetObjectList = new List<GameObject>();
                    foreach (Target go in targetList) { targetObjectList.Add(go.gObject); }
//                    targetObjectList.Add(targetList[0].gObject);
                    return targetObjectList;
                }                
            }
        }
        else
        {
            return null;
        }
    }

    public int UnitCountCheck(string puzzleID)
    {
        int unitCnt = 0;
        foreach (GameObject unit in LandManager.instance.GetComponent<UnitManager>().unitList)
        {
            if (unit.name == puzzleID) { unitCnt++; }
        }//unitCountCheck
        return unitCnt;
    }

    class Target
    {
        public GameObject gObject;
        public float dist;
        public Target(GameObject go, float dist)
        {
            gObject = go;
            this.dist = dist;
        }

    }
    #endregion

}
