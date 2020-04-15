using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;

/// <summary>
/// 给联机中某些对象添加可回溯功能
/// 不同于一般的联机架构（先在服务器上执行，再同步到客户端），
/// 回溯功能（包括回溯的倒放、物体状态的恢复）需要在所有客户端上执行
/// 【这是由于回溯时，timeScale = 0，服务器的数据无法及时传给客户端】

/// 外部方法：
/// void addToBackInTime(GameObject actor)：在屏幕外侧一段距离处生成物体时即调用
/// void removeFromBackInTime(GameObject actor)：在屏幕外侧一段距离处销毁物体时调用
/// </summary>
public class MYBackInTimeNetwork : MonoBehaviour// NetworkBehaviour
{
    /// <summary>
    /// 回溯的真实时间是（最长）MaxTimeInBack 秒
    /// </summary>
    const float MaxTimeInBack = 3; // 回溯最长时间（秒）
    const int Length = (int)(MaxTimeInBack / 0.1);
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
    List<GameObject> Original;
    //[SerializeField]
    //GameObject Dynamic;
    LinkedList<TraceableObject> backInTime;// = new LinkedList<TraceableObject>();
    [SerializeField]
    [Tooltip("绳子（可消失物体）")]
    GameObject[] Rattans;

    int tail = 0;   // 指向下一个要进队的位置
    int total = 0;  // 已经经过了多少个时间片
    ChangeColorToBlackAndWhite BlackAndWhite;
    // Use this for initialization

    // 除了服务器，客户端也要做全部的回溯操作，以克服客户端回溯不能从服务器端同步的问题
    void Start()
    {
        BlackAndWhite = GameObject.Find("B&W").GetComponent<ChangeColorToBlackAndWhite>();
        backInTime = new LinkedList<TraceableObject>();

        foreach (var ob in Original)
            addToBackInTime(ob);
        //*
        // 记录绳子的每个片段消失的时间，倒流后比较以决定是否恢复
        if (Rattans != null)    // 提取绳子的片段
        {
            foreach (GameObject rattan in Rattans)
            {
                Transform[] ps = rattan.GetComponentsInChildren<Transform>();  // 含孙子对象
                for (int i = 1; i < ps.Length; i++)
                    addToBackInTime(ps[i].gameObject);
            }
        }
        //*/

        //if (backInTime != null)
        //{
        //    foreach (TraceableObject tb in backInTime)
        //    {
        //        if (tb.ob.GetComponent<ITraceable>() != null)
        //            tb.ob.GetComponent<ITraceable>().Init(Length);
        //    }
            
        //}

        StartCoroutine("RecordHistory");

    }


    public void addToBackInTime(GameObject actor)
    {
        //backInTime.Add(actor);
        TraceableObject tb = new TraceableObject();
        tb.ob = actor;
        for (int i = 0; i < tb.pos.Length; i++)  // 用Vector3.x == 9999 表示还未生成时的值
            tb.pos[i] = new Vector3(9999, 0, 0);
        backInTime.AddLast(tb);

        if (actor.GetComponent<ITraceable>() != null)
            actor.GetComponent<ITraceable>().Init(Length);
    }

    public void removeFromBackInTime(GameObject actor)
    {
        foreach (var tb in backInTime)
            if (tb.ob == actor)
            {
                backInTime.Remove(tb);
                break;
            }
    }

    IEnumerator RecordHistory()
    {
        Transform stoneClock = Global.UI.transform.Find("StoneClock");

        while (true)
        {
            foreach (var tb in backInTime) // 记录历史物理参数
            {
                tb.pos[tail] = tb.ob.transform.position;
                tb.rot[tail] = tb.ob.transform.rotation;
                if (tb.ob.GetComponent<Rigidbody2D>())  // 若带刚体
                {
                    tb.bodyType[tail] = tb.ob.GetComponent<Rigidbody2D>().bodyType;
                    tb.velocity[tail] = tb.ob.GetComponent<Rigidbody2D>().velocity;
                    tb.angularVelocity[tail] = tb.ob.GetComponent<Rigidbody2D>().angularVelocity;
                }
                if (tb.ob.GetComponent<ITraceable>() != null)     // 若带附加状态
                {
                    tb.ob.GetComponent<ITraceable>().Save(tail);
                }
            }
            tail = (tail + 1) % Length;
            total++;

            if (stoneClock)
            {
                stoneClock.GetComponent<StoneClockNetwork>().Func(Length);
            }


            yield return new WaitForSeconds(0.1f);
        }
    }


    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(25, 25, 100, 30), GameManager.test);
    //}

    bool isBackingInTime = false;   // 防止嵌套运行回溯
    bool bBigBackInTime = false;    // 防止反复关底大回溯
    public void OnBackInTime()
    {
        //        GameManager.test = "OnBackInTime";

        if (!isBackingInTime)
        {
            isBackingInTime = true;
            // 黑白特效
            //Global.Player.GetComponent<PlayerEventsNetwork>().RpcBackTime();
            BlackAndWhite.Change();

            StopCoroutine("RecordHistory");
            foreach (var tb in backInTime)   // 关闭物体的受力，速度和角速度清零
            {
                if (tb.ob.GetComponent<Rigidbody2D>())
                {
                    //                tb.ob.GetComponent<Rigidbody2D>().isKinematic = true;
                    //if(ob.GetComponent<Collider2D>())
                    //    ob.GetComponent<Collider2D>().enabled = false;
                    tb.ob.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    tb.ob.GetComponent<Rigidbody2D>().angularVelocity = 0;
                }
                // 半透明
                //Color c = ob.GetComponent<SpriteRenderer>().color;
                //c.a = 0.5f;
                //ob.GetComponent<SpriteRenderer>().color = c;
            }
            StartCoroutine("BackToHistory");
        }
    }
    IEnumerator BackToHistory()
    {
        //GameManager.test = "BackToHistory";

        //StoneClock
        // GameObject.Find("StoneClock(Clone)").GetComponent<StoneClockNetwork>().year += 3;//玩家倒流后，总时间加3s
        // 确定回溯的总长
        if (total > Length)
            total = Length;
        // 石钟倒流
        //  GameObject.Find("StoneClock(Clone)").gameObject.GetComponent<StoneClockNetwork>().OnBackInTime(total, Length);
        //Global.Player.GetComponent<PlayerEventsNetwork>().CmdTime(total, Length);
        Global.UI.transform.Find("StoneClock").gameObject.GetComponent<StoneClockNetwork>().OnBackInTime(total, Length, MaxTimeInBack);


 //               print("Back Begin " + backInTime.Count);
        while (total-- > 0)
        {
            tail = (tail - 1 + Length) % Length;    // 上一个位置
//            List<TraceableObject> willRemove = new List<TraceableObject>(); // 【将来改为直接在链表上删除，提高效率】


            //print(backInTime.Count + "," + GameManager.ControllableObjects.Count);

            //try
            //{

                foreach (var tb in backInTime) // 恢复历史时刻的位置和角度
                {
                    if (tb.pos[tail].x == 9999) // 用Vector3.x == 9999 表示此时还未生成，需删除该对象
                    {
                        //Destroy(tb.ob);
                        //NetworkServer.Destroy(tb.ob);

                        // 销毁对象，不要放在倒流算法中，而是倒流结束后放到服务器上统一一起销毁
                        // 【联机游戏切记小心删除物体，因为跨机的引用容易出错】
                        // ∴先使能掉，再统一删除
                        tb.ob.SetActive(false);

                        //backInTime.Remove(tb);
//                        willRemove.Add(tb);
                    }
                    else
                    {
                        tb.ob.transform.position = tb.pos[tail];
                        tb.ob.transform.rotation = tb.rot[tail];
                    }
                }
            //}catch(Exception e)
            //{
            //    print(e.GetType().ToString() + " Exception " + backInTime.Count);
            //}


//            foreach (var tb in willRemove)
//                backInTime.Remove(tb);

                //NetworkServer.Destroy(tb.ob);
                //Destroy(tb.ob);


            yield return new /*WaitForSeconds*/WaitForSecondsRealtime(0.02f);
        }
//        print(backInTime.Count);
        foreach (var tb in backInTime) // 回溯的尽头，恢复物理属性，以及当时的速度和角速度；断裂绳子恢复
        {
            if (tb.ob.activeSelf)
            {
                if (tb.ob.GetComponent<Rigidbody2D>())
                {
//                    tb.ob.GetComponent<Rigidbody2D>().isKinematic = false;
                    //if(backInTime[i].GetComponent<Collider2D>())
                    //    backInTime[i].GetComponent<Collider2D>().enabled = true;
                    tb.ob.GetComponent<Rigidbody2D>().bodyType = tb.bodyType[tail];
                    tb.ob.GetComponent<Rigidbody2D>().velocity = tb.velocity[tail];
                    tb.ob.GetComponent<Rigidbody2D>().angularVelocity = tb.angularVelocity[tail];

                    //if(backInTime[i].GetComponent<RattanDelete>())  // 是绳子
                    //{
                    //    if (backInTime[i].GetComponent<RattanDelete>().Time > Time.time - TimeInBack)   // 断裂在回溯起点后
                    //    {
                    //        backInTime[i].GetComponent<HingeJoint2D>().enabled = true;
                    //    }
                    //}

                }
                if (tb.ob.GetComponent<ITraceable>() != null)
                {
                    tb.ob.GetComponent<ITraceable>().Load(tail);
                }
            }
        }

        // 关闭黑白特效
        BlackAndWhite.Change();
        //Global.Player.GetComponent<PlayerEventsNetwork>().RpcBackTime();
        StartCoroutine("RecordHistory");
        isBackingInTime = false;

        //        GameManager.test = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    void BigBackInTime()
    {
        //Global.Player.transform.DOMoveX(Global.Player.transform.position.x * 2, 5);
        Camera.main.transform.parent = Global.Player.transform;
        Global.Player.transform.DOMoveX(/*-60*/-80, 10);
        StartCoroutine("reverseStoneClock");
    }

    IEnumerator reverseStoneClock()
    {
        GameObject.Find("StoneClock").gameObject.GetComponent<StoneClockNetwork>().reverse = true;
        yield return new /*WaitForSeconds*/WaitForSecondsRealtime(10f);
        GameObject.Find("StoneClock").gameObject.GetComponent<StoneClockNetwork>().reverse = false;
        BlackAndWhite.Change();
    }

}
