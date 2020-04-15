using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //接触到某些地形后，添加刚体组件
        if( (collision.gameObject.tag== "Rock")/*|| (collision.gameObject.tag == "Light")*/)
        {
            
            this.gameObject.AddComponent<Rigidbody2D>();
            this.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 50);
            StartCoroutine(Die());
            // this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.2f, -1f), ForceMode2D.Force);
        }
        
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(100f);
        this.gameObject.SetActive(false);
    }
}
