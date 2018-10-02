using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class UnitBase : MonoBehaviour
    {
        public static void UnitIdle(GameObject target, string IdleNum="")
        {
            if (target.GetComponent<Animator>())
            {
                string aniName = target.transform.parent.name + "_Idle";
                aniName = IdleNum == "" ? aniName : aniName + IdleNum;
                target.GetComponent<Animator>().Play(aniName);
            }
        }

        public static void Unit_Death(GameObject target)
        {
            if (target.GetComponent<Animator>())
            {
                string aniName = target.transform.parent.name + "_Death";                
                target.GetComponent<Animator>().Play(aniName);
            }
        }

        public static Vector3 RandomUnitPos(int xMax, int yMax) {
            float x = Random.Range(0, xMax+1) * 0.2f;
            float y = Random.Range(0, yMax+1) * 0.2f;


            Vector3 unitPos = new Vector3(x-2,y-2,y-5);//z : 최소값이 0.1f이 나오도록.
            //Debug.Log(unitPos+", "+x+", "+y);
            return unitPos;
        }//min:-2 ~ max:2

        public static void Search(string targetID)
        {
            foreach(GameObject land in LandManager.instance.gotLandObjList)
            {
                if (land.name == LandManager.instance.currentLand.id.ToString()) {
                    for(int i=0; i< land.GetComponent<LandController>().units.transform.childCount; i++){
                        if(land.GetComponent<LandController>().units.transform.GetChild(i).name == targetID) { Debug.Log("찾았다"); }
                    }
                }
            }
        }

        public static int FindSameUnit(string targetName)
        {
            int sameCount = 0;
            if (LandManager.instance.GetComponent<UnitManager>().unitList.Count > 0)
            {
                for (int i = 0; i < LandManager.instance.GetComponent<UnitManager>().unitList.Count; i++)
                {
                    if (targetName == LandManager.instance.GetComponent<UnitManager>().unitList[i].name)
                    {
                        sameCount++;
                    }
                }
            }
            return sameCount;
        }
    }

    public class Fighter : MonoBehaviour
    {
        public static void Attack(GameObject target)
        {
            if (target.GetComponent<Animator>())
            {
                string aniName = target.transform.parent.name + "_Attack"; Debug.Log(aniName);
                target.GetComponent<Animator>().Play(aniName);
            }
        }
        public static void Hit(GameObject target)
        {
            if (target.GetComponent<Animator>())
            {
                string aniName = target.transform.parent.name + "_Death";
                target.GetComponent<Animator>().Play(aniName);
            }
        }
        public static void Afraide(GameObject target)
        {
            if (target.GetComponent<Animator>())
            {
                string aniName = target.transform.parent.name + "_Hit";
                target.GetComponent<Animator>().Play(aniName);
            }
        }
    }

    public class Mover : MonoBehaviour
    {
        public static void UnitMove(GameObject target, string Dir = "")
        {
            Debug.Log(Dir);
            if (target.GetComponent<Animator>())
            {
                string aniName;
                if ( Dir == "")
                {
                    aniName = target.transform.parent.name + "_Move";
                }
                else
                {
                    aniName = target.transform.parent.name + "_Move_" + Dir;
                }                
                Debug.Log(aniName);
                target.GetComponent<Animator>().Play(aniName);
            }
        }
    }
}
