using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour {

    public GameObject[] views;
    [HideInInspector]
    public GameObject firstView;
    [HideInInspector]
    public GameObject againView;
    [HideInInspector]
    public GameObject puzzleView;
    [HideInInspector]
    public GameObject popupView;

    private void Awake()
    {
        views = new GameObject[]
        {
        firstView = transform.GetChild(0).gameObject,
        againView = transform.GetChild(1).gameObject,
        puzzleView = transform.GetChild(2).gameObject,
        popupView = transform.GetChild(3).gameObject

        };
        LandManager.instance.BaseSetting();
    }

    /// <summary>
    /// 0:firstView, 1:againView, 2:puzzleView
    /// </summary>
    public void SceneOn(int num)
    {
        views[num].SetActive(true);
    }

    /// <summary>
    /// 0:firstView, 1:againView, 2:puzzleView
    /// </summary>
    public void SceneOff(int num)
    {
        views[num].SetActive(false);
    }

    public void SceneOffDelay(int num)
    {
        StartCoroutine(WaitForOff(num));
        
    }

    IEnumerator WaitForOff(int num)
    {
        yield return new WaitForSeconds(1f);
        views[num].SetActive(false);
    }
}
