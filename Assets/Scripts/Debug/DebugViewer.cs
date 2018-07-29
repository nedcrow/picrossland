using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugViewer : MonoBehaviour {

    [SerializeField]
    public List<GameObject> debugTextObjectList = new List<GameObject>();


    #region Singleton
    private static DebugViewer _instance;
    public static DebugViewer Instance
    {
        get
        {
            if (!_instance)
            {
                GameObject obj = GameObject.Find("Debug");
                if (obj == null)
                {
                    obj = new GameObject("Debug");
                    obj.AddComponent<DebugViewer>();
                }
                return obj.GetComponent<DebugViewer>();
            }
            return _instance;
        }
    }
    #endregion
}
