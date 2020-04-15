using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MYBackInTime : MonoBehaviour {

    const float MaxTimeInBack = 3; // 回溯最长时间（秒）
    const int Length = (int)(MaxTimeInBack / 0.1);
    const int MaxObject = 200;   // 最多需要回溯的物体个数
//#define Length (int)(TimeInBack / 0.1)
    [SerializeField]
    [Tooltip("可移动物体")]
    public List<GameObject> backInTime;
    [SerializeField]
    [Tooltip("绳子（可消失物体）")]
    GameObject[] Rattans;

    Vector3[,] pos = new Vector3[MaxObject, Length];        // [i][j]：第i个物体在第j个时间片的位置，循环队列
    Quaternion[,] rot = new Quaternion[MaxObject, Length];
    Vector2[,] velocity = new Vector2[MaxObject, Length];   // [i][j]：第i个物体在第j个时间片的速度，循环队列
    float[,] angularVelocity = new float[MaxObject, Length];
    RigidbodyType2D[,] bodyType = new RigidbodyType2D[MaxObject, Length];       // [i][j]：物体的刚体类型（可能会用脚本变换）
    int tail = 0;   // 指向下一个要进队的位置
    int total = 0;  // 已经经过了多少个时间片
    ChangeColorToBlackAndWhite BlackAndWhite;
    // Use this for initialization
    void Start () {
        BlackAndWhite = GameObject.Find("B&W").GetComponent<ChangeColorToBlackAndWhite>();
        
        // 记录绳子的每个片段消失的时间，倒流后比较以决定是否恢复
        if (Rattans != null)    // 提取绳子的片段
        {
            foreach (GameObject rattan in Rattans)
            {
                Transform[] ps = rattan.GetComponentsInChildren<Transform>();  // 含孙子对象
                for (int i = 1; i < ps.Length; i++)
                    backInTime.Add(ps[i].gameObject);
            }
        }

        if (backInTime != null)
        {
            foreach (GameObject ob in backInTime)
            {
                if (ob.GetComponent<ITraceable>() != null)
                    ob.GetComponent<ITraceable>().Init(Length);
            }
            StartCoroutine("RecordHistory");
        }

    }

    public void addToBackInTime(GameObject actor)
    {
        backInTime.Add(actor);
    }

    public void removeFromBackInTime(GameObject actor)
    {
        backInTime.Remove(actor);
    }

    IEnumerator RecordHistory()
    {
        GameObject circleProcess = GameObject.Find("UI Root/UI/StoneClock/CircleProcessBar");
        while (true)
        {
            for (int i = 0; i < backInTime.Count; i++) // 记录历史物理参数
            {
                pos[i, tail] = backInTime[i].transform.position;
                rot[i, tail] = backInTime[i].transform.rotation;
                if (backInTime[i].GetComponent<Rigidbody2D>())  // 若带刚体
                {
                    bodyType[i, tail] = backInTime[i].GetComponent<Rigidbody2D>().bodyType;
                    velocity[i, tail] = backInTime[i].GetComponent<Rigidbody2D>().velocity;
                    angularVelocity[i, tail] = backInTime[i].GetComponent<Rigidbody2D>().angularVelocity;
                }
                if(backInTime[i].GetComponent<ITraceable>() != null)     // 若带附加状态
                {
                    backInTime[i].GetComponent<ITraceable>().Save(tail);
                }
            }
            tail = (tail + 1) % Length;
            total++;

            if (1f - circleProcess.GetComponent<CircleProcess>().process > 0.01f)   // 转动石钟
            {
                circleProcess.GetComponent<CircleProcess>().process += 1f / Length;
            }
            else
                circleProcess.SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }
    }

    bool bBigBackInTime = false;    // 防止反复关底大回溯
    public void OnBackInTime()
    {
        // 关底大回溯
        if (SceneManager.GetActiveScene().name == "Game3" &&
            Global.SceneEvent.GetCurrentMyIndex() == Global.SceneEvent.GetMysteryCount() - 1 && backInTime == null)  // 关底大回溯
        {
            if (bBigBackInTime == false)
            {
                // 黑白特效
                BlackAndWhite.Change();
                bBigBackInTime = true;
                BigBackInTime();
            }
            return;
        }

        // 黑白特效
        BlackAndWhite.Change();

        StopCoroutine("RecordHistory");
        foreach (GameObject ob in backInTime)   // 关闭物体的受力，速度和角速度清零
        {
            if (ob.GetComponent<Rigidbody2D>())
            {
                ob.GetComponent<Rigidbody2D>().isKinematic = true;
                //if(ob.GetComponent<Collider2D>())
                //    ob.GetComponent<Collider2D>().enabled = false;
                ob.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                ob.GetComponent<Rigidbody2D>().angularVelocity = 0;
            }
            // 半透明
            //Color c = ob.GetComponent<SpriteRenderer>().color;
            //c.a = 0.5f;
            //ob.GetComponent<SpriteRenderer>().color = c;
        }
        StartCoroutine("BackToHistory");
    }
    IEnumerator BackToHistory()
    {
        // 确定回溯的总长
        if (total > Length)
            total = Length;
        // 石钟倒流
        GameObject.Find("StoneClock").gameObject.GetComponent<StoneClock>().OnBackInTime(total, Length);


        while (total-- > 0)
        {
            tail = (tail - 1 + Length) % Length;    // 上一个位置
            for (int i = 0; i < backInTime.Count; i++) // 恢复历史时刻的位置和角度
            {
                backInTime[i].transform.position = pos[i, tail] ;
                backInTime[i].transform.rotation = rot[i, tail];
            }
            yield return new /*WaitForSeconds*/WaitForSecondsRealtime(0.02f);
        }
        for (int i = 0; i < backInTime.Count; i++) // 回溯的尽头，恢复物理属性，以及当时的速度和角速度；断裂绳子恢复
        {
            if (backInTime[i].GetComponent<Rigidbody2D>())
            {
                backInTime[i].GetComponent<Rigidbody2D>().isKinematic = false;
                //if(backInTime[i].GetComponent<Collider2D>())
                //    backInTime[i].GetComponent<Collider2D>().enabled = true;
                backInTime[i].GetComponent<Rigidbody2D>().bodyType = bodyType[i, tail];
                backInTime[i].GetComponent<Rigidbody2D>().velocity = velocity[i, tail];
                backInTime[i].GetComponent<Rigidbody2D>().angularVelocity = angularVelocity[i, tail];

                //if(backInTime[i].GetComponent<RattanDelete>())  // 是绳子
                //{
                //    if (backInTime[i].GetComponent<RattanDelete>().Time > Time.time - TimeInBack)   // 断裂在回溯起点后
                //    {
                //        backInTime[i].GetComponent<HingeJoint2D>().enabled = true;
                //    }
                //}

            }
            if (backInTime[i].GetComponent<ITraceable>() != null)
            {
                backInTime[i].GetComponent<ITraceable>().Load(tail);
            }
        }

        // 关闭黑白特效
        BlackAndWhite.Change();

        StartCoroutine("RecordHistory");  
    }

    // Update is called once per frame
    void Update () {
		
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
        GameObject.Find("StoneClock").gameObject.GetComponent<StoneClock>().reverse = true;
        yield return new /*WaitForSeconds*/WaitForSecondsRealtime(10f);
        GameObject.Find("StoneClock").gameObject.GetComponent<StoneClock>().reverse = false;
        BlackAndWhite.Change();
    }

}
