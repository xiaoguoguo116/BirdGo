using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushDark : MonoBehaviour {

    const int CntRocks = 5;
    CameraFollow cameraFollow;
    GameObject Player;

    //[SerializeField]
    //float Distance = 500;
    [SerializeField]
    [Tooltip("是否提速并来回移动物体")]
    bool IsSpeedUp = false;
    [SerializeField]
    float PlayerAutoSpeed = 4;    // Player自动速度
    float PlayerSpeed = 0;  // 速度变化过程
    [SerializeField]
    [Tooltip("一般为 PlayerAutoSpeed + 1")]
    float CameraMoveSpeed = 5;

    [SerializeField]
    float InternalMin = 1, InternalMax = 3;
    float CurInternal;
    [SerializeField]
    int RockMaxCnt = 5, SquidMaxCnt = 3;
    LinkedList<GameObject> listRock = new LinkedList<GameObject>();
    LinkedList<GameObject> listSquid = new LinkedList<GameObject>();

    [SerializeField]
    RuntimeAnimatorController controller1, controller2;  //动画


    // Use this for initialization
    void Start() {
        Player = Global.Player;

        if (IsSpeedUp)
        {
            // 龟游泳动画循环
            Player.GetComponent<Animator>().runtimeAnimatorController = controller2;
            // 卷屏速度都加快
            cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        }

        CurInternal = Random.Range(InternalMin, InternalMax);
    }

    IEnumerator SpeedUp()
    {
        while(cameraFollow.cameraMoveSpeed < CameraMoveSpeed - 1e-4)
        {
            cameraFollow.cameraMoveSpeed = Mathf.Lerp(cameraFollow.cameraMoveSpeed, CameraMoveSpeed, Time.deltaTime);
            PlayerSpeed = Mathf.Lerp(PlayerSpeed, PlayerAutoSpeed, Time.deltaTime);
            yield return null;
        }
    }

    bool isStart = true;
    // Update is called once per frame
    void Update()
    {
        // 逐渐提速
        if (IsSpeedUp)
        {
            if (isStart)
            {
                isStart = false;
                StartCoroutine("SpeedUp");
            }
            // 龟自动游泳速度
            Player.transform.position = new Vector2(Player.transform.position.x + PlayerSpeed * Time.deltaTime, Player.transform.position.y);
        }

        // 随机生成障碍：由随机时间间隔生成
        CurInternal -= Time.deltaTime;
        if (CurInternal <= 0)
        {
            if (listRock.Count < RockMaxCnt)
            {
                CurInternal = Random.Range(InternalMin, InternalMax);
                int i = Random.Range(0, CntRocks * 2);
                GameObject rock;
                if (i < CntRocks)
                    rock = (GameObject)GameObject.Instantiate(Resources.Load("RocksDark\\石块们_" + i)/*, transform*/);
                else
                    rock = (GameObject)GameObject.Instantiate(Resources.Load("RocksDark\\长条石块_3")/*, transform*/);
                rock.transform.localScale = Random.Range(0.6f, 1f/*1.2f*/) * Vector2.one;
                rock.transform.localScale += new Vector3(0, 0, 1f);
                rock.transform.position = new Vector2(Player.transform.position.x + Random.Range(Global.CamWidth * 2.5f, Global.CamWidth * 3),
                    Random.Range(-Global.BGHight, Global.BGHight)
                    );

                listRock.AddLast(rock);

                if (IsSpeedUp)
                {
                    // 石头位移动画
                    Hashtable args = new Hashtable();
                    args.Add("easeType", iTween.EaseType.linear/*.easeInOutExpo*/);
                    args.Add("loopType", "pingPong");
                    //args.Add("loopType", "loop");
                    args.Add("time", 2f);
                    args.Add("x", Random.Range(-Global.BGHight / 4f, Global.BGHight / 4f));
                    args.Add("y", Random.Range(-Global.BGHight / 1f, Global.BGHight / 1f));
                    iTween.MoveBy(rock, args);
                }
            }

            // 生成乌贼
            if (listSquid.Count < SquidMaxCnt)
            {
                if (Random.Range(0f, 1f) < 0.5)
                {
                    GameObject squid = (GameObject)GameObject.Instantiate(Resources.Load("Animals\\Coral")/*, transform*/);
                    squid.transform.position = new Vector2(Player.transform.position.x + Random.Range(Global.CamWidth * 1.5f, Global.CamWidth * 2),
                        Random.Range(-Global.BGHight, Global.BGHight)
                        );
                    listSquid.AddLast(squid);
                    //print(listSquid.Count);
                }
            }

        }

        // 删除身后的物体
        GameObject DestroyObj = null;
        foreach (GameObject ob in listRock)
        {
            if (ob.transform.position.x < Player.transform.position.x - Global.CamWidth * 2)
            {
                DestroyObj = ob;
                break;
            }
        }
        if (DestroyObj)
        {
            listRock.Remove(DestroyObj);
            Destroy(DestroyObj);
        }
        DestroyObj = null;
        foreach (GameObject ob in listSquid)
        {
            if (ob.transform.position.x < Player.transform.position.x - Global.CamWidth * 1.5)
            {
                DestroyObj = ob;
                break;
            }
        }
        if (DestroyObj)
        {
            listSquid.Remove(DestroyObj);
            Destroy(DestroyObj);
            //print("remove" + listSquid.Count);
        }
    }

    private void OnDestroy()
    {
        if (IsSpeedUp)
        {
            if (Player) // 如果是场景退出，会为空
            {
                Player.GetComponent<Animator>().runtimeAnimatorController = controller1;
                Player.GetComponent<PlayerEvents>().SpeedDown(PlayerSpeed);
            }
        }
        
    }

}
