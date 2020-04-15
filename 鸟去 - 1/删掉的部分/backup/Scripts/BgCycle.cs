using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BgRefresh))]
public class BgCycle : MonoBehaviour
{
    //[SerializeField]
    //Transform target;//中心点
    [SerializeField]
    float speedRate;//背景移动速度比例 （0为静止 正为速度与飞机方向相同 负为速度与飞机方向相反）
    [SerializeField]
    int count = -1;//循环次数 负数无限循环 0不循环

    List<GameObject> backgrounds;//卷屏背景集合
    List<float> bgScales;//卷屏背景集合的尺寸
    float offset;//卷屏移动偏移量
    int firstLocation = 0;//当前卷屏的最左端背景在背景数组中的角码（背景数组不变）
    int lastLocation;//当前卷屏的最右端背景在背景数组中的角码。
    bool runtime = false;//处于编辑器(false)模式还是游戏运行模式(true)
    bool flag = true;//协程开关
    void Start()
    {
        //target = GameObject.FindGameObjectWithTag("Player").transform;
        initLocaltion();
        openRuntime();
        openCoroutine();
    }
    void Update()
    {

    }
    void openCoroutine()//打开协程
    {
        StartCoroutine(cycleBG());
        StartCoroutine(cycleCount());
        StartCoroutine(moveBG());
    }
    public bool isRuntime()//返回运行标志位
    {
        return runtime;
    }
    void openRuntime()//打开运行位
    {
        runtime = true;
    }
    public void bgRefresh()
    {
        initLocaltion();//对背景位置排列
    }
    void initLocaltion()//重新对背景位置排列
    {
        reCalculateBgData();
        float offset = 0f;//相对初始背景偏移量
        for (int i = 0; i < backgrounds.Count; i++)
        {
            if (i != 0)//第一个位置偏移初始为0，不用计算
            {
                offset += bgScales[i] / 2 + bgScales[i - 1] / 2;//计算偏移量
            }
            backgrounds[i].transform.localPosition = new Vector2(offset, 0f);//设置背景位置
        }
    }
    void reCalculateBgData()//重新计算背景集合和背景尺寸
    {
        backgrounds = new List<GameObject>();
        bgScales = new List<float>();
        int i = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject!=null)
            {
                backgrounds.Add(child.gameObject);
                bgScales.Add(calculateScales(i++));//计算背景尺寸
            }
        }
        calulateCycleOffset();
        i--;
        lastLocation = i;
    }
    void calulateCycleOffset()//计算出卷屏需要移动多少距离
    {
        offset = 0f;
        for (int i = 0; i < backgrounds.Count; i++)
        {
            offset += bgScales[i];
        }
    }
    float calculateScales(int i)//计算第i张背景图尺寸
    {
        if(backgrounds[i].GetComponent<SpriteRenderer>())
        {
            return backgrounds[i].transform.localScale.x * backgrounds[i].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }
        else
        {
            return 156.5f;
        }
    }

    IEnumerator cycleCount()//循环次数的监听
    {
        while (count > 0)
        {
            yield return null;
        }
        if (count == 0) closeCycle();
    }
    IEnumerator moveBG()//背景移动协程
    {
        if (Global.Player)
        {
            float before = Global.Player.transform.position.x;//上次位置
            float now;//本次位置
            float distance;//背景每帧前进的距离
            while (flag)
            {
                yield return null;
                if (Global.Player)
                {
                    now = Global.Player.transform.position.x;
                    distance = (now - before) * speedRate;
                    //transform.localPosition = new Vector3(transform.localPosition.x + distance, transform.localPosition.y, transform.localPosition.z);
                    transform.localPosition = new Vector3(transform.localPosition.x + distance, transform.localPosition.y, transform.localPosition.z);
                    before = now;
                }
            }
        }
        yield return null;
    }
    IEnumerator cycleBG()//背景循环协程
    {
        while (flag)
        {
            while (Global.Player && Global.Player.transform.position.x < backgrounds[firstLocation].transform.position.x)
                leftCycle();
            while (Global.Player && Global.Player.transform.position.x > backgrounds[lastLocation].transform.position.x)
                rightCycle();
            yield return null;
        }
        yield return null;
    }

    void closeCycle()//关闭循环卷屏
    {
        flag = false;
    }

    void leftCycle()//向左循环卷屏
    {
        // 0,1,2,3 =>
        // 3,0,1,2
        // F     L
        backgrounds[lastLocation].transform.localPosition = new Vector2(backgrounds[lastLocation].transform.localPosition.x - offset, 0f);
        firstLocation = lastLocation;
        lastLocation = (lastLocation + backgrounds.Count - 1) % backgrounds.Count;
        count--;
    }
    void rightCycle()//向右循环卷屏
    {
        // 0,1,2,3 =>
        // 1,2,3,0
        // F     L
        backgrounds[firstLocation].transform.localPosition = new Vector2(backgrounds[firstLocation].transform.localPosition.x + offset, 0f);
        lastLocation = firstLocation;
        firstLocation = (firstLocation + 1) % backgrounds.Count;
        count--;
    }
}
