using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

	public void Tutorial(string tutorialNum, Transform parent)
    {
        if (parent.GetChildCount() > 0) { Destroy(parent.GetChild(0).gameObject); }
        GameObject tutorial = Instantiate(Resources.Load<GameObject>("Prefabs/Tutorials/" + tutorialNum));
        if (tutorial != null) {
            tutorial.transform.SetParent(parent);
            tutorial.SetActive(true);
            tutorial.GetComponent<RectTransform>().localPosition = Vector3.zero;//SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 48*addScreenY);
        }
        Debug.Log("myName : " + name);
        Debug.Log("tutorial : "+tutorialNum);
    }

    public void NextTutorial(string tutorialNum="default")
    {
        if (tutorialNum == "default" || tutorialNum == "") {            
            tutorialNum = HarimTool.EditValue.EditText.Left(name, 3) + (System.Convert.ToInt32(HarimTool.EditValue.EditText.Right(name, 1)) + 1).ToString(); 
        }

        Tutorial(tutorialNum, transform.parent);
    }

}
