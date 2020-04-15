/*
 * 对于NetworkService.Spawn()无法修改parent问题的弥补
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;

public class ParentToMY : MonoBehaviour {

    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start () {
		transform.SetParent(Global.MYNetwork);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
