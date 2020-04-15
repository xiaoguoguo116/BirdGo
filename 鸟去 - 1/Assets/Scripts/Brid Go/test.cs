using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void apple() {
        //Debug.Log("app");
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
