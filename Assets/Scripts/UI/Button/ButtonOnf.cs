using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOnf : MonoBehaviour {

    public void ButtonOnOff(bool onf)
    {
        Debug.Log("ButtonOnf: "+onf);
        gameObject.SetActive(onf);
    }


}
