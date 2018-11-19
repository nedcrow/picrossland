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
        
        public void Pickup(GameObject go, float limit, float speed = 0.01f, Vector3 startPos = new Vector3())
        {
            if (startPos == Vector3.zero) { startPos = go.transform.position; }
            go.SetActive(true);
            StartCoroutine(Pickup_Co(go, limit, speed, startPos));
        }//target GameObject를 특정 속도로 수직 상승하게 함.

        IEnumerator Pickup_Co(GameObject go, float limit, float speed, Vector3 startPos)
        {
            go.SetActive(true);
            float addP = 0;            
            while (true)
            {
                addP = addP + speed;
                go.transform.position = new Vector3(startPos.x, startPos.y+addP, startPos.z);
                if(addP > limit) { go.SetActive(false); break; }
                yield return new WaitForSeconds(0.08f);
            }
        }
    }
}
