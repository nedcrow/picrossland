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
                effectObjList.Add(new GameObject());
                effectObjList[i].transform.SetParent(transform);
                effectObjList[i].SetActive(false);
            }
        }

        public void Pickup(GameObject go, float limit, float speed = 0.01f, Vector3 startPos = new Vector3())
        {
            if (startPos == Vector3.zero) { startPos = go.transform.position; }
            if (effectObjList.Count == 0) { effectObjList.Add(new GameObject()); }//GameObject부족 시 1차 보충. 
            Duplicate_AtoB(effectObjList[0], go, go.transform.parent);
            if (go.transform.GetChildCount() > 0)
            {
                if(go.transform.GetChildCount() > effectObjList.Count - 1) {
                    for (int i = 0; i < go.transform.GetChildCount()- effectObjList.Count-1; i++){
                        effectObjList.Add(Instantiate(new GameObject()));
                    }
                }//GameObject부족 시 2차 보충.
                for(int i=0; i< go.transform.GetChildCount(); i++)
                {
                    Duplicate_AtoB(effectObjList[1],go.transform.GetChild(i).gameObject, effectObjList[0].transform); //need debug
                    effectObjList.RemoveAt(1);
                }
            }//만약 자식obj가 필요할경우 자식obj도 복사.
            StartCoroutine(Pickup_Co(effectObjList[0], limit, speed, startPos));
            effectObjList.RemoveAt(0);
        }//target GameObject를 특정 속도로 수직 상승하게 함.

        void Duplicate_AtoB(GameObject a, GameObject b, Transform parentPos)
        {
            Debug.Log(a.name+", "+ b.name);
            a.transform.SetParent(parentPos);
            a.transform.position = b.transform.position;
            a.SetActive(true);
            if (b.GetComponent<SpriteRenderer>()) {
                if (!a.GetComponent<SpriteRenderer>()) { a.AddComponent<SpriteRenderer>(); }
                else
                {
                    a.GetComponent<SpriteRenderer>().sprite = b.GetComponent<SpriteRenderer>().sprite;
                }
            }
        }//기본적인 복사.

        IEnumerator Pickup_Co(GameObject tempObj, float limit, float speed, Vector3 startPos)
        {
            tempObj.SetActive(true);
            effectObjList_Active.Add(tempObj);
            active_Num = effectObjList_Active.Count-1;
            int myNum = active_Num;
            float addP = 0;            
            while (true)
            {
                addP = addP + speed;
                tempObj.transform.position = new Vector3(startPos.x, startPos.y+addP, startPos.z);
                if(addP > limit) {
                    int cnt = tempObj.transform.GetChildCount();
                    if (cnt > 0) {
                        for (int i = cnt-1; i < cnt; i--)
                        {
                            effectObjList.Add(tempObj.transform.GetChild(i).gameObject);
                        }
                    }
                    tempObj.SetActive(false);
                    tempObj.transform.SetParent(transform);
                    effectObjList.Add(tempObj);
                    effectObjList_Active.Remove(tempObj);
                    break;
                }
                yield return new WaitForSeconds(0.08f);
            }
        }
    }
}
