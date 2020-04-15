using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 两块圆石撬动的谜题
/// </summary>
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
