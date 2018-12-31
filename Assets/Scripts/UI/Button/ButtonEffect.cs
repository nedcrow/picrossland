﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEffect : MonoBehaviour {

    Coroutine effect_Co;
    /// <summary>
    /// sizeR & unit : pixel
    /// </summary>
    /// <param name="sizeR"></param>
    /// <param name="unit"></param>
	public void SideLight(int sizeR, int unit = 1)
    {
        if(effect_Co != null) { StopCoroutine(effect_Co); }
        effect_Co = StartCoroutine(sideLight_Co(sizeR, unit));        
    }

    IEnumerator sideLight_Co(int sizeR, int unit = 1)
    {
        int baseSizeX = 0 - sizeR; //base localPosition
        int baseSizeY = 0 + sizeR;
        
        while (true)
        {
            if(transform.localPosition.x >= 0 + sizeR && transform.localPosition.y <= 0 - sizeR) {
                transform.localPosition = new Vector3(baseSizeX, baseSizeY, transform.localPosition.z);
            } else {
                if (transform.localPosition.x < 0 + sizeR) { transform.position = new Vector3(transform.position.x + unit, transform.position.y, transform.position.z); }
                else { transform.position = new Vector3(transform.position.x, transform.position.y - unit, transform.position.z); }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
