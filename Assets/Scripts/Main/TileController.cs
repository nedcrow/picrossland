using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

    public bool answer;
    /// <summary>
    /// 0:empty, 1: check, 2: X-mark, 3: ?mark 
    /// </summary>
    public int check;
    public Vector2 pos;

    private void Awake()
    {
        answer = false;
        check = 0;
        pos = new Vector2(0, 0);
    }

    public void ClickedTile()
    {
        PuzzleManager.instance.puzzleKeyBrush.Bingo(new Vector2(4, 0));
        //클릭 당해버렸어;
        //내 위치를 알릴게. 완성된 줄 있니?
    }

    public void TileEffect()
    {
        StartCoroutine(Frizzy());
    }

    IEnumerator Frizzy()
    {
        float a = 0.6f; //max = 2 = 반원;
        float maxSize = 1.3f;
        float minSize = 0.5f;
        float speed = 0.25f;

        while (a < 2)
        {
            yield return new WaitForSeconds(0.083f); // about 12f/s
            float x = (maxSize - minSize) * (float)Math.Sin(a * Math.PI * 0.5f);
            transform.localScale = new Vector3(minSize + x, minSize + x, 1);
            a = a + speed;
        }
        transform.localScale = new Vector3(1,1,1);
        yield return null;
    }
}
