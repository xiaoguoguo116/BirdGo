using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "rattan")
        {
            this.gameObject.GetComponent<AudioSource>().Play();
            collision.GetComponent<HingeJoint2D>().enabled = false;
        }
    }
}
