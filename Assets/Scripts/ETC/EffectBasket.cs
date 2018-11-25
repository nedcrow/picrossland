using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectBasket
{
    public class EffectBasket : MonoBehaviour
    {
        #region Singleton
        private static EffectBasket _instance;
        public static EffectBasket instance
        {
            get
            {
                if (!_instance)
                {
                    GameObject obj = GameObject.Find("EffectBasket");
                    if (obj == null)
                    {
                        obj = new GameObject("EffectBasket");
                        obj.AddComponent<EffectBasket>();
                    }
                    return obj.GetComponent<EffectBasket>();
                }
                return _instance;
            }
        }
        #endregion

        int active_Num=0;

        List<GameObject> effectObjList = new List<GameObject>();
        List<GameObject> effectObjList_Active = new List<GameObject>();

        private void Start()
        {
            for(int i=0; i<5; i++)
            {
                AddEffectObj();
            }
        }

        void AddEffectObj() {
            effectObjList.Add(new GameObject());
            effectObjList[effectObjList.Count - 1].name = "emptyEffect";
            effectObjList[effectObjList.Count - 1].transform.SetParent(transform);
            effectObjList[effectObjList.Count - 1].SetActive(false);
        }

        public void Pickup(GameObject go, float limit, float speed = 0.01f, Vector3 startPos = new Vector3())
        {
            if (startPos == Vector3.zero) { startPos = go.transform.position; }
            if (effectObjList.Count == 0) { AddEffectObj(); }//GameObject부족 시 1차 보충. 
            Duplicate_AtoB(effectObjList[0], go);
            if (go.transform.GetChildCount() > 0)
            {
                if(go.transform.GetChildCount() > effectObjList.Count - 1) {
                    int needCnt = go.transform.GetChildCount() - (effectObjList.Count - 1); //필요 갯수 - 보유 갯수
                    for (int i = 0; i < needCnt ; i++){
                        AddEffectObj();
                    }
                }//GameObject부족 시 2차 보충.
                for(int i=0; i< go.transform.GetChildCount(); i++)
                {
                    Duplicate_AtoB(effectObjList[1],go.transform.GetChild(i).gameObject, effectObjList[0].transform);
                    effectObjList.RemoveAt(1);
                }
            }//만약 자식obj가 필요할경우 자식obj도 복사.
            StartCoroutine(Pickup_Co(effectObjList[0], limit, speed, startPos));
            effectObjList.RemoveAt(0);
            go.SetActive(false);
        }//target GameObject를 특정 속도로 수직 상승하게 함.

        void Duplicate_AtoB(GameObject a, GameObject b, Transform parentPos = null)
        {
            a.name = b.name;
            a.transform.SetParent(parentPos);
            a.transform.position = b.transform.position;
            a.transform.localScale = b.transform.lossyScale;
            a.SetActive(true);
            if (b.GetComponent<SpriteRenderer>()) {
                if (!a.GetComponent<SpriteRenderer>()) { a.AddComponent<SpriteRenderer>(); a.GetComponent<SpriteRenderer>().enabled = true; }                
                a.GetComponent<SpriteRenderer>().sprite = b.GetComponent<SpriteRenderer>().sprite;
                a.GetComponent<SpriteRenderer>().color = b.GetComponent<SpriteRenderer>().color;
            }
            else
            {
                if (a.GetComponent<SpriteRenderer>())
                {
                    a.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }//기본적인 복사.

        IEnumerator Pickup_Co(GameObject tempPickUpObj, float limit, float speed, Vector3 startPos)
        {
            tempPickUpObj.SetActive(true);
            effectObjList_Active.Add(tempPickUpObj);
            active_Num = effectObjList_Active.Count-1;
            int myNum = active_Num;
            float addP = 0;            
            while (true)
            {
                addP = addP + speed;
                tempPickUpObj.transform.position = new Vector3(startPos.x, startPos.y+addP, startPos.z);
                if(addP > limit) {
                    RemovPickUpObj(tempPickUpObj);
                    break;
                }
                yield return new WaitForSeconds(0.08f);
            }
        }

        void RemovPickUpObj(GameObject tempPickUpObj)
        {
            int cnt = tempPickUpObj.transform.GetChildCount();
            if (cnt > 0)
            {
                for (int i = cnt; i > 0; i--)
                {
                    effectObjList.Add(tempPickUpObj.transform.GetChild(i-1).gameObject);
                    effectObjList[effectObjList.Count - 1].SetActive(false);
                    effectObjList[effectObjList.Count - 1].transform.SetParent(transform);
                }
            }
            tempPickUpObj.SetActive(false);
            tempPickUpObj.transform.SetParent(transform);
            effectObjList.Add(tempPickUpObj);
            effectObjList_Active.Remove(tempPickUpObj);
        }
    }
}
