﻿using System.Collections;
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
                string aniName = target.transform.parent.name + "_Idle"; //Debug.Log("aniName(Idle) : "+aniName);
                aniName = (IdleNum == "") || (IdleNum == "0") ? aniName : aniName + IdleNum;
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

        public static List<GameObject> FindSameUnit(string targetName)
        {
            List<GameObject> sameList = new List<GameObject>();
            if (LandManager.instance.GetComponent<UnitManager>().unitList.Count > 0)
            {
                for (int i = 0; i < LandManager.instance.GetComponent<UnitManager>().unitList.Count; i++)
                {
                    if (targetName == LandManager.instance.GetComponent<UnitManager>().unitList[i].name)
                    {
                        sameList.Add(LandManager.instance.GetComponent<UnitManager>().unitList[i]);
                    }
                }
            }
            return sameList;
        }
    }

    public class FighterMotion : MonoBehaviour
    {
        public static void Attack(GameObject target, string attackKind = "")
        {
            if (target.GetComponent<Animator>())
            {
                string aniName = target.transform.parent.name + "_Attack"+attackKind; Debug.Log(aniName);
                target.GetComponent<Animator>().Play(aniName);
            }
        }       
        
        public static void Hit(GameObject target, string hitKind ="")
        {
            if (target.GetComponent<Animator>())
            {
                string aniName = target.transform.parent.name + "_Hit"+hitKind; //Debug.Log(aniName);
                target.GetComponent<Animator>().Play(aniName);
            }
        }

        public static void Afraide(GameObject target)
        {
            if (target.GetComponent<Animator>())
            {
                string aniName = target.transform.parent.name + "_Afraide";
                Debug.Log(aniName);
                target.GetComponent<Animator>().Play(aniName);
            }
        }
    }

    public class MoverMotion : MonoBehaviour
    {
        public static void UnitMove(GameObject target, string Dir = "")
        {
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
                target.GetComponent<Animator>().Play(aniName);
                //Debug.Log("aniName(Move) : " + aniName);
            }
        }

        public static void UnitRun(GameObject target, string Dir = "")
        {
            if (target.GetComponent<Animator>())
            {
                string aniName;
                if (Dir == "")
                {
                    aniName = target.transform.parent.name + "_Run";
                }
                else
                {
                    aniName = target.transform.parent.name + "_Run" + Dir;
                }
                target.GetComponent<Animator>().Play(aniName);
                //Debug.Log("aniName(Move) : " + aniName);
            }
        }

        public static void Contact(GameObject target, string unitID)
        {
            if (target.GetComponent<Animator>())
            {
                string aniName = target.transform.parent.name + "_Contact_"+unitID;
                //Debug.Log(aniName);
                target.GetComponent<Animator>().Play(aniName);
            }
        }

    }


}
