using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarNumberPanel : MonoBehaviour {

    public Text StarText;     //得星星数目的显示UI
    public int Num = 0;       //记录得星星数
    void Awake()
    {
        Num = 0;
        StarText.text = Num.ToString("0");  //开始获得星星数为0
    }
    public void AddStart()    //星星数+1，并显示在UI上
    {
        Num++;
        StarText.text = Num.ToString("0");//控制UI上星星数，并以整数形式显示
        if (Num == 3)
        {
            //等视频播放完成（需加判断条件），

            //显示面板
 
        }
    }
}
