using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghosting : MonoBehaviour
{
    [HideInInspector]
    public int stayUp;      //是否在其他物体内部停留，用来判断是否是在碰撞体内部停留时松开鼠标
    GameObject gameobject;
    int num;    //计量数，防止不停生成clone
    bool isIn;
    string HisTag, MyTag;
    public void OnMouseUp()   //用于虚影移动到其他物体内部又松了鼠标的情况
    {
        backPosition();
    }
    private void EnterOther(Collider2D collision) //进入碰撞体调用

    {
        Debug.Log("enter");
        if (collision.tag != "clone")
        {
           
           // isIn = true;   //确定进入碰撞体
            if (num == 0)  //该物体还没生成clone
            {
                HisTag = collision.tag;
                collision.tag = "barrier";
                //stayUp = 1;
                num++;
                gameobject = Instantiate(this.gameObject, this.transform.position, this.transform.rotation); //生成clone
                MyTag = this.tag;
                this.tag = "clone";
                gameobject.tag = "clone";
                Destroy(gameobject.GetComponent<Rigidbody2D>());
                gameobject.GetComponent<Ghosting>().num = 1;
                this.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                gameobject.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
    }
    private void StayOther(Collider2D collision) //待在其他碰撞体中调用
    {
        stayUp = 1;  //
        isIn = true;
        Debug.Log("stayUp");
    }
    private void ExitOther(Collider2D collision)  //离开碰撞体时调用
    {
        Debug.Log("exit");
        if (collision.tag == "barrier")
        {
            // isIn = false;
            //if (!isIn )
            {
                collision.tag = HisTag;
                this.tag = MyTag;
                this.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                Destroy(gameobject);  //销毁clone
               
            }

            num--;
            stayUp = 0;
        }

    }
    public void backPosition()
    {
        if (stayUp == 1)//此时是将物体拖拽到其他物体内部后松开鼠标，应该回到原位置
        {
            this.transform.position = gameobject.transform.position;
            this.transform.rotation = gameobject.transform.rotation;   //回到了原位置原旋转
        }
    }
}
