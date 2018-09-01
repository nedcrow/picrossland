using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkNineTeen : MonoBehaviour {

	public void NineTeenMotion(float time=0.0f)
    {
        float t = time == 0 ? 1 : time;
        StartCoroutine(NineTeenMotion_Co(t));
    }

    IEnumerator NineTeenMotion_Co(float cutline)
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        float sec=0.084f;
        float time=0;
        Vector3 startPos = transform.localPosition;
        while (true)
        {
            float rX = Random.Range(-0.05f, 0.06f);
            float rY = Random.Range(-0.05f, 0.06f);
            transform.localPosition = new Vector3(startPos.x+rX, startPos.y+rY, -1);
            if(time > cutline)
            {
                GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0);
                break;
            }
            yield return new WaitForSeconds(sec);
        }
    }
}
