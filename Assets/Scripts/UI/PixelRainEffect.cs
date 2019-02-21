using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelRainEffect : MonoBehaviour {

    #region EffectProperty
    public GameObject dropObj;
   
    [System.Serializable]
    public struct EffectBase {
        public float lifeTime;
        public int spwanCount; //CountPerSpawn
        public int width;
        public int height;
    }

    [System.Serializable]
    public struct EffectSpeed
    {
        public float framePerSecond; // perSecond
        public float framePerSpwan; // perSecond
        public int stepSize;
    }

    [System.Serializable]
    public struct EffectAlpha
    {
        public bool Onf;
        [Range(1.5f, 0)]
        public float startAlpha; // perSecond
        [Range(0.99f, -1)]
        public float endAlpha; // perSecond
    }

    public EffectBase effectBase = new EffectBase();
    public EffectSpeed dropSpeed = new EffectSpeed();
    public EffectAlpha dropAlpha = new EffectAlpha();
    #endregion
    // inspector의 이펙트 변수 설정에 사용.

    List<Drop> DropObjList_Active = new List<Drop>();
    List<Drop> DropObjList_Rest = new List<Drop>();

    Color tColor;

    private void Start()
    {
        StartCoroutine(PixelRain_Co());
    }

    IEnumerator PixelRain_Co()
    {
        DropObjCountSetting();
        float time = 0;
        float spwanTime = dropSpeed.framePerSpwan;
        while (true)
        {
            if (spwanTime >= dropSpeed.framePerSpwan) {
                DropSpawn();
                spwanTime = 0;
            }
            DropMove();
            time += 1 / dropSpeed.framePerSecond;
            spwanTime ++;
            yield return new WaitForSeconds(1/dropSpeed.framePerSecond);
        }
    }

    void DropObjCountSetting()
    {
        int count = (Mathf.CeilToInt(((effectBase.lifeTime * dropSpeed.framePerSecond) / dropSpeed.framePerSpwan)) + 1) * effectBase.spwanCount; //Debug.Log("DropSetting : " + count);

        for (int i = 0; i < count; i++)
        {
            DropObjList_Rest.Add(new Drop(Instantiate(dropObj)));
            DropObjList_Rest[i].dropObj.transform.SetParent(transform);
            DropObjList_Rest[i].dropObj.SetActive(false);
        }
    }

    void DropSpawn()
    {
        for (int i=0; i<effectBase.spwanCount; i++)
        {
            float posX = Random.Range(0,effectBase.width+1) - effectBase.width*0.5f;
            float posY = Random.Range(0,effectBase.height+1) - effectBase.height;
            Vector3 tPos = new Vector3(posX, posY, 0);

            DropObjList_Rest[0].dropObj.SetActive(true);
            if (dropAlpha.Onf == true)
            {
                tColor = DropObjList_Rest[0].dropObj.GetComponent<Image>().color;
                DropObjList_Rest[0].dropObj.GetComponent<Image>().color = new Vector4(tColor.r, tColor.g, tColor.b, dropAlpha.startAlpha);
            }
            DropObjList_Rest[0].dropObj.transform.localPosition = tPos;
            DropObjList_Active.Add(DropObjList_Rest[0]);
            DropObjList_Rest.RemoveAt(0);
        }
    }

    void DropMove()
    {
        float minusAlpha = 0;
        if (dropAlpha.Onf == true) {
            minusAlpha = Mathf.Abs(dropAlpha.startAlpha - dropAlpha.endAlpha) / (effectBase.lifeTime * dropSpeed.framePerSecond);
        } // 1회 Alpha감소치.
        
        bool remove = false;
        if (DropObjList_Active.Count > 0)
        {
            foreach (Drop drop in DropObjList_Active)
            {

                tColor = drop.dropObj.GetComponent<Image>().color;
                drop.dropObj.GetComponent<Image>().color = new Vector4(tColor.r, tColor.g, tColor.b,tColor.a-minusAlpha);                

                drop.dropObj.transform.localPosition = new Vector3(drop.dropObj.transform.localPosition.x, drop.dropObj.transform.localPosition.y - dropSpeed.stepSize, drop.dropObj.transform.localPosition.z);
                drop.dropTime += 1 / dropSpeed.framePerSecond;
                if (drop.dropTime >= effectBase.lifeTime) { remove = true; }
            }
            if (remove == true) { DropRemove(); }
        }
    }

    void DropRemove()
    {
        for (int i = 0; i < effectBase.spwanCount; i++)
        {
            DropObjList_Active[0].dropTime = 0;
            DropObjList_Active[0].dropObj.SetActive(false);
            DropObjList_Rest.Add(DropObjList_Active[0]);
            DropObjList_Active.RemoveAt(0);
        }
    }

   

}

public class Drop  {
    public GameObject dropObj;
    public float dropTime;
    public Drop (GameObject dropObj)
    {
        this.dropObj = dropObj;
        this.dropTime = 0;
    }
}

