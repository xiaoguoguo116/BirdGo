using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WallColideTurtle : MonoBehaviour {

	// Use this for initialization
    private bool Onece = true;
    private bool ifStart = false;
    //private StoneClock Clock;
	void Start () {
        //Clock = GameObject.Find("StoneClock").gameObject.GetComponent<StoneClock>();

    }
	
	// Update is called once per frame
	void Update () {
        if(!ifStart)
        {
            if (Camera.main.transform.parent == Global.Player.transform)
            {
                ifStart = true;
                StartMove();
            }
        }
	}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.tag == "Player" && Onece)
    //    {
    //        Onece = false;
    //        Invoke("StartMove", 2f);
    //        //gameObject.GetComponentInParent<MYWall>().MoveCamera(-44f, 15f);
    //        //UIManager uiManager = GameObject.Find("UI Root/UI").GetComponent<UIManager>();
    //        //uiManager.ShowVictory(4f);
    //    }
    //}

    void StartMove()
    {
        gameObject.GetComponentInParent<MYWall>().MoveCamera(-44f, 15f);
    }

}
