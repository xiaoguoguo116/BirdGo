using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponNetwork : TouchableNetwork, ITraceable
{
    [SerializeField]
    int ShotCnt = 1;            // 当前锯鳐还能冲刺的次数（锯鳐的状态参数）
    [SerializeField]
    float maxMoveDis = 10f;
    [SerializeField]
    float MoveSpeed = 1f;
    float MoveSpeedBackup;
    [Tooltip("是否反向")]
    [SerializeField]
    bool Reverse = false;
    [SerializeField]
    bool caught = false;

    bool inSceneView;   // 【待完善】
    Animator m_ani;
    bool bRushing = false;  // 是否正在冲刺
    int EventCnt = 0;       // 点击事件缓存，当前需要处理的点击事件个数（由于iTween动画结束前无法再次响应）
    ////////////////
    int[] record;
    public void Init(int count)
    {
        record = new int[count];
    }
    public void Save(int tail)
    {
        record[tail] = ShotCnt;
    }

    public void Load(int tail)
    {
        ShotCnt = record[tail];
        if (ShotCnt > 0)
        {
            m_ani.SetBool("stinger", true);
            GetComponent<SpriteRenderer>().color = Color.white;
            MoveSpeed = MoveSpeedBackup;
        }
        EventCnt = 0;   // 清缓存
    }
    ////////////////

    // Use this for initialization
    void Start()
    {
        base.Start();

        if(Reverse)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            MoveSpeed = -MoveSpeed;
            maxMoveDis = -maxMoveDis;
        }

        transform.Find("Mouse").gameObject.SetActive(false);
        m_ani = this.GetComponent<Animator>();
        m_ani.SetBool("stinger", true);
        MoveSpeedBackup = MoveSpeed;
        //if (GameObject.Find("GameManager").GetComponent<SceneEventManager>().GetCurrentSceneIndex(SceneManager.GetActiveScene().name) == 3)
        //{
        //    GetComponent<SpriteRenderer>().color = new Color(0.905f, 0.764f, 0.764f, 1f);
        // }


    }


    // Update is called once per frame
    void Update()
    {
        // 在游戏边界内才开启“割绳子”检测
        if (bRushing == true && inSceneView == true)
            transform.Find("Mouse").gameObject.SetActive(true);

        transform.Translate(new Vector3(Time.deltaTime * MoveSpeed, 0, 0));

        if (!bRushing && ShotCnt > 0 && EventCnt > 0)
        {
            bRushing = true;
            EventCnt--;
            float moveTime;
            if (caught)//被捆绑的锯鳐，切割变慢
            {
                moveTime = 1f;
            }
            else
            {
                moveTime = 1f;// 0.5f;
            }
            iTween.MoveBy(gameObject, iTween.Hash
                                (
                                    "x", maxMoveDis,
                                    "time", moveTime,
                                    "oncomplete", "RushDone",
                                    "easeType", iTween.EaseType.easeOutCubic
                                )
                            );      // 完成之前无法再次执行
                                    //transform.DOMoveX(maxMoveDis, moveTime).SetRelative().SetEase(Ease.Linear/*.OutExpo*/);
                                    //transform.DOLocalMoveX(transform.position.x + maxMoveDis, moveTime)./*SetRelative().*/SetEase(Ease.Linear/*.OutExpo*/);
            if (--ShotCnt <= 0)
            {
                m_ani.SetBool("stinger", false);
                GetComponent<SpriteRenderer>().color = new Color32(255, 212, 146, 255);
                EventCnt = 0;   // 清缓存
                MoveSpeed = 0;
            }
        }
    }
    //private void OnBecameVisible()
    //{
    //    inSceneView = true;
    //}
    //void OnBecameInvisible()
    //{
    //    inSceneView = false;
    //}
    void RushDone()
    {
        bRushing = false;

    }
    /// <summary>
    /// 锯鳐的冲刺（可以缓冲）
    /// </summary>
    public override void TouchEvent()
    {
        EventCnt++;
    }

    protected override void Effect()
    {
        
    }

    //public void TouchEventNetwork()
    //{
    //    int id = GameManager.Touchables.IndexOf(gameObject);
    //    if (id >= 0)
    //        Global.Player.GetComponent<PlayerEventsNetwork>().CmdTouchEvent(id);
    //}


}
