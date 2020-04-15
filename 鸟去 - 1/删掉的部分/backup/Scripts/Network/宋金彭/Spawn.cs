/*
 * 卡牌和乌贼是在边界范围内直接出现
 * 其他动物是从边界左边或右边进来
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawn : NetworkBehaviour
{
    [SerializeField]
    float InternalMin = 0.5f, InternalMax = 2;

    //动物预设
    public GameObject Whale;
    public GameObject WheaponNetwork;
    public GameObject Coral;

    //石头
    public GameObject Rock;
    //落石
    public GameObject MY0;

    //技能卡预设
    public GameObject WhaleSkill;
    public GameObject CoralSkill;
    public GameObject WeaponSkill;
    public GameObject TurtleSkill;

    //旋转
    protected Quaternion m_rotation;
    //生成时间间隔
    float CurInternal = 0;
    private float m_timer1 = 0;
    private float m_timer2 = 2;

    // 在左右边界之外固定距离处生成回溯可移动物体（轻石头、乌贼、锯鳐等【确保 BorthLine > 回溯的时间内物体移动的距离（以免出现不合理的情况）】
    float BorthLine = 10f;
    // 删去边界之外固定距离的物体【确保 DeathLine > BorthLine（以免生成后即刻被删）】
    float DeathLine = 25f;

    //父节点
    public Transform Mysterys;

    // 生成物的Tag及概率
    string[] Tags = new string[] { "Whale", "Weapon", "Squid", "Skill(whale)", "Skill(cuttle)", "Skill(saw)", "Skill(Turtle)" };
    float[] Probability = new float[] { 1, 1, 1, 1, 1, 1, 1 };

    public void MakeThings(GameObject prefab, Vector3 position)//联机生成动物、技能卡
    {

            GameObject ob = Instantiate(prefab, position, Quaternion.identity) as GameObject;
            ob.transform.SetParent(Mysterys);
            NetworkServer.Spawn(ob);    //在所有客户端

            if(ob.GetComponent<ITraceable>() != null)
                Global.MYNetwork/*.Find("MY0")*/.GetComponent<MYBackInTimeNetwork>().addToBackInTime(ob);

    }

    /// <summary>
    /// 在x坐标范围内随机生成一个石头
    /// </summary>
    /// <param name="minX"></param>
    /// <param name="maxX"></param>
    void MakeRock(float minX, float maxX)
    {
        const int CntRocks = 5;

        
        int i = Random.Range(0, CntRocks);
        GameObject rock;
        rock = (GameObject)GameObject.Instantiate(Resources.Load("RocksNetwork\\石块们_" + i), Mysterys);
//        rock.transform.localScale = Random.Range(0.5f, 0.8f/*1.2f*/) * Vector2.one;   // 【Spawn无法修改 parent 和 localScale 】
        rock.transform.position = new Vector3(/*GameManager.RightBorder.position.x + BorthLine*/Random.Range(minX, maxX), Random.Range(-Global.BGHight, Global.BGHight), 2);
        NetworkServer.Spawn(rock);
        GameManager.StaticObjects.Add(rock);


        //rock.transform.SetParent(GameObject.Find("B&W/Rocks").transform);
        //if (mYBackInTime)
        //    mYBackInTime.addToBackInTime(rock);

        ////{{ 石头微动
        //Hashtable args = new Hashtable();
        //args.Add("easeType", iTween.EaseType.linear/*.easeInOutExpo*/);
        //args.Add("loopType", "pingPong");
        //args.Add("time", 2f);
        //args.Add("x", Random.Range(-Global.BGHight / 4f, Global.BGHight / 4f));
        //args.Add("y", Random.Range(-Global.BGHight / 4f, Global.BGHight / 4f));
        //iTween.MoveBy(rock, args);
        ////}}
    }

    //[ServerCallback]
    void Start()
    {
        // 静态变量会保留上次游戏中的数据，需要主动清除
        GameManager.ControllableObjects.Clear();
        GameManager.StaticObjects.Clear();
        //Mysterys = GameObject.Find("Mysterys").transform;



        if (isServer)
        {
            // 在边界范围内生成石头
            for (int i = 0; i < 5; i++)
                MakeRock(GameManager.LeftBorder.position.x, GameManager.RightBorder.position.x + BorthLine);

            //*{{ Debug
            if (Global.Player.name == "Player")     // 【nn】此句为何？
                return;

            // 动物的z=2，卡牌z=1，保证卡牌在动物上层
            Vector3 position = new Vector3(GameManager.RightBorder.position.x - 20, (float)(Random.Range(-1f, 0) * 9), 2);
            //MakeThings(Whale, position);
            MakeThings(WheaponNetwork, position);
            MakeThings(Coral, position);
            

            position = new Vector3(GameManager.RightBorder.position.x - 20, (float)(Random.Range(-1f, 1f) * 8), 1);
            //MakeThings(WhaleSkill, position);
            MakeThings(WeaponSkill, position);
            //MakeThings(CoralSkill, position);

            position = new Vector3(GameManager.RightBorder.position.x - 20, (float)(Random.Range(-1f, 1f) * 8), 1);
            MakeThings(TurtleSkill, position);

            //*/
        }
    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {

        
        // 随机生成岩石：由随机时间间隔生成
        CurInternal -= Time.deltaTime;
        if (CurInternal <= 0)
        {
            CurInternal = Random.Range(InternalMin, InternalMax);
            MakeRock(GameManager.RightBorder.position.x + BorthLine, GameManager.RightBorder.position.x + BorthLine);

        }



        if (Global.Player.GetComponent<PlayerEventsNetwork>() == null)
            return;  
        // 删除可控对象
        foreach(var obj in GameManager.ControllableObjects)
        {
            if (obj.transform.position.x - GameManager.RightBorder.position.x >= DeathLine ||
                GameManager.LeftBorder.position.x - obj.transform.position.x >= DeathLine)
            {
                if (obj.GetComponent<ITraceable>() != null)
                    Global.MYNetwork/*.Find("MY0")*/.GetComponent<MYBackInTimeNetwork>().removeFromBackInTime(obj);

                GameManager.ControllableObjects.Remove(obj);
                NetworkServer.Destroy(obj);
                break;  // foreach一次循环只能删除一个
            }
        }
        // 删除静态对象
        foreach (var obj in GameManager.StaticObjects)
        {
            if (obj.transform.position.x - GameManager.RightBorder.position.x >= DeathLine ||
                GameManager.LeftBorder.position.x - obj.transform.position.x >= DeathLine)
            {
                GameManager.StaticObjects.Remove(obj);
                NetworkServer.Destroy(obj);
                break;  // foreach一次循环只能删除一个
            }
        }

        /*
        if (Global.Player.name == "Player")     // 【nn】此句为何？
            return;
        if (!Global.Player.GetComponent<PlayerEventsNetwork>().isServer)
            return;
        m_timer1 -= Time.deltaTime;
        if (m_timer1 <= 0)//左锯鳐
        {
            m_timer1 = Random.Range(6f, 10f);
            if (m_timer1 > 9f)
            {
                Transform obj = WheaponNetworkL.transform;
                obj.position = new Vector3(Global.Player.GetComponent<PlayerEventsNetwork>().left - 12, m_transform.transform.position.y + (float)(Random.Range(-1f, 1f) * 8), 0);
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletTrans1 = obj;
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletPre1 = WheaponNetworkL;
                Global.Player.GetComponent<PlayerEventsNetwork>().CmdMakeThings();
            }
        }
        m_timer2 -= Time.deltaTime;
        if (m_timer2 <= 0)
        {
            m_timer2 = Random.Range(5f, 8f);
            float mul2 = Random.value * 150;

            // 生成动物
            if (mul2 > 0 && mul2 <= 30)//右锯鳐
            {
                Transform obj = WheaponNetworkR.transform;
                obj.position = new Vector3(Global.Player.GetComponent<PlayerEventsNetwork>().right + 15, m_transform.position.y + (float)(Random.Range(-1f, 1f) * 8), 1);
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletTrans1 = obj;
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletPre1 = WheaponNetworkR;
                Global.Player.GetComponent<PlayerEventsNetwork>().CmdMakeThings();
            }
            if (mul2 >= 30 && mul2 < 60)//鲸鱼
            {
                Transform obj = Whale.transform;
                obj.position = new Vector3(Global.Player.GetComponent<PlayerEventsNetwork>().right + 15, m_transform.position.y + (float)(Random.Range(-1f, 0) * 9), 1);
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletTrans1 = obj;
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletPre1 = Whale;
                Global.Player.GetComponent<PlayerEventsNetwork>().CmdMakeThings();
            }
            if (mul2 <= 60)//乌贼
            {
                Transform obj = Coral.transform;
                GameObject GameWall = GameObject.Find("GameWall");
                obj.position = new Vector3(GameWall.transform.position.x + (float)(Random.Range(-0.8f, 0.8f) * 13.3 / 2), m_transform.position.y + (float)(Random.Range(-0.8f, 0.8f) * 10), 1);
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletTrans1 = obj;
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletPre1 = Coral;
                Global.Player.GetComponent<PlayerEventsNetwork>().CmdMakeThings();
            }

            // 生成环境
            if (mul2 >= 60 && mul2 < 100)//石头
            {
                Transform obj = Rock.transform;
                obj.position = new Vector3(Global.Player.GetComponent<PlayerEventsNetwork>().right + 15, m_transform.position.y + (float)(Random.Range(-0.8f, 0.8f) * 10), 1);
                obj.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 1f) * 360f);
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletTrans1 = obj;
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletPre1 = Rock;
                Global.Player.GetComponent<PlayerEventsNetwork>().CmdMakeThings();
            }
            if (mul2 >= 100)//火石+锯鳐
            {
                Transform obj = MY0.transform;
                obj.position = new Vector3(Global.Player.GetComponent<PlayerEventsNetwork>().right -31, m_transform.position.y + 6, 1);
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletTrans1 = obj;
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletPre1 = MY0;
                Global.Player.GetComponent<PlayerEventsNetwork>().CmdMakeThings();
            }

            // 生成技能卡牌
            if (mul2 <= 60)//乌龟倒流
            {
                Transform obj = TurtleSkill.transform;
                obj.position = new Vector3((Global.Player.GetComponent<PlayerEventsNetwork>().right + 15), m_transform.position.y + (float)(Random.Range(-1f, 1f) * 8), 1);
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletTrans1 = obj;
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletPre1 = TurtleSkill;
                Global.Player.GetComponent<PlayerEventsNetwork>().CmdMakeThings();
            }
            if (mul2 > 60&& mul2 <= 90)//乌贼喷墨
            {
                Transform obj = CoralSkill.transform;
                obj.position = new Vector3((Global.Player.GetComponent<PlayerEventsNetwork>().right + 15), m_transform.position.y + (float)(Random.Range(-1f, 1f) * 8), 1);
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletTrans1 = obj;
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletPre1 = CoralSkill;
                Global.Player.GetComponent<PlayerEventsNetwork>().CmdMakeThings();
            }
            if (mul2 > 90 && mul2 <= 120)//锯鳐割绳
            {
                Transform obj = WeaponSkill.transform;
                obj.position = new Vector3((Global.Player.GetComponent<PlayerEventsNetwork>().right + 15), m_transform.position.y + (float)(Random.Range(-1f, 1f) * 8), 1);
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletTrans1 = obj;
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletPre1 = WeaponSkill;
                Global.Player.GetComponent<PlayerEventsNetwork>().CmdMakeThings();
            }
            if (mul2 > 120&& mul2 <=150)//鲸鱼喷水
            {
                Transform obj = WhaleSkill.transform;
                obj.position = new Vector3((Global.Player.GetComponent<PlayerEventsNetwork>().right + 15), m_transform.position.y + (float)(Random.Range(-1f, 1f) * 8), 1);
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletTrans1 = obj;
                Global.Player.GetComponent<PlayerEventsNetwork>().bulletPre1 = WhaleSkill;
                Global.Player.GetComponent<PlayerEventsNetwork>().CmdMakeThings();
            }
        }
        //*/
    }
}
