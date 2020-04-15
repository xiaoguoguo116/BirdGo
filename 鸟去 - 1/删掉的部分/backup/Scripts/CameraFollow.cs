

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    //镜头平滑移动比率
    //默认情况下镜头不跟随玩家
    //一旦玩家飞的比摄像机快，镜头就会跟随玩家
    //为了避免跟随的时候掉帧，需要让镜头的移动速度线性增加
    [SerializeField]
    private float smoothingSpeed = 5f;

    [SerializeField]
    GameObject BG;

    [Tooltip("自动卷屏")]
    [SerializeField]
    bool IsAutoScroll = true;

    //{{ 自动卷屏机制（不可后退，会被卷死）
    //自动卷屏速度（摄像机在不跟随玩家对象的情况下的移动速度）
    [SerializeField]
    public float cameraMoveSpeed = 1;

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
        Global.Player = Global.Player;
        StartCoroutine("CameraOpen");

        if (IsAutoScroll)
            transform.position = new Vector3(Global.Player.transform.position.x, Global.Player.transform.position.y, -10f);
        else
        {
            if (isRight)
            {
                transform.position = new Vector3(Global.Player.transform.position.x + 10f, Global.Player.transform.position.y, -10f);
            }
            else
            {
                transform.position = new Vector3(Global.Player.transform.position.x, Global.Player.transform.position.y, -10f);
            }
        }

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
        if (IsAutoScroll)
        {
            //左边界
            float leftBorader = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0f)).x;
            Vector2 dp = Global.Player.transform.position - transform.position;
            Vector3 defaultOffset = new Vector3(transform.position.x + cameraMoveSpeed, Mathf.Clamp(Global.Player.transform.position.y, -Global.BGHight + Global.CamHight, Global.BGHight - Global.CamHight), -10f);

            if (dp.x < Global.CamWidth * 0.2)
            {
                // 自动卷屏（匀速）
                transform.position = Vector3.Lerp(transform.position, defaultOffset, smoothingSpeed * Time.deltaTime);
            }
            else
            {
                // 镜头追赶（距离越远追得越快）
                // 镜头的卷屏与追赶哪个快用哪个
                Vector3 playerOffset = new Vector3(Global.Player.transform.position.x, Mathf.Clamp(Global.Player.transform.position.y, -Global.BGHight + Global.CamHight, Global.BGHight - Global.CamHight), -10f);
                if ((defaultOffset - transform.position).sqrMagnitude > (playerOffset - transform.position).sqrMagnitude)
                    transform.position = Vector3.Lerp(transform.position, defaultOffset, smoothingSpeed * Time.deltaTime);
                else
                    transform.position = Vector3.Lerp(transform.position, playerOffset, smoothingSpeed * Time.deltaTime);
            }

            if (Global.Player.transform.position.x <= leftBorader - deadLine)
            {
                //Debug.Log("你挂了");
                //PlayerPrefs.SetInt("isDeadLastTime", 1);
                Global.Player.GetComponent<PlayerEvents>().m_life = 0;
            }
        }
        else
        {
            //左边界
            //float leftBorader = GameObject.Find("GameManager").GetComponent<SceneEventManager>().leftBorder;
            float leftBorader = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0f)).x;
            if (Global.Player.transform.position.x < leftBorader)
            {
                Global.Player.transform.position = new Vector3(leftBorader, Global.Player.transform.position.y, 1f);
            }
            Vector2 dp = Global.Player.transform.position - transform.position;
            Vector3 defaultOffset = new Vector3(transform.position.x, Mathf.Clamp(Global.Player.transform.position.y, -Global.BGHight + Global.CamHight, Global.BGHight - Global.CamHight), -10f);

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
        }
    }
}
