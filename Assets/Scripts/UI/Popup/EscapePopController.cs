using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EscapePopController : MonoBehaviour {

	public void ReadyToEscape(float limitSec)
    {
        transform.GetChild(0).GetComponent<Text>().color = Vector4.one;
        StartCoroutine(ReadyToEscape_Co(limitSec));
    }

    IEnumerator ReadyToEscape_Co(float limitSec)
    {
        float a=0;
        while (true)
        {
            a = a + 0.1f;
            transform.GetChild(0).GetComponent<Text>().color = new Vector4(1,1,1,limitSec-a);
            yield return new WaitForSeconds(0.01f);
            if(a >= limitSec) { break; }
        }
    }//a가 0.01f이면 limitSec만큼 걸림.
}
