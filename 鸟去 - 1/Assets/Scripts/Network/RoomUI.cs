using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class RoomUI : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    GameObject[] player = new GameObject[2];
    public void OnStart()
    {

        player[0] = GameObject.FindGameObjectWithTag("Player");
        player[1] = GameObject.FindGameObjectWithTag("OtherPlayer");
        DontDestroyOnLoad(player[0]);   // 本地
        //print("isLocalPlayer:" + player[0].GetComponent<NetworkIdentity>().isLocalPlayer);
        DontDestroyOnLoad(player[1]);   // 远端
        //print("isLocalPlayer:" + player[1].GetComponent<NetworkIdentity>().isLocalPlayer);
        //if (player[0].GetComponent<NetworkIdentity>().isLocalPlayer == false)
        //    player[0].tag = "OtherPlayer";
        //if (player[1].GetComponent<NetworkIdentity>().isLocalPlayer == false)
        //    player[1].tag = "OtherPlayer";
        SceneManager.LoadScene("Game5——联机");
    }

    private void OnDestroy()
    {
//        print("player[0]:" + player[0].GetComponent<NetworkIdentity>().isLocalPlayer);
//        print("player[1]:" + player[1].GetComponent<NetworkIdentity>().isLocalPlayer);
    }



}
