using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerEvents : TouchableBehaviour
{
    public Vector2 direction;
    [SerializeField]
    public float m_life = 10; //死亡旋转
    private string SceneName; //场景名称
    public float m_power = 10.0f;
    public Vector3 endPos;
    public float recordz;  //记录此时Z值
    bool isDead;
    
    private Rigidbody2D rigbody2d;
    // Use this for initialization
    void Start()
    {
        rigbody2d = GetComponent<Rigidbody2D>();
        GetScene();//调用获取场景方法
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {     
        recordz = this.gameObject.transform.rotation.z;

        if (!isDead && m_life <= 0)
        {
            isDead = true;
            gameObject.GetComponent<Animator>().SetTrigger("dead");//SetBool("Dead", true);
            Global.Input.SetActive(false);
            transform.GetComponent<CapsuleCollider2D>().enabled = false;

            transform.parent = Camera.main.transform;
            iTween.RotateTo(gameObject, new Vector3(0, 0, 180), 3);
            float w = gameObject.GetComponent<Renderer>().bounds.size.x;
            iTween.MoveTo(gameObject, new Vector3(transform.position.x + w, transform.position.y, transform.position.z), 3);

            gameObject.GetComponent<AudioSource>().Play();

            StartCoroutine(Dead());
        }
        GetComponent<Rigidbody2D>().angularVelocity = Mathf.Clamp(GetComponent<Rigidbody2D>().angularVelocity, -1000f, 1000f);
        GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, 15);
    }

    public override bool TouchEvent()
    {
        SceneEventManager sm = Global.SceneEvent;
        if (sm.GetCurMystery().GetComponent<MYBackInTime>())
        {
            sm.GetCurMystery().GetComponent<MYBackInTime>().OnBackInTime();
            return true;
        }
        else
            return false;
    }

    public void ClamSpeed()
    {
        rigbody2d = GetComponent<Rigidbody2D>();
        rigbody2d.velocity = new Vector2(0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            m_life = m_life - 10;
        }
    }


    private void GetScene()//调用此函数获得场景信息
    {
        SceneName = SceneManager.GetActiveScene().name;//获取场景名称
    }

    // 死亡UI
    public IEnumerator Dead()
    {
        yield return new WaitForSeconds(1f);

        //GameObject.Find("UI").GetComponent<UIManager>().deadPanel.SetActive(true);
        //yield return new WaitForSeconds(2f);
        SceneEventManager manager = GameObject.Find("GameManager").GetComponent<SceneEventManager>();
        if (Global.lastDeadMy == manager.GetCurrentMyIndex() && Global.lastDeadScene == SceneManager.GetActiveScene().name)//如果上次死亡谜题和关卡和这次相同，则连续死亡次数+1，否则重置死亡信息
        {
            Global.deadCount++;
        }
        else
        {
            Global.deadCount = 1;
            Global.lastDeadMy = manager.GetCurrentMyIndex();
            Global.lastDeadScene = SceneManager.GetActiveScene().name;
        }
        //GameObject deadPanel = GameObject.Find("UI").gameObject.GetComponent<UIManager>().deadPanel;
        GameObject deadPanel = Global.UI.GetComponent<UIManager>().deadPanel;
        deadPanel.SetActive(true);
        //if (isShortDead)
        //{
            // 【【较复杂的死亡UI
            //deadPanel.GetComponent<Animation>().Play("UIAC_turnDarkShort");
            //deadPanel.GetComponent<Image>().DOFade(1, 1f);
            //Transform timeRuler = root.transform.Find("UI/TimeRuler");
            //InvokeRepeating("VolumeDecrease", 3f, 0.2f);
            //if (timeRuler)
            //{
            //    int year = root.transform.Find("UI/TimeRuler").GetComponent<TimeRuler>().year;
            //    int year = (int)(root.transform.Find("UI/StoneClock").GetComponent<StoneClock>().year);
            //    deadPanel.transform.Find("Tip").GetComponentInChildren<Text>().text = (year - 1).ToString() + "年\n" + "最后一只玳瑁龟遇到了危机";
            //    deadPanel.transform.Find("Warn").GetComponentInChildren<Text>().text = "不过，它一定会顺利度过的";
            //}
            //else
            //{
            //    deadPanel.GetComponentInChildren<Text>().text = "\n" + "最后一只玳瑁龟遇到了危机";
            //    deadPanel.transform.Find("Warn").GetComponentInChildren<Text>().text = "不过，它一定会顺利度过的";
            //}
            //】】

            yield return new WaitForSeconds(7f);
            //值为0的时候跳转场景      
            SceneManager.LoadScene(SceneName);//加载所需场景,SceneName为场景名
        //}

        //else
        //{
        //    deadPanel.GetComponent<Animation>().Play("UIAC_turnDark");
        //    deadPanel.GetComponent<Image>().DOFade(1, 1f);
        //    Transform timeRuler = root.transform.Find("UI/TimeRuler");
        //    InvokeRepeating("VolumeDecrease", 16.5f, 0.5f);
        //    if (timeRuler)
        //    {
        //        //int year = root.transform.Find("UI/TimeRuler").GetComponent<TimeRuler>().year;
        //        int year = (int)(root.transform.Find("UI/StoneClock").GetComponent<StoneClock>().year);
        //        deadPanel.transform.Find("Tip").GetComponentInChildren<Text>().text = (year - 1).ToString() + "年\n" + "最后一只玳瑁龟消失了";
        //    }
        //    else
        //    {
        //        deadPanel.GetComponentInChildren<Text>().text = "\n" + "最后一只玳瑁龟消失了";
        //    }
        //    yield return new WaitForSeconds(21.5f);
        //    //值为0的时候跳转场景      
        //    SceneManager.LoadScene(SceneName);//加载所需场景,SceneName为场景名
        //}
        
    }

    //AudioSource bgm;
    //void VolumeDecrease()
    //{
    //    bgm = GameObject.Find("GameManager").GetComponent<AudioSource>();
    //    bgm.volume -= 0.1f;
    //    if (bgm.volume == 0)
    //    {
    //        CancelInvoke();
    //    }
    //}

    CameraFollow cameraFollow;
    float PlayerSpeed;
    /// <summary>
    /// 龟速从playerSpeed降为0；卷屏速度从当前还原为1
    /// </summary>
    public void SpeedDown(float playerSpeed)
    {
        if (Camera.main)    // 如果是场景退出，会为空
        {
            cameraFollow = Camera.main.GetComponent<CameraFollow>();
            PlayerSpeed = playerSpeed;
            StartCoroutine("SpeedDownSlowly");
        }
    }
    IEnumerator SpeedDownSlowly()
    {
        while (cameraFollow.cameraMoveSpeed > 1 + 1e-4)
        {
            cameraFollow.cameraMoveSpeed = Mathf.Lerp(cameraFollow.cameraMoveSpeed, 1, Time.deltaTime);
            PlayerSpeed = Mathf.Lerp(PlayerSpeed, 0, Time.deltaTime);
            //print(cameraFollow.cameraMoveSpeed + "," + PlayerSpeed);

            // 龟自动游泳速度
            transform.position = new Vector2(transform.position.x + PlayerSpeed * Time.deltaTime, transform.position.y);
            yield return null;
        }
    }
}