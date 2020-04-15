using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tide : MonoBehaviour {

	// Use this for initialization
    public float spped;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

        }
    }
}
