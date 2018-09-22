using System.Collections;
using UnityEngine;



public class AppEscapeChecker : MonoBehaviour
{
    [Range(1, 10)]
    public float limitSec = 3;
    public bool inputtedEscape;

    void Start()
    {
        inputtedEscape = false;
        StartCoroutine(EscapeCheck(limitSec));
    }

    IEnumerator EscapeCheck(float limitSec = 3.0f)
    {
        float inputTime = 0;
        while (true)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (inputtedEscape == false)
                {
                    LandManager.instance.views.popupView.GetComponent<PopupViewController>().escapePop.GetComponent<EscapePopController>().ReadyToEscape(limitSec);
                    //한 번 더 누르면 종료됩니다.
                    inputtedEscape = true;
                }
                else
                {
                    inputtedEscape = false;
                    HarimTool.Escape.Escape.AppQuit();
                    //종료
                    break; 
                }            
            }
            if (inputtedEscape == true)
            {
                if (inputTime < limitSec)
                {
                    inputTime += 0.01f;
                }
                else
                {
                    inputTime = 0;
                    inputtedEscape = false;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}


