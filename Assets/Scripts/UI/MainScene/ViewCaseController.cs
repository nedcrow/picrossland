using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCaseController : MonoBehaviour {

    public void CaseOnOff()
    {
        if (transform.GetChild(0).gameObject.activeSelf == true) {
            Debug.Log("what's the probolum. : "+ transform.GetChild(0).gameObject.name);
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            LandManager.instance.views.againView.transform.GetChild(2).GetComponent<PuzzleIconListController>().PuzzleIconOnOff();
            StartCoroutine(UnHideTouchCheck());
        }
    }

    IEnumerator UnHideTouchCheck()
    {
        Vector3 skillHome = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            bool touch = false;

            if (Input.GetMouseButton(0))
            {
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) { touch = true; }
            }
            if (Input.touchCount > 0 || touch == true)
            {
                if (transform.GetChild(0).gameObject.activeSelf == false) {
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.SetActive(true);
                    LandManager.instance.views.againView.transform.GetChild(2).GetComponent<PuzzleIconListController>().PuzzleIconOnOff();
                    LandManager.instance.views.againView.transform.GetChild(2).GetComponent<PuzzleIconListController>().StartDragCheck();

                    Debug.Log("Close HideBtn");
                    break;
                }                
            }
            yield return new WaitForSeconds(0.02f);
        }
    }
}
