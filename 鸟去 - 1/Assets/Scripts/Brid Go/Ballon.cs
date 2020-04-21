using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballon : MonoBehaviour {
    public float speed = 1;//气球上升速度
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.forward * speed;
    }
    void OnMouseDown()
    {

      
        Debug.Log("fly");
        //gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,speed));
        //gameObject.GetComponent<Rigidbody2D>().velocity = transform.forward * speed;

    }
    // Update is called once per frame
    void Update () {
		
	}
}
