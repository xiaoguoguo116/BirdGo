﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 给谜题中某些对象添加可回溯功能
/// 外部方法：
/// void addToBackInTime(GameObject actor)：在屏幕外侧一段距离处生成物体时即调用
/// void removeFromBackInTime(GameObject actor)：在屏幕外侧一段距离处销毁物体时调用
/// </summary>
public class StopGame : MonoBehaviour
{
    /// <summary>
    /// 回溯的真实时间是（最长）MaxTimeInBack 秒
    /// </summary>
   
    
    const int Length = 1;
    //const int MaxObject = 200;   // 最多需要回溯的物体个数


    class TraceableObject   // 记录可回溯物体的历史参数
    {
        public GameObject ob;
        public Vector3[] pos = new Vector3[Length];        // [j]：物体在第j个时间片的位置，循环队列（用Vector3.x == 9999 表示还未生成时）
        public Quaternion[] rot = new Quaternion[Length];
        public Vector2[] velocity = new Vector2[Length];   // [j]：物体在第j个时间片的速度，循环队列
        public float[] angularVelocity = new float[Length];
        public RigidbodyType2D[] bodyType = new RigidbodyType2D[Length];       // [j]：物体的刚体类型（可能会用脚本变换）
    }

    //#define Length (int)(TimeInBack / 0.1)
    [SerializeField]
    [Tooltip("可移动物体")]
    List<GameObject> StopInTime;
    //[SerializeField]
    //GameObject Dynamic;
    LinkedList<TraceableObject> BackInTime;// = new LinkedList<TraceableObject>();

    int tail = 0;   // 指向下一个要进队的位置
    public int total = 0;  // 已经经过了多少个时间片
    
    // Use this for initialization
    void Start()
    {
        

        BackInTime = new LinkedList<TraceableObject>();

        if (StopInTime != null)
            foreach (var ob in StopInTime)
                addToBackInTime(ob);

        //StartCoroutine("RecordHistory");

    }


    /// <summary>
    /// 动态加入可回溯对象（bDeleteUnborn = false 可防止Player被删，以及防止玩家“SL大法”
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="bDeleteUnborn">若回溯结束时add还未执行，则删除本物体</param>
    public void addToBackInTime(GameObject actor, bool bDeleteUnborn = true)
    {
        //backInTime.Add(actor);
        TraceableObject tb = new TraceableObject();
        tb.ob = actor;

        if (bDeleteUnborn)
        {
            for (int i = 0; i < tb.pos.Length; i++)
            {
                tb.pos[i] = new Vector3(9999, 0, 0);  // 用Vector3.x == 9999 表示还未生成时的值
            }
        }
        else
        {
            for (int i = 0; i < tb.pos.Length; i++)
            {

                // 以当下时刻状态填充进所有时间的状态
                tb.pos[i] = actor.transform.position;
                tb.rot[i] = actor.transform.rotation;
                if (tb.ob.GetComponent<Rigidbody2D>())  // 若带刚体
                {
                    tb.bodyType[i] = actor.GetComponent<Rigidbody2D>().bodyType;
                    tb.velocity[i] = actor.GetComponent<Rigidbody2D>().velocity;
                    tb.angularVelocity[i] = actor.GetComponent<Rigidbody2D>().angularVelocity;
                }
                if (actor.GetComponent<ITraceable>() != null)     // 若带附加状态
                {
                    actor.GetComponent<ITraceable>().Save(i);
                }
            }
        }

        BackInTime.AddLast(tb);

        if (actor.GetComponent<ITraceable>() != null)
            actor.GetComponent<ITraceable>().Init(Length);
    }


    IEnumerator RecordHistory()//记录暂停时所有可动物体的参数
    {

        while (true)
        {
            foreach (var tb in BackInTime) // 记录历史物理参数
            {
                tb.pos[tail] = tb.ob.transform.position;
                tb.rot[tail] = tb.ob.transform.rotation;
                if (tb.ob.GetComponent<Rigidbody2D>())  // 若带刚体
                {

                    tb.bodyType[tail] = tb.ob.GetComponent<Rigidbody2D>().bodyType;
                    tb.ob.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;//设置为悬空刚体
                    tb.velocity[tail] = tb.ob.GetComponent<Rigidbody2D>().velocity;
                    tb.angularVelocity[tail] = tb.ob.GetComponent<Rigidbody2D>().angularVelocity;
                }
                if (tb.ob.GetComponent<ITraceable>() != null)     // 若带附加状态
                {
                    tb.ob.GetComponent<ITraceable>().Save(tail);
                }
            }
            tail = (tail + 1) % Length;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void IsStopGame()
    {
        
        StartCoroutine("RecordHistory");
        StopCoroutine("RecordHistory");
        foreach (var tb in BackInTime)   // 关闭物体的受力，速度和角速度清零
            {
                if (tb.ob.GetComponent<Rigidbody2D>())
                {
                    tb.ob.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    tb.ob.GetComponent<Rigidbody2D>().angularVelocity = 0;
                }
                
            }
            
    }
    public IEnumerator BackToGame()
    {

        //total = 100;
        // 确定回溯的总长
        if (total > Length)
            total = Length;
        // 石钟倒流


        while (total-- > 0)
        {
            Debug.Log("55");
            tail = (tail - 1 + Length) % Length;    // 上一个位置
            List<TraceableObject> willRemove = new List<TraceableObject>(); // 【将来改为直接在链表上删除，提高效率】
            foreach (var tb in BackInTime) // 恢复历史时刻的位置和角度
            {
                if (tb.pos[tail].x == 9999) // 用Vector3.x == 9999 表示此时还未生成，需删除该对象
                {
                    //Destroy(tb.ob);
                    //backInTime.Remove(tb);
                    //tb.ob.SetActive(false);
                    willRemove.Add(tb);
                }
                else
                {
                    tb.ob.transform.position = tb.pos[tail];
                    tb.ob.transform.rotation = tb.rot[tail];
                }
            }
            foreach (var tb in willRemove)
            {
                Destroy(tb.ob);
                BackInTime.Remove(tb);
            }

            yield return new /*WaitForSeconds*/WaitForSecondsRealtime(0.02f);
        }
        foreach (var tb in BackInTime) // 回溯的尽头，恢复物理属性，以及当时的速度和角速度；断裂绳子恢复
        {
            if (tb.ob.GetComponent<Rigidbody2D>())
            {
                //tb.ob.GetComponent<Rigidbody2D>().isKinematic = false;
                //if(backInTime[i].GetComponent<Collider2D>())
                //    backInTime[i].GetComponent<Collider2D>().enabled = true;
                tb.ob.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                tb.ob.GetComponent<Rigidbody2D>().velocity = tb.velocity[tail];
                tb.ob.GetComponent<Rigidbody2D>().angularVelocity = tb.angularVelocity[tail];

  

            }
            if (tb.ob.GetComponent<ITraceable>() != null)
            {
                tb.ob.GetComponent<ITraceable>().Load(tail);
            }
        }

        //Global.Player.GetComponent<PlayerEventsNetwork>().RpcBackTime();
        //Debug.Log("back");
        //GameObject.Find("UI Root").GetComponent<UIManager>().ClickToPause();
        
      
    }

    // Update is called once per frame
    void Update()
    {

    }

}
