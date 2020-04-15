using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerNN : MonoBehaviour {
    public float Time;//记录已经度过时间
    public Slider SD;//进度条
    public float BeforeTime = -1; //记录上次的倒带后的开始时间
    private float MaxTime;//记录倒带时的未时间
    bool BackTimeKey = false;//用来控制部分不必要slider触发。
    public int ContrlTimes = 0;//用来控制一次暂停能倒流的次数
    GameObject UIRoot;
    public int BackTimes = 3;//用来控制一局能倒流的次数
    public void Awake()
    {
        GlobalNN.PSTool = GameObject.Find("PSTool");
       
        GlobalNN.GameManagerNN = GameObject.Find("GameManagerNN");
    }
    // Use this for initialization
    void Start () {
        Invoke("AddTime", 1f);
        UIRoot = GameObject.Find("UI Root");
    }
    void AddTime() {//递归协程计时器
        Time++;
        //Debug.Log(Time);
        SD.value = Time * 0.01f;
        Invoke("AddTime",1f);
    }
    public void BackTime() {//倒带的时间段只能是 上次倒流之后，现在时间之前
        if (BackTimeKey)
        {
            BackTimeKey = false;
            return;
        }
        if (!UIRoot.GetComponent<UIManager>().Pause)
            return;
        if ((BeforeTime != -1&& SD.value * 100 <= BeforeTime) || SD.value * 100 >= Time|| ContrlTimes > 2|| BackTimes < 1)//分别控制，不能再上次倒流开始之前，不能在未来的时间，不能超过一局或者一个暂停内的限制倒流次数
        {
            BackTimeKey = true;
            SD.value = Time * 0.01f;
            return;
        }
        
        float time = Time - SD.value * 100;
        time = time * 10;
        Global.SceneEvent.GetCurMystery().GetComponent<MYBackInTime>().total = (int)time;
        //UIRoot.GetComponent<UIManager>().ClickToBack();//
        Global.SceneEvent.GetCurMystery().GetComponent<MYBackInTime>().OnBackInTime();
        Time = SD.value*100;

        ContrlTimes++;

        //Debug.Log("chang");
        //BeforeTime = Time;
    }
    // Update is called once per frame
    void Update () {

    }
}
