/*
 * Player脚本的偏类
 * 负责Player的网络联机功能、其他对象的网络功能的Cmd函数也只能通过Player的脚本来调用
 * （已经尝试过对其他对象的Identity组件选中授权、或用代码授权，都无效）
 * 客户端执行顺序是
 * Awake()、OnStartClient()、OnStartLocalPlayer()、Start()
 */
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public partial class PlayerEventsNetwork : TouchableNetwork //NetworkBehaviour
{
    [SyncVar]
    public Color m_Color;

    [SyncVar]
    public string m_PlayerName;

    //this is the player number in all of the players
    [SyncVar]
    public int m_PlayerNumber;

    //This is the local ID when more than 1 player per client
    [SyncVar]
    public int m_LocalID;

    [HideInInspector]
    [SyncVar(hook = "OnDeathTimesChanged")]
    public int DeathTimes;


    [SyncVar]
    ChangeColorToBlackAndWhite BlackAndWhite;


    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            m_life = m_life - 10;

        }
    }
    [ServerCallback]
    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            m_life = m_life - 10;

        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        //{{ 可能能放到Awake()里
        transform.position = Global.Player.transform.position + new Vector3(10, 0, 0);

        // 把Global.Player指向的单机Player替换成联机自动生成的Player，并备份，将来退出的时候用于还原【这么做都是为了防止其他地方一开始就要引用Player时出错】
        //Destroy(Global.Player);
        Global.Player.SetActive(false);
        Global.PlayerHidden = Global.Player;

        Global.Player = gameObject;
        Global.Net = GetComponent<PlayerEventsNetwork>();

    }

/// <summary>
/// 在服务器端触发序号为id的动物的技能
/// </summary>
/// <param name="id">-1表示龟</param>
[Command]
    public void CmdTouchEvent(int id)
    {
        if(id >=0)
            GameManager.ControllableObjects[id].GetComponent<ITouchable>().TouchEvent();
        else
            TouchEvent();   // 龟技能
    }

    //void DeadAndReborn()
    //{
    //    DeathTimes++;
    //    RpcInvincible();
    //}

    [ClientRpc]
    void RpcDeath()
    {
        gameObject.GetComponent<Animator>().SetTrigger("dead");//SetBool("Dead", true);
        //Global.InputNetwork.SetActive(false);        
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        transform.SetParent(Camera.main.transform);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.GetComponent<CapsuleCollider2D>().enabled = false;
        transform.GetComponent<BoxCollider2D>().enabled = false;

        
        iTween.RotateTo(gameObject, new Vector3(0, 0, 180), 3);
        Invoke("Reborn", 3);
        //float w = gameObject.GetComponent<Renderer>().bounds.size.x;
        //iTween.MoveTo(gameObject, new Vector3(transform.position.x + w, transform.position.y, transform.position.z), 3);
    }

    //玩家复活后的1S无敌(无碰撞体)+闪烁
    //[ClientRpc]
    void Reborn()
    {
        isDead = false;
        //Global.InputNetwork.SetActive(true);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        transform.SetParent(null);
        gameObject.GetComponent<Animator>().SetTrigger("reborn");

        StartCoroutine(Invincible());
        if (isLocalPlayer)
        {
            // 把自己屏幕上的中心传给服务器，在服务器上重新设置出生位置（为客户端的屏幕中心）
            Vector2 center = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
            CmdRebornTransform(center);
            
        }
    }
    /// <summary>
    /// 无敌闪烁
    /// </summary>
    /// <returns></returns>
    IEnumerator Invincible()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.05f);
            GetComponent<Renderer>().enabled = true;
        }
        GetComponent<CapsuleCollider2D>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    /// <summary>
    /// 客户端向服务器发送自己重生的位置
    /// </summary>
    [Command]
    void CmdRebornTransform(Vector2 center)
    {
        
        transform.position = new Vector3(center.x, center.y, transform.position.z);
        Rigidbody.velocity = Vector2.zero;
        iTween.RotateTo(gameObject, new Vector3(0, 0, 0), 0.5f);
    }

    void OnDeathTimesChanged(int times)
    {
        DeathTimes = times;
        GameManager.RankListUpdate();
    }

    [ClientRpc]
    public void RpcBackTime()
    {
        BlackAndWhite.Change();
    }

    // 与坦克一致
    public override void OnStartClient()
    {

        base.OnStartClient();

        if (!isServer) //if not hosting, we had the tank to the gamemanger for easy access!
            //GameManager.AddTank(gameObject, m_PlayerNumber, m_Color, m_PlayerName, m_LocalID);
            GameManager.AllPlayers.Add(gameObject);

        var renderer = GetComponentInChildren<Renderer>();
        renderer.material.color = m_Color;


        GameManager.RankListUpdate();

    }

    [ClientRpc]
    public void RpcAnim()
    {
        Animator anim = GetComponent<Animator>();
        if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.move"))
        {
            anim.SetTrigger("interrupt");
            anim.SetTrigger("move");
        }
        else
        {
            anim.SetTrigger("move");
        }
    }
    //[Command]
    //public void CmdTime(int total, int Length)
    //{
    //    GameObject.Find("StoneClock(Clone)").GetComponent<StoneClockNetwork>().CmdOnBackInTime(total, Length);
    //}
    [Command]
    public void CmdClamSpeed()
    {
        Rigidbody.velocity = new Vector2(0, 0);
    }

    /// <summary>
    /// 龟的划屏受力
    /// </summary>
    /// <param name="force"></param>
    [Command]
    public void CmdForce(Vector2 force)
    {
        Rigidbody.AddForce(force);
        RpcAnim();  // 龟动画全部由服务器向客户端发送
    }

    /// <summary>
    /// id物体的划屏受力（乌贼、轻石头等）
    /// </summary>
    /// <param name="id"></param>
    /// <param name="force"></param>
    [Command]
    public void CmdDrawForce(int id, Vector2 force)
    {
        //try
        //{
            Rigidbody2D rb = GameManager.ControllableObjects[id].GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.AddForce(force);
        //}
        //catch(IndexOutOfRangeException e)
        //{
        //    print("id" + id);
        //}

    }


    //[ClientRpc]
    //void RpcDead2()
    //{
    //    gameObject.GetComponent<Animator>().SetBool("Dead", true);
    //    Global.InputNetwork.SetActive(false);
    //    transform.GetComponent<CapsuleCollider2D>().enabled = false;

    //    transform.parent = Camera.main.transform;
    //    iTween.RotateTo(gameObject, new Vector3(0, 0, 180), 3);
    //    float w = gameObject.GetComponent<Renderer>().bounds.size.x;
    //    iTween.MoveTo(gameObject, new Vector3(transform.position.x + w, transform.position.y, transform.position.z), 3);

    //    StartCoroutine(Dead());
    //}
    [Command]
    public void CmdRotateTo(Vector2 dst, float t)
    {
        if (dst == Vector2.zero)
            return;
        float angle = Vector2.SignedAngle(Vector2.right, dst);
        if (angle > 160 || angle < -160)    // 直线后退的操作，不要旋转
            angle = 0;
        angle = Mathf.Clamp(angle, -45, 45);

        StopCoroutine("RollBack");
        iTween.RotateTo(gameObject, new Vector3(0, 0, angle), t);

        StartCoroutine("RollBack");
    }




    [Command]
    public void CmdDestroyCard(GameObject ob) {
        //Destroy(ob);
        GameManager.StaticObjects.Remove(ob);
        NetworkServer.Destroy(ob);
    }


    [ClientCallback]
    void DestroyCard2(GameObject x)
    {
        //DestroyCard();
        Destroy(x);
    }
    [Command]
    public void CmdGetCard(GameObject y,float x) {
        //DestroyCard2(y);

        float left = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0f)).x;
        iTween.MoveTo(y, iTween.Hash
                       (
                            "x", left,   //1.91 0.6 -0.95
                            "y", x,
                           "speed", 18f,
                           "time", 120f,
                           "easeType", iTween.EaseType.easeOutCubic
                       )
                    );
    }

    public void GetCard(GameObject y,float x) {
        //DestroyCard(y);
        float left = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0f)).x;
        iTween.MoveTo(y, iTween.Hash
                       (
                            "x", left,   //1.91 0.6 -0.95
                            "y", x,
                           "speed", 18f,
                           "time", 120f,
                           "easeType", iTween.EaseType.easeOutCubic
                       )
                    );
    }


    //[Command]
    //void Cmdgameover()
    //{
    //    Rpcgameover();
    //}
    //[ClientRpc]
    //void Rpcgameover()
    //{
    //    GameObject.Find("InputNetwork").SetActive(false);
    //    //Global.UI.GetComponent<UIManagernetwork>().victorynetwork.SetActive(true);
    //    GameObject.Find("UI").GetComponent<UIManager>().victoryPanel.SetActive(true);
    //}

    //called on client when the Network destroy that object (it was destroyed on server)
    public override void OnNetworkDestroy()
    {
        GameManager.AllPlayers.Remove(gameObject);
        base.OnNetworkDestroy();
    }

}