using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTest : MonoBehaviour {



	// Use this for initialization
	void Start () {
        int[] sample = new int[7];
        int[] lotto = { 1, 2, 3, 4, 5, 6 };
        
        for(int i=0; i<7; i++)
        {
            int rNum = Random.Range(1, 46);            
            for(int j=0; j<i; j++)
            {
                int x=0;
                if(sample[j] == rNum) { x++; i = i - 1; }
                if (x == 0) { sample[i] = rNum; }
            }
        }
        foreach(int i in sample)
        {
            Debug.Log(i);
        }
        int loopCount = 100000000;
        int sameNum = 0;
        int loopNum = 0;
        bool bonus=false;
        for (int i=0; i< loopCount; i++)
        {
            for(int j=0; j<lotto.Length; j++)
            {
                for(int k=0; k<sample.Length; k++)
                {
                    if(lotto[j] == sample[k]) { sameNum++; if (k == 6) { bonus = true; } }
                    
                }
            }
            loopNum++;
            if (sameNum == 6) {
                if (bonus == true) { Debug.Log(loopNum + " 2등"); }
                else { i = loopCount + 1; Debug.Log(loopNum + " 1등"); }
            }
            else if (i >= loopCount - 1) { Debug.Log("end "+(i+1)+"번 반복"); }
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
