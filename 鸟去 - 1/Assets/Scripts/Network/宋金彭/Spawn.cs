/*
 * 卡牌和乌贼是在边界范围内直接出现
 * 其他动物是从边界左边或右边进来
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Spawn : NetworkBehaviour
{
    [SerializeField]
    float InternalMin = 0.5f, InternalMax = 2;
    [SerializeField]
    int MaxStaticCount = 5, MaxControllableCount = 5;

    //动物预设
    public GameObject Whale;
    public GameObject Weapon;
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


    // 在左右边界之外固定距离处生成回溯可移动物体（轻石头、乌贼、锯鳐等【确保 BorthLine > 回溯的时间内物体移动的距离（以免出现不合理的情况）】
    float BorthLine = 10f;
    // 删去边界之外固定距离的物体【确保 DeathLine > BorthLine（以免生成后即刻被删）】
    float DeathLine = 15f;

    // 鲸的父节点
    [SerializeField]
    Transform background;



    // 动物的z=2，卡牌z=1，保证卡牌在动物上层
    GameObject MakeThings(GameObject prefab, float minX, float maxX)//联机生成动物、技能卡
    {

        GameObject ob = Instantiate(prefab, Global.MYNetwork) as GameObject;
        ob.transform.position = new Vector3(Random.Range(minX, maxX), Random.Range(-Global.BGHight, Global.BGHight), ob.transform.position.z);
        NetworkServer.Spawn(ob);    //在所有客户端
        return ob;
    }
    GameObject MakeThings(GameObject prefab, float x)
    {
        GameObject ob = Instantiate(prefab, Global.MYNetwork) as GameObject;
        ob.transform.position = new Vector3(x, Random.Range(-Global.BGHight, Global.BGHight), ob.transform.position.z);
        NetworkServer.Spawn(ob);    //在所有客户端
        return ob;
    }


    /// <summary>
    /// 在右边出生点随机生成一个石头
    /// </summary>
    void MakeRocks()
    {
        MakeRocks(GameManager.RightBorder.position.x + BorthLine, GameManager.RightBorder.position.x + BorthLine);
    }
    /// <summary>
    /// 在x坐标范围内随机生成一个石头
    /// </summary>
    void MakeRocks(float minX, float maxX)
    {
        const int CntRocks = 5;
        int i = Random.Range(0, CntRocks);
        GameObject rockPref;
        //rockPref = (GameObject)GameObject.Instantiate(Resources.Load("RocksNetwork\\石块们_" + i), Global.MYNetwork);
        rockPref = (GameObject)Resources.Load("RocksNetwork\\石块们_" + i);
        GameObject rock = MakeThings(rockPref, minX, maxX);
        GameManager.StaticObjects.Add(rock);


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

    void MakeWeapon()
    {
        // 出生点在左边或右边
        float borthX = Random.value < 0.5? GameManager.LeftBorder.position.x - BorthLine: GameManager.RightBorder.position.x + BorthLine;
        // 如果出生点在左边则必须正向朝向且速度超过卷屏速度
        if (borthX == GameManager.LeftBorder.position.x - BorthLine)
        {
            GameObject weapon = MakeThings(Weapon, borthX);
            weapon.GetComponent<WeaponNetwork>().Reverse = false;
            weapon.GetComponent<WeaponNetwork>().MoveSpeed = Global.gameManager.cameraMoveSpeed * Random.Range(1.2f, 1.8f); // 要超过卷屏速度
        }
        else
        {
            //GameObject weapon = MakeThings(Weapon, borthX);
            //weapon.GetComponent<WeaponNetwork>().Reverse = Random.value < 0.5 ? false : true;
            //weapon.GetComponent<WeaponNetwork>().MoveSpeed = Global.gameManager.cameraMoveSpeed * Random.Range(0, 0.5f); // 要超过卷屏速度

            GameObject ob = Instantiate(Weapon, Global.MYNetwork) as GameObject;
            ob.transform.position = new Vector3(borthX, Random.Range(-Global.BGHight, Global.BGHight), ob.transform.position.z);
            ob.GetComponent<WeaponNetwork>().Reverse = Random.value < 0.5 ? false : true;
            NetworkServer.Spawn(ob);    // 同步变量的赋值必须放在Spawn之前！
            ob.GetComponent<WeaponNetwork>().MoveSpeed = Global.gameManager.cameraMoveSpeed * Random.Range(0, 0.5f); // 要超过卷屏速度
        }
    }

    // 须放到背景上
    void MakeWhale()
    {
        // 出生点在左边或右边
        float borthX = Random.value < 0.5 ? GameManager.LeftBorder.position.x - BorthLine : GameManager.RightBorder.position.x + BorthLine;
        // 如果出生点在左边则必须正向朝向且速度超过卷屏速度
        GameObject whale = null;
        if (borthX == GameManager.LeftBorder.position.x - BorthLine)
        {
            //whale = MakeThings(Whale, borthX);
            //whale.GetComponent<WhaleNetwork>().Reverse = false;
            //whale.GetComponent<WhaleNetwork>().MoveSpeed = Global.gameManager.cameraMoveSpeed * Random.Range(1.2f, 1.8f); // 要超过卷屏速度

            GameObject ob = Instantiate(Whale, Global.MYNetwork) as GameObject;
            ob.transform.position = new Vector3(borthX, Random.Range(-Global.BGHight, 0), ob.transform.position.z); // 下半屏
            ob.GetComponent<WhaleNetwork>().Reverse = false;
            NetworkServer.Spawn(ob);
            ob.GetComponent<WhaleNetwork>().MoveSpeed = Global.gameManager.cameraMoveSpeed * Random.Range(1.2f, 1.8f); // 要超过卷屏速度
        }
        else
        {
            //whale = MakeThings(Whale, borthX);
            //whale.GetComponent<WhaleNetwork>().Reverse = Random.value < 0.5 ? false : true;
            //whale.GetComponent<WhaleNetwork>().MoveSpeed = Global.gameManager.cameraMoveSpeed * Random.Range(0, 0.5f); // 要超过卷屏速度

            GameObject ob = Instantiate(Whale, Global.MYNetwork) as GameObject;
            ob.transform.position = new Vector3(borthX, Random.Range(-Global.BGHight, Global.BGHight), ob.transform.position.z);
            ob.GetComponent<WhaleNetwork>().Reverse = Random.value < 0.5 ? false : true;
            NetworkServer.Spawn(ob);
            ob.GetComponent<WhaleNetwork>().MoveSpeed = Global.gameManager.cameraMoveSpeed * Random.Range(0, 0.5f);
        }
//        whale.transform.SetParent(background);
//        print(whale.transform.parent);
    }

    void MakeCoral()
    {
        GameObject ob = MakeThings(Coral, GameManager.LeftBorder.position.x, GameManager.RightBorder.position.x);
    }
    void MakeSkill(GameObject prefab)
    {
        GameObject ob = MakeThings(prefab, GameManager.LeftBorder.position.x, GameManager.RightBorder.position.x);
        GameManager.StaticObjects.Add(ob);
    }

    //void OuterMake(GameObject make)
    //{
    //    if (make == Rock)
    //    {
    //        MakeRocks();
    //    }
    //}

    //[ServerCallback]
    void Start()
    {
        //background = GameObject.Find("bg3").transform;
        // 静态变量会保留上次游戏中的数据，需要主动清除
        GameManager.ControllableObjects.Clear();
        


        if (isServer)
        {
            GameManager.StaticObjects.Clear();

            // 在边界范围内生成石头
//            for (int i = 0; i < 3; i++)
//                MakeRocks(GameManager.LeftBorder.position.x, GameManager.RightBorder.position.x + BorthLine);
#if(true)
            StartCoroutine(RandomMake());
#else
            // Debug 测试乌贼刚出生就倒流；被边界销毁后马上倒流
            //InnerMake(Coral);
            //var coral = MakeThings(Coral, (GameManager.LeftBorder.position.x + GameManager.RightBorder.position.x) / 2);

            //MakeThings(Coral, GameManager.LeftBorder.position.x - BorthLine);
            //MakeThings(Coral, GameManager.LeftBorder.position.x - BorthLine);
            //MakeSkill(TurtleSkill);
            //MakeSkill(TurtleSkill);
            //MakeSkill(TurtleSkill);
            //MakeSkill(TurtleSkill);
            //MakeSkill(TurtleSkill);
            //MakeSkill(TurtleSkill);


#endif
        }
    }

    /// <summary>
    /// 每间隔随机时间生成一个物体
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomMake()
    {
        // 石头组（空对象，只是指代作用）
        GameObject Rocks = null;

        // 生成的物体及其概率比例
        // 生成点在边界外的
        List<GameObject> outerThings = new List<GameObject> { Rocks, Whale, Weapon, Coral };
        // 生成点在边界内的
        List<GameObject> innerThings = new List<GameObject> { WhaleSkill, WeaponSkill, CoralSkill, TurtleSkill };
        GameObject[] things = new GameObject[] {Rocks, Whale, Weapon, Coral, WhaleSkill, WeaponSkill, CoralSkill, TurtleSkill};
        List<float> probability = new List<float> { 2, 2, 2, 2, 1, 1, 1, 1 };
        //List<float> probability = new List<float> { 0, 0, 0, 10, 0, 0, 0, 0 };



        // 计算轮盘带
        float[] probabilityBand = new float[probability.Count];
        float sum = probability.Sum();
        //for(int i = 0; i < probability.Length; i++)
        //{
        //    sum += probability[i];
        //}
        probabilityBand[0] = probability[0] / sum;
        for (int i = 1; i < probability.Count; i++)
        {
            probabilityBand[i] = probabilityBand[i - 1] + probability[i] / sum;
        }

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(InternalMin, InternalMax));

            if (GameManager.StaticObjects.Count > MaxStaticCount || GameManager.ControllableObjects.Count > MaxControllableCount)
                continue;
            float rand = Random.value;
            GameObject make = null;
            for(int i = 0; i < probabilityBand.Length; i++)
            {
                if(rand < probabilityBand[i])
                {
                    make = things[i];
                    break;
                }
            }
            if (make == Rocks)
                MakeRocks();
            else if (make == Whale)
                MakeWhale();
            else if (make == Weapon)
                MakeWeapon();
            else if (make == Coral)
                MakeCoral();
            else
                MakeSkill(make);
        }
    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
        if (Global.Player.GetComponent<PlayerEventsNetwork>() == null)
            return;  

        foreach(var obj in GameManager.ControllableObjects)
        {
            // 乌贼是上下通屏的（提高乌贼的灵活性）
            if (obj.name == "CoralNetwork(Clone)")
            {
                if (obj.transform.position.y < -Global.BGHight)
                    obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y + 2 * Global.BGHight, obj.transform.position.z);
                else if (obj.transform.position.y > Global.BGHight)
                    obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y - 2 * Global.BGHight, obj.transform.position.z);
            }

            // 删除可控对象
            if (! obj.activeSelf ||
                obj.transform.position.x - GameManager.RightBorder.position.x >= DeathLine ||
                GameManager.LeftBorder.position.x - obj.transform.position.x >= DeathLine)
            {
                //if (obj.GetComponent<ITraceable>() != null)
                //    Global.MYNetwork/*.Find("MY0")*/.GetComponent<MYBackInTimeNetwork>().removeFromBackInTime(obj);

                //GameManager.ControllableObjects.Remove(obj);
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
    }
}
