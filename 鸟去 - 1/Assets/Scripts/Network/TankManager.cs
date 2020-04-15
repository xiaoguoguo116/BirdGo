using System;
using UnityEngine;

[Serializable]
public class TankManager
{
    // 该类用于控制坦克上的各种设置.
    // 它与Gamemanager一起控制坦克的行为
    // 玩家是否能控制他们的坦克在游戏的不同阶段

    public Color m_PlayerColor;               // 这是这个坦克子要染的颜色。
    //public Transform m_SpawnPoint;            // 坦克开火时的位置和方向
    [HideInInspector]
    public int m_PlayerNumber;                // 这指定了哪个玩家为服务器。
    [HideInInspector]
    public GameObject m_Instance;             // 创建坦克时对其实例化的引用
    [HideInInspector]
    public GameObject m_TankRenderers;        //Transform作为渲染器的父节点， 当坦克死亡时，此功能将被停用.
    //[HideInInspector]
    //public int m_Wins;                        // 到此为止，玩家获胜的次数
    [HideInInspector]
    public string m_PlayerName;                    // 大厅中设置的玩家姓名
    [HideInInspector]
    public int m_LocalPlayerID;                    // 播放器本地标识(如果同一台机器上有一个以上的播放器)



    // public TankMovement m_Movement;        // 在不同游戏阶段对各种控制对象的引用。
    //public TankShooting m_Shooting;
    // public TankHealth m_Health;
    //public TankLife m_life;
    public TankSetup m_Setup;

    public void Setup()
    {


        //Debug.Log("ssssssssssssss");
        // Get references to the components.
        //m_Movement = m_Instance.GetComponent<TankMovement>();
        //m_Shooting = m_Instance.GetComponent<TankShooting>();
        //m_Health = m_Instance.GetComponent<TankHealth>();
        m_Setup = m_Instance.GetComponent<TankSetup>();

        // Get references to the child objects.
        //m_TankRenderers = m_Health.m_TankRenderers;

        //在健康脚本中设置对amanger引用，以便在死亡时禁用控制
        //m_Health.m_Manager = this;

        // 在脚本中设置一致的玩家号
         //m_Movement.m_PlayerNumber = m_PlayerNumber;
         //m_Movement.m_LocalID = m_LocalPlayerID;

        //m_Shooting.m_PlayerNumber = m_PlayerNumber;
        //m_Shooting.m_localID = m_LocalPlayerID;

        //设置用于不同网络相关同步
         // m_Setup.m_Color = m_PlayerColor;
         //m_Setup.m_PlayerName = m_PlayerName;
        // m_Setup.m_PlayerNumber = m_PlayerNumber;
       // m_Setup.m_LocalID = m_LocalPlayerID;
    }


     //在游戏中玩家不能控制坦克的阶段使用。
    public void DisableControl()
    {
     // m_Movement.enabled = false;
      //m_Shooting.enabled = false;

    }


    // 用于游戏阶段，玩家应该能够控制他们的坦克。
    // public void EnableControl()
    //{
    //   m_Movement.enabled = true;
    //  m_Shooting.enabled = true;

    //    m_Movement.ReEnableParticles();
    // }

    public string GetName()
    {
        return m_Setup.m_PlayerName;
    }

    public void SetLeader(bool leader)
    { 
        m_Setup.SetLeader(leader);
    }

    public bool IsReady()
    {
        return m_Setup.m_IsReady;
    }

    // 在每轮开始时使用，以将坦克置于默认状态。
    // public void Reset()
    //{
    // m_Movement.SetDefaults();
    // m_Shooting.SetDefaults();
    // m_Health.SetDefaults();

    //  if (m_Movement.hasAuthority)
    //   {
    //      m_Movement.m_Rigidbody.position = m_SpawnPoint.position;
    //      m_Movement.m_Rigidbody.rotation = m_SpawnPoint.rotation;
    //  }
    //}
}
