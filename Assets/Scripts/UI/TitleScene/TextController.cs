using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {

    public int count = 3;

	void Start () {
        if (GetComponent<Text>()) { StartCoroutine(AddDot(GetComponent<Text>())); }
	}
	
	IEnumerator AddDot(Text target)
    {
        string startText = target.text;
        while (true)
        {
            for(int i=0; i<count; i++)
            {
                target.text = target.text + ".";
                yield return new WaitForSeconds(0.5f);
            }
            target.text = startText;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
