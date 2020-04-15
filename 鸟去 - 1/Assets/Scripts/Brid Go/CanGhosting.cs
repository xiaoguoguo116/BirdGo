using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanGhosting : MonoBehaviour {
    [HideInInspector]
    public bool canMove;
    private bool canRatate;
    [HideInInspector]
    public bool IsTrigger;
   
    void Update()
    {
        canMove = this.GetComponent<PSOperation>().Move;
        canRatate = this.GetComponent<PSOperation>().Rotate;
        if (canMove||canRatate)
            this.GetComponent<PolygonCollider2D>().isTrigger = true;
        else
            this.GetComponent<PolygonCollider2D>().isTrigger = false;
    }
}
