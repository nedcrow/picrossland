using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour {

    public void WaitLoading()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(WaitLoading_());
        
    }

    IEnumerator WaitLoading_()
    {
        GetComponent<Image>().color = new Vector4(0, 0, 0, 1.3f);
        while (true)
        {
            //Debug.Log("loading");
            if (MainDataBase.instance.loadAll == true) {               

                yield return new WaitForSeconds(0.5f);
                transform.GetComponent<ClickEffect>().clicked(1);//fadeOut                
                Debug.Log("done loading");
                break;
            }
            //gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);            
        }
    }
}
