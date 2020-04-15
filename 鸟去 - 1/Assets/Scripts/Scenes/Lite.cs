#define _TimeLimit

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lite : MonoBehaviour
{

    [SerializeField]
    [Tooltip("过期时间")]
    int year = 2019, month = 9, day = 1;
    // Use this for initialization
    void Start()
    {
#if (_TimeLimit)
        GetComponent<Text>().enabled = true;
        if (System.DateTime.Now >= new System.DateTime(year, month, day, 0, 0, 0, 0))
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

#else
        GameObject.Find("Lite").gameObject.SetActive(false);
#endif   
    }

    // Update is called once per frame
    void Update()
    {

    }
}
