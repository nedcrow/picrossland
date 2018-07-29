using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandViewController : MonoBehaviour {

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
}
