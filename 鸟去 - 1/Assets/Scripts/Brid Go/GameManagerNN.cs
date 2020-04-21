using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerNN : MonoBehaviour {
    public float Time;//记录已经度过时间
    public Slider SD;//进度条
    public float BeforeTime = -1; //记录上次的倒带后的开始时间
    private float MaxTime;//记录倒带时的未时间
    public float StopTime;//用来控制暂停时的时间
    public bool IsStop;
    bool BackTimeKey = false;//用来控制部分不必要slider触发。
    public int ContrlTimes = 0;//用来控制一次暂停能倒流的次数
    GameObject UIRoot;
    public int BackTimes = 3;//用来控制一局能倒流的次数
    UIManager uiManager;
    GameObject MYBackTime;
    public void Awake()
    {
        Global.UI = GameObject.Find("UI Root");
        Global.UI.SetActive(true);
        uiManager = Global.UI.GetComponent<UIManager>();

        if (SceneManager.GetActiveScene().name == "Game Network")
        {
            Global.InputNetwork = GameObject.Find("InputNetwork");
            Global.gameManager = GameObject.Find("GameManagerNetwork").GetComponent<GameManager>();
        }
        else
        {
            Global.Input = GameObject.Find("Input");
        }

        //Global.SceneEvent = this;
        if (SceneManager.GetActiveScene().name != "Game Network")
        {
            string current = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString("Scene", current);
            string farest = PlayerPrefs.GetString("FarestScene");
            if (Global.SceneName.IndexOf(farest) < Global.SceneName.IndexOf(current))
                PlayerPrefs.SetString("FarestScene", current);
        }



        GlobalNN.PSTool = GameObject.Find("PSTool");
        GlobalNN.GameManagerNN = GameObject.Find("GameManagerNN");
    }
    // Use this for initialization
    void Start () {
        MYBackTime = GameObject.Find("MYBackTime");
        Invoke("AddTime", 1f);
        UIRoot = GameObject.Find("UI Root");
    }
    public void AddTime() {//递归协程计时器
        
        //Debug.Log(Time);

        if (!IsStop)
        {
            Time++;
            SD.value = Time * 0.01f;
            Invoke("AddTime", 1f);
        }
        else
        {
            StopTime = Time;
            SD.value = StopTime * 0.01f;
        }
        
            
    }
    public void BackTime() {//倒带的时间段只能是 上次倒流之后，现在时间之前
        if (BackTimeKey)
        {
            BackTimeKey = false;
            return;
        }
        if (!UIRoot.GetComponent<UIManager>().Pause)//解决了游戏时，slider的时间变化引起的倒流
            return;
        if ((BeforeTime != -1&& SD.value * 100 <= BeforeTime) || SD.value * 100 >= Time|| ContrlTimes > 2|| BackTimes < 1)//分别控制，不能再上次倒流开始之前，不能在未来的时间，不能超过一局或者一个暂停内的限制倒流次数
        {
            BackTimeKey = true;
            SD.value = Time * 0.01f;
            return;
        }
        
        float time = Time - SD.value * 100;
        time = time * 10;
        MYBackTime.GetComponent<MYBackInTime>().total = (int)time;
        //UIRoot.GetComponent<UIManager>().ClickToBack();//
        
        if (time < 0.1f)//解决了暂停恢复游戏一瞬间时，时间的改变引起的一次倒流,而导致的其他多函数变化影响
            return;
       
        MYBackTime.GetComponent<MYBackInTime>().OnBackInTime();
        Time = SD.value*100;

        ContrlTimes++;

        //Debug.Log("chang");
        //BeforeTime = Time;
    }
    // Update is called once per frame
    void Update () {
    
    }
}
