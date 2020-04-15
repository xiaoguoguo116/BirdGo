using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flow : MonoBehaviour {
    //浮力
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up*140);
        if (this.gameObject.transform.position.y > 2.1)
        { this.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 110); }
    }
}
