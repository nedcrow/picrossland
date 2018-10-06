using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LinqTest : MonoBehaviour {

    List<float> testList = new List<float>();
	
	void Start () {
        testList.Add(12.23f);
        testList.Add(1.23f);
        testList.Add(14.23f);
        testList.Add(30.23f);

        testList.Sort(delegate (float a, float b) {
            return a.CompareTo(b);
        });

        foreach(float i in testList)
        {
            Debug.Log("alsdkjasldjlqnwlkxnfdkr : "+i);
        }
    }
}
