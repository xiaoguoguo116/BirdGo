using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangPSNN : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //GetComponent<PolygonCollider2D>().isTrigger = true;
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.name == "CanRotate" || collision.name == "CanMove"|| collision.name == name)
            return;
        if (!GameObject.Find("CanRotate").GetComponent<IsTouchRotate>().PSOBJ)
            return;
        if (GameObject.Find("CanRotate").GetComponent<IsTouchRotate>().PSOBJ.GetComponent<PSOperation>().CanPS)
        {
            GameObject.Find("PSTool").transform.position = new Vector3(GameObject.Find("PSTool").transform.position.x, GameObject.Find("PSTool").transform.position.y, 5);
            GameObject.Find("CanRotate").GetComponent<IsTouchRotate>().PSOBJ.GetComponent<PSOperation>().CanPS = false;
         
            GameObject.Find("PSTool").transform.position = new Vector3(-64, 64, 0);
        }
    }
    // Update is called once per frame
    void Update () {

    }
}
