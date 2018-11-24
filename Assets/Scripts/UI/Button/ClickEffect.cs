using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickEffect : MonoBehaviour
{

    List<IEnumerator> effectList;

    private void Start()
    {
        effectList = new List<IEnumerator>();
        effectList.Add(Frizzy());
        effectList.Add(FadeOut());
        effectList.Add(AfterLock(0.5f));
    }

    public void clicked(int efNum)
    {
        StartCoroutine(effectList[efNum]);
    }

    IEnumerator Frizzy()
    {
        float a = 0.6f; //max = 2 = 반원;
        float maxSize = 1.3f;
        float minSize = 1f;
        float speed = 0.25f;

        while (a < 2)
        {
            yield return new WaitForSeconds(0.062f); // about 16f/s
            float x = (maxSize - minSize) * (float)Math.Sin(a * Math.PI * 0.5f);
            GetComponent<RectTransform>().localScale = new Vector3(minSize + x, minSize + x, 1);
            a = a + speed;
        }
        GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        yield return null;
    }

    IEnumerator FadeOut()
    {
        float a = 1.3f;
        float speed = 0.2f;
        if (GetComponent<Image>())
        {
            Image image = GetComponent<Image>();
            image.color = new Vector4(0, 0, 0, a);

            while (a > 0.2f)
            {
                a = a - speed;
                image.color = new Vector4(0, 0, 0, a);
                yield return new WaitForSeconds(0.083f);
            }
            image.color = new Vector4(0, 0, 0, 0);
            GetComponent<RectTransform>().localScale = Vector3.one;
            gameObject.SetActive(false);
        }
    }

    IEnumerator AfterLock(float delayTime)
    {
        float waitSec = 0;
        if (GetComponent<Button>())
        {
            while (waitSec < delayTime)
            {
                GetComponent<Button>().enabled = false;
                waitSec += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            GetComponent<Button>().enabled = true;
        }
    }
}
