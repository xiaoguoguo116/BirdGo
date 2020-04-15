using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 绊住：撞到tag为“Zone”的物体立即静止
        if (collision.gameObject.tag == "Zone")
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
