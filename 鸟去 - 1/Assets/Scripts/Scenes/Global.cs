///
// 主要存放需要跨场景保存的数据
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global {

    static public float BGHight;                    // 背景图片的半高
    static public float BGWidth;                    // 背景图片的半宽
    static public float CamHight;                   // 镜头的半高
    static public float CamWidth;                   // 镜头的半宽

    static public GameObject UI;
    static public GameObject Input;
    static public GameObject InputNetwork;
    static public GameObject Player;
    static public GameObject PlayerHidden;  // 指向联机时被隐藏起来的原单机Player
    //static public SceneEventManager SceneEvent;

    //static public string[] SceneName = new string[] { "Game1", "Game2", "Game3", "Game5"};   //关卡场景名
    static public List<string> SceneName = new List<string> { "Game1", "Game2", "Game3", "Game5" };   //关卡场景名

    static public int LevelIndex;   // 关卡序号
    static public int LevelVideoIndex; // 0表示关头CG；1表示关底CG（影响CG播完后是否跳转场景）
    static public int MyIndex;

    static public int lastDeadMy; //上次死亡所在谜题序号
    static public string lastDeadScene; //上次死亡场景
    static public int deadCount;    //连续死亡次数

    //static public List<GameObject> AllPlayers = new List<GameObject>();
    static public Transform MYNetwork;// "Mysterys/MY0"
    static public GameManager gameManager;
    static public PlayerEventsNetwork Net;  // 连机的[Command]函数的“前缀”
    
}
