using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour {

    
    public Text StarText;                //得星星数目
    public GameObject Panel_Success;    //闯关成功panel
    public GameObject Panel_Choose;     //选关panel
    [HideInInspector]
    public int Num = 0;
    void Awake()
    {
        Num = 0;
        StarText.text = Num.ToString("0");  //开始获得星星数为0
    }
    public void AddStart()    //星星数+1，并显示在UI上
    {
        Num++;
        StarText.text = Num.ToString("0");//控制UI上星星数，并以整数形式显示
        if(Num==3)
        {
            //等视频播放完成（需加判断条件），

            //显示面板
            Panel_Success.SetActive(true);  
        }
    }
    public int chosen;       //输入所选关卡代号
    public int next;         //输入下个关卡代号
    public int replayScene;  //输入重玩的此关代号
    public void ToChoose()    //显示选关Panel
    {
        Panel_Success.SetActive(false);
        Panel_Choose.SetActive(true);
    }
    public void ToNext()
    {
        Panel_Success.SetActive(false);
        SceneManager.LoadScene("1");
    }
    public void replay()   //重玩此关
    {
        Panel_Success.SetActive(false);
        SceneManager.LoadScene("0");
    }
    public void chooseScene()  //加载关卡1
    {
        Panel_Choose.SetActive(false);
        SceneManager.LoadScene("1");
    }
}
