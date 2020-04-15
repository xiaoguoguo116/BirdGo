using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rush : MonoBehaviour {

    const int CntRocks = 5;
    CameraFollow cameraFollow;
    GameObject Player;

    //[SerializeField]
    //float Distance = 500;
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
    RuntimeAnimatorController controller1, controller2;  //动画

    private MYBackInTime mYBackInTime;
    // Use this for initialization
    void Start () {        
        Player = Global.Player;

        // 龟游泳动画循环
        Player.GetComponent<Animator>().runtimeAnimatorController = controller2;
        // 卷屏速度都加快
        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
//        StartCoroutine("SpeedUp");
        //cameraFollow.cameraMoveSpeed = CameraMoveSpeed;

        CurInternal = Random.Range(InternalMin, InternalMax);
        mYBackInTime = gameObject.GetComponent<MYBackInTime>();
        isStart = true;
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
    void Update () {
        if(isStart)
        {
            isStart = false;
            StartCoroutine("SpeedUp");
        }
        // 龟自动游泳速度
        Player.transform.position = new Vector2(Player.transform.position.x + PlayerSpeed * Time.deltaTime, Player.transform.position.y);

        // 随机生成障碍：由随机时间间隔生成
        CurInternal -= Time.deltaTime;
        if (CurInternal <= 0)
        {            
            CurInternal = Random.Range(InternalMin, InternalMax);
            int i = Random.Range(0, CntRocks * 2);
            GameObject rock;
            if (i < CntRocks)
            {
                rock = (GameObject)GameObject.Instantiate(Resources.Load("Rocks\\石块们_" + i)/*, transform*/);
                rock.transform.localScale = Random.Range(0.5f, 0.8f/*1.2f*/) * Vector2.one;
            }
            else
            {
                rock = (GameObject)GameObject.Instantiate(Resources.Load("Rocks\\长条钢筋_3")/*, transform*/);
                rock.transform.localScale = Random.Range(0.4f, 0.6f/*1.2f*/) * Vector2.one;
            }

            rock.transform.position = new Vector2(Player.transform.position.x + Random.Range(Global.CamWidth * 2.5f, Global.CamWidth * 3),
                Random.Range(-Global.BGHight, Global.BGHight)
            );
            //rock.transform.SetParent(GameObject.Find("B&W/Rocks").transform);
            if (mYBackInTime)
                mYBackInTime.addToBackInTime(rock);
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

    private void OnDestroy()
    {
        if (Player)
        {
            Player.GetComponent<Animator>().runtimeAnimatorController = controller1;
            Player.GetComponent<PlayerEvents>().SpeedDown(PlayerSpeed);
        }    
    }
}
