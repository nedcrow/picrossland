using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NickNameController : MonoBehaviour {

    public Text nickName;
    public Text inputF;
    public Text notice;
    public GameObject btn_Complte;

    private void Start()
    {
        Set_Btn_Complite();
    }

    void Set_Btn_Complite()
    {
        btn_Complte.GetComponent<Button>().onClick.AddListener(delegate {
            MainDataBase.instance.NickNameChange(nickName.text);
        });
    }

    public void FeildSelect()
    {
        notice.text = "";
        btn_Complte.SetActive(true);
    }
}
