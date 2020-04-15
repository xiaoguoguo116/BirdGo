using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristalBrake : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.GetComponent<Rigidbody2D>()) // 已经分裂了
            return;
        //接触到某些地形后，添加刚体组件
        if (collision.gameObject.tag == "Rock" || collision.gameObject.tag == "Rock2")
        {
            Transform[] chips = transform.parent.GetComponentsInChildren<Transform>();
            for (int i = 1; i < chips.Length; i++)
            {
                chips[i].gameObject.AddComponent<Rigidbody2D>();
                //ob.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 50);
            }
        }
        
        


    }

}
