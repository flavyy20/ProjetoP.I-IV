using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanScene : MonoBehaviour
{
    void Start()
    {
        foreach (var item in FindObjectsByType<Transform>(FindObjectsSortMode.None))
        {
            if (item.name.Contains("Vehicle") ) Destroy(item.gameObject);
        }
    }
}