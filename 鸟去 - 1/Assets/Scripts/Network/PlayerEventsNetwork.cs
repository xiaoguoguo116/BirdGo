/*
 * 玩家Player，即龟的相关功能
 * 客户端向服务器发送Cmd必须通过Player对象上的脚本，其他带Network Identity（不论选项如何设置）都无法实现
 * 访问方便起见，所有网络有关的脚本放在本类的一个偏类脚本PlayerUnet.cs里
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;

public partial class PlayerEventsNetwork : TouchableNetwork //NetworkBehaviour, ITouchableNetwork
{

    [SerializeField]
    float m_life = 10;

    private Rigidbody2D Rigidbody;

    public bool isDead;    // 死亡状态


    //GameObject[] skillButton = new GameObject[3];
    //public SyncListString SkillButton = new SyncListString();

    //bool islive; //判断玩家重生后是否移动
    //Vector2 relifeV = new Vector2(0, 0); //既用于重生玩家后重置速度，又用来判断重生后速度是否为0，以此来判断玩家是否滑屏。

    //float life = 3;  

    void Start()
    {
        //// 这些场景有关的初始化应该放在GameManager里
        BlackAndWhite = GameObject.Find("B&W").GetComponent<ChangeColorToBlackAndWhite>();

        Rigidbody = GetComponent<Rigidbody2D>();
        isDead = false;
    }

    public override bool TouchEvent()
    {
        // 龟的触发需要在所有客户端上运行回溯操作
        // 【由于回溯时，timeScale = 0，服务器的数据无法及时传给客户端】
        RpcEffect();
        return true;

    }



    // 复原姿态
    IEnumerator RollBack()
    {
        
        yield return new WaitForSeconds(1f);
        if (! Global.Player.GetComponent<PlayerEventsNetwork>().isDead) // 死亡状态下不用复原姿态
            iTween.RotateTo(gameObject, new Vector3(0, 0, 0), 3);
    }

    //
    public void Positioning()
    {
        transform.parent = Camera.main.transform;
    }


    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            if (m_life <= 0)
            {
                DeathTimes++;
                isDead = true;
                RpcDeath();

                m_life = 10;
            }
        }

        // 速度和角速度限制
        Rigidbody.angularVelocity = Mathf.Clamp(Rigidbody.angularVelocity, -1000f, 1000f);
        Rigidbody.velocity = Vector2.ClampMagnitude(Rigidbody.velocity, 15);
    }



    // 死亡UI
    IEnumerator Dead()
    {
        yield return new WaitForSeconds(1f);

        //GameObject.Find("UI").GetComponent<UIManager>().deadPanel.SetActive(true);
        //yield return new WaitForSeconds(2f);
        SceneEventManager manager = GameObject.Find("GameManager").GetComponent<SceneEventManager>();
        if (Global.lastDeadMy == manager.GetCurrentMyIndex() && Global.lastDeadScene == SceneManager.GetActiveScene().name)//如果上次死亡谜题和关卡和这次相同，则连续死亡次数+1，否则重置死亡信息
        {
            Global.deadCount++;
        }
        else
        {
            Global.deadCount = 1;
            Global.lastDeadMy = manager.GetCurrentMyIndex();
            Global.lastDeadScene = SceneManager.GetActiveScene().name;
        }
        GameObject deadPanel = GameObject.Find("UI").gameObject.GetComponent<UIManager>().deadPanel;
        deadPanel.SetActive(true);


        yield return new WaitForSeconds(7f);
        //值为0的时候跳转场景      
        SceneManager.LoadScene(GameManager.SceneName);//加载所需场景,SceneName为场景名



    }

    //AudioSource bgm;
    //void VolumeDecrease()
    //{
    //    bgm = GameObject.Find("GameManager").GetComponent<AudioSource>();
    //    bgm.volume -= 0.1f;
    //    if (bgm.volume == 0)
    //    {
    //        CancelInvoke();
    //    }
    //}

    CameraFollow cameraFollow;
    float PlayerSpeed;
    /// <summary>
    /// 龟速从playerSpeed降为0；卷屏速度从当前还原为1
    /// </summary>
    public void SpeedDown(float playerSpeed)
    {
        if (Camera.main)    // 如果是场景退出，会为空
        {
            cameraFollow = Camera.main.GetComponent<CameraFollow>();
            PlayerSpeed = playerSpeed;
            StartCoroutine("SpeedDownSlowly");
        }
    }
    IEnumerator SpeedDownSlowly()
    {
        while (cameraFollow.cameraMoveSpeed > 1 + 1e-4)
        {
            cameraFollow.cameraMoveSpeed = Mathf.Lerp(cameraFollow.cameraMoveSpeed, 1, Time.deltaTime);
            PlayerSpeed = Mathf.Lerp(PlayerSpeed, 0, Time.deltaTime);
            //print(cameraFollow.cameraMoveSpeed + "," + PlayerSpeed);

            // 龟自动游泳速度
            transform.position = new Vector2(transform.position.x + PlayerSpeed * Time.deltaTime, transform.position.y);
            yield return null;
        }
    }

    protected override void Effect()
    {//GameManager.test = "Effect";
        // 龟的触发需要在所有客户端上运行回溯操作
        //SceneEventManager sm = Global.SceneEvent;
        //if (sm.GetCurMystery().GetComponent<MYBackInTimeNetwork>())
        //{
        //    // PC进不来
        //    sm.GetCurMystery().GetComponent<MYBackInTimeNetwork>().OnBackInTime();

        //}
        if(Global.MYNetwork.GetComponent<MYBackInTimeNetwork>())
        {

            Global.MYNetwork.GetComponent<MYBackInTimeNetwork>().OnBackInTime();
        }
    }
}