using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;

public partial class PlayerEventsNetwork : NetworkBehaviour
{
    public GameObject bulletPre;   //时钟的prefab
    public Transform bulletTrans;  //生成时钟的位置

    public GameObject bulletPre1 = null;   //鲸鱼的prefab
    public Transform bulletTrans1 = null;  //生成鲸鱼的位置

    public int num;//记录自己在服务器数组中的序号

    public NetworkHash128 assetId { get; set; }
    public string name = "ss";

    const float kA = 1f;
    [SerializeField]
    public float m_life = 10; //死亡旋转
    private string SceneName; //场景名称
    public float m_power = 10.0f;
    public Vector3 endPos;
    public float recordz;  //记录此时Z值
    bool isDead;
    private Rigidbody2D rigbody2d;

    bool islive; //判断玩家重生后是否移动
    Vector2 relifeV = new Vector2(0, 0); //既用于重生玩家后重置速度，又用来判断重生后速度是否为0，以此来判断玩家是否滑屏。
                                         //float life = 3;  
    public int PlayerLoseLife = 0;
    GameObject bullet;

    //{{ 网络部分

    IEnumerator RollBack()
    {
        yield return new WaitForSeconds(1f);
        iTween.RotateTo(gameObject, new Vector3(0, 0, 0), 3);
    }

    //
    public void Positioning()
    {
        transform.parent = Camera.main.transform;
    }
    void Start()
    {
        if (isServer)
        {
            this.GetComponent<PlayerEventsNetwork>().CmdMaketime();
        }

        bullet = GameObject.Find("StoneClock(Clone)");
        bullet.transform.parent = GameObject.Find("UI").transform;
        rigbody2d = GetComponent<Rigidbody2D>();
        GetScene();//调用获取场景方法
        isDead = false;
    }


    // Update is called once per frame
    void Update()
    {
        recordz = this.gameObject.transform.rotation.z;
        
        if (GetComponent<Rigidbody2D>().velocity != relifeV)
        {
            transform.parent = null;//恢复乌龟到世界上，不跟随相机
        }
        if ((!isDead && m_life <= 0))
        {
            //Debug.Log("dead");
            PlayerLoseLife++;
            Debug.Log("life = " + PlayerLoseLife);
            // this.GetComponent<PlayerEventsNetwork>().CmdDead(num);
            //这里可以根据玩家是否滑屏来判断是否继续赋值位置。
            //islive = true;
            GetComponent<Transform>().position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Mathf.Abs(-Camera.main.transform.position.z * 0.5f)));
            GetComponent<Rigidbody2D>().velocity = relifeV;
            Invincible();
            //GetComponent<Transform>().Rotate = new Vector3(0,0,0);
            iTween.RotateTo(Global.Player, new Vector3(0, 0, 0), 0);
            if (m_life == 0)
            {
                m_life = 10;
                Invoke("live", 3);
            }


        }

        rigbody2d.angularVelocity = Mathf.Clamp(rigbody2d.angularVelocity, -1000f * kA, 1000f * kA);
    }


    //玩家复活后的3S无敌(无碰撞体)+闪烁
    public void Invincible()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        Invoke("Recovery", 3);
        for (int i = 0; i < 6; i++)
        {
            Invoke("ClosePlayer", (0.25f + (0.5f * i)));
        }
    }

    void OpenPlayer()
    {
        //player.SetActive(true);
        gameObject.GetComponent<Renderer>().enabled = true;
    }
    void ClosePlayer()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        // player.SetActive(false);
        Invoke("OpenPlayer", 0.25f);
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            m_life = m_life - 10;
           
        }
    }


    private void GetScene()//调用此函数获得场景信息
    {
        SceneName = SceneManager.GetActiveScene().name;//获取场景名称
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
        //if (isShortDead)
        //{
        // 【【较复杂的死亡UI
        //deadPanel.GetComponent<Animation>().Play("UIAC_turnDarkShort");
        //deadPanel.GetComponent<Image>().DOFade(1, 1f);
        //Transform timeRuler = root.transform.Find("UI/TimeRuler");
        //InvokeRepeating("VolumeDecrease", 3f, 0.2f);
        //if (timeRuler)
        //{
        //    int year = root.transform.Find("UI/TimeRuler").GetComponent<TimeRuler>().year;
        //    int year = (int)(root.transform.Find("UI/StoneClock").GetComponent<StoneClock>().year);
        //    deadPanel.transform.Find("Tip").GetComponentInChildren<Text>().text = (year - 1).ToString() + "年\n" + "最后一只玳瑁龟遇到了危机";
        //    deadPanel.transform.Find("Warn").GetComponentInChildren<Text>().text = "不过，它一定会顺利度过的";
        //}
        //else
        //{
        //    deadPanel.GetComponentInChildren<Text>().text = "\n" + "最后一只玳瑁龟遇到了危机";
        //    deadPanel.transform.Find("Warn").GetComponentInChildren<Text>().text = "不过，它一定会顺利度过的";
        //}
        //】】

        yield return new WaitForSeconds(7f);
        //值为0的时候跳转场景      
        SceneManager.LoadScene(SceneName);//加载所需场景,SceneName为场景名
                                          //}

        //else
        //{
        //    deadPanel.GetComponent<Animation>().Play("UIAC_turnDark");
        //    deadPanel.GetComponent<Image>().DOFade(1, 1f);
        //    Transform timeRuler = root.transform.Find("UI/TimeRuler");
        //    InvokeRepeating("VolumeDecrease", 16.5f, 0.5f);
        //    if (timeRuler)
        //    {
        //        //int year = root.transform.Find("UI/TimeRuler").GetComponent<TimeRuler>().year;
        //        int year = (int)(root.transform.Find("UI/StoneClock").GetComponent<StoneClock>().year);
        //        deadPanel.transform.Find("Tip").GetComponentInChildren<Text>().text = (year - 1).ToString() + "年\n" + "最后一只玳瑁龟消失了";
        //    }
        //    else
        //    {
        //        deadPanel.GetComponentInChildren<Text>().text = "\n" + "最后一只玳瑁龟消失了";
        //    }
        //    yield return new WaitForSeconds(21.5f);
        //    //值为0的时候跳转场景      
        //    SceneManager.LoadScene(SceneName);//加载所需场景,SceneName为场景名
        //}

    }

    AudioSource bgm;
    void VolumeDecrease()
    {
        bgm = GameObject.Find("GameManager").GetComponent<AudioSource>();
        bgm.volume -= 0.1f;
        if (bgm.volume == 0)
        {
            CancelInvoke();
        }
    }

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
}