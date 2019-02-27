using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarimTool.EditValue;

public class TutorialController : MonoBehaviour {

    /// <summary>
    /// parent에 tutorialPanal을 생성한다.
    /// </summary>
    /// <param name="tutorialNum"></param>
    /// <param name="parent"></param>
	public void Tutorial(string tutorialNum, Transform parent)
    {
        if (parent.GetChildCount() > 0) { Destroy(parent.GetChild(0).gameObject); }
        GameObject tutorial = Instantiate(Resources.Load<GameObject>("Prefabs/Tutorials/" + tutorialNum));
        if (tutorial != null) {
            tutorial.transform.SetParent(parent);
            tutorial.SetActive(true);
            PuzzleIconListController puzzleIconCon = PuzzleManager.instance.viewCon.againView.transform.GetChild(2).GetComponent<PuzzleIconListController>();
            //tutorial.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.height);
            //tutorial.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.width);
            tutorial.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 720); 
            tutorial.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1280);
            tutorial.GetComponent<RectTransform>().localPosition = Vector3.zero;
            tutorial.GetComponent<RectTransform>().localScale = Vector3.one;
            Debug.Log(string.Format("width : {0}, height : {1}, addScreenX : {2}", Screen.width, Screen.height, puzzleIconCon.addScreenX));
        }
        else
        {
            Debug.Log("Null Gameobject : tutorial _ " + tutorialNum);
        }        
    }

    public void NextTutorial(string tutorialNum="default")
    {
        if (tutorialNum == "default" || tutorialNum == "") {            
            tutorialNum = EditText.Left(name, 3) + (System.Convert.ToInt32(EditText.Right(name, 1)) + 1).ToString(); 
        }

        Tutorial(tutorialNum, transform.parent);
    }

}
