using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineM : MonoBehaviour
{
    private static CoroutineM _instance;

    public static CoroutineM Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("CoroutineManager");
                _instance = obj.AddComponent<CoroutineM>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }
}
