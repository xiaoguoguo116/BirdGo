using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoRock : MonoBehaviour {

    int count;
    [SerializeField]
    GameObject obstacle;
    [SerializeField]
    float mass = 0.5f;

    private void Update()
    {
        try
        {
            if (count >= 2 && obstacle.GetComponent<Rigidbody2D>() == null)
            {
                obstacle.AddComponent<Rigidbody2D>().mass = mass;
                
            }
        }
        catch (System.Exception)
        {
            return;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject colli = collision.gameObject;
        if (colli.tag.Equals("Rock"))
        {
            count += 1;
            colli.tag = "Untagged"; // 无法再用划屏操作
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject colli = collision.gameObject;
        if (colli.tag.Equals("Rock"))
        {
            count -= 1;
        }
    }
}
