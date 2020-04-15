using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameNetworkManager : NetworkBehaviour
{

    [SerializeField]
    GameObject StoneClockPrefab;
    // Use this for initialization
    void Start()
    {
        GameObject stoneClock;
        if (isServer)
        {
            print("isServer");
            stoneClock = Instantiate(StoneClockPrefab) as GameObject;
            stoneClock.name = "StoneClock";
            stoneClock.transform.parent = GameObject.Find("UI").transform;
            NetworkServer.Spawn(stoneClock);
        }
        else
        {
            var clock = GameObject.Find("StoneClock(Clone)");
            if (clock)
            {
                clock.name = "StoneClock";
                clock.transform.parent = GameObject.Find("UI").transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}