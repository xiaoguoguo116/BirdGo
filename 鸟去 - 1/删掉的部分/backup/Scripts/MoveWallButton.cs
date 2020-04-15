using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWallButton : MonoBehaviour {

    // Use this for initialization
    public GameObject moveWwall;
    private bool collided = false;
    private Shader MyShader;
	void Start () {
        MyShader = GetComponent<SpriteRenderer>().material.shader;
}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setCollided(bool collided)
    {
        this.collided = collided;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collided = !collided;
        if (collision.gameObject.tag == "Player")
        {
            moveWwall.GetComponent<Rigidbody2D>().gravityScale = -moveWwall.GetComponent<Rigidbody2D>().gravityScale;
            if (collided)
            {
                GetComponent<SpriteRenderer>().material.shader = Shader.Find("Sprites/Default_B&W");
            }
            else
            {
                GetComponent<SpriteRenderer>().material.shader = MyShader;
            }
        }
        
    }
}
