using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {



    public GameObject sample;
    Sprite[] loadSprite;
    Texture2D loadTexture;
    int num;
    private void Start()
    {
        num = 0;
        loadSprite = Resources.LoadAll<Sprite>("Sprite/Floor_Base_2");

        loadTexture = loadSprite[num].texture;


        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {

                Color pickColor = loadTexture.GetPixel(i,j);
                Instantiate(sample, new Vector3(i*0.2f, j*0.2f, 0), Quaternion.identity);                
                sample.GetComponent<SpriteRenderer>().color = pickColor;
                Debug.Log(pickColor);
            }            
        }
    }
    

}
