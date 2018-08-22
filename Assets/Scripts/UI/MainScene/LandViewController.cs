using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandViewController : MonoBehaviour {

    Button LandChangeButton_R;
    Button LandChangeButton_L;

    private void Start()
    {
        SetLandChangeButton();
    }

    public void SetLandName(int currentLandID)
    {
        if(Resources.Load<Sprite>("Sprite/Land/LandTitle" + currentLandID))
        {
            transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Land/LandTitle" + currentLandID);
        }
        else
        {
            Debug.Log("No Resource : LandTitle"+currentLandID);
        }

    }

    public void SetLandChangeButton()
    {
        LandChangeButton_R = transform.GetChild(2).GetComponent<Button>();
        LandChangeButton_L = transform.GetChild(3).GetComponent<Button>();

        LandChangeButton_R.onClick.AddListener(delegate {
            LandManager.instance.LandChange("R");
        }); // Right Button

        LandChangeButton_L.onClick.AddListener(delegate {
            LandManager.instance.LandChange("L");
        }); // Left Button
    }
}
