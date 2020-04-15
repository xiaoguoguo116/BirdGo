using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockOpen : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 撞动：撞到石头则从静止变为可移动，延时后恢复静止
        if (collision.gameObject.tag == "Rock" || collision.gameObject.tag == "Rock2")
        {
            if(GetComponent<Rigidbody2D>() == null)
                gameObject.AddComponent<Rigidbody2D>();
            this.gameObject.GetComponent<Rigidbody2D>().mass = 0.5f;
            Invoke("Freeze", 3f);
        }
    }

    void Freeze()
    {
        this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
