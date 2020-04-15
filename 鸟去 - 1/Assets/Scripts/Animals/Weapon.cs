using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Weapon : TouchableBehaviour, ITraceable
{
    [SerializeField]
    int ShotCnt = 1;
    public float maxMoveDis = 10f;

    Animator m_ani;
    AnimatorStateInfo stateInfo;


    public bool caught = false;
    bool bRushing = false;  // 是否正在冲刺
    int EventCnt = 0;       // 点击事件缓存（由于iTween动画结束前无法再次响应）
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
        }
        EventCnt = 0;   // 清缓存

        iTween.StopByName(gameObject, "myMoveBy");    // 若还没冲刺结束，则终止
        bRushing = false;
    }
    ////////////////

    // Use this for initialization
    void Start()
    {
        m_ani = this.GetComponent<Animator>();
        m_ani.SetBool("stinger", true);
        if (GameObject.Find("GameManager").GetComponent<SceneEventManager>().GetCurrentSceneIndex(SceneManager.GetActiveScene().name) == 3)
        {
            GetComponent<SpriteRenderer>().color = new Color(0.905f, 0.764f, 0.764f, 1f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!bRushing && ShotCnt > 0 && EventCnt > 0)
        {
            bRushing = true;
            EventCnt--;
            float moveTime;
            if (caught)//被捆绑的锯鳐，切割变慢
            {
                moveTime = 5f;
            }
            else
            {
                moveTime = 1f;// 0.5f;
            }

            iTween.MoveBy(gameObject, iTween.Hash
                (
                    "name","myMoveBy",
                    "x", maxMoveDis,
                    "time", moveTime,
                    "oncomplete", "RushDone",
                    "easeType", iTween.EaseType.easeOutCubic
                )
            );      // 完成之前无法再次执行
                    //transform.DOMoveX(maxMoveDis, moveTime).SetRelative().SetEase(Ease.Linear/*.OutExpo*/);
                    //transform.DOLocalMoveX(transform.position.x + maxMoveDis, moveTime)./*SetRelative().*/SetEase(Ease.Linear/*.OutExpo*/);

            //Rigidbody2D rig = GetComponent<Rigidbody2D>();
            //rig.AddRelativeForce(new Vector2(rig.mass * 20, 0), ForceMode2D.Impulse);
            //bRushing = false;


            if (--ShotCnt <= 0)
            {
                m_ani.SetBool("stinger", false);
                GetComponent<SpriteRenderer>().color = new Color32(255, 212, 146, 255);
                EventCnt = 0;   // 清缓存
            }
        }
    }


    void RushDone()
    {
        bRushing = false;

    }
    /// <summary>
    /// 锯鳐的冲刺（可以缓冲）
    /// </summary>
    public override bool TouchEvent()
    {
        if (ShotCnt > 0)
        {
            EventCnt++;
            return true;
        }
        else
            return false;

    }


}
