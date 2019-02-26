using System.Collections;
using System.Collections.Generic;
//using System.Net.NetworkInformation;
using UnityEngine;

public class NetworkConnectionChecker : MonoBehaviour {

    #region Singleton
    private static NetworkConnectionChecker _instance;
    public static NetworkConnectionChecker instance
    {
        get
        {
            if (!_instance)
            {
                GameObject obj = GameObject.Find("NetworkConnectionChecker");
                if (obj == null)
                {
                    obj = new GameObject("NetworkConnectionChecker");
                    obj.AddComponent<NetworkConnectionChecker>();
                }
                return obj.GetComponent<NetworkConnectionChecker>();
            }
            return _instance;
        }
    }
    #endregion

    public bool success;
    public static string CONNECTION_CHECK_URL = "http://clients3.google.com/gemerate_204";

    private void Awake()
    {
        ConnectionCheck();
    }

    void ConnectionCheck()
    {
        bool firstCheck = true;
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Network Connection error");
            firstCheck = false;
        }//notCunect
        else if(Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {

        }//local
        else 
        {
            
        }//wifi
        
        StartCoroutine(ConnectionCheck_C(firstCheck));
        //Debug.Log("Connection : "+success);
    }

    IEnumerator ConnectionCheck_C(bool firstCheck)
    {        
        float runTime =0;
        if (firstCheck == true)
        {
#if UNITY_EDITOR_WIN
            Debug.Log("firstCheck(editor)=" + firstCheck);
            success = true;
#endif
#if UNITY_ANDROID
            if(success == false)
            {
                Debug.Log("firstCheck(android)=" + firstCheck);
                Ping pinger = new Ping("216.58.216.164");//google.com
                while (pinger.isDone == false)
                {
                    runTime += 0.05f;
                    yield return new WaitForSeconds(0.01f);
                }
                success = true;
                Debug.Log("ping : " + pinger.isDone + " / " + pinger.time);
            }            
#endif

        }        
        yield return null;
    }

}
