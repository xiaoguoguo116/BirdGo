using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeButton : MonoBehaviour {

    // Use this for initialization
    public GameObject changeActor;
    private bool Once;
	void Start () {
        Once = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && Once)
        {
            changeTide(changeActor);
            Once = false;
        }
    }

    public void changeTide(GameObject changeActor)
    {
        changeActor.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        changeActor.transform.Translate(new Vector3(0f, -19.8f, 0f));
    }
}
