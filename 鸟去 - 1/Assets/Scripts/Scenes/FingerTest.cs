﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
//using UnityEngine.UI;
//using UnityEngine.Networking;

public class FingerTest : MonoBehaviour
{

    float leftBorder;
    float rightBorder;
    float topBorder;
    float downBorder;

    public float MaxX;
    public float MinX;

    [SerializeField]
    [Tooltip("点击产生闪光")]
    ParticleSystem clickEffect;
    [SerializeField]
    [Tooltip("点击产生水波")]
    RippleEffect TapEffect;
    [SerializeField]
    [Tooltip("划屏产生水流")]
    DirectionRippleEffect SwipEffect;
    public float maxDist;
    //    Cruise cruise;

    bool ShouldProcessTouch(int fingerIndex, Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        bool touchUI = Physics.Raycast(ray, float.PositiveInfinity);
        return !touchUI;
    }

    void Start()
    {
        FingerGestures.GlobalTouchFilter = ShouldProcessTouch;
        //        cruise = transform.GetComponent<Cruise>();
    }
    void Update()
    {
        //世界坐标的右上角  因为视口坐标右上角是1,1,点
        Vector3 cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(-Camera.main.transform.position.z)));
        //世界坐标左边界
        leftBorder = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
        //世界坐标右边界
        rightBorder = cornerPos.x;
        //世界坐标上边界
        topBorder = cornerPos.y;
        //世界坐标下边界
        downBorder = Camera.main.transform.position.y - (cornerPos.y - Camera.main.transform.position.y);

        if(Input.GetMouseButtonDown(1))
        {
            if (Global.Player)
                Global.Player.GetComponent<PlayerEvents>().TouchEvent();
        }
    }

    void OnSwipe(SwipeGesture gesture)
    {
        /*
        // 微小的划屏应该认为是点击
        if(gesture.Move.magnitude < Screen.width / 10f)
        {
            Tap(gesture);
            return;
        }
        //*/
        
        /*
        if (cruise.isCancelOnSwipe)
        {
            cruise.isCancelOnSwipe = false;
            return;
        }
        //*/

        // 划屏特效
        if (SwipEffect)
        {
            Vector2 end = Camera.main.ScreenToViewportPoint(gesture.Position);
            Vector2 start = Camera.main.ScreenToViewportPoint(gesture.StartPosition);
            SwipEffect.Emit(start, (end - start).normalized);
        }

        Swipe(gesture.Move);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="end">世界坐标系</param>
    /// <param name="start">世界坐标系</param>
    /// <param name="move">划屏向量（屏幕像素单位）</param>
    public void Swipe(Vector2 move)
    {
        gameObject.GetComponent<AudioSource>().Play();

        // 龟旋转
        Vector2 direction = move.normalized;
        Global.Player.GetComponent<PlayerEvents>().ClamSpeed();
        Global.Player.GetComponent<PlayerEvents>().direction = direction;
        RotateTo(direction, 2);

        /*  用于检测龟面前的物体
        RaycastHit2D[] hit;
        hit = Physics2D.RaycastAll(Player.transform.position, direction, 5f);
        float dis = 1f;
        int index = 0;
        while (hit[index].collider.gameObject.GetComponent<Rigidbody2D>() && !hit[index].collider.gameObject.tag.Equals("Player"))
        {
            Debug.Log(hit[index].collider.gameObject.name);
            dis = Mathf.Clamp(hit[index].distance - 1f, 0f, 1f) * 0f;
            break;
        }
        //*/

        // 基本力
        maxDist = Screen.width / 5f;
        float maxForce = 600f/*1000f*/, minForce = 300f;
        Vector2 force = move.normalized * Mathf.Clamp((move.magnitude / maxDist) * maxForce, minForce, maxForce);

        // 保证质量变化，加速度不变
        Vector2 forcePlayer = force * Global.Player.GetComponent<Rigidbody2D>().mass;
        Global.Player.GetComponent<Rigidbody2D>().AddForce(forcePlayer);

        if (Global.Player.activeInHierarchy)
        {
            // 动画
            Animator anim = Global.Player.GetComponent<Animator>();
            //*
            //Debug.Log(anim.GetCurrentAnimatorStateInfo(0).fullPathHash + "," + Animator.StringToHash("move"));
            if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.move"))
            //if(anim.GetCurrentAnimatorStateInfo(0).IsName("move"))
            {
                //Debug.Log("move");
                anim.SetTrigger("interrupt");
                anim.SetTrigger("move");
                //anim.SetTrigger("Idle");
            }
            else
            {
                anim.SetTrigger("move");
                //anim.SetTrigger("Idle");
            }
        }
        //*/

        // Rock vs Rock2
        // Rock  可以划动的石头
        GameObject[] oqs = GameObject.FindGameObjectsWithTag("Rock");
        foreach (GameObject oq in oqs)
        {
            if ((oq.transform.position.x < rightBorder) && (oq.transform.position.y < topBorder) && (oq.transform.position.y > downBorder)
          && (oq.transform.position.x > leftBorder))
            {
                if (oq.GetComponent<Rigidbody2D>() != null)
                {
                    Rigidbody2D rb = oq.GetComponent<Rigidbody2D>();
                    rb.velocity = Vector2.zero;
                    //force = move.normalized * Mathf.Clamp((move.magnitude / maxDist) * maxForce, minForce, maxForce);
                    rb.AddForce(force);
                }
            }
        }

        GameObject[] oss = GameObject.FindGameObjectsWithTag("Squid");
        foreach (GameObject os in oss)
        {
            if (os != null)
            {
                if ((os.transform.position.x < rightBorder) && (os.transform.position.y < topBorder) && (os.transform.position.y > downBorder)
          && (os.transform.position.x > leftBorder))
                {
                    Rigidbody2D rb = os.GetComponent<Rigidbody2D>();
                    rb.velocity = Vector2.zero;

                    Vector2 forceCoral = force * rb.mass / Global.Player.GetComponent<Rigidbody2D>().mass;    // 乌贼与龟同速的力
                    //float k = 0.7f;    // 乌贼的速度是龟的k倍
                    //rb.AddForce(forceCoral * k);
                    os.GetComponent<Coral>().AddForce(forceCoral);

                    os.GetComponent<Coral>().Flip(direction);
                }
            }
        }
    }


    /// <summary>
    /// 检测是否点击在UI上
    /// UGUI与FingerGestures一起使用会“冲突”：FingerGestures会穿透UGUI，需要此函数来判断是否点击在UI上
    /// PC与Android都有效
    /// </summary>
    public static bool IsPointerOverGameObject()
    {
        PointerEventData eventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;

        List<RaycastResult> list = new List<RaycastResult>();
        UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, list);
        return list.Count > 0;
    }

    void OnTap(TapGesture gesture)
    {
        Tap(gesture);
    }
    void Tap(DiscreteGesture gesture)
    {
        if (IsPointerOverGameObject())
            return;


        GameObject ob = gesture.Raycast.GameObject;
        if (ob != null &&
            (ob.tag == "Whale" || ob.tag == "Weapon" || ob.tag == "Squid" ||
            (ob.tag == "Player" && Global.SceneEvent.GetCurMystery().GetComponent<MYBackInTime>())
            )
            )
        {
            if(ob.GetComponent</*ITouchable*/TouchableBehaviour>().TouchEvent())
            {
                Vector2 effectPosition = Camera.main.ScreenToWorldPoint(gesture.Position);
                clickEffect.transform.position = new Vector3(effectPosition.x, effectPosition.y, -2f);
                clickEffect.Play();

                Vector2 clickPostion = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                TapEffect.Emit(clickPostion);

                ob.transform.Find("Panel").GetComponent<SpriteRenderer>().DOFade(0.5f, 0.05f).SetLoops(6, LoopType.Yoyo).SetUpdate(true);
            }
        }
        else
        {
            // 点击空地视为（从龟的位置开始到点击处的）划屏
            Vector3 move = Input.mousePosition - Camera.main.WorldToScreenPoint(Global.Player.transform.position);
            //if (move.magnitude >= Screen.width / 10f)
                Swipe(Input.mousePosition - Camera.main.WorldToScreenPoint(Global.Player.transform.position));
        }
    }

    // 龟旋转和恢复
    public void RotateTo(Vector2 dst, float t)
    {
        if (dst == Vector2.zero)
            return;
        float angle = Vector2.SignedAngle(Vector2.right, dst);
        if (angle > 160 || angle < -160)    // 直线后退的操作，不要旋转
            angle = 0;
        angle = Mathf.Clamp(angle, MinX, MaxX);

        StopCoroutine("RollBack");
        iTween.RotateTo(Global.Player, new Vector3(0, 0, angle), t);

        StartCoroutine("RollBack");
    }
    IEnumerator RollBack()
    {
        yield return new WaitForSeconds(1f);
        iTween.RotateTo(Global.Player, new Vector3(0, 0, 0), 3);
    }

    void AnimEnd()
    {
        Global.Player.GetComponent<Animator>().SetBool("move", false);
        Debug.Log("AnimEnd");
    }
}
