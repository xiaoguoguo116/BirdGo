using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanGhosting : MonoBehaviour {
    [HideInInspector]
    public bool canPs;
    private bool canRatate;
    [HideInInspector]
    public bool IsTrigger;
   
    void Update()
    {
        canPs = this.GetComponent<PSOperation>().CanPS;
        if (canPs)
        {
            this.GetComponent<PolygonCollider2D>().isTrigger = true;
            //this.
        }
        else
            this.GetComponent<PolygonCollider2D>().isTrigger = false;
    }
}
