using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetwork : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		if(!isLocalPlayer)  // 不能放在Awake里，不然这里会判断不准确
        {
            tag = "OtherPlayer";
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
