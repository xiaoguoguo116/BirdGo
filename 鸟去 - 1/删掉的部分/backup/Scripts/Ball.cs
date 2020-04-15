using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    //是否单击到球
    bool m_isHit = false;
    //单击球时的位置
    Vector3 m_startPos;
    //Sprite渲染器
    SpriteRenderer m_spriteRenderer;
    public float m_power = 10.0f;
    public Vector3 endPos;
    // Use this for initialization
   
    void Start () {
        m_spriteRenderer = this.GetComponent<SpriteRenderer>();
        

    }
    bool IsHit()
    {
        m_isHit = false;
        //获取鼠标位置
        Vector3 ms = Input.mousePosition;
        //将鼠标位置转化为世界坐标
        ms = Camera.main.ScreenToWorldPoint(ms);
        //获得球的位置
        Vector3 pos = this.transform.position;
        //获得球Sprite的宽和高(注意宽和高不是图片像素值的宽和高)
        
        float w = m_spriteRenderer.bounds.extents.x;
        float h = m_spriteRenderer.bounds.extents.y;
        //判断鼠标的位置是否在Sprite的矩形范围内
         if (ms.x > pos.x - w && ms.x < pos.x + w && ms.y > pos.y - h && ms.y < pos.y + h)
            //if(ms.x > leftBorder&&ms.x<rightBorder&&ms.y>downBorder&&ms.y<topBorder)
            {
            m_isHit = true;
            return true;
        }
        return m_isHit;
    }
    // Update is called once per frame
    void Update () {
        //如果单击鼠标左键并且碰到球
        if (Input.GetMouseButtonDown(0) && IsHit())
        {//记录位置
            m_startPos = Input.mousePosition;
        }
        //当释放鼠标
        if (Input.GetMouseButtonUp(0) && m_isHit)
        {
            endPos = Input.mousePosition;
            Vector3 v = -(m_startPos - endPos) * 1.0f;
            //将body type设为Dynamic
           // this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            //向球加一个力
            this.GetComponent<Rigidbody2D>().AddForce(v);
           // RotateTo(endPos, 5);

        }
    }
    //public void RotateTo(Vector3 dst, float t)
    //{

    //    dst.z = Mathf.Abs(dst.y+dst.x);


       
    //    iTween.RotateTo(gameObject, new Vector3(transform.position.x, transform.position.y, dst.z), t);
        
    //}
}
