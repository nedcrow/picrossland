using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {

    public Sprite plusIcon;
    public Sprite minusIcon;

    public Sprite ballot;
    public Sprite money;

    public Sprite[] icons;

    private void Awake()
    {
        LoadIcon();
    }

    void Start () {
        DontDestroyOnLoad(transform.gameObject);
    }

    void LoadIcon()
    {
        plusIcon = Resources.Load<Sprite>("Sprite/PlusIcon");
        minusIcon = Resources.Load<Sprite>("Sprite/MinusIcon");
        ballot = Resources.Load<Sprite>("Sprite/Ballot");
        money = Resources.Load<Sprite>("Sprite/Money");
        icons = Resources.LoadAll<Sprite>("Sprite/Icon");
    }

}
