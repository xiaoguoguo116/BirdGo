using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    [SerializeField]
    float ex_force = 1f;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() &&
            collision.gameObject.layer != LayerMask.NameToLayer("No WhaleWater"))
        {
            // force为扭矩向量
            // 力的大小与和鲸鱼y方向的距离成反比【要改成距离而不是y方向】
            float fy = Mathf.Min(100, 1500f / Mathf.Max(0.1f, collision.gameObject.transform.position.y - transform.position.y));
            fy *= ex_force;
            Vector3 force = fy * transform.TransformDirection(Vector3.up);
            Rigidbody2D A = collision.gameObject.GetComponent<Rigidbody2D>();
            A.AddForceAtPosition(force, transform.position, ForceMode2D.Force);
        }
        //x=-18.4
    }
}
