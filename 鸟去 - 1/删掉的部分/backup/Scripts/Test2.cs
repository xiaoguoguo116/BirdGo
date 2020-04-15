using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }
    private bool collidePlayer = false;
    private bool collideStone = false;
    [SerializeField]
    private float flatAngle;//平坦的角度
    [SerializeField]
    private float speed;//旋转的速度
                        // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.localEulerAngles);
        if (collideStone == true)//如果碰到石头，就不旋转
        {
            GetComponent<Rigidbody2D>().freezeRotation = false;
        }
        else if (transform.localEulerAngles.z < flatAngle)//如果没碰到石头又没平放，就往回转
        {
            GetComponent<Rigidbody2D>().freezeRotation = false;
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * speed));
        }
        else if (collidePlayer == true)//平放如果乌龟还在碰就锁定
        {
            GetComponent<Rigidbody2D>().freezeRotation = true;
        }
        else//乌龟没碰就往回走
        {
            GetComponent<Rigidbody2D>().freezeRotation = false;
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * speed));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag != "Player")
        {
            Debug.Log("石头碰着");
            this.collideStone = true;
        }
        else
        {
            Debug.Log("乌龟碰着");
            this.collidePlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Debug.Log("石头没碰了");
            this.collideStone = false;
        }
        else
        {
            Debug.Log("乌龟没碰了");
            this.collidePlayer = false;
        }
    }
}
