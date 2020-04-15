using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalNN  {

    static public float BGHight;           //背景图片的半高
    static public float BGWidth;           //背景图片的半宽
    static public float CamHight;          //镜头的半高
    static public float CamWidth;
    static public GameObject UIPause;      //暂停播放UI
    static public GameObject GameManagerNN;    //游戏管理
    static public GameObject PSTool;       //PS工具
    static public List<string> SceneName = new List<string> { "Game1", "Game2", "Game3", "Game5" }; //关卡场景名
    static public int LevelIndex;          //关卡序号
    static public int MyIndex;
    static public int lastDeadMy;         //上次游戏所在谜题序号
    static public string lastDeadScene;   //上次游戏场景
}
