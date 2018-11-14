using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {

    public Sprite plusIcon;
    public Sprite minusIcon;

    void Start () {
        DontDestroyOnLoad(transform.gameObject);
    }

    void LoadIcon()
    {
        plusIcon = Resources.Load<Sprite>("Sprite/PlusIcon");
        minusIcon = Resources.Load<Sprite>("Sprite/MinusIcon");
    }

}
