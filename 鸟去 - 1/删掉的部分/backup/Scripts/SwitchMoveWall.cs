using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMoveWall : Switch {

    // Use this for initialization
    GameObject moveWall;

    public override void SwitchUpdate(bool turnOn)
    {
        moveWall = base.Reference[0];
        if (turnOn)
            moveWall.GetComponent<Rigidbody2D>().gravityScale = -1;
        else
            moveWall.GetComponent<Rigidbody2D>().gravityScale = 1;


    }



}
