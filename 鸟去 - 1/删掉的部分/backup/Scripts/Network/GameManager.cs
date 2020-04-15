/*
 * 联机场景相关（模仿坦克的做法）
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine.SceneManagement;

//里面应设有的功能
//1.可以同步显示生命值
//2.可以判断游戏是否已经结束
//3.游戏中途暂停？？？？？是否设置该功能
//4.游戏胜利失败页面管理
public class GameManager : NetworkBehaviour
{
    [SerializeField]
    GameObject ClockPre;   //时钟的prefab

    public static string SceneName; //场景名称

    //[SyncVar]
    //ChangeColorToBlackAndWhite BlackAndWhite;




    static public GameManager s_Instance;

    //GameObject[] players;//判断场景内还有几个玩家
    //这是静态的，所以即使加载了场景(即从大厅)也可以添加坦克
    static public List<TankManager> m_Tanks = new List<TankManager>();             // 用于启用和禁用坦克不同方面的管理器集合。
    static public List<GameObject> AllPlayers = new List<GameObject>();     // 用于排行榜排序

    // 三种动物、轻石头
    // 服务器和客户端都需维持一个一致的可点击或可划屏的动物/石头（龟除外）数组（可随机访问），
    // 客户端点击动物（Tap -> TouchEventNetwork），或者点击动物卡（SkillTrigger），或者划屏（Swipe）
    // 都是先找到在数组里的id，然后通过Player.CmdTouchEvent在服务器端调用改动物的TouchEvent。
    // 加入数组是：通过该动物的Start()；【因为客户端也需要维持这个数组】
    // 移除数组是：服务器端直接移除；客户端通过该动物的OnNetworkDestroy()。
    static public List<GameObject> ControllableObjects = new List<GameObject>();

    // 随机生成的静态物体（石头、卡牌等）
    // 用于服务器端进行删除管理，客户端不需要维持该数组
    static public List<GameObject> StaticObjects = new List<GameObject>();

    // 客户端3个技能卡槽
    static public GameObject[] SkillButton = new GameObject[3];

    //// 移动边界预设
    //[SerializeField]
    //static GameObject MovingBorderPre;
    //static GameObject MovingBorder;

   
    //[Tooltip("预设")]
    //[SerializeField]
    //GameObject MovingBorderPre;
    public static Transform LeftBorder, RightBorder;
    [Header("Moving Border")]
    [Tooltip("左右边界墙的距离（屏幕宽度的倍数）")]
    [SerializeField]
    float BorderWidthFactor = 2;
    [Tooltip("移动速度")]
    [SerializeField]
    float cameraMoveSpeed = 1;
    [SerializeField]
    float smoothingSpeed = 5f;
    [SyncVar]
    float LeftBorderX;      // 左边界的x位置
    //[SyncVar/*(hook = "OnBorderDistChange")*/]   
    //float BorderDist;       // 左右边界间距离（以服务器上的为准）

    
    



    //public int m_NumRoundsToWin = 5;          // 一个玩家赢得比赛必须赢的回合数。
    public float m_StartDelay = 3f;           // 循环开始和循环形播放阶段之间的延迟.
    public float m_EndDelay = 3f;             // 循环播放结束和循环结束阶段之间的延迟.
                                              // 公共摄像机控制m _摄像机控制;     // 参camera控制脚本，以便在不同阶段进行控制.
    public Text m_MessageText;                // 引用覆盖文本以显示获胜文本等。
    public GameObject m_TankPrefab;           // 参考Tank控制的预置.

    public Transform[] m_SpawnPoint;
    
    [HideInInspector]
    [SyncVar]
    public bool m_GameIsFinished = false;

    //各种用户界面引用，用于在两轮之间隐藏屏幕。
    [Space]
    [Header("UI")]
    public CanvasGroup m_FadingScreen;  
    public CanvasGroup m_EndRoundScreen;
    //private int m_RoundNumber;                  // 现在是哪一轮比赛。
    //private WaitForSeconds m_StartWait;         // 回合开始时有一段延迟。
    //private WaitForSeconds m_EndWait;           //过去在回合或游戏结束时会有延迟。
    private TankManager m_RoundWinner;          // 提及本轮的获胜者。用来宣布谁赢了。
    private TankManager m_GameWinner;           // 提及游戏的获胜者。用来宣布谁赢了。

    public static Text RankText;
    void Awake()
    {
        s_Instance = this;
        Global.MYNetwork = GameObject.Find("Mysterys/MY0").transform;
        RankText = GameObject.Find("RankingList").GetComponent<Text>();
        LeftBorder = GameObject.Find("LeftBorder").transform;
        RightBorder = GameObject.Find("RightBorder").transform;
    }


    private void Start()
    {
        // 这些场景有关的初始化应该放在GameManager里
        
        SceneName = SceneManager.GetActiveScene().name;//获取场景名称

        GameObject clock;
        
        if (isServer)
        {
            // 所有端生成石钟
            //bullet = Instantiate(ClockPre, bulletTrans.position, Quaternion.identity) as GameObject;
            clock = Instantiate(ClockPre) as GameObject;
            clock.transform.SetParent(Global.UI.transform);
            NetworkServer.Spawn(clock);    //在所有客户端
        }
        //clock = GameObject.Find("StoneClock(Clone)");
        //clock.transform.parent = Global.UI/* GameObject.Find("UI")*/.transform;
        //clock.transform.SetParent(Global.UI.transform);



        if (isServer)
        {
            // 移动边框，初始化为以服务器玩家为中心对称的左右边界
            // 
            LeftBorderX = Global.Player.transform.position.x - BorderWidthFactor * Global.CamWidth;
            LeftBorder.position = new Vector2(LeftBorderX, 0);
            // 右边界的距离
            //BorderDist = BorderWidthFactor * Global.CamWidth * 2;
            //RightBorder.localPosition = new Vector2(BorderDist, 0);
           
        }
        else
        {
            // 客户端先不放置边框，防止出现意外的碰撞
//            LeftBorder.gameObject.SetActive(false);
        }




        // 技能卡槽
        SkillButton[0] = GameObject.Find("skill(1)");
        SkillButton[1] = GameObject.Find("skill(2)");
        SkillButton[2] = GameObject.Find("skill(3)");
    }

    //// 当客户端接收到时，开启客户端的边框【不知为何触发不了】
    //void OnBorderDistChange(float dist)
    //{
    //    BorderDist = dist;
    //    LeftBorder.gameObject.SetActive(true);
    //    RightBorder.localPosition = new Vector2(BorderDist, 0);
    //}
    public void Update()
    {

    }
    public void FixedUpdate()
    {
        if (isServer)
        {
            // 左边界匀速移动
            Vector2 defaultOffset = new Vector2(LeftBorder.position.x + cameraMoveSpeed, 0);
            LeftBorder.position = Vector2.Lerp(LeftBorder.position, defaultOffset, smoothingSpeed * Time.deltaTime);
            LeftBorderX = LeftBorder.position.x;
        }
        else
        {
            // 客户端读取同步变量的值
            //LeftBorder.position = new Vector2(LeftBorderX, LeftBorder.position.y);
            Vector2 defaultOffset = new Vector2(LeftBorderX, LeftBorder.position.y);
            LeftBorder.position = Vector2.Lerp(LeftBorder.position, defaultOffset, smoothingSpeed * Time.deltaTime);
        }
    }

    public static void RankListUpdate()
    {
        // 对AllPlayer数组按死亡次数排序
        AllPlayers.Sort((x, y) => x.GetComponent<PlayerEventsNetwork>().DeathTimes.CompareTo(y.GetComponent<PlayerEventsNetwork>().DeathTimes));

        // 更新排行榜
        GameManager.RankText.text = "玩家\t\t\t死亡次数\n";
        foreach (var ob in AllPlayers)
        {
            GameManager.RankText.text += ob.GetComponent<PlayerEventsNetwork>().m_PlayerName + "\t\t\t" +
                ob.GetComponent<PlayerEventsNetwork>().DeathTimes + "\n";
        }
    }

    public static void GameOver() {
        //Global.AllPlayers.Clear();

        Global.Player = Global.PlayerHidden;

        LobbyManager.s_Singleton.ServerReturnToLobby();
    }

    /// <summary>
    /// 服务器端调用，并把参数赋值给Player的[SyncVar]变量
    /// </summary>
    /// <param name="tank"></param>
    /// <param name="playerNum"></param>
    /// <param name="c"></param>
    /// <param name="name"></param>
    /// <param name="localID"></param>
    static public void AddPlayer(GameObject tank, int playerNum, Color c, string name, int localID)
    {
        // 把参数赋值给Player的[SyncVar]变量
        tank.GetComponent<PlayerEventsNetwork>().m_Color = c;
        tank.GetComponent<PlayerEventsNetwork>().m_PlayerName = name;
        tank.GetComponent<PlayerEventsNetwork>().m_LocalID = localID;
        AllPlayers.Add(tank);

    }
    /// <summary>
    /// Add a tank from the lobby hook
    /// </summary>
    /// <param name="tank">大厅实例化的实际游戏对象，这是一种网络行为</param>
    /// <param name="playerNum">The number of the player (based on their slot position in the lobby)</param>
    /// <param name="c">玩家的颜色，在大厅里选择</param>
    /// <param name="name">玩家的名字，在大厅里选择</param>
    /// <param name="localID">localID。例如，如果两个玩家在同一台机器上，这将是1 & 2</param>
    static public void AddTank(GameObject tank, int playerNum, Color c, string name, int localID)
    {
        TankManager tmp = new TankManager();
        tmp.m_Instance = tank;
        tmp.m_PlayerNumber = playerNum;
        tmp.m_PlayerColor = c;
        tmp.m_PlayerName = name;
        tmp.m_LocalPlayerID = localID;
        tmp.Setup();

        m_Tanks.Add(tmp);
    }

    public void RemoveTank(GameObject tank)
    {
        TankManager toRemove = null;
        foreach (var tmp in m_Tanks)
        {
            if (tmp.m_Instance == tank)
            {
                toRemove = tmp;
                break;
            }
        }

        if (toRemove != null)
            m_Tanks.Remove(toRemove);
    }

    // 这从一开始就被调用，并将一个接一个地运行游戏的每个阶段。仅在服务器上(因为启动仅在服务器上调用)
    private IEnumerator GameLoop()
    {
        while (m_Tanks.Count < 2)
        {
            Debug.Log("m_Tanks.Count = " + m_Tanks.Count);
            yield return null;
        }

        //等待以确保一切准备就绪
        yield return new WaitForSeconds(2.0f);

        // 从运行“round starting”coro utine开始，但在完成之前不要返回。
        //yield return StartCoroutine(RoundStarting());

        // 一旦完成了“循环开始”的corutine，运行“循环播放”的corutine，但是在它完成之前不要返回。
        yield return StartCoroutine(RoundPlaying());

        // 一旦执行返回到这里，运行“圆化”协程.
       // yield return StartCoroutine(RoundEnding());

        // 该代码直到“RoundEnding”完成后才运行。此时，检查游戏是否有赢家。
        if (m_GameWinner != null)
        {// 如果有游戏赢家，等待一定数量或所有玩家确认再次开始游戏
            m_GameIsFinished = true;
            float leftWaitTime = 15.0f;
            bool allAreReady = false;
            int flooredWaitTime = 15;

            while (leftWaitTime > 0.0f && !allAreReady)
            {
                yield return null;

                allAreReady = true;
                foreach (var tmp in m_Tanks)
                {
                    allAreReady &= tmp.IsReady();
                }

                leftWaitTime -= Time.deltaTime;

                int newFlooredWaitTime = Mathf.FloorToInt(leftWaitTime);

                if (newFlooredWaitTime != flooredWaitTime)
                {
                    flooredWaitTime = newFlooredWaitTime;
                    string message = EndMessage(flooredWaitTime);
                    RpcUpdateMessage(message);
                }
            }

            LobbyManager.s_Singleton.ServerReturnToLobby();
        }
        else
        {
            // 如果还没有赢家，重新启动这个协程，这样循环就会继续。
            // 注意这个循环不会停止。这意味着游戏循环的当前版本将会结束。
            StartCoroutine(GameLoop());
        }
    }


    //private IEnumerator RoundStarting()
    //{
        //我们通知所有客户回合开始
       // RpcRoundStarting();

        // 等待指定的时间长度，直到将控制权交还给游戏循环。
        //yield return m_StartWait;
    //}

    [ClientRpc]
    void RpcRoundStarting()
    {
        // 一旦回合开始，重置坦克并确保它们不能移动。
        //  ResetAllTanks();
        DisableTankControl();

        // 将相机的变焦和位置调整到适合重置坦克的位置。
        //m_CameraControl.SetAppropriatePositionAndSize();

        // 增加轮数并显示文本，向玩家显示轮数
        //m_RoundNumber++;
        //m_MessageText.text = "ROUND " + m_RoundNumber;


        StartCoroutine(ClientRoundStartingFade());
    }

    private IEnumerator ClientRoundStartingFade()
    {
        float elapsedTime = 0.0f;
        float wait = m_StartDelay - 0.5f;

        yield return null;

        while (elapsedTime < wait)
        {
            //if(m_RoundNumber == 1)
             //   m_FadingScreen.alpha = 1.0f - (elapsedTime / wait);
          //  else
              //  m_EndRoundScreen.alpha = 1.0f - (elapsedTime / wait);

            elapsedTime += Time.deltaTime;

            //有时，由于数据包丢失，同步会滞后，所以我们要确保我们的坦克被重新生成
            //if (elapsedTime / wait < 0.5f)
            //  ResetAllTanks();

            yield return null;
        }
    }

    private IEnumerator RoundPlaying()
    {
        //通知客户回合现在开始，他们应该允许玩家移动。
        RpcRoundPlaying();

        // 当没有坦克断线了...
        while (!OneTankLeft())
        {
            // ... 下一帧返回.
            yield return null;
        }
    }

    [ClientRpc]
    void RpcRoundPlaying()
    {
        // 一旦回合开始，让玩家控制坦克.
        EnableTankControl();

        //清除屏幕上的文本.
        m_MessageText.text = string.Empty;
    }

   // private IEnumerator RoundEnding()
    //{
        // 清除前一轮的获胜者.
       // m_RoundWinner = null;

        // 看看现在这轮比赛结束了没有赢家。
       // m_RoundWinner = GetRoundWinner();

        // 如果有赢家，增加他们的分数.
       // if (m_RoundWinner != null)
           // m_RoundWinner.m_Wins++;

        // 现在获胜者的分数增加了，看看游戏中是否有人有一个。
       // m_GameWinner = GetGameWinner();

       // RpcUpdateMessage(EndMessage(0));

        //通知客户他们应该禁用坦克控制
      //  RpcRoundEnding();

        // 等待指定的时间长度，直到将控制权交还给游戏循环。
        //yield return m_EndWait;
   // }

    [ClientRpc]
    private void RpcRoundEnding()
    {
        DisableTankControl();
        StartCoroutine(ClientRoundEndingFade());
    }

    [ClientRpc]
    private void RpcUpdateMessage(string msg)
    {
        m_MessageText.text = msg;
    }

    private IEnumerator ClientRoundEndingFade()
    {
        float elapsedTime = 0.0f;
        float wait = m_EndDelay;
        while (elapsedTime < wait)
        {
            m_EndRoundScreen.alpha = (elapsedTime / wait);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // 这是用来检查是否有一个或更少的坦克剩余，因此回合应该结束。
    private bool OneTankLeft()
    {
        //从零开始计算剩余的坦克数量.
        int numTanksLeft = 0;

        //检查所有的坦克...
        for (int i = 0; i < m_Tanks.Count; i++)
        {
            // ... 如果它们是活动的，增加计数器.
            // if (m_Tanks[i].m_TankRenderers.activeSelf)
            // numTanksLeft++;
        }

        // 如果有一个或更少的坦克剩余返回真，否则返回假.
        return numTanksLeft <= 1;
    }


    // 这个功能是找出这轮比赛中是否有赢家。
    // 调用该函数时，假设当前有1个或更少的坦克处于活动状态
    private TankManager GetRoundWinner()
    {
        // 检查所有的坦克...
        for (int i = 0; i < m_Tanks.Count; i++)
        {
            // ... 如果其中一个是活跃的，它就是赢家，所以把它退回去.
            //if (m_Tanks[i].m_TankRenderers.activeSelf)
            // return m_Tanks[i];

        }
        // 如果没有坦克是活跃的，这是一个平局，所以返回null.
        return null;
    }


    // 这个功能是找出游戏中是否有赢家.
    private TankManager GetGameWinner()
    {
        int maxScore = 0;

        // 检查所有的坦克...
       // for (int i = 0; i < m_Tanks.Count; i++)
        //{
         //   if(m_Tanks[i].m_Wins > maxScore)
         //   {
    //           maxScore = m_Tanks[i].m_Wins;
         //   }

            // ... 如果他们中的一个有足够的回合来赢得比赛，就把它退回去.
           // if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
             //   return m_Tanks[i];
      //  }

        //第二次打开/关闭坦克上的皇冠
        //(note : 如果最大分数为0，我们不会判定，因为还没有人是当前的胜利者！)
        for (int i = 0; i < m_Tanks.Count && maxScore > 0; i++)
        {
          //  m_Tanks[i].SetLeader(maxScore == m_Tanks[i].m_Wins);
        }

        // 如果没有坦克有足够的回合获胜，返回空值.
        return null;
    }


    // 以坦克的颜色返回每个玩家的分数.
    private string EndMessage(int waitTime)
    {
        // 默认情况下，这一轮没有赢家，所以是平局.
        string message = "DRAW!";


        // 如果有一个游戏获胜者，设置消息说哪个玩家赢得了游戏。
        if (m_GameWinner != null)
            message = "<color=#" + ColorUtility.ToHtmlStringRGB(m_GameWinner.m_PlayerColor) + ">"+ m_GameWinner.m_PlayerName + "</color> WINS THE GAME!";
        // 如果有赢家，请将消息更改为以他们的颜色显示“PLAYER #”和获胜消息。
        else if (m_RoundWinner != null)
            message = "<color=#" + ColorUtility.ToHtmlStringRGB(m_RoundWinner.m_PlayerColor) + ">" + m_RoundWinner.m_PlayerName + "</color> WINS THE ROUND!";

        // 在平局或胜利者的信息之后，在领先者面前增加一些空间.
        message += "\n\n";

        // 浏览所有的坦克，用他们的颜色显示他们的得分.
       // for (int i = 0; i < m_Tanks.Count; i++)
        //{
           // message += "<color=#" + ColorUtility.ToHtmlStringRGB(m_Tanks[i].m_PlayerColor) + ">" + m_Tanks[i].m_PlayerName + "</color>: " + m_Tanks[i].m_Wins + " WINS " 
              //s  + (m_Tanks[i].IsReady()? "<size=15>READY</size>" : "") + " \n";
        //}

        if (m_GameWinner != null)
            message += "\n\n<size=20 > Return to lobby in " + waitTime + "\nPress Fire to get ready</size>";

        return message;
    }


    // 该功能用于重新打开所有坦克，并重置其位置和属性。
   // private void ResetAllTanks()
    //{
        //for (int i = 0; i < m_Tanks.Count; i++)
       // {
          //  m_Tanks[i].m_SpawnPoint = m_SpawnPoint[m_Tanks[i].m_Setup.m_PlayerNumber];
            //m_Tanks[i].Reset();
       // }
   // }


    private void EnableTankControl()
    {
        //for (int i = 0; i < m_Tanks.Count; i++)
      //  {
          //  m_Tanks[i].EnableControl();
      //  }
    }


    private void DisableTankControl()
    {
       // for (int i = 0; i < m_Tanks.Count; i++)
       // {
          //  m_Tanks[i].DisableControl();
       // }
    }
    //判断游戏是否结束，并且对应弹出结束UI
    //[ServerCallback]
    /*private void FindWinner()
    {
        if (isServer)
            players = GameObject.FindGameObjectsWithTag("Player");
        if (players[0].GetComponent<PlayerEventsNetwork>().life < 1)
        {

        }
        if (players[1].GetComponent<PlayerEventsNetwork>().life < 1)
        {

        }

    }*/

}
