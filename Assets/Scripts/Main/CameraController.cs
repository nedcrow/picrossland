using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Vector3 CameraPos_Land()
    {
        Vector3 cPos = new Vector3(2.5f, -1, -10);

        return cPos;
    }

    public Vector3 CameraPos_Puzzle(int x, float tileUnit)
    {
        Vector3 cPos = new Vector3(0, 0, -10);
        switch (x)
        {
            case 10:
                cPos = new Vector3((x * 0.5f * 0.92f) * tileUnit, (x * 0.5f * 0.7f) * tileUnit, -10);
                break;
            case 15:
                cPos = new Vector3((x * 0.5f * 0.88f) * tileUnit, (x * 0.5f * 0.67f) * tileUnit, -10);
                break;
            case 20:
                cPos = new Vector3((x * 0.5f * 0.87f) * tileUnit, (x * 0.5f * 0.65f) * tileUnit, -10);
                break;
            case 25:
                cPos = new Vector3((x * 0.5f * 0.86f) * tileUnit, (x * 0.5f * 0.64f) * tileUnit, -10);
                break;
            case 30:
                cPos = new Vector3((x * 0.5f * 0.85f) * tileUnit, (x * 0.5f * 0.64f) * tileUnit, -10);
                break;
            default:
                Debug.Log("Puzzle size error");
                break;
        }

        if (x == 10 || x == 20 || x == 30)
        {
        }
        else if (x == 10)
        {
        }
        return cPos;
    }
}