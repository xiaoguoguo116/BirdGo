

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Networking;

public class CameraFollowNetwork : MonoBehaviour
{
    //镜头平滑移动比率
    //默认情况下镜头不跟随玩家
    //一旦玩家飞的比摄像机快，镜头就会跟随玩家
    //为了避免跟随的时候掉帧，需要让镜头的移动速度线性增加
    [SerializeField]
    private float smoothingSpeed = 5f;

    [SerializeField]
    GameObject BG;

    //[Header("移动边框")]
    //[Tooltip("左右边界墙")]
    //[SerializeField]
    //Transform LeftBorder, RightBorder;
    //[Tooltip("左右边界墙的距离（屏幕宽度的倍数）")]
    //[SerializeField]
    //float BorderWidthFactor = 2;

    //{{ 自动卷屏机制（不可后退，会被卷死）
    //自动卷屏速度（摄像机在不跟随玩家对象的情况下的移动速度）
    [SerializeField]
    public float cameraMoveSpeed = 1;   // 【在联机模式里，此处变量没用了】


    //当玩家到达左边界时，即玩家的x坐标与左边界在世界坐标的x相同时
    //为了给玩家一定的操作空间，不让玩家一到左边界就当场去世
    //定义了一个缓冲值，使得真正的死亡界限为 leftborder - deadline
    [SerializeField]
    float deadLine = 2;
    //}}

    //{{ 非自动卷屏机制（可左右无限移动，不会卷死）
    [Tooltip("玩家在镜头左边，取消则为中心")]
    [SerializeField]
    bool isRight = false;
    //}}

    private void Awake()
    {
        if (BG != null)
        {
            Global.BGHight = BG.GetComponent<Renderer>().bounds.size.y / 2;
            Global.BGWidth = BG.GetComponent<Renderer>().bounds.size.x / 2;
        }
        Global.CamHight = GetComponent<Camera>().orthographicSize;
        Global.CamWidth = Global.CamHight * GetComponent<Camera>().aspect;

    }
    // Use this for initialization
    void Start()
    {
        //Global.Player = Global.Player;
        StartCoroutine("CameraOpen");


            if (isRight)
            {
                transform.position = new Vector3(Global.Player.transform.position.x + 10f, Global.Player.transform.position.y, -10f);
            }
            else
            {
                transform.position = new Vector3(Global.Player.transform.position.x, Global.Player.transform.position.y, -10f);
            }

        //LeftBorder.position = new Vector2(transform.position.x - BorderWidthFactor * Global.CamWidth, 0);
        //RightBorder.localPosition = new Vector2(BorderWidthFactor * Global.CamWidth * 2, 0);
        //LeftBorder = GameManager.LeftBorder;
        //RightBorder = GameManager.RightBorder;


        /*
        // 第三关喷墨
        GameObject bigInk;
        if(bigInk = transform.Find("BigInk").gameObject)
        {
            bigInk.GetComponent<ParticleSystem>().Stop();
        }
        //*/
    }
    IEnumerator CameraOpen()
    {
        float d = 0.1f;
        GetComponent<Camera>().orthographicSize = 5;
        while (GetComponent<Camera>().orthographicSize < 10)
        {
            GetComponent<Camera>().orthographicSize += d;
            yield return new WaitForSeconds(0.02f);
            d *= 0.98f;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Global.Player == null)
            return;
        //if (isServer)
        //{
        //    // 左边界匀速移动
        //    Vector2 defaultOffset = new Vector2(LeftBorder.position.x + cameraMoveSpeed, 0);
        //    LeftBorder.position = Vector2.Lerp(LeftBorder.position, defaultOffset, smoothingSpeed * Time.deltaTime);
        //}

        if (isRight)
        {
            // 镜头追赶
            Vector3 playerOffset = new Vector3(Global.Player.transform.position.x + 10f, Mathf.Clamp(Global.Player.transform.position.y, -Global.BGHight + Global.CamHight, Global.BGHight - Global.CamHight), -10f);
            transform.position = Vector3.Lerp(transform.position, playerOffset, smoothingSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 playerOffset = new Vector3(Global.Player.transform.position.x, Mathf.Clamp(Global.Player.transform.position.y, -Global.BGHight + Global.CamHight, Global.BGHight - Global.CamHight), -10f);
            transform.position = Vector3.Lerp(transform.position, playerOffset, smoothingSpeed * Time.deltaTime);
        }

        // 镜头遇边界修正
        float x = Mathf.Clamp(transform.position.x, GameManager.LeftBorder.position.x + Global.CamWidth, GameManager.RightBorder.position.x - Global.CamWidth);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);


        // 触碰边界死亡，改成龟碰撞检测tag，与其他不可接触物体做法统一

            //if (Global.Player.transform.position.x <= leftBorader - deadLine)
            //{
            //    //Debug.Log("你挂了");
            //    //PlayerPrefs.SetInt("isDeadLastTime", 1);
            //    Global.Player.GetComponent<PlayerEvents>().m_life = 0;
            //}


    }
}
