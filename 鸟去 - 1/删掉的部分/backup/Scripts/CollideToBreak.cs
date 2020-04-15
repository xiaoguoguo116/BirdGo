using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideToBreak : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private bool collided = false;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "GreenStone" && !collided)
        {
            collided = true;
            Transform[] circleStones = this.transform.parent.GetComponentsInChildren<Transform>();
            foreach (Transform i in circleStones)
                i.gameObject.AddComponent<Rigidbody2D>();
        }
    }
}
