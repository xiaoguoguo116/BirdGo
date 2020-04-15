using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushFall : MonoBehaviour
{

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
    private GameObject[] FallStones;
    private MYBackInTime mYBackInTime;
    //[SerializeField]
    GameObject coral;
    private bool collided;
    private int stoneIndex = 0;
    // Use this for initialization

    private void Awake()
    {
        mYBackInTime = gameObject.GetComponentInParent<MYBackInTime>();
        FallStones = new GameObject[gameObject.GetComponentsInChildren<Transform>().Length-1];
        for(int i = 1; i < gameObject.GetComponentsInChildren<Transform>().Length; ++i)
        {
            mYBackInTime.addToBackInTime(gameObject.GetComponentsInChildren<Transform>()[i].gameObject);
            FallStones[i-1] = gameObject.GetComponentsInChildren<Transform>()[i].gameObject;
            gameObject.GetComponentsInChildren<Transform>()[i].gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void Start()
    {
        collided = false;
        

    }

    IEnumerator SpeedUp()
    {
        while (cameraFollow.cameraMoveSpeed < CameraMoveSpeed - 1e-4)
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
        if(collided)
        {
            if (isStart)
            {
                isStart = false;
                StartCoroutine("SpeedUp");
            }
            // 龟自动游泳速度
            Player.transform.position = new Vector2(Player.transform.position.x + PlayerSpeed * Time.deltaTime, Player.transform.position.y);
            coral.transform.position = new Vector2(coral.transform.position.x + PlayerSpeed * Time.deltaTime, coral.transform.position.y);
            // 随机生成障碍：由随机时间间隔生成
            CurInternal -= Time.deltaTime;
            if (CurInternal <= 0 && stoneIndex < FallStones.Length)
            {
                CurInternal = Random.Range(InternalMin, InternalMax);
                //if (i < CntRocks)
                //FallStone = (GameObject)GameObject.Instantiate(Resources.Load("FallStones\\FallStone_" + i.ToString())/*, transform*/);
                FallStones[stoneIndex].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                FallStones[stoneIndex].GetComponent<Rigidbody2D>().angularVelocity = Random.Range(0f, 360f) ;
                //else
                //FallStone = (GameObject)GameObject.Instantiate(Resources.Load("Rocks\\长条钢筋_3")/*, transform*/);
                FallStones[stoneIndex].transform.localScale = Random.Range(0.6f, 1f/*1.2f*/) * Vector3.one;
                FallStones[stoneIndex].transform.position = new Vector2(Player.transform.position.x + Random.Range(Global.CamWidth * 1f, Global.CamWidth * 3f),
                    Random.Range(Global.BGHight * 1.5f, Global.BGHight * 3f)
                    );
                stoneIndex++;
                //FallStone.transform.SetParent(GameObject.Find("B&W/Rocks").transform);

            }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !collided)
        {
            Player = Global.Player;

            // 龟游泳动画循环
            Player.GetComponent<Animator>().runtimeAnimatorController = controller2;
            // 卷屏速度都加快
            cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
            //        StartCoroutine("SpeedUp");
            //cameraFollow.cameraMoveSpeed = CameraMoveSpeed;

            CurInternal = Random.Range(InternalMin, InternalMax);
            isStart = true;
            collided = true;

            coral = GameObject.FindGameObjectWithTag("Squid");
            mYBackInTime.removeFromBackInTime(coral);
        }
    }

}
